using UnityEngine;
using UnityEngine.UI;

namespace SouvlakiTycoon
{
    [RequireComponent(typeof(Text))]
    public class LocalizedTextComponent : MonoBehaviour
    {
        [Tooltip("Το μοναδικό Key από το LocalizationManager")]
        public string localizationKey;

        private Text textComponent;
        private string currentDynamicSuffix = "";
        private string currentDynamicPrefix = "";

        void Awake()
        {
            textComponent = GetComponent<Text>();
        }

        void Start()
        {
            UpdateText();
        }

        void OnEnable()
        {
            UpdateText();
        }

        // Απλή ενημέρωση κειμένου (για σταθερά μενού όπως SHOP, UPGRADES)
        public void UpdateText()
        {
            if (textComponent == null) textComponent = GetComponent<Text>();
            
            if (LocalizationManager.Instance != null && !string.IsNullOrEmpty(localizationKey))
            {
                string baseTranslation = LocalizationManager.Instance.GetTranslatedValue(localizationKey);
                textComponent.text = $"{currentDynamicPrefix}{baseTranslation}{currentDynamicSuffix}";
            }
        }

        // Premium λειτουργία για δυναμικά κείμενα (π.χ. DAY + " " + 365)
        public void UpdateTextWithDynamicValue(string prefix, string suffix)
        {
            currentDynamicPrefix = prefix;
            currentDynamicSuffix = suffix;
            UpdateText();
        }
    }
}
