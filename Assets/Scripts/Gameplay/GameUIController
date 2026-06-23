using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SouvlakiTycoon
{
    public class GameUIController : MonoBehaviour
    {
        [Header("Top Left Banner (Athens/Global)")]
        [SerializeField] private LocalizedTextComponent bannerTitleLocalized;
        [SerializeField] private LocalizedTextComponent bannerSubtitleLocalized;

        [Header("Top Bar UI Elements")]
        [SerializeField] private LocalizedTextComponent dayLocalizedText;
        [SerializeField] private Text satisfactionText;
        [SerializeField] private Text customerCountText;
        [SerializeField] private Text goldText;
        [SerializeField] private Text diamondText;

        [Header("Side Menu Buttons (Static Translations)")]
        [SerializeField] private LocalizedTextComponent shopButtonText;
        [SerializeField] private LocalizedTextComponent upgradesButtonText;
        [SerializeField] private LocalizedTextComponent staffButtonText;
        [SerializeField] private LocalizedTextComponent recipesButtonText;

        [Header("Right Board Panel")]
        [SerializeField] private LocalizedTextComponent boardTitleLocalized; 
        [SerializeField] private Text boardValueText; // Δείχνει το €150 ή το Global Rank #1
        [SerializeField] private LocalizedTextComponent boardRewardLabelLocalized;
        [SerializeField] private Text boardRewardValueText; // Δείχνει το €200 ή το €5,230

        [Header("Play Button")]
        [SerializeField] private LocalizedTextComponent playButtonLocalized;
        [SerializeField] private Text playButtonLevelText; // Δείχνει το LEVEL 3

        [Header("Pop-up Panels")]
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private GameObject upgradesPanel;
        [SerializeField] private GameObject staffPanel;
        [SerializeField] private GameObject recipesPanel;

        private void Start()
        {
            // Αρχικό Setup για να σιγουρευτούμε ότι τα static μενού πήραν τα keys τους
            if (shopButtonText != null) shopButtonText.localizationKey = "SHOP";
            if (upgradesButtonText != null) upgradesButtonText.localizationKey = "UPGRADES";
            if (staffButtonText != null) staffButtonText.localizationKey = "STAFF";
            if (recipesButtonText != null) recipesButtonText.localizationKey = "RECIPES";
            
            // Παράδειγμα αρχικού visualization με βάση το screenshot 1 (Athens 1985)
            LoadAthensUiDemo();
        }

        // Μέθοδος που φορτώνει το στήσιμο της Αθήνας (Screenshot 3379_4)
        public void LoadAthensUiDemo()
        {
            if (bannerTitleLocalized != null) bannerTitleLocalized.localizationKey = "START_SMALL";
            if (bannerSubtitleLocalized != null) bannerSubtitleLocalized.localizationKey = "ATHENS_1985";
            
            if (dayLocalizedText != null)
            {
                dayLocalizedText.localizationKey = "DAY";
                dayLocalizedText.UpdateTextWithDynamicValue("", " 1"); // DAY 1
            }

            satisfactionText.text = "85%";
            customerCountText.text = "12";
            goldText.text = "120";
            diamondText.text = "5";

            if (boardTitleLocalized != null) boardTitleLocalized.localizationKey = "TODAYS_GOAL";
            boardValueText.text = "€150";

            if (boardRewardLabelLocalized != null) boardRewardLabelLocalized.localizationKey = "REWARD";
            boardRewardValueText.text = "200";

            if (playButtonLocalized != null) playButtonLocalized.localizationKey = "PLAY";
            playButtonLevelText.text = "LEVEL 3";

            LocalizationManager.Instance.UpdateAllLocalizedTexts();
        }

        // Μέθοδος που μεταμορφώνει το UI σε Global Empire (Screenshot 3380_4)
        public void LoadGlobalEmpireUiDemo()
        {
            if (bannerTitleLocalized != null) bannerTitleLocalized.localizationKey = "BUILD_UPGRADE";
            if (bannerSubtitleLocalized != null) bannerSubtitleLocalized.localizationKey = "GLOBAL_SUCCESS";

            if (dayLocalizedText != null)
            {
                dayLocalizedText.localizationKey = "DAY";
                dayLocalizedText.UpdateTextWithDynamicValue("", " 365"); // DAY 365
            }

            satisfactionText.text = "98%";
            customerCountText.text = "128";
            
            StopAllCoroutines();
            StartCoroutine(AnimateGoldValue(15750));
            diamondText.text = "250";

            if (boardTitleLocalized != null) boardTitleLocalized.localizationKey = "GLOBAL_RANK";
            boardValueText.text = "#1";

            if (boardRewardLabelLocalized != null) boardRewardLabelLocalized.localizationKey = "DAILY_PROFIT";
            boardRewardValueText.text = "€5,230";

            if (playButtonLocalized != null) playButtonLocalized.localizationKey = "PLAY";
            playButtonLevelText.text = "LEVEL 45";

            LocalizationManager.Instance.UpdateAllLocalizedTexts();
        }

        private IEnumerator AnimateGoldValue(int targetGold)
        {
            int startGold = 0;
            float duration = 0.7f;
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
    }
}
