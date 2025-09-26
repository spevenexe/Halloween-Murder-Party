using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace QuestSystem
{
    public class QuestManager : DontDestroySingleton<QuestManager>
    {
        private List<Quest> activeQuests = new();
        private List<Quest> completedQuests = new();

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
            Debug.Log(quest);

            if (completedQuests.Contains(quest)) return;

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