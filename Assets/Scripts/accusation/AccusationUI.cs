using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Accusation
{
    public class AccusationUI : MonoBehaviour
    {
        #region Singleton
        public static AccusationUI instance { get; private set; }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }
        #endregion

        [SerializeField] private TMP_Text interactionUIPrompt;
        [SerializeField] private GameObject interactionUI;

        private PlayerData playerData;

        void Start()
        {
            playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        }

        public void ShowAccuseUI(bool _value, bool isSafe=false)
        {
            interactionUI.SetActive(_value);

            if (!_value) return;

            string safetyLevel = isSafe ? "Safe" : "Risky";

            // a bit inefficient to reset the strings in this manner on each prompt, but could potentially be useful if rebinding is implemented
            interactionUIPrompt.text = $"{playerData.AccuseInput.GetBindingDisplayString()} - Accuse ({safetyLevel})";
        }
    }
}