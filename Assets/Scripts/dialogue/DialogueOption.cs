using QuestSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    [RequireComponent(typeof(Button))]
    public class DialogueOption : MonoBehaviour
    {
        private Option option;

        private TMP_Text textbox;
        private Button button;

        void Awake()
        {
            textbox = GetComponentInChildren<TMP_Text>();
            button = GetComponent<Button>();
        }

        void OnEnable()
        {
            button.onClick.AddListener(SelectOption);
        }

        void OnDisable()
        {
            button.onClick.RemoveAllListeners();
        }

        public void SetOption(Option opt)
        {
            option = opt;
            textbox.text = option.message;
        }

        public void SelectOption()
        {
            DialogueUI.instance.ShowOptions(false);

            DialogueSource stashed = DialogueUI.instance.stashed;
            stashed.SetDialogue(option.dialogueAfterSelect);
            DialogueUI.instance.StartDialogue(stashed);

            if(option.questData != null)
                QuestManager.instance.StartQuest(new(option.questData));
        }
    }
}