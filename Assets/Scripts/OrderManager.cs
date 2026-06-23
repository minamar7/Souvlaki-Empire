using UnityEngine;
using System.Collections.Generic;

namespace SouvlakiTycoon
{
    public class OrderManager : MonoBehaviour
    {
        public static OrderManager Instance { get; private set; }

        [Header("Available Food Sprites")]
        [Tooltip("Σύρε εδώ από τα Assets σου το icon_souvlaki_wrap και το icon_gyro_pita.")]
        public List<Sprite> foodIcons = new List<Sprite>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Δίνει μια τυχαία παραγγελία (Sprite) στον πελάτη όταν κάθεται στο τραπέζι.
        /// </summary>
        public Sprite GetRandomOrder()
        {
            if (foodIcons == null || foodIcons.Count == 0)
            {
                Debug.LogError("OrderManager: Δεν έχεις βάλει τα εικονίδια των φαγητών στον Inspector!");
                return null;
            }

            // Επιλέγει στην τύχη ανάμεσα στο σουβλάκι και τον γύρο
            int randomIndex = Random.Range(0, foodIcons.Count);
            return foodIcons[randomIndex];
        }

        /// <summary>
        /// Ελέγχει αν το φαγητό που ετοίμασε ο παίκτης ταιριάζει με την παραγγελία του πελάτη.
        /// </summary>
        public bool CheckOrderMatch(Sprite activeOrder, string foodType)
        {
            if (activeOrder == null) return false;

            // Απλό check με βάση το όνομα του αρχείου (π.χ. αν περιέχει "wrap" ή "gyro")
            string spriteName = activeOrder.name.ToLower();
            
            if (foodType.ToLower() == "wrap" && spriteName.Contains("wrap")) return true;
            if (foodType.ToLower() == "gyro" && spriteName.Contains("gyro")) return true;

            return false;
        }
    }
}
