using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SouvlakiTycoon
{
    public class LocalizedTextComponent : MonoBehaviour
    {
        [Header("Localization Key")]
        [Tooltip("Βάλε εδώ το Key της μετάφρασης (π.χ. START_SMALL, SHOP, DAY)")]
        [SerializeField] private string localizationKey;

        private Text unityUIText;
        private TextMeshProUGUI tmProText;

        private void Awake()
        {
            unityUIText = GetComponent<Text>();
            tmProText = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            UpdateText();
        }

        /// <summary>
        /// Διαβάζει το Key και ενημερώνει το κείμενο στη σωστή γλώσσα.
        /// Καλείται αυτόματα στην εκκίνηση και κάθε φορά που αλλάζει η γλώσσα.
        /// </summary>
        public void UpdateText()
        {
            if (LocalizationManager.Instance == null || string.IsNullOrEmpty(localizationKey)) return;

            string translatedValue = LocalizationManager.Instance.GetTranslatedValue(localizationKey);

            if (tmProText != null)
            {
                tmProText.text = translatedValue;
            }
            else if (unityUIText != null)
            {
                unityUIText.text = translatedValue;
            }
        }

        /// <summary>
        /// Επιτρέπει την αλλαγή του Key δυναμικά μέσω κώδικα (π.χ. αν αλλάζει ο τίτλος ενός επιπέδου)
        /// </summary>
        public void SetKey(string newKey)
        {
            localizationKey = newKey;
            UpdateText();
        }
    }
}
