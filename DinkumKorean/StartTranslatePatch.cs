﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using I2LocPatch;

namespace DinkumKorean
{
    public static class StartTranslatePatch
    {
        [HarmonyPostfix, HarmonyPatch(typeof(AnimalManager), "Start")]
        public static void AnimalManager_Start_Patch()
        {
            var mgr = AnimalManager.manage;
            foreach (var a in mgr.allAnimals)
            {
                a.animalName = TextLocData.GetLoc(DinkumChinesePlugin.Inst.AnimalsTextLocList, a.animalName);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(LoadingScreenImageAndTips), "OnEnable")]
        public static void LoadingScreenImageAndTips_OnEnable_Patch(LoadingScreenImageAndTips __instance)
        {
            for (int i = 0; i < __instance.tips.Length; i++)
            {
                string ori = __instance.tips[i];
                for (int j = 0; j < DinkumKoreanPlugin.Inst.TipsTextLocList.Count; j++)
                {
                    // 이미 번역된 경우 건너뛰기
                    if (DinkumKoreanPlugin.Inst.TipsTextLocList[j].Loc == ori)
                    {
                        return;
                    }
                }
                string t = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.TipsTextLocList, ori);
                if (t == ori)
                {
                    Debug.Log($"LoadingScreenImageAndTips 번역할 텍스트:[{t}]，DynamicTextLoc에 추가해주세요");
                }
                else
                {
                    __instance.tips[i] = t;
                }
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(MailManager), "Start")]
        public static void MailManager_Start_Patch()
        {
            var mgr = MailManager.manage;
            mgr.animalResearchLetter.letterText
               = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.MailTextLocList, mgr.animalResearchLetter.letterText);
            mgr.returnTrapLetter.letterText
               = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.MailTextLocList, mgr.returnTrapLetter.letterText);
            mgr.devLetter.letterText
               = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.MailTextLocList, mgr.devLetter.letterText);
            mgr.catalogueItemLetter.letterText
               = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.MailTextLocList, mgr.catalogueItemLetter.letterText);
            mgr.craftmanDayOff.letterText
               = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.MailTextLocList, mgr.craftmanDayOff.letterText);
            foreach (var m in mgr.randomLetters)
            {
                m.letterText = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.MailTextLocList, m.letterText);
            }
            foreach (var m in mgr.thankYouLetters)
            {
                m.letterText = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.MailTextLocList, m.letterText);
            }
            foreach (var m in mgr.didNotFitInInvLetter)
            {
                m.letterText = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.MailTextLocList, m.letterText);
            }
            foreach (var m in mgr.fishingTips)
            {
                m.letterText = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.MailTextLocList, m.letterText);
            }
            foreach (var m in mgr.bugTips)
            {
                m.letterText = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.MailTextLocList, m.letterText);
            }
            foreach (var m in mgr.licenceLevelUp)
            {
                m.letterText = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.MailTextLocList, m.letterText);
            }
        }
    }
}