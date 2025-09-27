using DialogueSystem;
using UnityEngine;

namespace QuestSystem
{
    public class QuestGiver : NPC
    {
        [SerializeField] private Quest quest = null;
        public Quest Quest => quest;


        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            QuestManager.instance.questCompleted.AddListener(TryCompleteQuest);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

        }

        public void SetQuest(Quest quest)
        {
            this.quest = quest;
        }

        private void TryCompleteQuest(Quest q)
        {
            if (this.quest == null || quest != q) return;

            EnqueueDialogue(q.CompletionDialogue);
            this.quest = null; // to prevent re-accepting the quest
        }

        protected override void ShowCurrentSentence()
        {
            base.ShowCurrentSentence();
            var sentences = dialogueObject.Sentences;
            var current = sentences[currentSentence];

            if (current.questData != null)
            {
                quest = new(current.questData);
                QuestManager.instance.StartQuest(quest);
            }
        }
    }
}