using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SouvlakiTycoon
{
    public class GameUIController : MonoBehaviour
    {
        [Header("Top Bar UI Elements")]
        [SerializeField] private Text dayText;
        [SerializeField] private Text satisfactionText;
        [SerializeField] private Text customerCountText;
        [SerializeField] private Text goldText;
        [SerializeField] private Text diamondText;

        [Header("Side Menu Buttons")]
        [SerializeField] private Button shopButton;
        [SerializeField] private Button upgradesButton;
        [SerializeField] private Button staffButton;
        [SerializeField] private Button recipesButton;

        [Header("Right Board Panel")]
        [SerializeField] private Text goalTitleText;
        [SerializeField] private Text goalAmountText;
        [SerializeField] private Text rewardAmountText;
        [SerializeField] private Button playButton;
        [SerializeField] private Text playButtonLevelText;

        [Header("Pop-up Panels (For Navigation)")]
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private GameObject upgradesPanel;
        [SerializeField] private GameObject staffPanel;
        [SerializeField] private GameObject recipesPanel;

        private void Awake()
        {
            // Setup Listeners για τα αριστερά κουμπιά
            shopButton.onClick.AddListener(() => TogglePanel(shopPanel));
            upgradesButton.onClick.AddListener(() => TogglePanel(upgradesPanel));
            staffButton.onClick.AddListener(() => TogglePanel(staffPanel));
            recipesButton.onClick.AddListener(() => TogglePanel(recipesPanel));
            
            playButton.onClick.AddListener(OnPlayButtonPressed);
        }

        // Ενημερώνει τα UI στοιχεία με "Smooth" εφέ για τα νούμερα (όπως το Gold 15,750)
        public void UpdateTopBar(int day, float satisfaction, int customers, int gold, int diamonds)
        {
            if (dayText != null) dayText.text = $"DAY {day}";
            if (satisfactionText != null) satisfactionText.text = $"{satisfaction:F0}%";
            if (customerCountText != null) customerCountText.text = customers.ToString();
            if (diamondText != null) diamondText.text = diamonds.ToString();
            
            // Κάνουμε animate το χρυσό για πιο "premium" αίσθηση
            StopAllCoroutines();
            StartCoroutine(AnimateGoldValue(gold));
        }

        public void UpdateRightBoard(string cityTitle, int goalMoney, int rewardMoney, int currentLevel)
        {
            if (goalTitleText != null) goalTitleText.text = cityTitle;
            if (goalAmountText != null) goalAmountText.text = $"€{goalMoney:N0}";
            if (rewardAmountText != null) rewardAmountText.text = $"€{rewardMoney:N0}";
            if (playButtonLevelText != null) playButtonLevelText.text = $"PLAY\nLEVEL {currentLevel}";
        }

        private IEnumerator AnimateGoldValue(int targetGold)
        {
            int startGold = int.Parse(goldText.text.Replace(",", ""));
            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                int current = (int)Mathf.Lerp(startGold, targetGold, elapsed / duration);
                goldText.text = current.ToString("N0");
                yield return null;
            }
            goldText.text = targetGold.ToString("N0");
        }

        // Διαχειρίζεται το άνοιγμα/κλείσιμο των panels με ασφάλεια
        private void TogglePanel(GameObject targetPanel)
        {
            if (targetPanel == null) return;

            // Κλείσιμο όλων των άλλων ανοιχτών pop-ups πρώτα
            shopPanel.SetActive(false);
            upgradesPanel.SetActive(false);
            staffPanel.SetActive(false);
            recipesPanel.SetActive(false);

            // Toggle το επιλεγμένο
            targetPanel.SetActive(!targetPanel.activeSelf);
            
            // Audio Cue ή Micro-vibration εδώ (προαιρετικά)
        }

        private void OnPlayButtonPressed()
        {
            Debug.Log("Starting Level Session... Spawning Customers!");
            // Κλείνουμε τα μενού και ξεκινάει το gameplay loop της πόλης
            shopPanel.SetActive(false);
            upgradesPanel.SetActive(false);
            staffPanel.SetActive(false);
            recipesPanel.SetActive(false);
        }
    }
}
