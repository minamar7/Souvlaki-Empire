using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SouvlakiTycoon
{
    [System.Serializable]
    public class CityStage
    {
        public string cityID;          // Π.χ. "ATHENS", "ITALY", "TOKYO", "LONDON", "GLOBAL"
        public string cityDisplayName; // Το όνομα που θα φαίνεται στην οθόνη
        public int startFromLevel;     // Από ποιο level ξεκινάει η πόλη
        public int endAtLevel;         // Σε ποιο level τελειώνει
        public Sprite backgroundSprite;// Το 3D/Cartoon φόντο με τα landmarks της πόλης
        public int initialTargetGoal;  // Ο βασικός οικονομικός στόχος για αυτή την πόλη
    }

    public class CityProgressionManager : MonoBehaviour
    {
        [Header("Player Progress")]
        public int globalPlayerLevel = 1;
        public int currentDay = 1;
        public int currentGold = 120;
        public int currentDiamonds = 5;

        [Header("City Configurations")]
        public List<CityStage> gameCities = new List<CityStage>();

        [Header("UI Canvas Elements")]
        public Image backgroundImageDisplay;
        public Text locationTitleText;
        public Text todaysGoalText;
        public Text playButtonText;
        
        [Header("Top Bar UI Elements")]
        public Text dayUIText;
        public Text goldUIText;
        public Text diamondUIText;

        private CityStage activeCity;

        void Start()
        {
            // Φόρτωση της προόδου του παίκτη από τη συσκευή
            LoadData();
            
            // Υπολογισμός της τρέχουσας πόλης με βάση το Level
            UpdateActiveCity();
            
            // Ενημέρωση όλων των γραφικών στοιχείων της οθόνης
            UpdateGameUI();
        }

        public void UpdateActiveCity()
        {
            foreach (CityStage city in gameCities)
            {
                if (globalPlayerLevel >= city.startFromLevel && globalPlayerLevel <= city.endAtLevel)
                {
                    activeCity = city;
                    return;
                }
            }
            
            // Αν ο παίκτης ξεπεράσει όλα τα προκαθορισμένα επίπεδα, μένει στο Global Empire
            if (gameCities.Count > 0)
            {
                activeCity = gameCities[gameCities.Count - 1];
            }
        }

        public void UpdateGameUI()
        {
            if (activeCity == null) return;

            // 1. Αλλαγή του background με τα landmarks της πόλης (Ακρόπολη, Κολοσσαίο κτλ)
            if (backgroundImageDisplay != null && activeCity.backgroundSprite != null)
            {
                backgroundImageDisplay.sprite = activeCity.backgroundSprite;
            }

            // 2. Δυναμική αλλαγή τίτλων (Start Small vs Global Success)
            if (locationTitleText != null)
            {
                if (globalPlayerLevel >= 45)
                {
                    locationTitleText.text = "BUILD & UPGRADE\nGLOBAL SUCCESS!";
                }
                else
                {
                    locationTitleText.text = "START SMALL\n" + activeCity.cityDisplayName.ToUpper();
                }
            }

            // 3. Ενημέρωση Στόχου και Κουμπιού Play
            if (todaysGoalText != null)
            {
                // Ο στόχος αυξάνεται ελαφρώς σε κάθε level της ίδιας πόλης για πρόκληση
                int calculatedGoal = activeCity.initialTargetGoal + ((globalPlayerLevel - activeCity.startFromLevel) * 25);
                todaysGoalText.text = "€" + calculatedGoal.ToString("N0");
            }

            if (playButtonText != null)
            {
                playButtonText.text = "PLAY\nLEVEL " + globalPlayerLevel.ToString();
            }

            // 4. Ενημέρωση του Top Bar (Day, Gold, Diamonds)
            if (dayUIText != null) dayUIText.text = "DAY " + currentDay.ToString();
            if (goldUIText != null) goldUIText.text = currentGold.ToString("N0");
            if (diamondUIText != null) diamondUIText.text = currentDiamonds.ToString();
        }

        // Καλείται όταν ο παίκτης κερδίζει το Level
        public void WinLevel()
        {
            globalPlayerLevel++;
            currentDay++;
            
            // Παράδειγμα ανταμοιβής
            currentGold += 200; 

            SaveData();
            UpdateActiveCity();
            UpdateGameUI();
        }

        private void SaveData()
        {
            PlayerPrefs.SetInt("GlobalPlayerLevel", globalPlayerLevel);
            PlayerPrefs.SetInt("CurrentDay", currentDay);
            PlayerPrefs.SetInt("CurrentGold", currentGold);
            PlayerPrefs.SetInt("CurrentDiamonds", currentDiamonds);
            PlayerPrefs.Save();
        }

        private void LoadData()
        {
            globalPlayerLevel = PlayerPrefs.GetInt("GlobalPlayerLevel", 1);
            currentDay = PlayerPrefs.GetInt("CurrentDay", 1);
            currentGold = PlayerPrefs.GetInt("CurrentGold", 120);
            currentDiamonds = PlayerPrefs.GetInt("CurrentDiamonds", 5);
        }
    }
}
