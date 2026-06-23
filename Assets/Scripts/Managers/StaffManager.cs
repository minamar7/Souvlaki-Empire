using UnityEngine;
using System.Collections.Generic;

namespace SouvlakiTycoon
{
    public class StaffManager : MonoBehaviour
    {
        public static StaffManager Instance { get; private set; }

        [System.Serializable]
        public class StaffMember
        {
            public string staffID;        // "Griller_1", "Delivery_1"
            public string nameKey;        // Key για τη μετάφραση του τίτλου (π.χ. "STAFF_GRILLER")
            public bool isHired = false;
            public int hireCost = 1000;   // Κόστος πρόσληψης
            public int salary = 100;      // Μισθός που αφαιρείται στο τέλος κάθε ημέρας
            public float speedBonus = 0.15f; // Πόσο βοηθάνε στην ταχύτητα ή στα έσοδα
        }

        [Header("Staff Roster")]
        public List<StaffMember> availableStaff = new List<StaffMember>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeDefaultStaff();
                LoadStaffState();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeDefaultStaff()
        {
            if (availableStaff.Count == 0)
            {
                // Έμπειρος Ψήστης - Μειώνει τον χρόνο ψησίματος
                availableStaff.Add(new StaffMember { 
                    staffID = "Griller_1", 
                    nameKey = "STAFF_GRILLER", 
                    isHired = false, 
                    hireCost = 800, 
                    salary = 50, 
                    speedBonus = 0.25f 
                });

                // Διανομέας (Delivery) - Αυξάνει τις αυτόματες πωλήσεις ανά δευτερόλεπτο
                availableStaff.Add(new StaffMember { 
                    staffID = "Delivery_1", 
                    nameKey = "STAFF_DELIVERY", 
                    isHired = false, 
                    hireCost = 1200, 
                    salary = 80, 
                    speedBonus = 0.20f 
                });
            }
        }

        /// <summary>
        /// Προσπάθεια πρόσληψης υπαλλήλου από το μενού Staff
        /// </summary>
        public bool TryHireStaff(string staffID)
        {
            StaffMember member = availableStaff.Find(s => s.staffID == staffID);
            if (member == null || member.isHired) return false;

            if (GameProgressionManager.Instance != null && GameProgressionManager.Instance.gold >= member.hireCost)
            {
                GameProgressionManager.Instance.gold -= member.hireCost;
                member.isHired = true;
                SaveStaffState();
                
                // Ενημέρωση του UI αν χρειάζεται
                if (LocalizationManager.Instance != null)
                {
                    LocalizationManager.Instance.UpdateAllLocalizedTexts();
                }
                
                Debug.Log($"Προσλήφθηκε επιτυχώς ο/η: {staffID}!");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Καλείται στο τέλος της ημέρας (από το GameProgressionManager) για να πληρωθούν οι μισθοί
        /// </summary>
        public int PayDailySalaries()
        {
            int totalExpenses = 0;
            foreach (var member in availableStaff)
            {
                if (member.isHired)
                {
                    totalExpenses += member.salary;
                }
            }

            if (GameProgressionManager.Instance != null)
            {
                GameProgressionManager.Instance.gold -= totalExpenses;
                // Εξασφαλίζουμε ότι ο χρυσός δεν θα πάει κάτω από το μηδέν
                if (GameProgressionManager.Instance.gold < 0) 
                    GameProgressionManager.Instance.gold = 0;
            }

            return totalExpenses;
        }

        /// <summary>
        /// Επιστρέφει το συνολικό bonus ταχύτητας από όλους τους ψηστες
        /// </summary>
        public float GetTotalGrillSpeedBonus()
        {
            float bonus = 0f;
            foreach (var member in availableStaff)
            {
                if (member.isHired && member.staffID.Contains("Griller"))
                {
                    bonus += member.speedBonus;
                }
            }
            return bonus;
        }

        private void SaveStaffState()
        {
            foreach (var member in availableStaff)
            {
                PlayerPrefs.SetInt("StaffHired_" + member.staffID, member.isHired ? 1 : 0);
            }
            PlayerPrefs.Save();
        }

        private void LoadStaffState()
        {
            foreach (var member in availableStaff)
            {
                if (PlayerPrefs.HasKey("StaffHired_" + member.staffID))
                {
                    member.isHired = PlayerPrefs.GetInt("StaffHired_" + member.staffID) == 1;
                }
            }
        }
    }
}
