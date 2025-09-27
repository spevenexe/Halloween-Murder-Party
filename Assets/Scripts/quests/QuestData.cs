using DialogueSystem;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "New Quest Data ", menuName = "Quest System/Quest Data")]
    public class QuestData : ScriptableObject
    {
        [SerializeField] private string title;
        public string Title => title;

        [TextArea(3, 10)]
        [SerializeField] private string description;
        public string Description => description;

        [SerializeField] private DialogueData completedDialogue;
        public DialogueData CompletedDialogue => completedDialogue;
    }
}