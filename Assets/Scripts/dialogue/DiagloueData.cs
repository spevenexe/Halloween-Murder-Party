using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Data Container for NPC Dialogue. Also contains a list of potential subsequent dialogues, allowing for branching text.
    /// </summary>
    [CreateAssetMenu(fileName = "New Dialogue Data", menuName = "Dialogue System/New Dialogue Data")]
    [System.Serializable]
    public class DialogueData : ScriptableObject
    {
        [SerializeField] private List<NPC_Centence> sentences = new List<NPC_Centence>();
        public List<NPC_Centence> Sentences { get => sentences; }

        [SerializeField] public List<Branch> nextBranches;
        /// <summary>
        /// A list of potential branches to next dialogue
        /// </summary>
        public List<Branch> NextBranches { get => nextBranches; }
    
        /// <summary>
        /// A potential branch of dialogue. Contains dialgoue data and the flags that need to be satisfied in order to branch to that dialogue. 
        /// </summary>
        [System.Serializable]
        public struct Branch
        {
            public DialogueData DialogueData;
            // check this against the current flag state of the game/dialogue character
            public DialogueFlags KeyFlags;
        }
    }
}
