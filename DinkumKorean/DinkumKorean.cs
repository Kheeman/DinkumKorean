﻿using TMPro;
using I2.Loc;
using System;
using BepInEx;
using XYModLib;
using System.IO;
using I2LocPatch;
using HarmonyLib;
using System.Linq;
using UnityEngine;
using System.Text;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DinkumKorean
{
    [BepInPlugin(GUID, PluginName, Version)]
    public class DinkumKoreanPlugin : BaseUnityPlugin
    {
        public const string GUID = "Kheeman.Dinkum.DinkumKorean";
        public const string PluginName = "DinkumKorean";
        public const string Version = "1.0.5";
        public static DinkumKoreanPlugin Inst;

        public static IJson Json
        {
            get
            {
                if (_json == null)
                {
                    _json = new LitJsonHelper();
                }
                return _json;
            }
        }

        private static IJson _json;

        public static bool Pause
        {
            get
            {
                return pause;
            }
            set
            {
                pause = value;
            }
        }

        private static bool pause;

        public ConfigEntry<bool> DevMode;
        public ConfigEntry<bool> DontLoadLocOnDevMode;
        public ConfigEntry<bool> LogNoTranslation;

        public List<TextLocData> DynamicTextLocList = new List<TextLocData>();
        public List<TextLocData> PostTextLocList = new List<TextLocData>();
        public List<TextLocData> QuestTextLocList = new List<TextLocData>();
        public List<TextLocData> TipsTextLocList = new List<TextLocData>();
        public List<TextLocData> MailTextLocList = new List<TextLocData>();
        public List<TextLocData> AnimalsTextLocList = new List<TextLocData>();

        public List<TextLocData> HoverTextLocList = new List<TextLocData>();
        public List<TextLocData> NPCNameTextLocList = new List<TextLocData>();

        public UIWindow DebugWindow;
        public UIWindow ErrorWindow;
        public string ErrorStr;
        public bool IsPluginLoaded;

        /// <summary>
        /// 게임 시작 시 한 번만 처리하면 됩니다.
        /// </summary>
        public void OnGameStartOnceFix()
        {
            ReplaceNPCNames();
            ReplaceHoverTexts();
        }

        /// <summary>
        /// NPC 이름 바꾸기
        /// </summary>
        public void ReplaceNPCNames()
        {
            List<NPCDetails> coms = new List<NPCDetails>();
            coms.AddRange(Resources.FindObjectsOfTypeAll<NPCDetails>());
            foreach (var com in coms)
            {
                string cnText = TextLocData.GetLoc(NPCNameTextLocList, com.NPCName);
                com.NPCName = cnText;
            }
        }

        public void ReplaceHoverTexts()
        {
            List<HoverToolTipOnButton> coms = new List<HoverToolTipOnButton>();
            coms.AddRange(Resources.FindObjectsOfTypeAll<HoverToolTipOnButton>());
            foreach (var com in coms)
            {
                string cnText = TextLocData.GetLoc(HoverTextLocList, com.hoveringText);
                com.hoveringText = cnText;
            }
        }

        private void Awake()
        {
            Inst = this;
            DevMode = Config.Bind<bool>("Dev", "DevMode", false, "개발 모드에서 단축키를 눌러 개발 모드를 트리거할 수 있습니다.");
            DontLoadLocOnDevMode = Config.Bind<bool>("Dev", "DontLoadLocOnDevMode", true, "개발 모드에서는 DynamicText Post Quest 번역이 로드되지 않아 덤핑에 편리합니다.");
            LogNoTranslation = Config.Bind<bool>("Tool", "LogNoTranslation", true, "번역되지 않은 대상을 출력할 수 있습니다.");
            DebugWindow = new UIWindow("언어 테스트 도구 [Ctrl+Numpad 4]");
            DebugWindow.OnWinodwGUI = DebugWindowGUI;
            ErrorWindow = new UIWindow($"번역에 오류가 발생 {PluginName} v{Version}");
            ErrorWindow.OnWinodwGUI = ErrorWindowFunc;
            try
            {
                Harmony.CreateAndPatchAll(typeof(DinkumKoreanPlugin));
                Harmony.CreateAndPatchAll(typeof(ILPatch));
                Harmony.CreateAndPatchAll(typeof(StringReturnPatch));
                Harmony.CreateAndPatchAll(typeof(StartTranslatePatch));
                Harmony.CreateAndPatchAll(typeof(SpritePatch));
            }
            catch (ExecutionEngineException ex)
            {
                ErrorStr = $"언어에 오류가 있습니다. 사용자 이름 또는 게임 경로에 영어가 아닌 문자가 포함된 것으로 추측됩니다. \n예외 정보:\n{ex}";
                ErrorWindow.Show = true;
            }
            catch (Exception ex)
            {
                ErrorStr = $"언어에 오류가 있습니다. \n예외 정보:\n{ex}";
                ErrorWindow.Show = true;
            }
            if (DevMode.Value && DontLoadLocOnDevMode.Value)
            {
                return;
            }
            Invoke("LogFlagTrue", 2f);
            DynamicTextLocList = TextLocData.LoadFromTxtFile($"{Paths.PluginPath}/I2LocPatch/DynamicTextLoc.txt");
            PostTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/PostTextLoc.json");
            QuestTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/QuestTextLoc.json");
            TipsTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/TipsTextLoc.json");
            MailTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/MailTextLoc.json");
            AnimalsTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/AnimalsTextLoc.json");

            NPCNameTextLocList = TextLocData.LoadFromTxtFile($"{Paths.PluginPath}/I2LocPatch/NPCNamesLoc.csv");
            HoverTextLocList = TextLocData.LoadFromTxtFile($"{Paths.PluginPath}/I2LocPatch/HoverTextLoc.csv");
        }

        public void LogFlagTrue()
        {
            IsPluginLoaded = true;
        }

        public void ErrorWindowFunc()
        {
            GUILayout.Label("새 버전이 있는지 확인하세요");
            GUILayout.Label(ErrorStr);
        }

        private void Start()
        {
            OnGameStartOnceFix();
        }

        private void Update()
        {
            if (DevMode.Value)
            {
                // Ctrl + Numpad 4 GUI 전환
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha4))
                {
                    DebugWindow.Show = !DebugWindow.Show;
                }
                // Ctrl + Numpad 5 게임을 일시 중지, 게임 속도 1
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha5))
                {
                    Pause = !Pause;
                    Time.timeScale = Pause ? 0 : 1;
                }
                // Ctrl + Numpad 6 게임을 일시 중지, 게임 속도 10.
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha6))
                {
                    Pause = !Pause;
                    Time.timeScale = Pause ? 1 : 10;
                }
                // Ctrl + Numpad 7 숨겨진 텍스트를 제외한 장면의 모든 텍스트를 덤프합니다.
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha7))
                {
                    DumpText(false);
                }
                // Ctrl + Numpad 8 숨겨진 텍스트를 포함하여 장면의 모든 텍스트를 덤프합니다.
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha8))
                {
                    DumpText(true);
                }
            }
            FixChatFont();
        }

        private void OnGUI()
        {
            DebugWindow.OnGUI();
            ErrorWindow.OnGUI();
        }

        private Vector2 cv;

        public void DebugWindowGUI()
        {
            GUILayout.BeginVertical("기능", GUI.skin.window);
            if (GUILayout.Button("[Ctrl+Numpad 5] 게임 일시 중지로 전환, 게임 속도 1"))
            {
                Pause = !Pause;
                Time.timeScale = Pause ? 0 : 1;
            }
            if (GUILayout.Button("[Ctrl+Numpad 6] 게임 일시 중지로 전환, 게임 속도 10"))
            {
                Pause = !Pause;
                Time.timeScale = Pause ? 1 : 10;
            }
            if (GUILayout.Button("대괄호 확인"))
            {
                CheckKuoHao();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Dump", GUI.skin.window);
            if (GUILayout.Button("[Ctrl+Numpad 7] 숨겨진 텍스트를 제외한 장면의 모든 텍스트 덤프"))
            {
                DumpText(false);
            }
            if (GUILayout.Button("[Ctrl+Numpad 8] 숨겨진 텍스트를 포함하여 장면의 모든 텍스트 덤프"))
            {
                DumpText(true);
            }
            if (GUILayout.Button("다국어 테이블에 없는 모든 대화 덤프(미완성)"))
            {
                DumpAllConversation();
            }
            if (GUILayout.Button("dump post(미완성)"))
            {
                DumpAllPost();
            }
            if (GUILayout.Button("dump quest(미완성)"))
            {
                DumpAllQuest();
            }
            if (GUILayout.Button("dump mail(미완성)"))
            {
                DumpAllMail();
            }
            if (GUILayout.Button("dump tips(미완성)"))
            {
                DumpAllTips();
            }
            if (GUILayout.Button("dump animals(미완성)"))
            {
                DumpAnimals();
            }
            if (GUILayout.Button("번역된 키가 없는 아이템 덤프(미완성)"))
            {
                DumpAllUnTermItem();
            }
            GUILayout.EndVertical();
        }

        private int lastChatCount;
        private bool isChatHide;
        private float showChatCD;

        public void FixChatFont()
        {
            if (ChatBox.chat != null)
            {
                if (isChatHide)
                {
                    showChatCD -= Time.deltaTime;
                    if (showChatCD < 0)
                    {
                        isChatHide = false;
                        foreach (var chat in ChatBox.chat.chatLog)
                        {
                            chat.contents.enabled = false;
                            chat.contents.enabled = true;
                        }
                    }
                }
                if (ChatBox.chat.chatLog.Count != lastChatCount)
                {
                    lastChatCount = ChatBox.chat.chatLog.Count;
                    isChatHide = true;
                    showChatCD = 0.5f;
                }
            }
        }

        public static void LogInfo(string log)
        {
            Inst.Logger.LogInfo(log);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(OptionsMenu), "Start")]
        public static void OptionsMenuStartPatch()
        {
            LocalizationManager.CurrentLanguage = "Chinese";
        }
        
        [HarmonyPrefix, HarmonyPatch(typeof(RealWorldTimeLight), "setUpDayAndDate")]
        public static bool RealWorldTimeLight_setUpDayAndDate_Patch(RealWorldTimeLight __instance)
        {
            __instance.seasonAverageTemp = __instance.seasonAverageTemps[WorldManager.Instance.month - 1];
            __instance.DayText.text = __instance.getDayName(WorldManager.Instance.day - 1);
            __instance.DateText.text = (WorldManager.Instance.day + (WorldManager.Instance.week - 1) * 7).ToString("00");
            __instance.SeasonText.text = __instance.getSeasonName(WorldManager.Instance.month - 1);
            SeasonManager.manage.checkSeasonAndChangeMaterials();
            return false;
        }
        
        [HarmonyPrefix, HarmonyPatch(typeof(Conversation), "getIntroName")]
        public static bool Conversation_getIntroName(Conversation __instance, ref string __result, int i)
        {
            if (Inst.DevMode.Value && Inst.DontLoadLocOnDevMode.Value) return true;
            string result = $"{__instance.saidBy}/{__instance.gameObject.name}_Intro_{i.ToString("D3")}";
            __result = result;
            if (!LocalizationManager.Sources[0].ContainsTerm(result))
            {
                if (__instance.startLineAlt.sequence.Length > i)
                {
                    if (string.IsNullOrWhiteSpace(__instance.startLineAlt.sequence[i]))
                    {
                        __result = result;
                    }
                    else
                    {
                        __result = result + "_" + __instance.startLineAlt.sequence[i].GetHashCode();
                    }
                }
            }
            if (Inst.DevMode.Value)
                Debug.Log($"Conversation_getIntroName {__result}");
            return false;
        }
        
        [HarmonyPrefix, HarmonyPatch(typeof(Conversation), "getOptionName")]
        public static bool Conversation_getOptionName(Conversation __instance, ref string __result, int i)
        {
            if (Inst.DevMode.Value && Inst.DontLoadLocOnDevMode.Value) return true;
            string result = $"{__instance.saidBy}/{__instance.gameObject.name}_Option_{i.ToString("D3")}";
            __result = result;
            if (!LocalizationManager.Sources[0].ContainsTerm(result))
            {
                if (__instance.optionNames.Length > i)
                {
                    if (string.IsNullOrWhiteSpace(__instance.optionNames[i]))
                    {
                        __result = result;
                    }
                    else
                    {
                        __result = result + "_" + __instance.optionNames[i].GetHashCode();
                    }
                }
            }
            if (Inst.DevMode.Value)
                Debug.Log($"Conversation_getOptionName {__result}");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Conversation), "getResponseName")]
        public static bool Conversation_getResponseName(Conversation __instance, ref string __result, int i, int r)
        {
            if (Inst.DevMode.Value && Inst.DontLoadLocOnDevMode.Value) return true;
            string result = $"{__instance.saidBy}/{__instance.gameObject.name}_Response_{i.ToString("D3")}_{r.ToString("D3")}";
            __result = result;
            if (!LocalizationManager.Sources[0].ContainsTerm(result))
            {
                if (__instance.responesAlt.Length > i)
                {
                    if (__instance.responesAlt[i].sequence.Length > r)
                    {
                        if (string.IsNullOrWhiteSpace(__instance.responesAlt[i].sequence[r]))
                        {
                            __result = result;
                        }
                        else
                        {
                            __result = result + "_" + __instance.responesAlt[i].sequence[r].GetHashCode();
                        }
                    }
                }
            }
            if (Inst.DevMode.Value)
                Debug.Log($"Conversation_getResponseName {__result}");
            return false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(LocalizationManager), "TryGetTranslation")]
        public static void Localize_OnLocalize(string Term, bool __result)
        {
            if (Inst.IsPluginLoaded && Inst.LogNoTranslation.Value)
            {
                if (!__result)
                {
                    Debug.LogWarning($"LocalizationManager 번역을 가져오지 못했습니다:Term:{Term}");
                }
            }
        }

        public static Queue<TextMeshProUGUI> waitShowTMPs = new Queue<TextMeshProUGUI>();

        /// <summary>
        /// 检查翻译中的括号是否匹配
        /// </summary>
        public void CheckKuoHao()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            // 索引和Excel表中的行对应的偏移
            int hangOffset = 3;
            int findCount = 0;
            StringBuilder sb = new StringBuilder();
            LogInfo($"번역에서 괄호 확인 시작:");
            Regex reg = new Regex(@"(?is)(?<=\<)[^\>]+(?=\>)");
            var mResourcesCache = Traverse.Create(ResourceManager.pInstance).Field("mResourcesCache").GetValue<Dictionary<string, UnityEngine.Object>>();
            LanguageSourceAsset asset = mResourcesCache.Values.First() as LanguageSourceAsset;
            int len = asset.SourceData.mTerms.Count;
            for (int i = 0; i < len; i++)
            {
                var term = asset.SourceData.mTerms[i];
                if (string.IsNullOrWhiteSpace(term.Languages[3])) continue;
                MatchCollection mc1 = reg.Matches(term.Languages[0]);
                MatchCollection mc2 = reg.Matches(term.Languages[3]);
                if (mc1.Count != mc2.Count)
                {
                    string log = $"줄번호:{i + hangOffset} Key:{term.Term} 괄호 갯수가 맞지 않습니다. 원본 괄호 {mc1.Count}개 번역 괄호 {mc2.Count}개 원문:{term.Languages[0]} 번역:{term.Languages[3]}";
                    LogInfo(log);
                    sb.AppendLine(log);
                    findCount++;
                }
                else if (mc1.Count > 0)
                {
                    for (int j = 0; j < mc1.Count; j++)
                    {
                        if (mc1[j].Value != mc2[j].Value)
                        {
                            string log = $"줄번호:{i + hangOffset} Key:{term.Term} 대괄호{j}의 내용이 일치하지 않습니다. 원본:<{mc1[j].Value}> 번역:<{mc2[j].Value}>";
                            LogInfo(log);
                            sb.AppendLine(log);
                            findCount++;
                        }
                    }
                }
            }
            sw.Stop();
            LogInfo($"확인 후，문제있는 항목 {findCount}개 발견됨，{sw.ElapsedMilliseconds}ms 소요됨.");
            System.IO.File.WriteAllText($"{Paths.GameRootPath}/CheckKuoHao.txt", sb.ToString());
        }

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public string GetPath(Transform t)
        {
            List<string> paths = new List<string>();
            StringBuilder sb = new StringBuilder();
            paths.Add(t.name);
            Transform p = t.parent;
            while (p != null)
            {
                paths.Add(p.name);
                p = p.parent;
            }
            for (int i = paths.Count - 1; i >= 0; i--)
            {
                sb.Append(paths[i]);
                if (i != 0)
                {
                    sb.Append('/');
                }
            }
            return sb.ToString();
        }
               

        #region Dump

        /// <summary>
        /// Dump当前的文本
        /// </summary>
        /// <param name="includeInactive"></param>
        public void DumpText(bool includeInactive)
        {
            StringBuilder sb = new StringBuilder();
            var tmps = GameObject.FindObjectsOfType<TextMeshProUGUI>(includeInactive);
            foreach (var tmp in tmps)
            {
                var i2 = tmp.GetComponent<Localize>();
                if (i2 != null) continue;
                sb.AppendLine("===========");
                sb.AppendLine($"path:{GetPath(tmp.transform)}");
                sb.AppendLine($"text:{tmp.text.StrToI2Str()}");
            }
            File.WriteAllText($"{Paths.GameRootPath}/I2/TextDump.txt", sb.ToString());
            LogInfo($"Dump완료,{Paths.GameRootPath}/I2/TextDump.txt");
        }

        public void DumpAllConversation()
        {
            List<Conversation> conversations = new List<Conversation>();
            // 直接从资源搜索单独的Conversation
            conversations.AddRange(Resources.FindObjectsOfTypeAll<Conversation>());

            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine($"Key\tEnglish");
            List<string> terms = new List<string>();
            I2File i2File = new I2File();
            i2File.Name = "NoTermConversation";
            i2File.Languages = new List<string>() { "English" };

            foreach (var c in conversations)
            {
                // Intro
                for (int i = 0; i < c.startLineAlt.sequence.Length; i++)
                {
                    string key = c.getIntroName(i);
                    if (!LocalizationManager.Sources[0].ContainsTerm(key))
                    {
                        if (!string.IsNullOrWhiteSpace(c.startLineAlt.sequence[i]))
                        {
                            string term = $"{key}_{c.startLineAlt.sequence[i].GetHashCode()}";
                            string line = $"{term}\t{c.startLineAlt.sequence[i].StrToI2Str()}";
                            if (terms.Contains(term))
                            {
                                string log = $"중복 대사 무시. {line}";
                                Logger.LogError(log);
                            }
                            else
                            {
                                terms.Add(term);
                                TermLine termLine = new TermLine();
                                termLine.Name = term;
                                termLine.Texts = new string[] { c.startLineAlt.sequence[i] };
                                i2File.Lines.Add(termLine);
                                //sb.AppendLine(line);
                                LogInfo(line);
                            }
                        }
                    }
                }
                // Option
                for (int j = 0; j < c.optionNames.Length; j++)
                {
                    if (!c.optionNames[j].Contains("<"))
                    {
                        string key = c.getOptionName(j);
                        if (!LocalizationManager.Sources[0].ContainsTerm(key))
                        {
                            if (!string.IsNullOrWhiteSpace(c.optionNames[j]))
                            {
                                string term = $"{key}_{c.optionNames[j].GetHashCode()}";
                                string line = $"{term}\t{c.optionNames[j].StrToI2Str()}";
                                if (terms.Contains(term))
                                {
                                    string log = $"중복 대사 무시. {line}";
                                    Logger.LogError(log);
                                }
                                else
                                {
                                    terms.Add(term);
                                    //sb.AppendLine(line);
                                    TermLine termLine = new TermLine();
                                    termLine.Name = term;
                                    termLine.Texts = new string[] { c.optionNames[j] };
                                    i2File.Lines.Add(termLine);
                                    LogInfo(line);
                                }
                            }
                        }
                    }
                }
                // Respone
                for (int k = 0; k < c.responesAlt.Length; k++)
                {
                    for (int l = 0; l < c.responesAlt[k].sequence.Length; l++)
                    {
                        string key = c.getResponseName(k, l);
                        if (!LocalizationManager.Sources[0].ContainsTerm(key))
                        {
                            if (!string.IsNullOrWhiteSpace(c.responesAlt[k].sequence[l]))
                            {
                                string term = $"{key}_{c.responesAlt[k].sequence[l].GetHashCode()}";
                                string line = $"{term}\t{c.responesAlt[k].sequence[l].StrToI2Str()}";
                                if (terms.Contains(term))
                                {
                                    string log = $"중복 대사 무시. {line}";
                                    Logger.LogError(log);
                                }
                                else
                                {
                                    terms.Add(term);
                                    //sb.AppendLine(line);
                                    TermLine termLine = new TermLine();
                                    termLine.Name = term;
                                    termLine.Texts = new string[] { c.responesAlt[k].sequence[l] };
                                    i2File.Lines.Add(termLine);
                                    LogInfo(line);
                                }
                            }
                        }
                    }
                }
            }
            i2File.WriteCSVTable($"{Paths.GameRootPath}/I2/{i2File.Name}.csv");
            LogInfo($"Dump {i2File.Name} 완료");
        }

        public void DumpAllPost()
        {
            List<BullitenBoardPost> list = new List<BullitenBoardPost>();
            list.Add(BulletinBoard.board.announcementPosts[0]);
            list.Add(BulletinBoard.board.huntingTemplate);
            list.Add(BulletinBoard.board.captureTemplate);
            list.Add(BulletinBoard.board.tradeTemplate);
            list.Add(BulletinBoard.board.photoTemplate);
            list.Add(BulletinBoard.board.cookingTemplate);
            list.Add(BulletinBoard.board.smeltingTemplate);
            list.Add(BulletinBoard.board.compostTemplate);
            list.Add(BulletinBoard.board.sateliteTemplate);
            list.Add(BulletinBoard.board.craftingTemplate);
            list.Add(BulletinBoard.board.shippingRequestTemplate);
            List<TextLocData> list2 = new List<TextLocData>();
            foreach (var p in list)
            {
                list2.Add(new TextLocData(p.title, ""));
                list2.Add(new TextLocData(p.contentText, ""));
            }
            var json = Json.ToJson(list2, true);
            File.WriteAllText($"{Paths.GameRootPath}/I2/PostTextLoc.json", json);
            Debug.Log(json);
        }

        public void DumpAllQuest()
        {
            var mgr = QuestManager.manage;
            List<TextLocData> list = new List<TextLocData>();
            foreach (var q in mgr.allQuests)
            {
                list.Add(new TextLocData(q.QuestName, ""));
                list.Add(new TextLocData(q.QuestDescription, ""));
            }
            var json = Json.ToJson(list, true);
            File.WriteAllText($"{Paths.GameRootPath}/I2/QuestTextLoc.json", json);
            Debug.Log(json);
        }

        public void DumpAllMail()
        {
            var mgr = MailManager.manage;
            List<TextLocData> list = new List<TextLocData>();
            list.Add(new TextLocData(mgr.animalResearchLetter.letterText, ""));
            list.Add(new TextLocData(mgr.returnTrapLetter.letterText, ""));
            list.Add(new TextLocData(mgr.devLetter.letterText, ""));
            list.Add(new TextLocData(mgr.catalogueItemLetter.letterText, ""));
            list.Add(new TextLocData(mgr.craftmanDayOff.letterText, ""));
            foreach (var m in mgr.randomLetters) list.Add(new TextLocData(m.letterText, ""));
            foreach (var m in mgr.thankYouLetters) list.Add(new TextLocData(m.letterText, ""));
            foreach (var m in mgr.didNotFitInInvLetter) list.Add(new TextLocData(m.letterText, ""));
            foreach (var m in mgr.fishingTips) list.Add(new TextLocData(m.letterText, ""));
            foreach (var m in mgr.bugTips) list.Add(new TextLocData(m.letterText, ""));
            foreach (var m in mgr.licenceLevelUp) list.Add(new TextLocData(m.letterText, ""));
            var json = Json.ToJson(list, true);
            File.WriteAllText($"{Paths.GameRootPath}/I2/MailTextLoc.json", json);
            Debug.Log(json);
        }

        public void DumpAllTips()
        {
            var mgr = GameObject.FindObjectOfType<LoadingScreenImageAndTips>(true);
            List<TextLocData> list = new List<TextLocData>();
            foreach (var tip in mgr.tips) list.Add(new TextLocData(tip, ""));
            var json = Json.ToJson(list, true);
            File.WriteAllText($"{Paths.GameRootPath}/I2/TipsTextLoc.json", json);
            Debug.Log(json);
        }

        public void DumpAnimals()
        {
            var mgr = AnimalManager.manage;
            List<TextLocData> list = new List<TextLocData>();
            foreach (var a in mgr.allAnimals) list.Add(new TextLocData(a.animalName, ""));
            var json = Json.ToJson(list, true);
            File.WriteAllText($"{Paths.GameRootPath}/I2/AnimalsTextLoc.json", json);
            Debug.Log(json);
        }

        public void DumpAllUnTermItem()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Key\tEnglish");
            List<string> keys = new List<string>();
            foreach (var item in Inventory.Instance.allItems)
            {
                int id = Inventory.Instance.getInvItemId(item);
                string nameKey = "InventoryItemNames/InvItem_" + id.ToString();
                string descKey = "InventoryItemDescriptions/InvDesc_" + id.ToString();
                if (!LocalizationManager.Sources[0].ContainsTerm(nameKey))
                {
                    string line = nameKey + "\t" + item.itemName;
                    LogInfo(line);
                    if (keys.Contains(nameKey))
                    {
                        string log = $"중복 키가 나옴 {nameKey} 추가 안함";
                        Logger.LogError(log);
                    }
                    else
                    {
                        keys.Add(nameKey);
                        sb.AppendLine(line);
                    }
                }
                if (!LocalizationManager.Sources[0].ContainsTerm(descKey))
                {
                    string line = descKey + "\t" + item.itemDescription;
                    LogInfo(line);
                    if (keys.Contains(descKey))
                    {
                        string log = $"중복 키가 나옴 {descKey} 추가 안함";
                        Logger.LogError(log);
                    }
                    else
                    {
                        keys.Add(descKey);
                        sb.AppendLine(line);
                    }
                }
            }
            File.WriteAllText($"{Paths.GameRootPath}/I2/UnTermItem.csv", sb.ToString());
        }

        #endregion Dump
    }
}