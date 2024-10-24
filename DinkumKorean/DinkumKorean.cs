using TMPro;
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
        public const string Version = "1.0.6";
        public static DinkumKoreanPlugin Inst;

        public static Queue<TextMeshProUGUI> waitShowTMPs = new Queue<TextMeshProUGUI>();

        public ConfigEntry<bool> DevMode;
        public ConfigEntry<bool> DontLoadLocOnDevMode;
        public ConfigEntry<bool> LogNoTranslation;

        public UIWindow DebugWindow;
        public UIWindow ErrorWindow;

        public string ErrorStr;
        public bool IsPluginLoaded;

        public List<TextLocData> DynamicTextLocList = new List<TextLocData>();
        public List<TextLocData> HoverTextLocList = new List<TextLocData>();
        public List<TextLocData> MailTextLocList = new List<TextLocData>();
        public List<TextLocData> NPCNameTextLocList = new List<TextLocData>();
        public List<TextLocData> PostTextLocList = new List<TextLocData>();
        public List<TextLocData> QuestTextLocList = new List<TextLocData>();
        public List<TextLocData> TipsTextLocList = new List<TextLocData>();
        public List<TextLocData> TopNotificationLocList = new List<TextLocData>();

        //public List<TextLocData> AnimalsTextLocList = new List<TextLocData>();

        private static IJson _json;
        private static bool pause;

        private Vector2 cv;
        private bool isChatHide;
        private int lastChatCount;
        private float showChatCD;
        private float tipsCD = 20;

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
                //Harmony.CreateAndPatchAll(typeof(DinkumKoreanPlugin));
                Harmony.CreateAndPatchAll(typeof(OtherPatch));
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
            Invoke("OnGameStartOnceFix", 2f);
            DynamicTextLocList = TextLocData.LoadFromTxtFile($"{Paths.PluginPath}/I2LocPatch/DynamicTextLoc.txt");
            NPCNameTextLocList = TextLocData.LoadFromTxtFile($"{Paths.PluginPath}/I2LocPatch/NPCNamesLoc.csv");
            HoverTextLocList = TextLocData.LoadFromTxtFile($"{Paths.PluginPath}/I2LocPatch/HoverTextLoc.csv");
            PostTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/PostTextLoc.json");
            QuestTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/QuestTextLoc.json");
            TipsTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/TipsTextLoc.json");
            MailTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/MailTextLoc.json");
            TopNotificationLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/TopNotification.json");
            //AnimalsTextLocList = TextLocData.LoadFromJsonFile($"{Paths.PluginPath}/I2LocPatch/AnimalsTextLoc.json");

            OnGameStartOnceFix();
        }

        public static void LogInfo(string log)
        {
            Inst.Logger.LogInfo(log);
        }

        /// <summary>
        /// 번역의 대괄호가 일치하는지 확인하세요.
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
                DumpTool.DumpText(false);
            }
            if (GUILayout.Button("[Ctrl+Numpad 8] 숨겨진 텍스트를 포함하여 장면의 모든 텍스트 덤프"))
            {
                DumpTool.DumpText(true);
            }
            if (GUILayout.Button("一键导出全部原文(需要未汉化状态)"))
            {
                List<string> ignoreTermList = new List<string>();
                var list1 = DumpTool.DumpAllConversationObject();
                var list2 = DumpTool.DumpAllItem();
                ignoreTermList.AddRange(list1);
                ignoreTermList.AddRange(list2);
                I2LocPatchPlugin.Instance.DumpAllLocRes(ignoreTermList);
                DumpTool.DumpAllPost();
                DumpTool.DumpAllQuest();
                DumpTool.DumpAllMail();
                DumpTool.DumpAllTips();
                DumpTool.DumpAnimals();
                DumpTool.DumpNPCNames();
                DumpTool.DumpHoverText();
                DumpTool.DumpInventoryLootTableTimeWeatherMaster_locationName();
            }
            GUILayout.EndVertical();
        }

        public void ErrorWindowFunc()
        {
            GUILayout.Label("새 버전이 있는지 확인하세요");
            GUILayout.Label(ErrorStr);
        }

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

        public void LogFlagTrue()
        {
            IsPluginLoaded = true;
        }

        /// <summary>
        /// 게임 시작 시 한 번만 처리하면 됩니다.
        /// </summary>
        public void OnGameStartOnceFix()
        {
            //ReplaceNPCNames();
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
                string cnDesc = TextLocData.GetLoc(HoverTextLocList, com.hoveringDesc);
                com.hoveringText = cnText;
                com.hoveringDesc = cnDesc;
            }
        }

        private void OnGUI()
        {
            DebugWindow.OnGUI();
            ErrorWindow.OnGUI();
            if (tipsCD > 0)
            {
                //GUILayout.Label($"\n[{(int)tipsCD}s]温馨提示：汉化mod是开源免费的，不需要花钱买，Dinkum汉化交流QQ频道-> 4X游戏频道 频道号:7opslk1lrt");
            }
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
                    DumpTool.DumpText(false);
                }
                // Ctrl + Numpad 8 숨겨진 텍스트를 포함하여 장면의 모든 텍스트를 덤프합니다.
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha8))
                {
                    DumpTool.DumpText(true);
                }
            }
            FixChatFont();
        }

        /*  OtherPatch.cs 이동
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
        */

        /// <summary>
        /// 경로 가져오기
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
    }
}