using UnityEngine;

namespace QuestSystem
{
    public class Quest
    {
        private QuestData data;
        public string Title => data.Title;
        public string Description => data.Description;
        public enum QuestState
        {
            NotStarted,
            Started,
            CanTurnIn,
            Complete,
            Failed
        }
        public QuestState completionState;


        public Quest(QuestData data)
        {
            this.data = data;
            completionState = QuestState.NotStarted;
        }

        public override bool Equals(object other)
        {
            if (other is Quest quest)
                return Equals(quest);
            else
                return false;
        }

        public bool Equals(Quest other)
        {
            if (other is null) return false;
            else
            {
                return Title == other.Title;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Quest q1, Quest q2)
        {
            if (q1 is null) return q2 is null;
            return q1.Equals(q2);
        }

        public static bool operator !=(Quest q1, Quest q2)
        {
            if (q1 is null) return !(q2 is null);
            return !q1.Equals(q2);
        }
    }
}