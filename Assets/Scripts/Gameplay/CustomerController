using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SouvlakiTycoon
{
    public class CustomerController : MonoBehaviour
    {
        [Header("Order Settings")]
        public List<Ingredient> customerOrder = new List<Ingredient>();
        public float maxPatience = 25f;
        private float currentPatience;

        [Header("UI Components")]
        public Slider patienceSlider; // Το πράσινο slider από το 3379.png
        public Image orderIconDisplay; // Το εικονίδιο της παραγγελίας (Σουβλάκι, Πίτσα, Σούσι)
        public GameObject checkmarkIcon; // Το πράσινο checkmark όταν παραδώσεις σωστά

        private CityProgressionManager progressionManager;
        private bool isOrderServed = false;

        void Start()
        {
            progressionManager = FindObjectOfType<CityProgressionManager>();
            currentPatience = maxPatience;

            if (patienceSlider != null)
            {
                patienceSlider.maxValue = maxPatience;
                patienceSlider.value = maxPatience;
            }

            GenerateOrderBasedOnCity();
        }

        void Update()
        {
            if (isOrderServed) return;

            // Μείωση υπομονής με το χρόνο
            currentPatience -= Time.deltaTime;
            if (patienceSlider != null)
            {
                patienceSlider.value = currentPatience;
                
                // Αλλαγή χρώματος μπάρας (Πράσινο -> Κίτρινο -> Κόκκινο)
                UpdateSliderColor();
            }

            if (currentPatience <= 0)
            {
                CustomerAngryLeave();
            }
        }

        void GenerateOrderBasedOnCity()
        {
            // Εδώ διαβάζουμε το Level/Πόλη από τον ProgressionManager
            int currentLevel = progressionManager != null ? progressionManager.globalPlayerLevel : 1;

            if (currentLevel <= 15) // ΑΘΗΝΑ
            {
                customerOrder.Add(Ingredient.Pita);
                customerOrder.Add(Ingredient.GiroXoirino);
                customerOrder.Add(Ingredient.Ntomata);
                // TODO: Set Sprite για Σουβλάκι στο orderIconDisplay
            }
            else if (currentLevel > 15 && currentLevel <= 30) // ΙΤΑΛΙΑ
            {
                customerOrder.Add(Ingredient.PizzaDough);
                customerOrder.Add(Ingredient.Mozzarella);
                // TODO: Set Sprite για Πίτσα
            }
            else // ΤΟΚΙΟ
            {
                customerOrder.Add(Ingredient.NoriSheet);
                customerOrder.Add(Ingredient.SushiRice);
                // TODO: Set Sprite για Σούσι
            }
        }

        void UpdateSliderColor()
        {
            // Απλή αλλαγή χρώματος ανάλογα με το ποσοστό υπομονής
            var fillImage = patienceSlider.fillRect.GetComponent<Image>();
            float ratio = currentPatience / maxPatience;

            if (ratio > 0.5f) fillImage.color = Color.green;
            else if (ratio > 0.2f) fillImage.color = Color.yellow;
            else fillImage.color = Color.red;
        }

        public void ServeCustomer(List<Ingredient> playerIngredients)
        {
            bool isCorrect = CheckRecipe(playerIngredients);

            if (isCorrect)
            {
                isOrderServed = true;
                if (checkmarkIcon != null) checkmarkIcon.SetActive(true);
                
                // Δίνουμε λεφτά στον παίκτη και κλείνουμε το level αν πιάσει το goal
                progressionManager.currentGold += 150;
                progressionManager.UpdateGameUI();
                
                Destroy(gameObject, 1.0f); // Φεύγει χαρούμενος
            }
            else
            {
                // Λάθος υλικά, χάνει χρόνο υπομονής ως "ποινή"
                currentPatience -= 5f; 
                Debug.Log("Μάστορα, άλλο ζήτησα!");
            }
        }

        bool CheckRecipe(List<Ingredient> playerIngredients)
        {
            if (customerOrder.Count != playerIngredients.Count) return false;
            foreach (var ing in customerOrder)
            {
                if (!playerIngredients.Contains(ing)) return false;
            }
            return true;
        }

        void CustomerAngryLeave()
        {
            isOrderServed = true;
            Debug.Log("Πολύ αργείς, έφυγα!");
            // TODO: Μείωση του Satisfaction % στο Top Bar
            Destroy(gameObject);
        }
    }
}
