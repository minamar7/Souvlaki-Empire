using UnityEngine;
using System;

namespace SouvlakiTycoon
{
    public class UpgradeManager : MonoBehaviour
    {
        public static UpgradeManager Instance { get; private set; }

        [System.Serializable]
        public class UpgradeItem
        {
            public string upgradeID;      // π.χ. "Grill_Speed", "Counter_Space"
            public string nameKey;        // Key για το Localization (π.χ. "UPGRADE_GRILL")
            public int currentLevel = 0;
            public int maxLevel = 5;
            public int baseCost = 100;
            public float costMultiplier = 1.5f;
        }

        [Header("Upgrade Items")]
        public UpgradeItem grillSpeedUpgrade = new UpgradeItem { upgradeID = "Grill_Speed", nameKey = "UPGRADE_GRILL", baseCost = 150 };
        public UpgradeItem meatQualityUpgrade = new UpgradeItem { upgradeID = "Meat_Quality", nameKey = "UPGRADE_MEAT", baseCost = 200 };

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadUpgrades();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Επιστρέφει το τρέχον κόστους μιας αναβάθμισης βάσει του level της
        public int GetUpgradeCost(UpgradeItem item)
        {
            if (item.currentLevel >= item.maxLevel) return -1; // Maxed out
            return Mathf.RoundToInt(item.baseCost * Mathf.Pow(item.costMultiplier, item.currentLevel));
        }

        // Προσπάθεια αγοράς της αναβάθμισης
        public bool TryPurchaseUpgrade(string upgradeID)
        {
            UpgradeItem item = GetUpgradeItemByID(upgradeID);
            if (item == null || item.currentLevel >= item.maxLevel) return false;

            int cost = GetUpgradeCost(item);

            // Έλεγχος αν έχουμε αρκετά λεφτά στο Progression Manager
            if (GameProgressionManager.Instance != null && GameProgressionManager.Instance.gold >= cost)
            {
                GameProgressionManager.Instance.gold -= cost; // Αφαίρεση χρημάτων
                item.currentLevel++;
                
                SaveUpgrades();
                
                // Ενημέρωση του UI αν υπάρχει
                if (GameUIController.Instance != null)
                {
                    // GameUIController.Instance.UpdateUI(); 
                }

                Debug.Log($"Αγοράστηκε το {upgradeID}! Νέο Level: {item.currentLevel}");
                return true;
            }

            Debug.Log("Δεν υπάρχουν αρκετά χρήματα!");
            return false;
        }

        private UpgradeItem GetUpgradeItemByID(string id)
        {
            if (grillSpeedUpgrade.upgradeID == id) return grillSpeedUpgrade;
            if (meatQualityUpgrade.upgradeID == id) return meatQualityUpgrade;
            return null;
        }

        private void SaveUpgrades()
        {
            PlayerPrefs.SetInt("Upgrade_" + grillSpeedUpgrade.upgradeID, grillSpeedUpgrade.currentLevel);
            PlayerPrefs.SetInt("Upgrade_" + meatQualityUpgrade.upgradeID, meatQualityUpgrade.currentLevel);
            PlayerPrefs.Save();
        }

        private void LoadUpgrades()
        {
            grillSpeedUpgrade.currentLevel = PlayerPrefs.GetInt("Upgrade_" + grillSpeedUpgrade.upgradeID, 0);
            meatQualityUpgrade.currentLevel = PlayerPrefs.GetInt("Upgrade_" + meatQualityUpgrade.upgradeID, 0);
        }

        // Επιστρέφει multiplier για την ταχύτητα ψησίματος (χρησιμοποιείται στο GrillStation)
        public float GetGrillSpeedMultiplier()
        {
            // Κάθε level μειώνει τον χρόνο κατά 15%
            return 1f - (grillSpeedUpgrade.currentLevel * 0.15f);
        }
    }
}
