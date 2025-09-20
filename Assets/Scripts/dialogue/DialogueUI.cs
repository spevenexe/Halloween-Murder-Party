using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

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
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            //Hide dialogue and interaction UI at awake
            dialogueWindow.SetActive(false);
            interactionUI.SetActive(false);
        }

        #endregion

        private DialogueSource currentDialogueSource;
        private bool typing;
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

        [Header("Settings")]
        [SerializeField] private bool animateText = true;

        [Range(0.1f, 1f)]
        [SerializeField] private float textAnimationSpeed = 0.5f;

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
            if (currentDialogueSource == null)
                return;

            //Tell the current dialogue manager to display the next sentence. This function also gives information if we are at the last sentence
            currentDialogueSource.NextSentence(out bool lastSentence);

            //If last sentence remove current dialogue manager
            if (lastSentence)
            {
                currentDialogueSource = null;
            }
        }

        public void StartDialogue(DialogueSource _DialogueSource)
        {
            //Delay timer
            startDialogueDelayTimer = 0.1f;

            //Store dialogue manager
            currentDialogueSource = _DialogueSource;

            //Start displaying dialogue
            currentDialogueSource.StartDialogue();
        }

        public void ShowSentence(DialogueCharacter _dialogueCharacter, string _message)
        {
            StopAllCoroutines();

            dialogueWindow.SetActive(true);

            portrait.sprite = _dialogueCharacter.characterPhoto;
            nameText.text = _dialogueCharacter.characterName;
            currentMessage = _message;

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
            currentDialogueSource = null;
        }

        public void ShowInteractionUI(bool _value)
        {
            interactionUI.SetActive(_value);

            // a bit inefficient to reset the strings in this manner on each prompt, but could potentially be useful if rebinding is implemented
            interactionUIPrompt.text = $"[{playerData.InteractInput.GetBindingDisplayString()}] Talk";
            nextMessagePrompt.text = $"[{playerData.InteractInput.GetBindingDisplayString()}] Continue";
        }

        public bool IsProcessingDialogue()
        {
            if(currentDialogueSource != null)
            {
                return true;
            }

            return false;
        }

        public bool IsTyping()
        {
            return typing;
        }

        public int CurrentDialogueSentenceLenght()
        {
            if (currentDialogueSource == null)
                return 0;

            return currentDialogueSource.CurrentSentenceLength();
        }

        IEnumerator WriteTextToTextmesh(string _text, TextMeshProUGUI _textMeshObject)
        {
            typing = true;

            _textMeshObject.text = "";
            char[] _letters = _text.ToCharArray();

            float _speed = 1f - textAnimationSpeed;

            foreach(char _letter in _letters)
            {
                _textMeshObject.text += _letter;

                if(_textMeshObject.text.Length == _letters.Length)
                {
                    typing = false;
                }

                yield return new WaitForSeconds(0.1f * _speed);
            }
        }
    }
}