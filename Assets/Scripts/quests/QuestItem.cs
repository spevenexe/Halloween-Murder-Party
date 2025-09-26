using DialogueSystem;
using Interaction;
using UnityEngine;

namespace QuestSystem
{
    public class QuestItem : ItemEntity
    {
        [SerializeField] private QuestGiver npc;
        [SerializeField] private DialogueData questItemCollectedDialogue;
        private Quest quest;


        protected override void Awake()
        {
            base.Awake();

            quest = npc.Quest;
        }

        public override void Activate(PlayerData playerData = null)
        {
            base.Activate(playerData);

            npc.SatisfyQuest();
        }
    }
}