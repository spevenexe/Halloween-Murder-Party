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

        [SerializeField] private List<Branch> nextBranches;
        /// <summary>
        /// A list of potential branches to next dialogue
        /// </summary>
        public List<Branch> NextBranches { get => nextBranches; }

        // for checking whether to decrement the number of remaining interactions
        [SerializeField] private int interactionCost = 1;
        public int InteractionCost { get => interactionCost; }

        [SerializeField] private bool doLoop = true;
        public bool DoLoop { get => doLoop; }

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

    /// <summary>
    /// Runtime wrapper for <c>DialogueData</c>. Allows for data change without modifying the file.
    /// </summary>
    public class DialogueObject
    {
        public readonly List<NPC_Centence> Sentences;

        public readonly List<DialogueData.Branch> NextBranches;

        // for checking whether to decrement the number of remaining interactions
        public bool HasBeenRead = false;
        public int InteractionCost { get; private set; }
        public bool DoLoop { get; private set; }

        public DialogueObject(DialogueData dialogueData)
        {
            Sentences = dialogueData.Sentences;
            NextBranches = dialogueData.NextBranches;
            InteractionCost = dialogueData.InteractionCost;
            DoLoop = dialogueData.DoLoop;
        }

        /// <summary>
        /// set the <c>HasBeenRead</c> to true
        /// </summary>
        public void SetRead()
        {
            HasBeenRead = true;
            InteractionCost = 0;
        }
    }
}
