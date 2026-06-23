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
            public string locationID;             // "Athens", "NewYork", "Paris", "London", "Tokyo", "Sydney", "Rome", "Dubai", "Rio"
            public string bannerTitleKey;         // Key για το LocalizedTextComponent του Banner
            public Sprite backgroundSprite;       // Το φόντο της εκάστοτε πόλης
            public int unlockLevelRequired;       // Απαιτούμενο επίπεδο παίκτη για ξεκλείδωμα
            public float profitMultiplier = 1.0f; // Πολλαπλασιατής εσόδων λόγω τοποθεσίας
        }

        [Header("Location Configuration")]
        [Tooltip("Μπορείς να προσθέσεις ή να επεξεργαστείς τις πόλεις και τα Sprites τους απευθείας από τον Inspector.")]
        public List<Location> locations = new List<Location>();
        public int currentLocationIndex = 0;

        [Header("UI Render References")]
        [Tooltip("Σύρε εδώ το UI Image Component που δείχνει το φόντο του παιχνιδιού.")]
        [SerializeField] private Image backgroundImageDisplay;
        
        [Header("Banner Reference")]
        [Tooltip("Σύρε εδώ το LocalizedTextComponent του Top-Left Banner για να αλλάζει ο τίτλος της πόλης.")]
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

        /// <summary>
        /// Γεμίζει τη λίστα με τις default ρυθμίσεις αν είναι άδεια, 
        /// χωρίς όμως να υπερκαλύπτει τις αλλαγές που κάνεις στον Inspector.
        /// </summary>
        private void InitializeDefaultLocations()
        {
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

        /// <summary>
        /// Διαχειρίζεται τη μετακίνηση/ταξίδι του παίκτη σε μια νέα πόλη
        /// </summary>
        public void TravelToLocation(int locationIndex)
        {
            if (locationIndex < 0 || locationIndex >= locations.Count) return;

            // Τραβάμε το τρέχον επίπεδο του παίκτη (χρησιμοποιώντας το progression save)
            int playerLevel = PlayerPrefs.GetInt("PlayerLevel", 1); 

            if (playerLevel >= locations[locationIndex].unlockLevelRequired)
            {
                currentLocationIndex = locationIndex;
                SaveLocationProgress();
                UpdateLocationUI();
                
                // Αν υπάρχει GameManager, κάνουμε trigger μια ανανέωση της σκηνής ή των εσόδων
                if (GameManager.Instance != null)
                {
                    Debug.Log($"[LocationManager]: Επιτυχές ταξίδι στο {locations[locationIndex].locationID}!");
                }
            }
            else
            {
                Debug.LogWarning($"[LocationManager]: Η περιοχή {locations[locationIndex].locationID} είναι κλειδωμένη! Απαιτείται Level {locations[locationIndex].unlockLevelRequired}.");
            }
        }

        /// <summary>
        /// Ενημερώνει live τα γραφικά στοιχεία και τα κείμενα στη σκηνή
        /// </summary>
        public void UpdateLocationUI()
        {
            if (locations.Count == 0 || currentLocationIndex >= locations.Count) return;

            Location currentLoc = locations[currentLocationIndex];

            // Αλλαγή του Sprite στο Background Image
            if (backgroundImageDisplay != null)
            {
                if (currentLoc.backgroundSprite != null)
                {
                    backgroundImageDisplay.sprite = currentLoc.backgroundSprite;
                }
                else
                {
                    Debug.LogWarning($"[LocationManager]: Λείπει το background sprite για την πόλη: {currentLoc.locationID}");
                }
            }

            // Αλλαγή του Localization Key στο UI Banner
            if (bannerCityText != null)
            {
                bannerCityText.SetKey(currentLoc.bannerTitleKey);
            }
        }

        /// <summary>
        /// Επιστρέφει τον πολλαπλασιαστή κέρδους της τρέχουσας πόλης.
        /// Χρησιμοποίησέ τον στο GameManager κατά τον υπολογισμό της τιμής πώλησης.
        /// </summary>
        public float GetCurrentLocationMultiplier()
        {
            if (locations.Count == 0 || currentLocationIndex >= locations.Count) return 1.0f;
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
