using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SouvlakiTycoon
{
    public class GrillStation : MonoBehaviour
    {
        public Ingredient ingredientToCook; // Τι ψήνεται εδώ (π.χ. GiroXoirino ή PizzaDough)
        public float cookTime = 4f;         // Πόσα δευτερόλεπτα θέλει για να ψηθεί
        public float burnTime = 8f;         // Πότε καίγεται και αχρηστεύεται

        public Slider progressSlider;       // Η μπάρα ψησίματος πάνω από την ψησταριά
        public SpriteRenderer foodRenderer; // Το visual του φαγητού (Ωμό -> Ψημένο -> Καμένο)
        
        [Header("Visual States")]
        public Sprite rawSprite;
        public Sprite cookedSprite;
        public Sprite burnedSprite;

        private enum CookState { Empty, Cooking, Cooked, Burned }
        private CookState currentState = CookState.Empty;
        private float timer = 0f;

        void Start()
        {
            if (progressSlider != null) progressSlider.gameObject.SetActive(false);
            UpdateVisual();
        }

        // Καλείται όταν ο παίκτης βάζει ωμό κρέας/υλικό στην ψησταριά
        public void StartCooking()
        {
            if (currentState != CookState.Empty) return;

            currentState = CookState.Cooking;
            timer = 0f;
            
            if (progressSlider != null)
            {
                progressSlider.gameObject.SetActive(true);
                progressSlider.maxValue = burnTime;
                progressSlider.value = 0f;
            }
            
            UpdateVisual();
        }

        void Update()
        {
            if (currentState == CookState.Empty) return;

            timer += Time.deltaTime;
            if (progressSlider != null) progressSlider.value = timer;

            // Έλεγχος κατάστασης ψησίματος
            if (currentState == CookState.Cooking && timer >= cookTime)
            {
                currentState = CookState.Cooked;
                UpdateVisual();
                Debug.Log("Το υλικό ψήθηκε στην εντέλεια!");
            }
            else if (currentState == CookState.Cooked && timer >= burnTime)
            {
                currentState = CookState.Burned;
                UpdateVisual();
                Debug.Log("Μάστορα, μας κάηκε το κρέας!");
            }
        }

        // Καλείται όταν ο παίκτης κάνει click/tap για να μαζέψει το υλικό
        public void CollectFood(System.Collections.Generic.List<Ingredient> playerPlate)
        {
            if (currentState == CookState.Cooked)
            {
                playerPlate.Add(ingredientToCook); // Το βάζει στο πιάτο/τυλιχτό του
                ResetStation();
            }
            else if (currentState == CookState.Burned)
            {
                // Το πετάει στα σκουπίδια
                ResetStation();
                Debug.Log("Πετάχτηκε καμένο υλικό.");
            }
        }

        void ResetStation()
        {
            currentState = CookState.Empty;
            timer = 0f;
            if (progressSlider != null) progressSlider.gameObject.SetActive(false);
            UpdateVisual();
        }

        void UpdateVisual()
        {
            if (foodRenderer == null) return;

            switch (currentState)
            {
                case CookState.Empty:
                    foodRenderer.sprite = null;
                    break;
                case CookState.Cooking:
                    foodRenderer.sprite = rawSprite;
                    break;
                case CookState.Cooked:
                    foodRenderer.sprite = cookedSprite;
                    break;
                case CookState.Burned:
                    foodRenderer.sprite = burnedSprite;
                    break;
            }
        }
    }
}
