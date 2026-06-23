using UnityEngine;
using System.Collections.Generic;

namespace SouvlakiTycoon
{
    public class GameProgressionManager : MonoBehaviour
    {
        public static GameProgressionManager Instance { get; private set; }

        [Header("Economy & Stats")]
        public int currentDay = 1;
        public int currentGold = 120;
        public int currentDiamonds = 5;
        [Range(0, 100)] public int customerSatisfaction = 85;
        public int globalRank = 5000; // Ξεκινάει χαμηλά, ανεβαίνει στην αυτοκρατορία

        [Header("Level & Progress Settings")]
        public int currentLevel = 1;
        public int currentLevelXp = 0;
        public int xpToNextLevel = 100;

        [Header("Daily Goals (Αθήνα 1985 vs Empire)")]
        public int todaysGoldGoal = 150;
        public int dailyGoldEarned = 0;
        public int dailyRewardGold = 200;

        [Header("Customer Spawner Settings")]
        [SerializeField] private GameObject customerPrefab;
        [SerializeField] private Transform[] customerSeatPositions; // Τα τραπέζια/θέσεις για τους πελάτες
        [SerializeField] private Sprite[] availableFoodIcons;     // Τα εικονίδια των σουβλακίων για τις παραγγελίες
        
        private List<GameObject> activeCustomers = new List<GameObject>();
        private bool isDayActive = false;
        private float spawnTimer = 0f;
        public float spawnDelay = 5f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadGameProgress();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Αρχικό update του UI με το που ανοίγει το παιχνίδι
            UpdateUIElements();
        }

        private void Update()
        {
            if (!isDayActive) return;

            // Μηχανισμός Spawner Πελατών
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnDelay)
            {
                spawnTimer = 0f;
                TrySpawnCustomer();
            }
        }

        // --- GAME LOOP CONTROLS ---
        public void StartBusinessDay()
        {
            if (isDayActive) return;

            isDayActive = true;
            dailyGoldEarned = 0;
            spawnTimer = 0f;
            
            Debug.Log($"Ημέρα {currentDay} Ξεκίνησε! Στόχος: {todaysGoldGoal}€");
        }

        public void EndBusinessDay()
        {
            isDayActive = false;
            
            // Καθαρισμός εναπομεινάντων πελατών
            foreach (var customer in activeCustomers)
            {
                if (customer != null) Destroy(customer);
            }
            activeCustomers.Clear();

            // Έλεγχος αν επιτεύχθηκε ο στόχος της ημέρας (Όπως στο Daily Goal Panel)
            if (dailyGoldEarned >= todaysGoldGoal)
            {
                currentGold += dailyRewardGold;
                customerSatisfaction = Mathf.Clamp(customerSatisfaction + 5, 0, 100);
                AddXp(50);
                Debug.Log("Ο στόχος της ημέρας επιτεύχθηκε! Προστέθηκε το Reward.");
            }
            else
            {
                customerSatisfaction = Mathf.Clamp(customerSatisfaction - 10, 0, 100);
                Debug.Log("Ο στόχος χάθηκε. Η ικανοποίηση των πελατών έπεσε.");
            }

            // Προετοιμασία επόμενης μέρας
            currentDay++;
            CalculateNextDayGoals();
            SaveGameProgress();
            UpdateUIElements();
        }

        // --- ECONOMY ENGINE ---
        public void EarnMoney(int goldAmount, int satisfactionImpact)
        {
            currentGold += goldAmount;
            dailyGoldEarned += goldAmount;
            
            // Δυναμικός υπολογισμός του Satisfaction %
            customerSatisfaction = Mathf.Clamp(customerSatisfaction + satisfactionImpact, 0, 100);
            
            // Αν είμαστε σε υψηλό level, κάθε πώληση ανεβάζει το Global Rank
            if (currentLevel >= 10 && globalRank > 1)
            {
                globalRank -= Random.Range(1, 5); // Ανεβαίνει προς το #1
            }

            AddXp(goldAmount / 2);
            UpdateUIElements();
        }

        public void SpendMoney(int goldAmount)
        {
            currentGold -= goldAmount;
            UpdateUIElements();
            SaveGameProgress();
        }

        public void AddDiamonds(int amount)
        {
            currentDiamonds += amount;
            UpdateUIElements();
            SaveGameProgress();
        }

        private void AddXp(int amount)
        {
            currentLevelXp += amount;
            if (currentLevelXp >= xpToNextLevel)
            {
                currentLevelXp -= xpToNextLevel;
                currentLevel++;
                xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
                Debug.Log($"Level Up! Τώρα είσαι Level {currentLevel}");
            }
        }

        private void CalculateNextDayGoals()
        {
            // Κλιμάκωση δυσκολίας ανάλογα με το Level και τη μέρα
            todaysGoldGoal = Mathf.RoundToInt(150 * (1 + (currentDay * 0.1f)) * (1 + (currentLevel * 0.2f)));
            dailyRewardGold = Mathf.RoundToInt(todaysGoldGoal * 1.3f);
        }

        // --- SPAWNER LOGIC ---
        private void TrySpawnCustomer()
        {
            // Εύρεση άδειας θέσης/τραπεζιού
            int availableIndex = GetAvailableSeatIndex();
            
            if (availableIndex == -1 || customerPrefab == null || availableFoodIcons.Length == 0) return;

            // Spawn του πελάτη στη θέση του
            Transform seat = customerSeatPositions[availableIndex];
            GameObject newCustomer = Instantiate(customerPrefab, seat.position, seat.rotation);
            activeCustomers.Add(newCustomer);

            // Setup της τυχαίας παραγγελίας και του UI Bubble
            CustomerOrderUI orderUI = newCustomer.GetComponentInChildren<CustomerOrderUI>();
            if (orderUI != null)
            {
                Sprite randomFood = availableFoodIcons[Random.Range(0, availableFoodIcons.Length)];
                float patienceTime = Mathf.Max(8f, 20f - (currentLevel * 0.5f)); // Πιο δύσκολο σε μεγάλα levels
                orderUI.SetupOrder(randomFood, patienceTime);
            }
        }

        private int GetAvailableSeatIndex()
        {
            // Απλή λογική ελέγχου θέσεων (μπορεί να επεκταθεί με raycasts ή bools στα τραπέζια)
            if (activeCustomers.Count < customerSeatPositions.Length)
            {
                return activeCustomers.Count;
            }
            return -1; // Όλα τα τραπέζια γεμάτα
        }

        // --- UI REFRESH SYNC ---
        private void UpdateUIElements()
        {
            if (GameUIController.Instance != null)
            {
                GameUIController.Instance.UpdateAllPanels();
            }
        }

        // --- DATA PERSISTENCE (SAVE / LOAD) ---
        private void SaveGameProgress()
        {
            PlayerPrefs.SetInt("SavedDay", currentDay);
            PlayerPrefs.SetInt("SavedGold", currentGold);
            PlayerPrefs.SetInt("SavedDiamonds", currentDiamonds);
            PlayerPrefs.SetInt("SavedSatisfaction", customerSatisfaction);
            PlayerPrefs.SetInt("SavedLevel", currentLevel);
            PlayerPrefs.SetInt("SavedXp", currentLevelXp);
            PlayerPrefs.SetInt("SavedRank", globalRank);
            PlayerPrefs.Save();
        }

        private void LoadGameProgress()
        {
            // Default τιμές για το ξεκίνημα (Αθήνα 1985)
            currentDay = PlayerPrefs.GetInt("SavedDay", 1);
            currentGold = PlayerPrefs.GetInt("SavedGold", 120);
            currentDiamonds = PlayerPrefs.GetInt("SavedDiamonds", 5);
            customerSatisfaction = PlayerPrefs.GetInt("SavedSatisfaction", 85);
            currentLevel = PlayerPrefs.GetInt("SavedLevel", 1);
            currentLevelXp = PlayerPrefs.GetInt("SavedXp", 0);
            globalRank = PlayerPrefs.GetInt("SavedRank", 5000);
            
            CalculateNextDayGoals();
        }
    }
}
