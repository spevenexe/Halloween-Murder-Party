using DialogueSystem;
using Interaction;
using UnityEngine;

namespace QuestSystem
{
    public class QuestItem : ItemEntity
    {
        [SerializeField] private QuestData questData;
        [SerializeField] private DialogueData questItemCollectedDialogue;
        private Quest quest;


        protected override void Awake()
        {
            base.Awake();

            quest = new(questData);
        }

        public override void Activate(PlayerData playerData = null)
        {
            base.Activate(playerData);

            QuestManager.instance.CompleteQuest(quest);
        }
    }
}