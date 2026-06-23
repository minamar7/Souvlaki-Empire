using UnityEngine;
using UnityEngine.UI; // Αν χρησιμοποιείς απλό UI Text
// using TMPro; // Ξεσχολίασέ το αν χρησιμοποιείς TextMeshPro

namespace SouvlakiTycoon
{
    public class LocalizedTextComponent : MonoBehaviour
    {
        [Header("Localization Key")]
        [Tooltip("Βάλε εδώ το KEY της μετάφρασης, π.χ. SHOP, UPGRADES, PLAY")]
        [SerializeField] private string localizationKey;

        private Text uiText;
        // private TextMeshProUGUI tmpText; // Ξεσχολίασέ το αν έχεις TextMeshPro

        private void Start()
        {
            uiText = GetComponent<Text>();
            // tmpText = GetComponent<TextMeshProUGUI>(); // Ξεσχολίασέ το αν έχεις TextMeshPro
            
            UpdateText();
        }

        public void UpdateText()
        {
            if (LocalizationManager.Instance == null) return;

            string translatedString = LocalizationManager.Instance.GetTranslatedValue(localizationKey);

            if (uiText != null) uiText.text = translatedString;
            // if (tmpText != null) tmpText.text = translatedString; // Ξεσχολίασέ το αν έχεις TextMeshPro
        }
    }
}
