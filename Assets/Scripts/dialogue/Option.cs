using QuestSystem;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem
{
    [System.Serializable]
    public class Option
    {
        public string message;
        public DialogueData dialogueAfterSelect;
        public QuestData questData;
    }
}
