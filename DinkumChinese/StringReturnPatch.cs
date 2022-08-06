using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace DinkumChinese
{
    public static class StringReturnPatch
    {
        [HarmonyPostfix, HarmonyPatch(typeof(GenerateMap), "getBiomeNameUnderMapCursor")]
        public static void GenerateMap_getBiomeNameUnderMapCursor_Patch(ref string __result)
        {
            __result = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, __result.StrToI2Str());
        }

        [HarmonyPostfix, HarmonyPatch(typeof(GenerateMap), "getBiomeNameById")]
        public static void GenerateMap_getBiomeNameById_Patch(ref string __result, int id)
        {
            GenerateMap.biomNames biomNames = (GenerateMap.biomNames)id;
            __result = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, biomNames.ToString());
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PostOnBoard), "getTitleText")]
        public static void PostOnBoard_getTitleText_Patch(PostOnBoard __instance, ref string __result, int postId)
        {
            string titleOri = __instance.getPostPostsById().title.StrToI2Str();
            string title = TextLocData.GetLoc(DinkumChinesePlugin.Inst.PostTextLocList, titleOri);
            __result = title.Replace("<boardRewardItem>",
                __instance.getPostPostsById().getBoardRewardItem(postId)).Replace("<boardHuntRequestAnimal>",
                __instance.getPostPostsById().getBoardHuntRequestAnimal(postId)).Replace("<boardRequestItem>",
                __instance.getPostPostsById().getBoardRequestItem(postId));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PostOnBoard), "getContentText")]
        public static void PostOnBoard_getContentText_Patch(PostOnBoard __instance, ref string __result, int postId)
        {
            string textOri = __instance.getPostPostsById().contentText.StrToI2Str();
            string text = TextLocData.GetLoc(DinkumChinesePlugin.Inst.PostTextLocList, textOri);
            __result = text.Replace("<boardRewardItem>",
                __instance.getPostPostsById().getBoardRewardItem(postId)).Replace("<boardHuntRequestAnimal>",
                __instance.getPostPostsById().getBoardHuntRequestAnimal(postId)).Replace("<getAnimalsInPhotoList>",
                __instance.getPostPostsById().getRequirementsNeededInPhoto(postId)).Replace("<boardRequestItem>",
                __instance.getPostPostsById().getBoardRequestItem(postId));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(QuestTracker), "displayMainQuest")]
        public static void QuestTracker_displayMainQuest_Patch(QuestTracker __instance, int questNo)
        {
            string nameOri = QuestManager.manage.allQuests[questNo].QuestName.StrToI2Str();
            string name = TextLocData.GetLoc(DinkumChinesePlugin.Inst.QuestTextLocList, nameOri);
            string descOri = QuestManager.manage.allQuests[questNo].QuestDescription.StrToI2Str();
            string desc = TextLocData.GetLoc(DinkumChinesePlugin.Inst.QuestTextLocList, descOri).Replace("<IslandName>", Inventory.inv.islandName);
            __instance.questTitle.text = name;
            __instance.questDesc.text = desc;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(QuestButton), "setUp")]
        public static void QuestButton_setUp_Patch(QuestButton __instance, int questNo)
        {
            if (__instance.isMainQuestButton)
            {
                string nameOri = QuestManager.manage.allQuests[questNo].QuestName.StrToI2Str();
                string name = TextLocData.GetLoc(DinkumChinesePlugin.Inst.QuestTextLocList, nameOri);
                __instance.buttonText.text = name;
            }
            else if (__instance.isQuestButton)
            {
            }
            else
            {
                __instance.buttonText.text = __instance.buttonText.text.Replace("Request for ", "") + " 요청";
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(QuestNotification), "showQuest")]
        public static void QuestNotification_showQuest_Patch(QuestNotification __instance)
        {
            string nameOri = __instance.displayingQuest.QuestName.StrToI2Str();
            string name = TextLocData.GetLoc(DinkumChinesePlugin.Inst.QuestTextLocList, nameOri);
            __instance.QuestText.text = name;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(QuestTracker), "pinTheTask")]
        public static void QuestTracker_pinTheTask_Patch(QuestTracker __instance, QuestTracker.typeOfTask type, int id)
        {
            if (type == QuestTracker.typeOfTask.Quest)
            {
                if (!QuestManager.manage.isQuestCompleted[id])
                {
                    string nameOri = QuestManager.manage.allQuests[id].QuestName.StrToI2Str();
                    string name = TextLocData.GetLoc(DinkumChinesePlugin.Inst.QuestTextLocList, nameOri);
                    string pinText = __instance.pinMissionText.text.Replace(QuestManager.manage.allQuests[id].QuestName, name);
                    __instance.pinMissionText.text = pinText;
                }
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PickUpNotification), "fillButtonPrompt")]
        public static void PickUpNotification_fillButtonPrompt_Patch(PickUpNotification __instance, string buttonPromptText)
        {
            string text = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, buttonPromptText);
            __instance.itemText.text = text;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(AnimalHouseMenu), "fillData")]
        public static void AnimalHouseMenu_fillData_Patch(AnimalHouseMenu __instance)
        {
            __instance.eatenText.text = __instance.eatenText.text.Replace("Eaten", "먹이주기");
            __instance.shelterText.text = __instance.shelterText.text.Replace("Shelter", "쉼터");
            __instance.pettedText.text = __instance.pettedText.text.Replace("Petted", "쓰다듬기");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CustomNetworkManager), "refreshLobbyTypeButtons")]
        public static void CustomNetworkManager_refreshLobbyTypeButtons_Patch(CustomNetworkManager __instance)
        {
            __instance.friendGameText.text = __instance.friendGameText.text.Replace("Friends Only", "친구만");
            __instance.inviteOnlyText.text = __instance.inviteOnlyText.text.Replace("InviteOnly", "초대만");
            __instance.publicGameText.text = __instance.publicGameText.text.Replace("Public", "공개");
            __instance.lanGameText.text = __instance.lanGameText.text.Replace("LAN", "LAN");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Licence), "getConnectedSkillName")]
        public static void Licence_getConnectedSkillName_Patch(ref string __result)
        {
            __result = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, __result);
        }

        [HarmonyPrefix, HarmonyPatch(typeof(SeasonAndTime), "capitaliseFirstLetter")]
        public static bool SeasonAndTime_capitaliseFirstLetter_Patch(ref string __result, string toChange)
        {
            Debug.Log($"SeasonAndTime_capitaliseFirstLetter_Patch 1:{toChange}");
            __result = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, toChange);
            Debug.Log($"SeasonAndTime_capitaliseFirstLetter_Patch 2:{__result}");
            return false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(NPCRequest), "setRandomBugNoAndLocation")]
        public static void NPCRequest_setRandomBugNoAndLocation_Patch(NPCRequest __instance)
        {
            __instance.itemFoundInLocation = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, __instance.itemFoundInLocation);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(NPCRequest), "setRandomFishNoAndLocation")]
        public static void NPCRequest_setRandomFishNoAndLocation_Patch(NPCRequest __instance)
        {
            __instance.itemFoundInLocation = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, __instance.itemFoundInLocation);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(AnimalManager), "fillAnimalLocation")]
        public static void AnimalManager_fillAnimalLocation_Patch(ref string __result)
        {
            Debug.Log($"AnimalManager_fillAnimalLocation_Patch 1:{__result}");
            __result = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, __result);
            Debug.Log($"AnimalManager_fillAnimalLocation_Patch 2:{__result}");
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AnimalManager), "capitaliseFirstLetter")]
        public static bool AnimalManager_capitaliseFirstLetter_Patch(ref string __result, string toChange)
        {
            Debug.Log($"AnimalManager_capitaliseFirstLetter_Patch 1:{toChange}");
            __result = TextLocData.GetLoc(DinkumChinesePlugin.Inst.DynamicTextLocList, toChange);
            Debug.Log($"AnimalManager_capitaliseFirstLetter_Patch 2:{__result}");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Inventory), "getExtraDetails")]
        public static bool Inventory_getExtraDetails_Patch(Inventory __instance, int itemId, ref string __result)
        {
            var _this = __instance;
            string text = "";
            if (_this.allItems[itemId].placeable && _this.allItems[itemId].placeable.tileObjectGrowthStages && !_this.allItems[itemId].consumeable)
            {
                string text2 = "";
                if (_this.allItems[itemId].placeable.tileObjectGrowthStages.growsInSummer && _this.allItems[itemId].placeable.tileObjectGrowthStages.growsInWinter && _this.allItems[itemId].placeable.tileObjectGrowthStages.growsInSpring && _this.allItems[itemId].placeable.tileObjectGrowthStages.growsInAutum)
                {
                    text2 = "모든 계절 ";
                }
                else
                {
                    text2 += "확인";
                    if (_this.allItems[itemId].placeable.tileObjectGrowthStages.growsInSummer)
                    {
                        text2 += "여름";
                    }
                    if (_this.allItems[itemId].placeable.tileObjectGrowthStages.growsInAutum)
                    {
                        if (text2 != "확인")
                        {
                            text2 += "과 ";
                        }
                        text2 += "가을";
                    }
                    if (_this.allItems[itemId].placeable.tileObjectGrowthStages.growsInWinter)
                    {
                        if (text2 != "확인")
                        {
                            text2 += "과 ";
                        }
                        text2 += "겨울";
                    }
                    if (_this.allItems[itemId].placeable.tileObjectGrowthStages.growsInSpring)
                    {
                        if (text2 != "확인")
                        {
                            text2 += "과";
                        }
                        text2 += "봄";
                    }
                }
                if (_this.allItems[itemId].placeable.tileObjectGrowthStages.needsTilledSoil)
                {
                    text = text + "" + text2 + "에 재배 적합.";
                }
                if (_this.allItems[itemId].placeable.tileObjectGrowthStages.objectStages.Length != 0)
                {
                    if (_this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto)
                    {
                        text = string.Concat(new string[]
                        {
                        text,
                        "옆에<b>",
                        _this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto.tileObjectGrowthStages.harvestSpots.Length.ToString(),
                        "</b>에서 <b>",
                        _this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto.tileObjectGrowthStages.harvestDrop.getInvItemName(),
                        "</b>이(가) 나오므로 주변에 공간이 필요합니다. 이 식물은 최대 4개의 가지를 나눌 수 있습니다"
                        });
                    }
                    else
                    {
                        text = string.Concat(new string[]
                        {
                        text,
                        _this.allItems[itemId].placeable.tileObjectGrowthStages.harvestDrop.getInvItemName(),
                        _this.allItems[itemId].placeable.tileObjectGrowthStages.harvestSpots.Length.ToString(),
                        "개를 수확하는데 ",
                        _this.allItems[itemId].placeable.tileObjectGrowthStages.objectStages.Length.ToString(),
                        "일이 걸립니다."
                        });
                    }
                }
                if (!_this.allItems[itemId].placeable.tileObjectGrowthStages.diesOnHarvest && !_this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto)
                {
                    text = string.Concat(new string[]
                    {
                    text,
                    "앞으로 ",
                    Mathf.Abs(_this.allItems[itemId].placeable.tileObjectGrowthStages.takeOrAddFromStateOnHarvest).ToString(),
                    "일마다",
                    _this.allItems[itemId].placeable.tileObjectGrowthStages.harvestDrop.getInvItemName(),
                    _this.allItems[itemId].placeable.tileObjectGrowthStages.harvestSpots.Length.ToString(),
                    "개를 수확할 수 있습니다."
                    });
                }
                if (!WorldManager.manageWorld.allObjectSettings[_this.allItems[itemId].placeable.tileObjectId].walkable)
                {
                    text += "아, 여기에 붙어서 자라기 위한 식물 받침대도 필요합니다.";
                }
            }
            __result = text;
            return false;
        }
    }
}