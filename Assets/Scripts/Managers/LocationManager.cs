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
            public string locationID;       // "Athens", "NewYork", "Paris", "Tokyo"
            public string bannerTitleKey;   // Key για το αριστερό banner (π.χ. "ATHENS_1985", "NEW_YORK_GLOBAL")
            public Sprite backgroundSprite; // Η φωτογραφία/background της συγκεκριμένης πόλης
            public int unlockLevelRequired; // Σε ποιο επίπεδο ξεκλειδώνει η πόλη
            public float profitMultiplier = 1.0f; // Έξτρα κέρδος λόγω ακριβότερης αγοράς εξωτερικού!
        }

        [Header("Location Settings")]
        public List<Location> locations = new List<Location>();
        public int currentLocationIndex = 0;

        [Header("UI References")]
        [Tooltip("Σύρε εδώ το Image Component του φόντου της σκηνής.")]
        [SerializeField] private Image backgroundImageDisplay;
        
        [Header("Banner Reference")]
        [Tooltip("Σύρε εδώ το LocalizedTextComponent του Top-Left Banner για να αλλάζει το όνομα της πόλης.")]
        [SerializeField] private LocalizedTextComponent bannerCityText;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
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

        /// <summary>
        /// Μεταφέρει τον παίκτη στην επόμενη πόλη αν πληροί τις προϋποθέσεις επιπέδου
        /// </summary>
        public void TravelToLocation(int locationIndex)
        {
            if (locationIndex < 0 || locationIndex >= locations.Count) return;

            // Έλεγχος αν ο παίκτης έχει το απαιτούμενο level (από το GameManager / Progression)
            int playerLevel = 1; 
            if (GameProgressionManager.Instance != null)
            {
                // Υποθέτουμε ότι το level βασίζεται στις ημέρες ή την γενική πρόοδο
                playerLevel = PlayerPrefs.GetInt("PlayerLevel", 1); 
            }

            if (playerLevel >= locations[locationIndex].unlockLevelRequired)
            {
                currentLocationIndex = locationIndex;
                SaveLocationProgress();
                UpdateLocationUI();
                Debug.Log($"Καλώς ήρθατε στο {locations[locationIndex].locationID}!");
            }
            else
            {
                Debug.Log("Δεν έχεις φτάσει ακόμα στο απαιτούμενο επίπεδο για αυτή την πόλη!");
            }
        }

        /// <summary>
        /// Αλλάζει τη φωτογραφία φόντου και το κείμενο του banner live στο UI
        /// </summary>
        public void UpdateLocationUI()
        {
            if (locations.Count == 0 || currentLocationIndex >= locations.Count) return;

            Location currentLoc = locations[currentLocationIndex];

            // Αλλαγή φωτογραφίας πόλης
            if (backgroundImageDisplay != null && currentLoc.backgroundSprite != null)
            {
                backgroundImageDisplay.sprite = currentLoc.backgroundSprite;
            }

            // Αλλαγή κειμένου στο Banner (Athens, New York, κλπ.)
            if (bannerCityText != null)
            {
                bannerCityText.SetKey(currentLoc.bannerTitleKey);
            }
        }

        /// <summary>
        /// Επιστρέφει τον πολλαπλασιαστή κέρδους της τρέχουσας πόλης για να επηρεάζει τις πωλήσεις
        /// </summary>
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
