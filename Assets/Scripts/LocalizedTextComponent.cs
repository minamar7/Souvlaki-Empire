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

        void Start()
        {
            textComponent = GetComponent<Text>();
            UpdateText();
        }

        void OnEnable()
        {
            UpdateText();
        }

        public void UpdateText()
        {
            if (textComponent == null) textComponent = GetComponent<Text>();
            
            if (LocalizationManager.Instance != null && !string.IsNullOrEmpty(localizationKey))
            {
                textComponent.text = LocalizationManager.Instance.GetTranslatedValue(localizationKey);
            }
        }
    }
}
