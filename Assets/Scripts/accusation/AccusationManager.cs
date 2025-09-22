using UnityEngine;

namespace Accusation
{
    public class AccusationManager : MonoBehaviour
    {
        #region Singleton
        public static AccusationManager instance { get; private set; }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            playerAura = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAura>();
        }
        #endregion

        private PlayerAura playerAura;

        void OnEnable()
        {
            playerAura.AuraChanged.AddListener(Log);
        }

        void OnDisable()
        {
            playerAura.AuraChanged.RemoveListener(Log);
        }

        public void CalculateAuraPenalty(NPC npc, out int auraDelta)
        {
            // TODO: something fancier:
            auraDelta = 20 * (npc.IsMonster ? 1 : -1);

            playerAura.AddAura(auraDelta);
        }

        public void Log(int newAura, int delta)
        {
            Debug.Log($"Aura is now {newAura}, changed from {newAura - delta}");
        }
    }
}