using UnityEngine;
using System.Collections.Generic;

namespace SouvlakiTycoon
{
    public class RecipeManager : MonoBehaviour
    {
        public static RecipeManager Instance { get; private set; }

        [System.Serializable]
        public class Recipe
        {
            public string recipeID;       // π.χ. "Souvlaki_Wrap", "Gyro_Pita", "Fries"
            public string nameKey;        // Key για το Localization
            public bool isUnlocked = false;
            public int basePrice = 5;     // Πόσα χρυσά νομίσματα δίνει βασικά
            public int unlockCost = 500;  // Κόστος ξεκλειδώματος στο Shop
        }

        [Header("Recipe Book")]
        public List<Recipe> availableRecipes = new List<Recipe>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeDefaultRecipes();
                LoadRecipes();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeDefaultRecipes()
        {
            if (availableRecipes.Count == 0)
            {
                // Το κλασικό σουβλάκι είναι ξεκλειδωμένο από την αρχή (Αθήνα 1985!)
                availableRecipes.Add(new Recipe { recipeID = "Souvlaki_Wrap", nameKey = "RECIPE_SOUVLAKI", isUnlocked = true, basePrice = 5, unlockCost = 0 });
                // Ο γύρος κλειδωμένος, θέλει λεφτά για να μπει στο μενού
                availableRecipes.Add(new Recipe { recipeID = "Gyro_Pita", nameKey = "RECIPE_GYRO", isUnlocked = false, basePrice = 8, unlockCost = 350 });
                // Οι πατάτες κλειδωμένες
                availableRecipes.Add(new Recipe { recipeID = "Fries", nameKey = "RECIPE_FRIES", isUnlocked = false, basePrice = 3, unlockCost = 150 });
            }
        }

        /// <summary>
        /// Υπολογίζει την τελική τιμή πώλησης μαζί με το bonus ποιότητας από το UpgradeManager!
        /// </summary>
        public int GetSellPrice(string recipeID)
        {
            Recipe recipe = availableRecipes.Find(r => r.recipeID == recipeID);
            if (recipe == null) return 0;

            int finalPrice = recipe.basePrice;

            // Αν έχει αναβαθμιστεί η ποιότητα κρέατος, δίνει έξτρα κέρδος (+20% ανά επίπεδο)
            if (UpgradeManager.Instance != null)
            {
                int meatLevel = UpgradeManager.Instance.meatQualityUpgrade.currentLevel;
                finalPrice += Mathf.RoundToInt(recipe.basePrice * (meatLevel * 0.2f));
            }

            return finalPrice;
        }

        /// <summary>
        /// Προσπάθεια ξεκλειδώματος νέας συνταγής από το Shop
        /// </summary>
        public bool TryUnlockRecipe(string recipeID)
        {
            Recipe recipe = availableRecipes.Find(r => r.recipeID == recipeID);
            if (recipe == null || recipe.isUnlocked) return false;

            if (GameProgressionManager.Instance != null && GameProgressionManager.Instance.gold >= recipe.unlockCost)
            {
                GameProgressionManager.Instance.gold -= recipe.unlockCost;
                recipe.isUnlocked = true;
                SaveRecipes();
                Debug.Log($"Ξεκλειδώθηκε η συνταγή: {recipeID}!");
                return true;
            }

            return false;
        }

        private void SaveRecipes()
        {
            foreach (var recipe in availableRecipes)
            {
                PlayerPrefs.SetInt("RecipeUnlock_" + recipe.recipeID, recipe.isUnlocked ? 1 : 0);
            }
            PlayerPrefs.Save();
        }

        private void LoadRecipes()
        {
            foreach (var recipe in availableRecipes)
            {
                if (PlayerPrefs.HasKey("RecipeUnlock_" + recipe.recipeID))
                {
                    recipe.isUnlocked = PlayerPrefs.GetInt("RecipeUnlock_" + recipe.recipeID) == 1;
                }
            }
        }
    }
}
