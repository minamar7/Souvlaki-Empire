using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SouvlakiTycoon
{
    public class LocationManager : MonoBehaviour
    {
        public static LocationManager Instance { get; private set; }

        [System.Serializable]
        public class Location
        {
            public string locationID;       // "Athens", "NewYork", "Paris", "London", "Tokyo", "Sydney", "Rome", "Dubai", "Rio"
            public string bannerTitleKey;   // Key για το αριστερό banner
            public Sprite backgroundSprite; // Η φωτογραφία/background της πόλης
            public int unlockLevelRequired; // Σε ποιο επίπεδο ξεκλειδώνει
            public float profitMultiplier = 1.0f; // Πολλαπλασιατής κέρδους εξωτερικού
        }

        [Header("Location Settings")]
        public List<Location> locations = new List<Location>();
        public int currentLocationIndex = 0;

        [Header("UI References")]
        [Tooltip("Σύρε εδώ το Image Component του φόντου της σκηνής.")]
        [SerializeField] private Image backgroundImageDisplay;
        
        [Header("Banner Reference")]
        [Tooltip("Σύρε εδώ το LocalizedTextComponent του Top-Left Banner.")]
        [SerializeField] private LocalizedTextComponent bannerCityText;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeDefaultLocations();
                LoadLocationProgress();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            UpdateLocationUI();
        }

        private void InitializeDefaultLocations()
        {
            // Αν η λίστα είναι άδεια στον Inspector, τη γεμίζουμε με τη σωστή σειρά εξέλιξης (Progression)
            if (locations.Count == 0)
            {
                locations.Add(new Location { locationID = "Athens", bannerTitleKey = "ATHENS_1985", unlockLevelRequired = 1, profitMultiplier = 1.0f });
                locations.Add(new Location { locationID = "NewYork", bannerTitleKey = "NEW_YORK_GLOBAL", unlockLevelRequired = 5, profitMultiplier = 1.5f });
                locations.Add(new Location { locationID = "Paris", bannerTitleKey = "PARIS_GLOBAL", unlockLevelRequired = 12, profitMultiplier = 2.0f });
                locations.Add(new Location { locationID = "London", bannerTitleKey = "LONDON_GLOBAL", unlockLevelRequired = 20, profitMultiplier = 2.5f });
                locations.Add(new Location { locationID = "Tokyo", bannerTitleKey = "TOKYO_GLOBAL", unlockLevelRequired = 30, profitMultiplier = 3.5f });
                locations.Add(new Location { locationID = "Sydney", bannerTitleKey = "SYDNEY_GLOBAL", unlockLevelRequired = 40, profitMultiplier = 4.5f });
                locations.Add(new Location { locationID = "Rome", bannerTitleKey = "ROME_GLOBAL", unlockLevelRequired = 50, profitMultiplier = 5.5f });
                locations.Add(new Location { locationID = "Dubai", bannerTitleKey = "DUBAI_GLOBAL", unlockLevelRequired = 65, profitMultiplier = 7.0f });
                locations.Add(new Location { locationID = "Rio", bannerTitleKey = "RIO_GLOBAL", unlockLevelRequired = 80, profitMultiplier = 10.0f });
            }
        }

        public void TravelToLocation(int locationIndex)
        {
            if (locationIndex < 0 || locationIndex >= locations.Count) return;

            int playerLevel = PlayerPrefs.GetInt("PlayerLevel", 1); 

            if (playerLevel >= locations[locationIndex].unlockLevelRequired)
            {
                currentLocationIndex = locationIndex;
                SaveLocationProgress();
                UpdateLocationUI();
                Debug.Log($"Ταξίδι στο: {locations[locationIndex].locationID}!");
            }
            else
            {
                Debug.Log("Κλειδωμένη περιοχή! Χρειάζεσαι μεγαλύτερο Level.");
            }
        }

        public void UpdateLocationUI()
        {
            if (locations.Count == 0 || currentLocationIndex >= locations.Count) return;

            Location currentLoc = locations[currentLocationIndex];

            if (backgroundImageDisplay != null && currentLoc.backgroundSprite != null)
            {
                backgroundImageDisplay.sprite = currentLoc.backgroundSprite;
            }

            if (bannerCityText != null)
            {
                bannerCityText.SetKey(currentLoc.bannerTitleKey);
            }
        }

        public float GetCurrentLocationMultiplier()
        {
            if (locations.Count == 0) return 1.0f;
            return locations[currentLocationIndex].profitMultiplier;
        }

        private void SaveLocationProgress()
        {
            PlayerPrefs.SetInt("CurrentLocationIndex", currentLocationIndex);
            PlayerPrefs.Save();
        }

        private void LoadLocationProgress()
        {
            currentLocationIndex = PlayerPrefs.GetInt("CurrentLocationIndex", 0);
        }
    }
}
