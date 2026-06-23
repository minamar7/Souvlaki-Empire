using UnityEngine;
using UnityEngine.UI;

namespace SouvlakiTycoon
{
    public class CustomerOrderUI : MonoBehaviour
    {
        [Header("UI Elements (Speech Bubble)")]
        [SerializeField] private GameObject bubblePanel; // Το συννεφάκι
        [SerializeField] private Image foodIconImage;     // Το εικονίδιο του σουβλακίου
        [SerializeField] private Image timerCircleImage;  // Το κυκλικό ρολόι (Filled Image)
        [SerializeField] private Slider patienceSlider;   // Η πράσινη μπάρα στο κάτω μέρος
        [SerializeField] private GameObject checkmarkIcon; // Το πράσινο Checkmark αν ολοκληρωθεί

        [Header("Order Settings")]
        public float totalPatienceTime = 15f;
        private float currentPatienceTime;
        private bool isOrderActive = false;

        public void SetupOrder(Sprite foodSprite, float patienceDuration)
        {
            foodIconImage.sprite = foodSprite;
            totalPatienceTime = patienceDuration;
            currentPatienceTime = totalPatienceTime;
            
            // Αρχικό Setup βάσει του design της εικόνας 3379_5.png
            patienceSlider.maxValue = totalPatienceTime;
            patienceSlider.value = totalPatienceTime;
            timerCircleImage.fillAmount = 1f;
            
            checkmarkIcon.SetActive(false);
            bubblePanel.SetActive(true);
            isOrderActive = true;
        }

        private void Update()
        {
            if (!isOrderActive) return;

            // Μείωση χρόνου υπομονής
            currentPatienceTime -= Time.deltaTime;
            
            // Ενημέρωση των UI στοιχείων (Μπάρα και Κυκλικό Ρολόι)
            patienceSlider.value = currentPatienceTime;
            timerCircleImage.fillAmount = currentPatienceTime / totalPatienceTime;

            // Αλλαγή χρώματος μπάρας αν τελειώνει ο χρόνος (Από πράσινο σε κόκκινο)
            patienceSlider.fillRect.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, currentPatienceTime / totalPatienceTime);

            if (currentPatienceTime <= 0)
            {
                OrderFailed();
            }
        }

        public void CompleteOrder()
        {
            isOrderActive = false;
            checkmarkIcon.SetActive(true); // Εμφανίζει το πράσινο check όπως στο screenshot
            patienceSlider.gameObject.SetActive(false);
            
            // Μικρό animation/καθυστέρηση πριν εξαφανιστεί ο πελάτης
            Invoke(nameof(HideBubble), 1f);
        }

        private void OrderFailed()
        {
            isOrderActive = false;
            Debug.Log("Ο πελάτης έχασε την υπομονή του και έφυγε!");
            HideBubble();
        }

        private void HideBubble()
        {
            bubblePanel.SetActive(false);
        }
    }
}
