using HarmonyLib;
using I2.Loc;
using I2LocPatch;
using UnityEngine;

namespace DinkumKorean
{
    public static class OtherPatch
    {
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
            if (DinkumKoreanPlugin.Inst.DevMode.Value && DinkumKoreanPlugin.Inst.DontLoadLocOnDevMode.Value) return true;
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
            if (DinkumKoreanPlugin.Inst.DevMode.Value)
                Debug.Log($"Conversation_getIntroName {__result}");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Conversation), "getOptionName")]
        public static bool Conversation_getOptionName(Conversation __instance, ref string __result, int i)
        {
            if (DinkumKoreanPlugin.Inst.DevMode.Value && DinkumKoreanPlugin.Inst.DontLoadLocOnDevMode.Value) return true;
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
            if (DinkumKoreanPlugin.Inst.DevMode.Value)
                Debug.Log($"Conversation_getOptionName {__result}");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Conversation), "getResponseName")]
        public static bool Conversation_getResponseName(Conversation __instance, ref string __result, int i, int r)
        {
            if (DinkumKoreanPlugin.Inst.DevMode.Value && DinkumKoreanPlugin.Inst.DontLoadLocOnDevMode.Value) return true;
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
            if (DinkumKoreanPlugin.Inst.DevMode.Value)
                Debug.Log($"Conversation_getResponseName {__result}");
            return false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(LocalizationManager), "TryGetTranslation")]
        public static void Localize_OnLocalize(string Term, bool __result)
        {
            if (DinkumKoreanPlugin.Inst.IsPluginLoaded && DinkumKoreanPlugin.Inst.LogNoTranslation.Value)
            {
                if (!__result)
                {
                    Debug.LogWarning($"LocalizationManager 번역을 가져오지 못했습니다:Term:{Term}");
                }
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(OptionsMenu), "Start")]
        public static void OptionsMenuStartPatch()
        {
            LocalizationManager.CurrentLanguage = "Chinese";
        }
    }
}
