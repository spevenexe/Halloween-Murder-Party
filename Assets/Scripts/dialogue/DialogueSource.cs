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
        [SerializeField] private List<NPC_Centence> sentences = new List<NPC_Centence>();

        [Header("References")]
        [SerializeField] private AudioSource audioSource;

        [Header("Events")]
        public UnityEvent startDialogueEvent;
        public UnityEvent nextSentenceDialogueEvent;
        public UnityEvent endDialogueEvent;

        void OnEnable()
        {
            OnHighlight.AddListener(RevealInteractPrompt);
            OnDehighlight.AddListener(HideInteractPrompt);
        }

        void OnDisable()
        {
            OnHighlight.RemoveAllListeners();
            OnDehighlight.RemoveAllListeners();
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
            PlaySound(sentences[currentSentence].sentenceSound);

            //Cooldown timer
            coolDownTimer = sentences[currentSentence].skipDelayTime;
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

            nextSentenceDialogueEvent.Invoke();

            //If last sentence stop dialogue and return
            if (currentSentence > sentences.Count - 1)
            {
                StopDialogue();

                lastSentence = true;

                endDialogueEvent.Invoke();

                return;
            }

            //If not last sentence continue...
            lastSentence = false;

            //Play dialogue sound
            PlaySound(sentences[currentSentence].sentenceSound);

            //Show next sentence in dialogue UI
            ShowCurrentSentence();

            //Cooldown timer
            coolDownTimer = sentences[currentSentence].skipDelayTime;
        }

        public void StopDialogue()
        {
            //Hide dialogue UI
            DialogueUI.instance.ClearText();
            DialogueUI.instance.ClearDialogueSource();

            //Stop audiosource so that the speaker's voice does not play in the background
            if(audioSource != null)
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
            if (sentences[currentSentence].dialogueCharacter != null)
            {
                //Show sentence on the screen
                DialogueUI.instance.ShowSentence(sentences[currentSentence].dialogueCharacter, sentences[currentSentence].sentence);

                //Invoke sentence event
                sentences[currentSentence].sentenceEvent.Invoke();
            }
            else
            {
                DialogueCharacter _dialogueCharacter = new DialogueCharacter();
                _dialogueCharacter.characterName = "";
                _dialogueCharacter.characterPhoto = null;

                DialogueUI.instance.ShowSentence(_dialogueCharacter, sentences[currentSentence].sentence);

                //Invoke sentence event
                sentences[currentSentence].sentenceEvent.Invoke();
            }
        }

        public int CurrentSentenceLength()
        {
            if(sentences.Count <= 0)
                return 0;

            return sentences[currentSentence].sentence.Length;
        }

        public override void Activate(PlayerData playerData = null)
        {
            if (!dialogueIsOn)
            {
                startDialogueEvent.Invoke();

                //If component found start dialogue
                DialogueUI.instance.StartDialogue(this);

                //Hide interaction UI
                DialogueUI.instance.ShowInteractionUI(false);

                dialogueIsOn = true;
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

        public UnityEvent sentenceEvent;
    }
}