using UnityEngine;
using System.Collections;

namespace SouvlakiTycoon
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Gameplay Settings")]
        [Tooltip("Πόσα δευτερόλεπτα κρατάει μια ημέρα στο παιχνίδι.")]
        [SerializeField] private float dayDuration = 60f;
        private float currentDayTimer = 0f;
        private bool isDayActive = false;

        [Header("Current Shift Stats")]
        public int souvlakisSoldToday = 0;
        public int moneyEarnedToday = 0;

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

        private void Start()
        {
            // Ξεκινάμε αυτόματα την πρώτη ημέρα για δοκιμή
            StartNewDay();
        }

        private void Update()
        {
            if (!isDayActive) return;

            // Τρέξιμο του χρονομέτρου της ημέρας
            currentDayTimer -= Time.deltaTime;

            // Αυτοματοποιημένες πωλήσεις από το Staff (π.χ. Delivery) ανά δευτερόλεπτο
            HandleStaffAutomation();

            if (currentDayTimer <= 0)
            {
                EndCurrentDay();
            }
        }

        public void StartNewDay()
        {
            currentDayTimer = dayDuration;
            souvlakisSoldToday = 0;
            moneyEarnedToday = 0;
            isDayActive = true;
            Debug.Log("Η ημέρα ξεκίνησε! Ψήστε και πουλήστε!");
        }

        /// <summary>
        /// Χειροκίνητο ή αυτόματο πούλημα σουβλακίου όταν ο παίκτης ολοκληρώνει μια παραγγελία.
        /// Υπολογίζει αυτόματα το profit multiplier ανάλογα με την τρέχουσα χώρα/πόλη!
        /// </summary>
        public void SellSouvlaki(string recipeID)
        {
            if (!isDayActive) return;

            if (RecipeManager.Instance != null)
            {
                // Βασική τιμή συνταγής
                int basePrice = RecipeManager.Instance.GetSellPrice(recipeID);
                
                // Πολλαπλασιαστής χώρας (αν υπάρχει LocationManager, αλλιώς 1.0f)
                float locationMultiplier = 1.0f;
                if (LocationManager.Instance != null)
                {
                    locationMultiplier = LocationManager.Instance.GetCurrentLocationMultiplier();
                }

                // Τελική στρογγυλοποιημένη τιμή χρυσού
                int finalPrice = Mathf.RoundToInt(basePrice * locationMultiplier);
                
                if (GameProgressionManager.Instance != null)
                {
                    GameProgressionManager.Instance.gold += finalPrice;
                    moneyEarnedToday += finalPrice;
                    souvlakisSoldToday++;
                    
                    Debug.Log($"Πουλήθηκε {recipeID} στην τρέχουσα τοποθεσία (x{locationMultiplier}) για {finalPrice} χρυσά!");
                }
            }
        }

        private void HandleStaffAutomation()
        {
            // Ασφαλής έλεγχος αν υπάρχει StaffManager και αν το roster έχει γεμίσει
            if (StaffManager.Instance != null && StaffManager.Instance.availableStaff != null)
            {
                var deliveryStaff = StaffManager.Instance.availableStaff.Find(s => s.staffID == "Delivery_1");
                
                // Αν βρεθεί ο διανομέας και είναι προσληφθείς
                if (deliveryStaff != null && deliveryStaff.isHired)
                {
                    // 1% πιθανότητα ανά frame να γίνει αυτόματη πώληση standard wrap λόγω delivery
                    if (Random.value < 0.01f)
                    {
                        SellSouvlaki("Souvlaki_Wrap");
                    }
                }
            }
        }

        public void EndCurrentDay()
        {
            isDayActive = false;
            Debug.Log($"Η ημέρα τελείωσε! Πουλήθηκαν: {souvlakisSoldToday} σουβλάκια. Έσοδα: {moneyEarnedToday} χρυσά.");

            // Πληρωμή μισθών στο προσωπικό
            int expenses = 0;
            if (StaffManager.Instance != null)
            {
                expenses = StaffManager.Instance.PayDailySalaries();
                Debug.Log($"Πληρώθηκαν {expenses} χρυσά για μισθούς προσωπικού.");
            }

            // Προώθηση στην επόμενη ημέρα
            if (GameProgressionManager.Instance != null)
            {
                GameProgressionManager.Instance.IncrementDay();
            }

            // Εδώ μπορείς να ανοίξεις το "End of Day UI Panel" δείχνοντας τα στατιστικά
        }

        public float GetRemainingDayPercentage()
        {
            return Mathf.Clamp01(currentDayTimer / dayDuration);
        }
    }
}
