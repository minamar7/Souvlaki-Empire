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
        [Tooltip("Σύρε εδώ το UI Image component της σημαίας πάνω δεξιά.")]
        [SerializeField] private UnityEngine.UI.Image currentFlagImage;
        
        // Ο πίνακας θα γεμίσει αυτόματα στο Awake με τις σημαίες που θα φτιάξει ο κώδικας!
        private Sprite[] flagSprites = new Sprite[10]; 

        private Dictionary<string, Dictionary<Language, string>> localizedText = new Dictionary<string, Dictionary<Language, string>>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                // Παραγωγή των 10 σημαιών live με κώδικα!
                GenerateProceduralFlags();
                
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
            AddTranslation("START_SMALL", "START SMALL", "ΞΕΚΙΝΑ ΤΑΠΕΙΝΑ", "KLEIN ANFANGEN", "COMMENCER PETIT", "EMPEZAR PEQUEÑO", "INIZIA IN PICCOLO", "COMEÇAR PEQUENO", "НАЧНИ С ΜΑΛΟГО", "从小做起", "छोटे से शुरुआत");
            AddTranslation("ATHENS_1985", "ATHENS, 1985", "ΑΘΗΝΑ, 1985", "ATHEN, 1985", "ATHÈNES, 1985", "ATENAS, 1985", "ATENE, 1985", "ATENAS, 1985", "АФИНЫ, 1985", "雅典, 1985", "एθेंस, 1985");
            
            // --- ΤΙΤΛΟΙ ΠΡΟΟΔΟΥ (Top Left Banner - Κατάσταση Αυτοκρατορίας) ---
            AddTranslation("BUILD_UPGRADE", "BUILD & UPGRADE", "ΧΤΙΣΕ & ΑΝΑΒΑΘΜΙΣΕ", "BAUEN & VERBESSERN", "CONSTRUIRE & AMÉLIORER", "CONSTRUIR Y MEJORAR", "COSTRUISCI E MIGLIORA", "CONSTRUIR & MELHORAR", "СТРОЙ И УΛΥЧШАЙ", "建造 & 升级", "बनाएं और अपग्रेड करें");
            AddTranslation("GLOBAL_SUCCESS", "GLOBAL SUCCESS!", "ΠΑΓΚΟΣΜΙΑ ΕΠΙΤΥΧΙΑ!", "WELTERFOLG!", "SUCCÈS MONDIAL!", "¡ÉXITO GLOBAL!", "SUCESSO GLOBALE!", "SUCESSO GLOBAL!", "МИРОВОЙ УСПЕХ!", "全球成功!", "वैश्विक सफलता!");

            // --- ΣΤΑΤΙΣΤΙΚΑ & UI (Top Bar) ---
            AddTranslation("DAY", "DAY", "ΗΜΕΡΑ", "TAG", "JOUR", "DÍA", "GIORNO", "DIA", "ДЕНЬ", "天", "दिन");

            // --- ΑΡΙΣΤΕΡΟ ΜΕΝΟΥ (Side Menu) ---
            AddTranslation("SHOP", "SHOP", "ΜΑΓΑΖΙ", "LADEN", "BOUTIQUE", "TIENDA", "NEGOZIO", "LOJA", "ΜΑΓΑЗИН", "商店", "दुकान");
            AddTranslation("UPGRADES", "UPGRADES", "ΑΝΑΒΑΘΜΙΣΕΙΣ", "VERBESSERUNGEN", "AMÉLIORATIONS", "MEJORAS", "MIGLIORAMENTI", "MELHORIAS", "УΛΥЧШЕНИЯ", "升级", "अपग्रेड");
            AddTranslation("STAFF", "STAFF", "ΠΡΟΣΩΠΙΚΟ", "PERSONAL", "PERSONNEL", "PERSONAL", "PERSONALE", "EQUIPE", "ΠЕРСОНАЛ", "员工", "कर्मचारी");
            AddTranslation("RECIPES", "RECIPES", "ΣΥΝΤΑΓΕΣ", "REZEPTE", "RECETTES", "RECETAS", "RICETTE", "RECEITAS", "РЕЦЕΠΤЫ", "食谱", "व्यंजन विधि");

            // --- ΔΕΞΙ ΠΑΝΕΛ (Gameplay Goals & Stats) ---
            AddTranslation("TODAYS_GOAL", "TODAY'S GOAL", "ΣΤΟΧΟΣ ΗΜΕΡΑΣ", "TAGESZIEL", "OBJECTIF DU JOUR", "OBJETIVO DE HOY", "OBIETTIVO DI OGGI", "META DE HOJE", "ЦЕЛЬ ДНЯ", "今日目标", "आज का लक्ष्य");
            AddTranslation("REWARD", "REWARD", "ΑΜΟΙΒΗ", "BELOHNUNG", "RÉCOMPENSE", "RECOMPENSA", "RICOMPENSA", "RECOMPENSA", "НАΓΡΑΔΑ", "奖励", "इनाम");
            AddTranslation("GLOBAL_RANK", "GLOBAL RANK", "ΠΑΓΚΟΣΜΙΑ ΚΑΤΑΤΑΞΗ", "WELTRANGLISTE", "CLASSEMENT MONDIAL", "RANG MUNDIAL", "CLASSIFICA GLOBALE", "CLASSIFICAÇÃO GLOBAL", "МИРОВОЙ ΡΕЙΤИНΓ", "全球排名", "वैश्विक रैंक");
            AddTranslation("DAILY_PROFIT", "DAILY PROFIT", "ΚΕΡΔΟΣ ΗΜΕΡΑΣ", "TÄGLICHER GEWINN", "PROFIT JOURNALIER", "GANANCIA DIARIA", "PROFITTO GIORNALIERO", "LUCRO DIÁRIO", "ЕЖЕДНЕВНАЯ ΠРИБЫЛЬ", "每日利润", "दैनिक लाभ");
            
            // --- ΚΟΥΜΠΙ PLAY & LEVEL ---
            AddTranslation("PLAY", "PLAY", "ΠΑΙΞΕ", "SPIELEN", "JOUER", "JUGAR", "GIOCA", "JOGAR", "ИГРАТЬ", "玩", "खेलें");
            AddTranslation("LEVEL", "LEVEL", "LEVEL", "LEVEL", "NIVEAU", "NIVEL", "LIVELLO", "NÍVEL", "УРОВЕНЬ", "等级", "स्तर");

            // --- ΜΕΝΟΥ ΑΝΑΒΑΘΜΙΣΕΩΝ & SHOP (Upgrades & Shop UI) ---
            AddTranslation("UPGRADE_GRILL", "SPEEDY GRILL", "ΓΡΗΓΟΡΗ ΨΗΣΤΑΡΙΑ", "SCHNELLER GRILL", "GRIL RAPIDE", "PARRILLA RÁPIDA", "GRIGLIA RAPIDA", "GRELHADOR RÁPIDO", "БЫСТРЫЙ ГРИЛЬ", "快速烤架", "तेज ग्रिल");
            AddTranslation("UPGRADE_MEAT", "MEAT QUALITY", "ΠΟΙΟΤΗΤΑ ΚΡΕΑΤΟΣ", "FLEISCHQUALITÄT", "QUALITÉ DE VIANDE", "CALIDAD DE CARNE", "QUALÀ DELLA CARNE", "QUALIDADE DA CARNE", "КАЧЕСТВО МЯСΑ", "肉质", "मांस की गुणवत्ता");
            AddTranslation("COST", "COST", "ΚΟΣΤΟΣ", "KOSTEN", "COÛT", "COSTO", "COSTO", "CUSTO", "СТОИМОСТЬ", "费用", "लागत");
            AddTranslation("MAX_LEVEL", "MAX LEVEL!", "ΜΕΓΙΣΤΟ ΕΠΙΠΕΔΟ!", "MAXIMALES LEVEL!", "NIVEAU MAX!", "¡NIVEL MÁXIMO!", "LIVELLO MASSIMO!", "NÍVEL MÁXIMO!", "МАКС. УРОВЕНЬ!", "最高等级!", "अधिकतम स्तर!");
            AddTranslation("NOT_ENOUGH_GOLD", "NEED MORE GOLD!", "ΧΡΕΙΑΖΕΣΑΙ ΧΡΥΣΟ!", "MEHR GOLD NÖTIG!", "PLUS D'OR REQUIS!", "¡NECESITAS MÁS ORO!", "SERVE PIÙ ORO!", "MAIS OURO NECESSÁRIO!", "НУЖНО БОЛЬШЕ ЗΟΛΟΤΑ!", "需要 更多黄金!", "और सोना चाहिए!");
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

        // --- ENGINE ΠΑΡΑΓΩΓΗΣ ΣΗΜΑΙΩΝ ΜΕ ΚΩΔΙΚΑ ---
        private void GenerateProceduralFlags()
        {
            int w = 64;
            int h = 42;

            for (int i = 0; i < 10; i++)
            {
                Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
                tex.filterMode = FilterMode.Point;
                Language lang = (Language)i;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        Color c = Color.white;
                        float normX = (float)x / w;
                        float normY = (float)y / h;

                        switch (lang)
                        {
                            case Language.English: // UK (Σταυρός)
                                c = (x > w/2 - 4 && x < w/2 + 4) || (y > h/2 - 3 && y < h/2 + 3) ? Color.red : new Color(0f, 0.15f, 0.5f);
                                break;
                            case Language.Greek: // Greece (Ρίγες & Σταυρός)
                                if (x < 24 && y > h - 19) {
                                    c = (x > 9 && x < 15) || (y > h - 11 && y < h - 7) ? Color.white : new Color(0f, 0.4f, 0.8f);
                                } else {
                                    c = (Mathf.FloorToInt(normY * 9) % 2 == 0) ? Color.white : new Color(0f, 0.4f, 0.8f);
                                }
                                break;
                            case Language.German: // Germany (Μαύρο, Κόκκινο, Κίτρινο)
                                if (normY > 0.66f) c = Color.black;
                                else if (normY > 0.33f) c = Color.red;
                                else c = new Color(1f, 0.8f, 0f);
                                break;
                            case Language.French: // France (Μπλε, Λευκό, Κόκκινο)
                                if (normX < 0.33f) c = new Color(0f, 0.15f, 0.5f);
                                else if (normX < 0.66f) c = Color.white;
                                else c = Color.red;
                                break;
                            case Language.Spanish: // Spain (Κόκκινο, Κίτρινο, Κόκκινο)
                                c = (normY > 0.25f && normY < 0.75f) ? new Color(1f, 0.8f, 0f) : Color.red;
                                break;
                            case Language.Italian: // Italy (Πράσινο, Λευκό, Κόκκινο)
                                if (normX < 0.33f) c = new Color(0f, 0.5f, 0.2f);
                                else if (normX < 0.66f) c = Color.white;
                                else c = Color.red;
                                break;
                            case Language.Portuguese: // Portugal (Πράσινο, Κόκκινο)
                                c = (normX < 0.4f) ? new Color(0f, 0.4f, 0.15f) : Color.red;
                                break;
                            case Language.Russian: // Russia (Λευκό, Μπλε, Κόκκινο)
                                if (normY > 0.66f) c = Color.white;
                                else if (normY > 0.33f) c = new Color(0f, 0.2f, 0.7f);
                                else c = Color.red;
                                break;
                            case Language.Chinese: // China (Κόκκινο με κίτρινο αστέρι/pixel)
                                c = Color.red;
                                if (x > 5 && x < 12 && y > h - 12 && y < h - 5) c = new Color(1f, 0.8f, 0f);
                                break;
                            case Language.Hindi: // India (Πορτοκαλί, Λευκό, Πράσινο)
                                if (normY > 0.66f) c = new Color(1f, 0.6f, 0.2f);
                                else if (normY > 0.33f) c = (x > w/2 - 3 && x < w/2 + 3 && y > h/2 - 3 && y < h/2 + 3) ? Color.blue : Color.white;
                                else c = new Color(0f, 0.5f, 0.2f);
                                break;
                        }
                        tex.SetPixel(x, y, c);
                    }
                }
                tex.Apply();
                flagSprites[i] = Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f));
            }
        }
    }
}
 