using UnityEngine;
using System.Collections.Generic;

namespace SouvlakiTycoon
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }

        public enum Language 
        { 
            English, Greek, German, French, Spanish, 
            Italian, Portuguese, Russian, Chinese, Hindi 
        }
        
        public Language currentLanguage = Language.English;

        [Header("UI Flag Settings")]
        [Tooltip("Σύρε εδώ το UI Image component που βρίσκεται πάνω δεξιά κοντά στο γρανάζι.")]
        [SerializeField] private UnityEngine.UI.Image currentFlagImage;
        
        [Tooltip("Αυτός ο πίνακας γεμίζει αυτόματα! Μη σέρνεις πράγματα χειροκίνητα.")]
        [SerializeField] private Sprite[] flagSprites = new Sprite[10]; 

        [Header("Asset Pipeline Path")]
        [Tooltip("Το μονοπάτι μέσα στο Project σου όπου έχεις αποθηκεύσει τις 10 σημαίες.")]
        [SerializeField] private string flagsFolderPath = "Sprites/Flags";

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
            // --- ΤΙΤΛΟΙ ΠΡΟΟΔΟΥ (Top Left Banner - Κατάσταση Καντίνας) ---
            AddTranslation("START_SMALL", "START SMALL", "ΞΕΚΙΝΑ ΤΑΠΕΙΝΑ", "KLEIN ANFANGEN", "COMMENCER PETIT", "EMPEZAR PEQUEÑO", "INIZΙΑ IN PICCOLO", "COMEÇAR PEQUENO", "НАЧНИ С МАЛОГО", "从小做起", "छोटे से शुरुआत");
            AddTranslation("ATHENS_1985", "ATHENS, 1985", "ΑΘΗΝΑ, 1985", "ATHEN, 1985", "ATHÈNES, 1985", "ATENAS, 1985", "ATENE, 1985", "ATENAS, 1985", "АФИНЫ, 1985", "雅典, 1985", "एथेंस, 1985");
            
            // --- ΤΙΤΛΟΙ ΠΡΟΟΔΟΥ (Top Left Banner - Κατάσταση Αυτοκρατορίας) ---
            AddTranslation("BUILD_UPGRADE", "BUILD & UPGRADE", "ΧΤΙΣΕ & ΑΝΑΒΑΘΜΙΣΕ", "BAUEN & VERBESSERN", "CONSTRUIRE & AMÉLIORER", "CONSTRUIR Y MEJORAR", "COSTRUISCI E MIGLIORA", "CONSTRUIR & MELHORAR", "СТРОЙ ИЛУЧШАЙ", "建造 & 升级", "बनाएं और अपग्रेड करें");
            AddTranslation("GLOBAL_SUCCESS", "GLOBAL SUCCESS!", "ΠΑΓΚΟΣΜΙΑ ΕΠΙΤΥΧΙΑ!", "WELTERFOLG!", "SUCCÈS MONDIAL!", "¡ÉXITO GLOBAL!", "SUCESSO GLOBALE!", "SUCESSO GLOBAL!", "МИРОВОЙ УСПЕХ!", "全球成功!", "वैश्विक सफलता!");

            // --- ΣΤΑΤΙΣΤΙΚΑ & UI (Top Bar) ---
            AddTranslation("DAY", "DAY", "ΗΜΕΡΑ", "TAG", "JOUR", "DÍA", "GIORNO", "DIA", "ДЕНЬ", "天", "दिन");

            // --- ΑΡΙΣΤΕΡΟ ΜΕΝΟΥ (Side Menu) ---
            AddTranslation("SHOP", "SHOP", "ΜΑΓΑΖΙ", "LADEN", "BOUTIQUE", "TIENDA", "NEGOZIO", "LOJA", "МАГАЗИН", "商店", "दुकान");
            AddTranslation("UPGRADES", "UPGRADES", "ΑΝΑΒΑΘΜΙΣΕΙΣ", "VERBESSERUNGEN", "AMÉLIORATIONS", "MEJORAS", "MIGLIORAMENTI", "MELHORIAS", "УЛУЧШЕНИЯ", "升级", "अपग्रेड");
            AddTranslation("STAFF", "STAFF", "ΠΡΟΣΩΠΙΚΟ", "PERSONAL", "PERSONNEL", "PERSONAL", "PERSONALE", "EQUIPE", "ΠЕРСОНАЛ", "员工", "कर्मचारी");
            AddTranslation("RECIPES", "RECIPES", "ΣΥΝΤΑΓΕΣ", "REZEPTE", "RECETTES", "RECETAS", "RICETTE", "RECEITAS", "РЕЦЕΠТЫ", "食谱", "व्यंजन विधि");

            // --- ΔΕΞΙ ΠΑΝΕΛ (Gameplay Goals & Stats) ---
            AddTranslation("TODAYS_GOAL", "TODAY'S GOAL", "ΣΤΟΧΟΣ ΗΜΕΡΑΣ", "TAGESZIEL", "OBJECTIF DU JOUR", "OBJETIVO DE HOY", "OBIETTIVO DI OGGI", "META DE HOJE", "ЦЕЛЬ ДНЯ", "今日目标", "आज का लक्ष्य");
            AddTranslation("REWARD", "REWARD", "ΑΜΟΙΒΗ", "BELOHNUNG", "RÉCOMPENSE", "RECOMPENSA", "RICOMPENSA", "RECOMPENSA", "НАΓΡΑΔΑ", "奖励", "इनाम");
            AddTranslation("GLOBAL_RANK", "GLOBAL RANK", "ΠΑΓΚΟΣΜΙΑ ΚΑΤΑΤΑΞΗ", "WELTRANGLISTE", "CLASSEMENT MONDIAL", "RANG MUNDIAL", "CLASSIFICA GLOBALE", "CLASSIFICAÇÃO GLOBAL", "МИРОВОЙ ΡΕЙТИНГ", "全球排名", "वैश्विक रैंक");
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
            int savedLang = PlayerPrefs.GetInt("SelectedLanguage", 0);
            currentLanguage = (Language)savedLang;
            UpdateFlagVisual();
        }

        private void UpdateFlagVisual()
        {
            if (currentFlagImage != null && flagSprites != null && (int)currentLanguage < flagSprites.Length)
            {
                if (flagSprites[(int)currentLanguage] != null)
                {
                    currentFlagImage.sprite = flagSprites[(int)currentLanguage];
                }
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

        // --- AUTOMATION ENGINE FOR UNITY EDITOR ---
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (flagSprites == null || flagSprites.Length != 10)
            {
                flagSprites = new Sprite[10];
            }
            AutoLoadSpritesFromProject();
        }

        [ContextMenu("Force Reload Flags")]
        public void AutoLoadSpritesFromProject()
        {
            string[] languageNames = System.Enum.GetNames(typeof(Language));
            bool changesMade = false;

            for (int i = 0; i < languageNames.Length; i++)
            {
                if (flagSprites[i] == null)
                {
                    // Ψάχνει στο Resources ή απευθείας με το όνομα του αρχείου στο path
                    string fullPath = flagsFolderPath + "/" + languageNames[i];
                    Sprite loadedSprite = Resources.Load<Sprite>(fullPath);

                    if (loadedSprite == null)
                    {
                        // Εναλλακτικό check αν οι σημαίες σου είναι χύμα στο Resources με μικρά γράμματα
                        loadedSprite = Resources.Load<Sprite>(flagsFolderPath + "/" + languageNames[i].ToLower());
                    }

                    if (loadedSprite != null)
                    {
                        flagSprites[i] = loadedSprite;
                        changesMade = true;
                    }
                }
            }

            if (changesMade)
            {
                Debug.Log("<color=#00FF00><b>[LocalizationManager]</b></color> Οι σημαίες φορτώθηκαν και ταξινομήθηκαν αυτόματα βάσει του Enum!");
            }
        }
#endif
    }
}
