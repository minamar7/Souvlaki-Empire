using UnityEngine;
using System.Collections.Generic;

namespace SouvlakiTycoon
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }

        public enum Language { English, Greek }
        public Language currentLanguage = Language.English;

        // Το Dictionary που κρατάει όλες τις μεταφράσεις του παιχνιδιού
        private Dictionary<string, Dictionary<Language, string>> localizedText = new Dictionary<string, Dictionary<Language, string>>();

        private void Awake()
        {
            // Singleton pattern για να έχουμε πρόσβαση από παντού
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeTranslations();
                LoadLanguagePreference();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeTranslations()
        {
            // Εδώ ορίζουμε τα Keys για κάθε UI στοιχείο που βλέπουμε στις φωτογραφίες
            AddTranslation("START_SMALL", "START SMALL", "ΞΕΚΙΝΑ ΤΑΠΕΙΝΑ");
            AddTranslation("GLOBAL_SUCCESS", "GLOBAL SUCCESS!", "ΠΑΓΚΟΣΜΙΑ ΕΠΙΤΥΧΙΑ!");
            AddTranslation("TODAYS_GOAL", "TODAY'S GOAL", "ΣΤΟΧΟΣ ΗΜΕΡΑΣ");
            AddTranslation("REWARD", "REWARD", "ΑΜΟΙΒΗ");
            AddTranslation("PLAY", "PLAY", "ΠΑΙΞΕ");
            AddTranslation("SHOP", "SHOP", "ΜΑΓΑΖΙ");
            AddTranslation("UPGRADES", "UPGRADES", "ΑΝΑΒΑΘΜΙΣΕΙΣ");
            AddTranslation("STAFF", "STAFF", "ΠΡΟΣΩΠΙΚΟ");
            AddTranslation("RECIPES", "RECIPES", "ΣΥΝΤΑΓΕΣ");
            AddTranslation("DAY", "DAY", "ΗΜΕΡΑ");
            AddTranslation("ANGRY_LEAVE", "Too slow! I'm leaving!", "Πολύ αργείς, έφυγα!");
            AddTranslation("CHEF_PROMPT", "Welcome Chef! Enter your name:", "Καλώς όρισες Μάστορα! Γράψε το όνομά σου:");
        }

        private void AddTranslation(string key, string enText, string elText)
        {
            localizedText[key] = new Dictionary<Language, string>
            {
                { Language.English, enText },
                { Language.Greek, elText }
            };
        }

        // Η κύρια συνάρτηση που θα καλούν τα UI Texts για να πάρουν τη μετάφραση
        public string GetTranslatedValue(string key)
        {
            if (!localizedText.ContainsKey(key))
            {
                Debug.LogWarning($"Translation key not found: {key}");
                return key;
            }
            return localizedText[key][currentLanguage];
        }

        // Αλλαγή γλώσσας live από το μενού
        public void SetLanguage(Language lang)
        {
            currentLanguage = lang;
            PlayerPrefs.SetInt("SelectedLanguage", (int)lang);
            PlayerPrefs.Save();

            // Event για να ενημερωθούν αυτόματα όλα τα κείμενα στην οθόνη
            FindObjectsOfType<LocalizedTextComponent>().Length.ToString(); // Trigger update
            UpdateAllLocalizedTexts();
        }

        private void LoadLanguagePreference()
        {
            // Default είναι τα Αγγλικά (0) αν δεν έχει αποθηκευτεί τίποτα
            int savedLang = PlayerPrefs.GetInt("SelectedLanguage", 0);
            currentLanguage = (Language)savedLang;
        }

        public void UpdateAllLocalizedTexts()
        {
            LocalizedTextComponent[] allTexts = FindObjectsOfType<LocalizedTextComponent>();
            foreach (var textComp in allTexts)
            {
                textComp.UpdateText();
            }
        }
    }
}
