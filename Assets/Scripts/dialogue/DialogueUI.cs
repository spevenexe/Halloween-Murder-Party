using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using InventorySystem;
using Accusation;

namespace DialogueSystem
{
    public class DialogueUI : MonoBehaviour
    {
        #region Singleton

        public static DialogueUI instance { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            //Hide dialogue and interaction UI at awake
            dialogueWindow.SetActive(false);
            interactionUI.SetActive(false);

            dialogueOptions = DOparent.GetComponentsInChildren<DialogueOption>();
        }

        #endregion

        public DialogueSource CurrentDialogueSource { get; private set; }
        private bool typing;
        // helps consume an input to prevent interaction from instantly playing the text
        private bool justRemovedSource = false;
        private string currentMessage;
        private float startDialogueDelayTimer;

        [Header("References")]
        [SerializeField] private Image portrait;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private GameObject dialogueWindow;
        [SerializeField] private GameObject interactionUI;
        [SerializeField] private TMP_Text interactionUIPrompt;
        [SerializeField] private TMP_Text nextMessagePrompt;

        // to move the characte display around
        [SerializeField] private RectTransform leftDisplay, rightDisplay;
        [SerializeField] private RectTransform characterDisplayTransform;

        [Header("Settings")]
        [SerializeField] private bool animateText = true;

        [Range(0.1f, 1f)]
        [SerializeField] private float textAnimationSpeed = 0.5f;

        [Header("Dialogue Options")]
        [SerializeField] private GameObject DOparent;
        private DialogueOption[] dialogueOptions;

        #region Handling Player Input
        private PlayerData playerData;

        void OnEnable()
        {
            playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();

            playerData.InteractInput.performed += instance.NextSentenceSoft;
        }

        void OnDisable()
        {
            playerData.InteractInput.performed -= instance.NextSentenceSoft;
        }

        #endregion

        private void Update()
        {
            //Delay timer
            if (startDialogueDelayTimer > 0f)
            {
                startDialogueDelayTimer -= Time.deltaTime;
            }

        }

        /// <summary>
        /// If a sentence is being written and this function is called, the sentence is completed instead of immediately moving to the next sentence.
        /// This function needs to be called twice if you want to switch to a new sentence.
        /// </summary>
        public void NextSentenceSoft(InputAction.CallbackContext context)
        {
            if (startDialogueDelayTimer <= 0f)
            {
                if (!typing)
                {
                    NextSentenceHard();
                }
                else
                {
                    StopAllCoroutines();
                    typing = false;
                    messageText.text = currentMessage;
                }
            }
        }

        /// <summary>
        /// Even if a sentence is being written, with this function immediately moves to the next sentence.
        /// </summary>
        public void NextSentenceHard()
        {
            //Continue only if we have dialogue
            if (CurrentDialogueSource == null)
                return;

            //Tell the current dialogue manager to display the next sentence. This function also gives information if we are at the last sentence
            CurrentDialogueSource.NextSentence(out bool lastSentence);

            //If last sentence remove current dialogue manager
            if (lastSentence)
            {
                CurrentDialogueSource = null;
                justRemovedSource = true;
            }
        }

        public void StartDialogue(DialogueSource _DialogueSource)
        {
            if (_DialogueSource == null) return;

            //Delay timer
            startDialogueDelayTimer = 0.1f;

            //Store dialogue manager
            CurrentDialogueSource = _DialogueSource;
            stashed = CurrentDialogueSource;

            //Start displaying dialogue
            CurrentDialogueSource.StartDialogue();

            InventoryUI.instance.Hide();
            AccusationUI.instance.ShowAccuseUI(false);
            ShowInteractionUI(false);
            ShowOptions(false);
        }

        public void StopDialogue()
        {
            if (CurrentDialogueSource != null) CurrentDialogueSource.StopDialogue();
        }

        public void ShowSentence(DialogueCharacter _dialogueCharacter, string _message,NPC_Centence.DisplaySide display = NPC_Centence.DisplaySide.Auto)
        {
            StopAllCoroutines();

            dialogueWindow.SetActive(true);

            portrait.sprite = _dialogueCharacter.characterPhoto;
            nameText.text = _dialogueCharacter.characterName;
            currentMessage = _message;

            // set the location of the text
            switch (display)
            {
                case NPC_Centence.DisplaySide.Right:
                    characterDisplayTransform.position = rightDisplay.position;
                    break;
                case NPC_Centence.DisplaySide.Left:
                default:
                    characterDisplayTransform.position = leftDisplay.position;
                    break;
            }

            if (animateText)
            {
                StartCoroutine(WriteTextToTextmesh(_message, messageText));
            }
            else
            {
                messageText.text = _message;
            }
        }

        public void ClearText()
        {
            dialogueWindow.SetActive(false);
        }

        public void ClearDialogueSource()
        {
            CurrentDialogueSource = null;
        }

        public void ShowInteractionUI(bool _value, string message = "")
        {
            if (OptionsShowing)
            {
                interactionUI.SetActive(false);
                return;
            }

            interactionUI.SetActive(_value);

            // a bit inefficient to reset the strings in this manner on each prompt, but could potentially be useful if rebinding is implemented
            interactionUIPrompt.text = message;
            nextMessagePrompt.text = $"{playerData.InteractInput.GetBindingDisplayString()} - Continue";
        }

        public bool IsProcessingDialogue()
        {
            if (CurrentDialogueSource != null)
            {
                return true;
            }

            return false;
        }

        public bool IsTyping()
        {
            return typing;
        }

        public bool JustRemovedSource()
        {
            bool ret = justRemovedSource;
            justRemovedSource = false;
            return ret;
        }


        public int CurrentDialogueSentenceLength()
        {
            if (CurrentDialogueSource == null)
                return 0;

            return CurrentDialogueSource.CurrentSentenceLength();
        }

        IEnumerator WriteTextToTextmesh(string _text, TextMeshProUGUI _textMeshObject)
        {
            typing = true;

            _textMeshObject.text = "";
            char[] _letters = _text.ToCharArray();

            float _speed = 1f - textAnimationSpeed;

            foreach (char _letter in _letters)
            {
                _textMeshObject.text += _letter;

                if (_textMeshObject.text.Length == _letters.Length)
                {
                    typing = false;
                }

                yield return new WaitForSeconds(0.1f * _speed);
            }
        }

        #region Dialogue Options

        public DialogueSource stashed { get; private set; }

        public void ShowOptions(bool _value)
        {
            DOparent.SetActive(_value);
            if (!_value || stashed is null || stashed.options.Length <= 0)
            {
                // PlayerCamera.instance.UnlockCamera();
                return;
            }

            PlayerCamera.instance.LockCamera();

            Option[] options = stashed.options;
            int i;
            for (i = 0; i < options.Length; i++)
            {
                DialogueOption current = dialogueOptions[i];
                current.gameObject.SetActive(true);
                current.SetOption(options[i]);
            }

            for (; i < dialogueOptions.Length; i++)
            {
                dialogueOptions[i].gameObject.SetActive(false);
            }
        }

        public bool OptionsShowing => DOparent.activeSelf;

        #endregion
    }
}