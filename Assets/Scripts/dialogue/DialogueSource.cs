using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem
{
    public class DialogueSource : Interactable
    {
        private int currentSentence;
        private float coolDownTimer;
        private bool dialogueIsOn;

        [Header("Dialogue")]
        [SerializeField] protected DialogueData dialogueData;
        // if the next conversation is available, this allows for smooth conversations without stopping
        private DialogueData prev;
        protected DialogueFlags flags = new(false, false);

        [Header("References")]
        [SerializeField] private AudioSource audioSource;

        [Header("Events")]
        public UnityEvent<DialogueFlags> startDialogueEvent;
        public UnityEvent<DialogueFlags> nextSentenceDialogueEvent;
        public UnityEvent<DialogueFlags> endDialogueEvent;

        void Awake()
        {
            prev = dialogueData;
        }

        protected virtual void OnEnable()
        {
            OnHighlight.AddListener(RevealInteractPrompt);
            OnDehighlight.AddListener(HideInteractPrompt);

            endDialogueEvent.AddListener(NextDialogue);
        }

        protected virtual void OnDisable()
        {
            OnHighlight.RemoveAllListeners();
            OnDehighlight.RemoveAllListeners();

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
        private void RevealInteractPrompt()
        {
            if (dialogueData != null)
                DialogueUI.instance.ShowInteractionUI(true);
        }

        private void HideInteractPrompt()
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
            PlaySound(dialogueData.Sentences[currentSentence].sentenceSound);

            //Cooldown timer
            coolDownTimer = dialogueData.Sentences[currentSentence].skipDelayTime;
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
            if (currentSentence > dialogueData.Sentences.Count - 1)
            {
                StopDialogue();

                lastSentence = true;

                endDialogueEvent.Invoke(flags);

                return;
            }

            //If not last sentence continue...
            lastSentence = false;

            //Play dialogue sound
            PlaySound(dialogueData.Sentences[currentSentence].sentenceSound);

            //Show next sentence in dialogue UI
            ShowCurrentSentence();

            //Cooldown timer
            coolDownTimer = dialogueData.Sentences[currentSentence].skipDelayTime;
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
            var sentences = dialogueData.Sentences;
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
                dialogueData.Sentences[currentSentence].sentenceEvent.Invoke(flags);
            }
        }

        public int CurrentSentenceLength()
        {
            if (dialogueData.Sentences.Count <= 0)
                return 0;

            return dialogueData.Sentences[currentSentence].sentence.Length;
        }

        public override void Activate(PlayerData playerData = null)
        {
            // either we just interacted to close the text window, or there's more (different) text
            if (!dialogueIsOn && dialogueData != null &&
            (!DialogueUI.instance.JustRemovedSource() || prev != dialogueData))
            {
                startDialogueEvent.Invoke(flags);

                //If component found start dialogue
                DialogueUI.instance.StartDialogue(this);

                //Hide interaction UI
                DialogueUI.instance.ShowInteractionUI(false);

                dialogueIsOn = true;
            }
        }

        // set the next dialogue data object, if it exists
        protected virtual void NextDialogue(DialogueFlags flags)
        {
            if (dialogueData != null)
            {
                prev = dialogueData;
                if (dialogueData.NextBranches != null && dialogueData.NextBranches.Count > 0)
                {
                    // default behavior is first in list
                    dialogueData = dialogueData.NextBranches[0].DialogueData;
                }
                else
                {
                    // no branches? no more dialogue
                    dialogueData = null;
                }
            }
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