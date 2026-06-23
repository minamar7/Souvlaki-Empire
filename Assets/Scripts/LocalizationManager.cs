using UnityEngine;
using System.Collections.Generic;

namespace SouvlakiTycoon
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }

        // Οι 10 γλώσσες βάσει των προδιαγραφών σου
        public enum Language 
        { 
            English, Greek, German, French, Spanish, 
            Italian, Portuguese, Russian, Chinese, Hindi 
        }
        
        public Language currentLanguage = Language.English;

        [Header("UI Flag Reference (Optional)")]
        [SerializeField] private UnityEngine.UI.Image currentFlagImage;
        [SerializeField] private Sprite[] flagSprites; // Βάλε τις σημαίες με την ίδια σειρά του Enum στον Inspector

        private Dictionary<string, Dictionary<Language, string>> localizedText = new Dictionary<string, Dictionary<Language, string>>();

        private void Awake()
        {
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
            // --- ΤΙΤΛΟΙ ΠΡΟΟΔΟΥ (Top Left Banner) ---
            AddTranslation("START_SMALL", "START SMALL", "ΞΕΚΙΝΑ ΤΑΠΕΙΝΑ", "KLEIN ANFANGEN", "COMMENCER PETIT", "EMPEZAR PEQUEÑO", "INIZΙΑ IN PICCOLO", "COMEÇAR PEQUENO", "НАЧНИ С ΜΑΛОΓΟ", "从小做起", "छोटे से शुरुआत");
            AddTranslation("ATHENS_1985", "ATHENS, 1985", "ΑΘΗΝΑ, 1985", "ATHEN, 1985", "ATHÈNES, 1985", "ATENAS, 1985", "ATENE, 1985", "ATENAS, 1985", "АФИНЫ, 1985", "雅典, 1985", "एथेंस, 1985");
            AddTranslation("GLOBAL_SUCCESS", "GLOBAL SUCCESS!", "ΠΑΓΚΟΣΜΙΑ ΕΠΙΤΥΧΙΑ!", "WELTERFOLG!", "SUCCÈS MONDIAL!", "¡ÉXITO GLOBAL!", "SUCCESSO GLOBALE!", "SUCESSO GLOBAL!", "МИРОВОЙ УСПЕХ!", "全球 Αν功!", "वैश्विक सफलता!");

            // --- ΣΤΑΤΙΣΤΙΚΑ & UI (Top Bar) ---
            AddTranslation("DAY", "DAY", "ΗΜΕΡΑ", "TAG", "JOUR", "DÍA", "GIORNO", "DIA", "ДЕНЬ", "天", "दिन");

            // --- ΑΡΙΣΤΕΡΟ ΜΕΝΟΥ (Side Menu) ---
            AddTranslation("SHOP", "SHOP", "ΜΑΓΑΖΙ", "LADEN", "BOUTIQUE", "TIENDA", "NEGOZIO", "LOJA", "МАГАЗИН", "商店", "दुकान");
            AddTranslation("UPGRADES", "UPGRADES", "ΑΝΑΒΑΘΜΙΣΕΙΣ", "VERBESSERUNGEN", "AMÉLIORATIONS", "MEJORAS", "MIGLIORAMENTI", "MELHORIAS", "УЛУЧШЕНИЯ", "升级", "अपग्रेड");
            AddTranslation("STAFF", "STAFF", "ΠΡΟΣΩΠΙΚΟ", "PERSONAL", "PERSONNEL", "PERSONAL", "PERSONALE", "EQUIPE", "ПЕРСОНАЛ", "员工", "कर्मचारी");
            AddTranslation("RECIPES", "RECIPES", "ΣΥΝΤΑΓΕΣ", "REZEPTE", "RECETTES", "RECETAS", "RICETTE", "RECEITAS", "РЕЦЕΠТЫ", "食谱", "व्यंजन विधि");

            // --- ΔΕΞΙ ΠΑΝΕΛ (Gameplay Goals) ---
            AddTranslation("TODAYS_GOAL", "TODAY'S GOAL", "ΣΤΟΧΟΣ ΗΜΕΡΑΣ", "TAGESZIEL", "OBJECTIF DU JOUR", "OBJETIVO DE HOY", "OBIETTIVO DI OGGI", "META DE HOJE", "ЦЕЛЬ ДНЯ", "今日目标", "आज का लक्ष्य");
            AddTranslation("REWARD", "REWARD", "ΑΜΟΙΒΗ", "BELOHNUNG", "RÉCOMPENSE", "RECOMPENSA", "RICOMPENSA", "RECOMPENSA", "НАГРАДА", "奖励", "इनाम");
            AddTranslation("GLOBAL_RANK", "GLOBAL RANK", "ΠΑΓΚΟΣΜΙΑ ΚΑΤΑΤΑΞΗ", "WELTRANGLISTE", "CLASSEMENT MONDIAL", "RANG MUNDIAL", "CLASSIFICA GLOBALE", "CLASSIFICAÇÃO GLOBAL", "МИРОВОЙ РЕЙТИНГ", "全球排名", "वैश्विक रैंक");
            AddTranslation("DAILY_PROFIT", "DAILY PROFIT", "ΚΕΡΔΟΣ ΗΜΕΡΑΣ", "TÄGLICHER GEWINN", "PROFIT JOURNALIER", "GANANCIA DIARIA", "PROFITTO GIORNALIERO", "LUCRO DIÁRIO", "ЕЖЕДНЕВНАЯ ПРИБЫЛЬ", "每日利润", "दैनिक लाभ");
            
            // --- ΚΟΥΜΠΙ PLAY ---
            AddTranslation("PLAY", "PLAY", "ΠΑΙΞΕ", "SPIELEN", "JOUER", "JUGAR", "GIOCA", "JOGAR", "ИГРАТЬ", "玩", "खेलें");
            AddTranslation("LEVEL", "LEVEL", "LEVEL", "LEVEL", "NIVEAU", "NIVEL", "LIVELLO", "NÍVEL", "УРОВЕНЬ", "等级", "स्तर");
        }

        private void AddTranslation(string key, params string[] translations)
        {
            var dict = new Dictionary<Language, string>();
            for (int i = 0; i < translations.Length; i++)
            {
                dict[(Language)i] = translations[i];
            }
            localizedText[key] = dict;
        }

        public string GetTranslatedValue(string key)
        {
            if (!localizedText.ContainsKey(key)) return key;
            return localizedText[key][currentLanguage];
        }

        public void SetLanguage(int langIndex)
        {
            currentLanguage = (Language)langIndex;
            PlayerPrefs.SetInt("SelectedLanguage", langIndex);
            PlayerPrefs.Save();

            UpdateFlagVisual();
            UpdateAllLocalizedTexts();
        }

        private void LoadLanguagePreference()
        {
            // Default 0 = English
            int savedLang = PlayerPrefs.GetInt("SelectedLanguage", 0);
            currentLanguage = (Language)savedLang;
            UpdateFlagVisual();
        }

        private void UpdateFlagVisual()
        {
            // Αν έχεις συνδέσει ένα Image component και έχεις βάλει τα sprites των σημαιών στον Inspector
            if (currentFlagImage != null && flagSprites != null && (int)currentLanguage < flagSprites.Length)
            {
                currentFlagImage.sprite = flagSprites[(int)currentLanguage];
            }
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
