namespace DialogueSystem
{
    /// <summary>
    /// container for boolean flags that are to be checked by <c>DialogueSource</c>
    /// </summary>
    [System.Serializable]
    public class DialogueFlags
    {
        public bool isHappy; // just some test flags
        public bool isEvil;

        public DialogueFlags(bool isHappy, bool isEvil)
        {
            this.isHappy = isHappy;
            this.isEvil = isEvil;
        }

        /// <summary>
        /// Check <c>flags</c> against <c>key</c> to see if <c>key</c> can be satisfied. Note that <c>key</c> only concerns itself with matching <b>true</b> values. That is, <c>flags</c> could have more than 
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool FlagsSatisfied(DialogueFlags flags, DialogueFlags key)
        {
            // conditional disjunctional equivalence. Basically, we only care the vaule in flags if key is true
            return (flags.isHappy || !key.isHappy) &&
                    (flags.isEvil || !key.isEvil);
        }

        
    }
}