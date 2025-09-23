using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DialogueSystem
{
    public class DialogueSource : Interactable
    {
        private int currentSentence;
        private float coolDownTimer;
        private bool dialogueIsOn;

        [Header("Dialogue")]
        [SerializeField] protected DialogueData dialogueData;
        protected DialogueObject dialogueObject;
        // if the next conversation is available, this allows for smooth conversations without stopping
        private DialogueObject prev = null;
        // configure the default state of the NPC
        [SerializeField] protected DialogueFlags flags = new();

        [Header("References")]
        [SerializeField] private AudioSource audioSource;

        [Header("Events")]
        public UnityEvent<DialogueFlags> startDialogueEvent;
        public UnityEvent<DialogueFlags> nextSentenceDialogueEvent;
        public UnityEvent<DialogueFlags> endDialogueEvent;

        protected override void Awake()
        {
            base.Awake();
            dialogueObject = new(dialogueData);
        }

        protected virtual void OnEnable()
        {
            OnHighlight.AddListener(RevealInteractPrompt);
            OnDehighlight.AddListener(HideInteractPrompt);

            endDialogueEvent.AddListener(NextDialogue);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            startDialogueEvent.RemoveAllListeners();
            nextSentenceDialogueEvent.RemoveAllListeners();
            endDialogueEvent.RemoveAllListeners();
        }

        private void Update()
        {
            //Timer
            if (coolDownTimer > 0f)
            {
                coolDownTimer -= Time.deltaTime;
            }
        }

        //Show interaction UI
        protected override void RevealInteractPrompt()
        {
            if (dialogueObject != null)
            {
                string message = $"[{PlayerData.instance.InteractInput.GetBindingDisplayString()}] Talk";

                if (!dialogueObject.HasBeenRead)
                {
                    if (InteractionLimitManager.instance.NumInteracts < dialogueObject.InteractionCost)
                    {
                        message += $" (Not Enough Interactions!)";
                    }
                    else
                    {
                        string plurality = (dialogueObject.InteractionCost == 1) ? "Interaction" : "Interactions";
                        message += string.Format(" (Costs {0} {1})",dialogueObject.InteractionCost,plurality);
                    }
                }

                DialogueUI.instance.ShowInteractionUI(true,message);
            }
        }

        protected override void HideInteractPrompt()
        {
            //Hide interaction UI
            DialogueUI.instance.ShowInteractionUI(false);

            //Stop dialogue
            StopDialogue();
        }

        public void StartDialogue()
        {
            //Reset sentence index
            currentSentence = 0;

            //Show first sentence in dialogue UI
            ShowCurrentSentence();

            //Play dialogue sound
            PlaySound(dialogueObject.Sentences[currentSentence].sentenceSound);

            //Cooldown timer
            coolDownTimer = dialogueObject.Sentences[currentSentence].skipDelayTime;
        }

        public void NextSentence(out bool lastSentence)
        {
            //The next sentence cannot be changed immediately after starting
            if (coolDownTimer > 0f)
            {
                lastSentence = false;
                return;
            }

            //Add one to sentence index
            currentSentence++;

            nextSentenceDialogueEvent.Invoke(flags);

            //If last sentence stop dialogue and return
            if (currentSentence > dialogueObject.Sentences.Count - 1)
            {
                StopDialogue();

                lastSentence = true;

                endDialogueEvent.Invoke(flags);

                if (PlayerData.instance.Target == this)
                    RevealInteractPrompt();

                return;
            }

            //If not last sentence continue...
            lastSentence = false;

            //Play dialogue sound
            PlaySound(dialogueObject.Sentences[currentSentence].sentenceSound);

            //Show next sentence in dialogue UI
            ShowCurrentSentence();

            //Cooldown timer
            coolDownTimer = dialogueObject.Sentences[currentSentence].skipDelayTime;
        }

        public void StopDialogue()
        {
            //Hide dialogue UI
            DialogueUI.instance.ClearText();
            DialogueUI.instance.ClearDialogueSource();

            //Stop audiosource so that the speaker's voice does not play in the background
            if (audioSource != null)
            {
                audioSource.Stop();
            }

            //Remove trigger refence
            dialogueIsOn = false;
        }

        private void PlaySound(AudioClip _audioClip)
        {
            //Play the sound only if it exists
            if (_audioClip == null || audioSource == null)
                return;

            //Stop the audioSource so that the new sentence does not overlap with the old one
            audioSource.Stop();

            //Play sentence sound
            audioSource.PlayOneShot(_audioClip);
        }

        private void ShowCurrentSentence()
        {
            var sentences = dialogueObject.Sentences;
            if (sentences[currentSentence].dialogueCharacter != null)
            {
                //Show sentence on the screen
                DialogueUI.instance.ShowSentence(sentences[currentSentence].dialogueCharacter, sentences[currentSentence].sentence);

                //Invoke sentence event
                sentences[currentSentence].sentenceEvent.Invoke(flags);
            }
            else
            {
                DialogueCharacter _dialogueCharacter = new DialogueCharacter();
                _dialogueCharacter.characterName = "";
                _dialogueCharacter.characterPhoto = null;

                DialogueUI.instance.ShowSentence(_dialogueCharacter, sentences[currentSentence].sentence);

                //Invoke sentence event
                dialogueObject.Sentences[currentSentence].sentenceEvent.Invoke(flags);
            }
        }

        public int CurrentSentenceLength()
        {
            if (dialogueObject.Sentences.Count <= 0)
                return 0;

            return dialogueObject.Sentences[currentSentence].sentence.Length;
        }

        public override void Activate(PlayerData playerData = null)
        {
            // either we just interacted to close the text window, or there's more (different) text
            // make sure to calculate this always, so that DialogueUI updates its value
            // bool justRemovedSource = DialogueUI.instance.JustRemovedSource();
            if (!dialogueIsOn && dialogueObject != null &&
            // (!justRemovedSource || (prev != dialogueObject && !dialogueObject.DoLoop)) &&
            InteractionLimitManager.instance.NumInteracts > 0)
            {
                startDialogueEvent.Invoke(flags);

                //If component found start dialogue
                DialogueUI.instance.StartDialogue(this);

                //Hide interaction UI
                DialogueUI.instance.ShowInteractionUI(false);

                dialogueIsOn = true;
            }
        }

        // set the next dialogue data object, if it exists. Otherwise, loop back to the beginning of the current object.
        protected virtual void NextDialogue(DialogueFlags flags)
        {
            if (dialogueObject != null)
            {
                prev = dialogueObject;
                if (dialogueObject.NextBranches != null && dialogueObject.NextBranches.Count > 0)
                {
                    // default behavior is first in list
                    DialogueData nextDialogueData = dialogueObject.NextBranches[0].DialogueData;
                    dialogueObject = new(nextDialogueData);
                }
                else if (!dialogueObject.DoLoop)
                {
                    // no branches or looping? no more dialogue
                    dialogueObject = null;
                }
            }
        }

        protected void SetDialogue(DialogueData newData)
        {
            prev = dialogueObject;
            dialogueObject = new(newData);
        }
    }

    [System.Serializable]
    public class NPC_Centence
    {
        [Header("------------------------------------------------------------")]

        public DialogueCharacter dialogueCharacter;

        [TextArea(3, 10)]
        public string sentence;

        public float skipDelayTime = 0.5f;

        public AudioClip sentenceSound;

        [SerializeField] private DialogueFlags keyFlags;

        public UnityEvent<DialogueFlags> sentenceEvent;
    }
}