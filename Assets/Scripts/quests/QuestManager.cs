using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace QuestSystem
{
    public class QuestManager : Singleton<QuestManager>
    {
        [SerializeField] private List<Quest> activeQuests;
        [SerializeField] private List<Quest> completedQuests;

        public UnityEvent<Quest> questStarted;
        public UnityEvent<Quest> questCompleted;

        protected override void Awake()
        {
            base.Awake();

            questStarted ??= new();
            questCompleted ??= new();
        }

        void OnEnable()
        {
        }

        void OnDisable()
        {
            questStarted.RemoveAllListeners();
            questCompleted.RemoveAllListeners();
        }

        public void StartQuest(Quest quest)
        {
            if (completedQuests.Contains(quest))
            {
                questCompleted.Invoke(quest);
                return;
            }

            activeQuests.Add(quest);
            questStarted.Invoke(quest);
        }

        public void UpdateQuest(Quest quest, Quest.QuestState questState)
        {
            foreach (Quest q in activeQuests)
            {
                if (q == quest)
                {
                    quest.completionState = questState;
                    // update the UI
                }
            }
        }

        public void CompleteQuest(Quest quest)
        {
            activeQuests.Remove(quest);
            quest.completionState = Quest.QuestState.Complete;

            questCompleted.Invoke(quest);
            completedQuests.Add(quest);
        }
    }
}