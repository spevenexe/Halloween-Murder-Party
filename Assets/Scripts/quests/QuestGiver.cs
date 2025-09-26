using DialogueSystem;
using UnityEngine;

namespace QuestSystem
{
    public class QuestGiver : NPC
    {
        [SerializeField] private QuestData questData;
        public Quest Quest { get; private set; }

        [SerializeField] private DialogueData questSatisfiedDialogue;

        protected override void Awake()
        {
            base.Awake();

            Quest = new(questData);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            endDialogueEvent.AddListener(TryCompleteQuest);
        }

        public void SetQuest(Quest quest)
        {
            this.Quest = quest;
        }

        private void TryCompleteQuest()
        {
            if (Quest == null || Quest.completionState != Quest.QuestState.CanTurnIn) return;

            canSafeCheck = true; // the payoff

            Quest.completionState = Quest.QuestState.Complete;
            QuestManager.instance.questCompleted.Invoke(Quest);

            Quest = null; // to prevent re-accepting the quest
        }

        public void SatisfyQuest()
        {
            SetDialogue(questSatisfiedDialogue);
            Quest.completionState = Quest.QuestState.CanTurnIn;
        }
    }
}