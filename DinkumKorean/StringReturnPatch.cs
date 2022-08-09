using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using I2.Loc;
using UnityEngine;

namespace DinkumKorean
{
    public static class StringReturnPatch
    {
        [HarmonyPostfix, HarmonyPatch(typeof(GenerateMap), "getBiomeNameUnderMapCursor")]
        public static void GenerateMap_getBiomeNameUnderMapCursor_Patch(ref string __result)
        {
            __result = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.DynamicTextLocList, __result.StrToI2Str());
        }

        [HarmonyPostfix, HarmonyPatch(typeof(GenerateMap), "getBiomeNameById")]
        public static void GenerateMap_getBiomeNameById_Patch(ref string __result, int id)
        {
            GenerateMap.biomNames biomNames = (GenerateMap.biomNames)id;
            __result = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.DynamicTextLocList, biomNames.ToString());
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PostOnBoard), "getTitleText")]
        public static void PostOnBoard_getTitleText_Patch(PostOnBoard __instance, ref string __result, int postId)
        {
            string titleOri = __instance.getPostPostsById().title.StrToI2Str();
            string title = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.PostTextLocList, titleOri);
            __result = title.Replace("<boardRewardItem>",
                __instance.getPostPostsById().getBoardRewardItem(postId)).Replace("<boardHuntRequestAnimal>",
                __instance.getPostPostsById().getBoardHuntRequestAnimal(postId)).Replace("<boardRequestItem>",
                __instance.getPostPostsById().getBoardRequestItem(postId));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PostOnBoard), "getContentText")]
        public static void PostOnBoard_getContentText_Patch(PostOnBoard __instance, ref string __result, int postId)
        {
            string textOri = __instance.getPostPostsById().contentText.StrToI2Str();
            string text = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.PostTextLocList, textOri);
            __result = text.Replace("<boardRewardItem>",
                __instance.getPostPostsById().getBoardRewardItem(postId)).Replace("<boardHuntRequestAnimal>",
                __instance.getPostPostsById().getBoardHuntRequestAnimal(postId)).Replace("<getAnimalsInPhotoList>",
                __instance.getPostPostsById().getRequirementsNeededInPhoto(postId)).Replace("<boardRequestItem>",
                __instance.getPostPostsById().getBoardRequestItem(postId));
        }

        ////??
        //[HarmonyPostfix, HarmonyPatch(typeof(MailManager), "showLetter")]
        //public static void PostOnBoard_showLetter_Patch(MailManager __instance)
        //{
        //    __instance.letterText.text = KoreanCheck.ReplaceJosa(__instance.letterText.text);
        //}

        //ADD
        [HarmonyPostfix, HarmonyPatch(typeof(ConversationManager), "readConversationSegment")]
        public static void ConversationManager_readConversationSegment(ConversationManager __instance)
        {
            string text = __instance.conversationTextPro.text;
            __instance.conversationTextPro.text = KoreanCheck.ReplaceJosa(text);
        }


        [HarmonyPostfix, HarmonyPatch(typeof(AnimalHouseMenu), "openConfirm")]
        public static void AnimalHouseMenu_openConfirm_Patch(AnimalHouseMenu __instance, ref FarmAnimalDetails ___showingAnimal)
        {
            string text = ___showingAnimal.animalName + "(을)를 <sprite=11>" + __instance.getSellValue().ToString("n0") + "에 판매할까요?";
            text = KoreanCheck.ReplaceJosa(text);
            __instance.confirmText.text = text;
        }

        //ADD
        [HarmonyPostfix, HarmonyPatch(typeof(Quest), "getMissionObjText")]
        public static void Quest_getMissionObjText_Patch(Quest __instance, ref string __result)
        {
            string ret = "";

            if (__instance.attractResidentsQuest)
            {
                ret = "<sprite=12> " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "(와)과 대화하기";

                if (NPCManager.manage.getNoOfNPCsMovedIn() < 5)
                    ret = "<sprite=12> " + Inventory.inv.islandName + "(으)로 이주할 영주권자 총 5명을 유치하세요 [ " + NPCManager.manage.getNoOfNPCsMovedIn() + "/5]";

                if (BuildingManager.manage.currentlyMoving == __instance.requiredBuilding[0].tileObjectId)
                    ret = "<sprite=12> 베이스 텐트가 이동되면 " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "(와)과 대화하세요";
            }

            if (__instance.questToUseChanger)
            {
                ret = "<sprite=12> 베이스 텐트의 제작대에서 " + __instance.placeableToPlace.getInvItemName() + "(을)를 제작하세요.\n<sprite=12> " + __instance.placeableToPlace.getInvItemName() + "(을)를 바깥쪽에 배치하세요.\n<sprite=12> 주석광석을 " + __instance.placeableToPlace.getInvItemName() + "에 넣고 " + __instance.requiredItems[0].getInvItemName() + "(이)가 될 때까지 기다립니다.\n<sprite=12> " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "에게 " + __instance.requiredItems[0].getInvItemName() + "(을)를 가져갑니다.";

                if (__instance.checkIfHasAllRequiredItems())
                    ret = "<sprite=13> 베이스 텐트의 제작대에서 " + __instance.placeableToPlace.getInvItemName() + "(을)를 제작하세요.\n<sprite=13> " + __instance.placeableToPlace.getInvItemName() + "(을)를 바깥쪽에 배치하세요.\n<sprite=13> 주석광석을 " + __instance.placeableToPlace.getInvItemName() + "에 넣고 " + __instance.requiredItems[0].getInvItemName() + "(이)가 될 때까지 기다립니다.\n<sprite=12> " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "에게 " + __instance.requiredItems[0].getInvItemName() + "(을)를 가져갑니다.";

                if (__instance.checkIfHasBeenPlaced())
                    ret = "<sprite=13> 베이스 텐트의 제작대에서 " + __instance.placeableToPlace.getInvItemName() + "(을)를 제작하세요.\n<sprite=13> " + __instance.placeableToPlace.getInvItemName() + "(을)를 바깥쪽에 배치하세요.\n<sprite=12> 주석광석을 " + __instance.placeableToPlace.getInvItemName() + "에 넣고 " + __instance.requiredItems[0].getInvItemName() + "(이)가 될 때까지 기다립니다.\n<sprite=12> " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "에게 " + __instance.requiredItems[0].getInvItemName() + "(을)를 가져갑니다.";

                if (Inventory.inv.getAmountOfItemInAllSlots(Inventory.inv.getInvItemId(__instance.placeableToPlace)) >= 1)
                    ret = "<sprite=13> 베이스 텐트의 제작대에서 " + __instance.placeableToPlace.getInvItemName() + "(을)를 제작하세요.\n<sprite=12> " + __instance.placeableToPlace.getInvItemName() + "(을)를 바깥쪽에 배치하세요.\n<sprite=12> 주석광석을 " + __instance.placeableToPlace.getInvItemName() + "에 넣고 " + __instance.requiredItems[0].getInvItemName() + "(이)가 될 때까지 기다립니다.\n<sprite=12> " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "에게 " + __instance.requiredItems[0].getInvItemName() + "(을)를 가져갑니다.";
            }

            if (__instance.placeOrHaveItem)
            {
                ret = "<sprite=12> " + __instance.requiredItems[0].getInvItemName() + "(을)를 구입하세요.\n" + "<sprite=12> " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "(와)과 대화하기";

                if (__instance.checkIfHasInInvOrHasBeenPlaced())
                    ret = "<sprite=13> " + __instance.requiredItems[0].getInvItemName() + "(을)를 구입하세요.\n" + "<sprite=12> " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "(와)과 대화하기";
            }

            if (__instance.autoCompletesOnDate)
            {
                ret = "<sprite=12> " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "(와)과 대화하기";

                if (!__instance.isPastDate())
                    ret = "[선택 사항] 오늘의 할일 완료하기\n<sprite=12>침낭을 놓고 휴식을 취하세요.";
            }

            if (__instance.questForFood)
            {
                ret = "<sprite=12> 먹을거리 찾기.\n" + "<sprite=12> " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "(와)과 대화하기";

                if (__instance.checkIfFoodInInv())
                    ret = "<sprite=13> 먹을거리 찾기.\n" + "<sprite=12> " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "(와)과 대화하기";
            }

            if (__instance.requiredItems.Length != 0)
            {
                string text = "\t";
                for (int i = 0; i < __instance.requiredItems.Length; i++)
                {
                    text = text + "\n[" + Inventory.inv.getAmountOfItemInAllSlots(Inventory.inv.getInvItemId(__instance.requiredItems[i])) + "/" + __instance.requiredStacks[i] + "] " + __instance.requiredItems[i].getInvItemName();
                }

                ret = "<sprite=12> 요청한 아이템을 수집하세요." + text + "\n" + "<sprite=12> " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "에게 가져갑니다";

                if (__instance.checkIfHasAllRequiredItems())
                {
                    ret = "<sprite=13> 요청한 아이템을 수집하세요.\n" + "<sprite=12> " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "에게 가져갑니다";
                }
            }

            if ((bool)__instance.deedToApplyFor)
            {
                ret = "<sprite=12> 계약 장소의 건설 상자에 필요한 아이템을 넣습니다.";

                if (!DeedManager.manage.checkIfDeedHasBeenBought(__instance.deedToApplyFor))
                {
                    ret = "<sprite=12> " + __instance.deedToApplyFor.getInvItemName() + "(을)를 신청할 마을에 대해 " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "에게 물어보세요";
                }

                if (!__instance.checkIfHasBeenPlaced())
                {
                    ret = "<sprite=12> " + __instance.deedToApplyFor.getInvItemName() + "(을)를 배치하세요";
                }

                if (DeedManager.manage.checkIfDeedMaterialsComplete(__instance.deedToApplyFor))
                {
                    ret = "<sprite=12> " + __instance.deedToApplyFor.getInvItemName() + "의 건설이 완료될 때까지 기다리세요";
                }
            }

            if (__instance.requiredBuilding != null && __instance.placeableToPlace != null)
            {
                ret = "<sprite=13> " + __instance.placeableToPlace.getInvItemName() + "(을)를 배치하세요\n" + "<sprite=12> " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "(와)과 대화하기";

                if (!__instance.checkIfHasBeenPlaced())
                {
                    ret = "<sprite=12> " + __instance.placeableToPlace.getInvItemName() + "(을)를 배치하세요\n" + "<sprite=12> " + NPCManager.manage.NPCDetails[__instance.offeredByNpc].NPCName + "(와)과 대화하기";
                }
            }

            if (!string.IsNullOrEmpty(ret))
                __result = KoreanCheck.ReplaceJosa(ret);
            else
                __result = "";
        }

        [HarmonyPostfix, HarmonyPatch(typeof(QuestButton), "setUp")]
        public static void QuestButton_setUp_Patch(QuestButton __instance, int questNo)
        {
            if (__instance.isMainQuestButton)
            {
                string nameOri = QuestManager.manage.allQuests[questNo].QuestName.StrToI2Str();
                string name = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.QuestTextLocList, nameOri);
                __instance.buttonText.text = name;
            }
            else if (__instance.isQuestButton)
            {
            }
            else
            {
                __instance.buttonText.text = __instance.buttonText.text.Replace("Request for ", "") + "의 요청";
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(QuestNotification), "showQuest")]
        public static void QuestNotification_showQuest_Patch(QuestNotification __instance)
        {
            string nameOri = __instance.displayingQuest.QuestName.StrToI2Str();
            string name = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.QuestTextLocList, nameOri);
            __instance.QuestText.text = name;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(QuestTracker), "displayMainQuest")]
        public static void QuestTracker_displayMainQuest_Patch(QuestTracker __instance, int questNo)
        {
            string nameOri = QuestManager.manage.allQuests[questNo].QuestName.StrToI2Str();
            string name = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.QuestTextLocList, nameOri);
            string descOri = QuestManager.manage.allQuests[questNo].QuestDescription.StrToI2Str();
            string desc = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.QuestTextLocList, descOri).Replace("<IslandName>", Inventory.inv.islandName);
            __instance.questTitle.text = name;
            __instance.questDesc.text = KoreanCheck.ReplaceJosa(desc);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(QuestTracker), "pinTheTask")]
        public static void QuestTracker_pinTheTask_Patch(QuestTracker __instance, QuestTracker.typeOfTask type, int id)
        {
            if (type == QuestTracker.typeOfTask.Quest)
            {
                if (!QuestManager.manage.isQuestCompleted[id])
                {
                    string nameOri = QuestManager.manage.allQuests[id].QuestName.StrToI2Str();
                    string name = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.QuestTextLocList, nameOri);
                    string pinText = __instance.pinMissionText.text.Replace(QuestManager.manage.allQuests[id].QuestName, name);
                    __instance.pinMissionText.text = pinText;
                }
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(QuestTracker), "updatePinnedTask")]
        public static void QuestTracker_updatePinnedTask_Patch(QuestTracker __instance, ref QuestTracker.typeOfTask ___pinnedType, ref int ___pinnedId)
        {
            if (___pinnedType == QuestTracker.typeOfTask.Quest)
            {
                string nameOri = QuestManager.manage.allQuests[___pinnedId].QuestName;
                string name = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.QuestTextLocList, nameOri);
                __instance.pinMissionText.text = name + "\n<size=11>" + QuestManager.manage.allQuests[___pinnedId].getMissionObjText();
            }

            if (___pinnedType == QuestTracker.typeOfTask.Request)
            {
                if (!NPCManager.manage.npcStatus[___pinnedId].completedRequest)
                {
                    __instance.pinMissionText.text = NPCManager.manage.NPCDetails[___pinnedId].NPCName + "의 요청\n<size=11>" + NPCManager.manage.NPCRequests[___pinnedId].getMissionText(___pinnedId);
                }
            }
        }

        //ADD
        [HarmonyPostfix, HarmonyPatch(typeof(QuestTracker), "displayRequest")]
        public static void QuestTracker_displayRequest_Patch(QuestTracker __instance, int requestNo)
        {
            __instance.questTitle.text = NPCManager.manage.NPCDetails[requestNo].NPCName + "의 요청";
            string text = KoreanCheck.ReplaceJosa(NPCManager.manage.NPCDetails[requestNo].NPCName + "(이)가 " + NPCManager.manage.NPCRequests[requestNo].getDesiredItemName() + "(을)를 가져와 달라고 요청했습니다");
            __instance.questDesc.text = text;
            __instance.questDateGiven.text = "하루가 끝나기 전까지";
        }

        //ADD
        [HarmonyPostfix, HarmonyPatch(typeof(BulletinBoard), "getMissionText")]
        public static void BulletinBoard_getMissionText_Patch(BulletinBoard __instance, int missionNo, ref string __result)
        {
            var attachedPosts = __instance.attachedPosts;
            BullitenBoardPost postPostsById = attachedPosts[missionNo].getPostPostsById();
            string text = "";
            if (attachedPosts[missionNo].isTrade)
            {
                text = attachedPosts[missionNo].getPostPostsById().getBoardRequestItem(missionNo) + "(와)과 " + attachedPosts[missionNo].getPostedByName() + "교환하기";
                text = KoreanCheck.ReplaceJosa(text);
            }

            if (attachedPosts[missionNo].isHuntingTask)
            {
                if (attachedPosts[missionNo].readyForNPC)
                {
                    text = "<sprite=12> " + attachedPosts[missionNo].getPostedByName() + "(와)과 대화하기";
                    text = KoreanCheck.ReplaceJosa(text);
                }

                text = "<sprite=12> 지도에서 마지막으로 알려진 위치를 이용해서 " + postPostsById.getBoardHuntRequestAnimal(missionNo) + "(을)를 사냥하세요";
                text = KoreanCheck.ReplaceJosa(text);
            }

            if (attachedPosts[missionNo].isInvestigation)
            {
                text = "<sprite=13> 지도의 위치를 방문하여 조사하세요";
                text = KoreanCheck.ReplaceJosa(text);
            }

            __result = text;
        }

        //ADD
        [HarmonyPostfix, HarmonyPatch(typeof(Task), MethodType.Constructor, new Type[] { typeof(int), typeof(int) })]
        public static void Task_Constructor_Patch(Task __instance, int firstDailyTaskNo, int taskIdMax)
        {
            var tileObjectToInteract = __instance.tileObjectToInteract;
            var requiredPoints = __instance.requiredPoints;
            string missionText = "";

            if (firstDailyTaskNo == 0)
            {
                if ((bool)tileObjectToInteract.tileObjectGrowthStages.harvestDrop)
                {
                    missionText = tileObjectToInteract.tileObjectGrowthStages.harvestDrop.getInvItemName() + " " + requiredPoints + "개 수확하기";
                }
                else
                {
                    missionText = tileObjectToInteract.name + " 수확하기";
                }
            }

            if (firstDailyTaskNo == 1)
            {
                missionText = "곤충 " + requiredPoints + "마리 잡기";
            }

            if (firstDailyTaskNo == 2)
            {
                missionText = "아이템 " + requiredPoints + "개 제작하기";
            }

            __instance.missionText = missionText;
        }

        //ADD
        [HarmonyPostfix, HarmonyPatch(typeof(Task), "generateTask")]
        public static void QuestTracker_generateTask_Patch(Task __instance)
        {
            int taskTypeId = __instance.taskTypeId;
            int requiredPoints = __instance.requiredPoints;
            var tileObjectToInteract = __instance.tileObjectToInteract;
            string missionText = "";
            if (1 == taskTypeId)
            {
                if ((bool)tileObjectToInteract.tileObjectGrowthStages.harvestDrop)
                {
                    missionText = tileObjectToInteract.tileObjectGrowthStages.harvestDrop.getInvItemName() + " " + requiredPoints + "개 수확";
                }
                else
                {
                    missionText = tileObjectToInteract.name + " 수확";
                }
            }
            else if (2 == taskTypeId && NPCManager.manage.getNoOfNPCsMovedIn() > 0)
            {
                missionText = "주민 " + requiredPoints + "명과 대화";
            }
            else if (37 == taskTypeId)
            {
                missionText = "과일 " + requiredPoints + "개 묻기";
            }
            else if (73 == taskTypeId)
            {
                missionText = "조개껍질 " + requiredPoints + "개 수집";
            }
            else if (WorldManager.manageWorld.day != 1 && 34 == taskTypeId)
            {
                if (!NPCManager.manage.NPCDetails[3].mySchedual.dayOff[WorldManager.manageWorld.day - 1])
                {
                    missionText = "조개껍질 " + requiredPoints + "개 팔기";
                }
            }
            else if (WorldManager.manageWorld.day != 1 && 87 == taskTypeId)
            {
                if (!NPCManager.manage.NPCDetails[3].mySchedual.dayOff[WorldManager.manageWorld.day - 1])
                {
                    missionText = "과일 " + requiredPoints + "개 팔기";
                }
            }
            else if (42 == taskTypeId)
            {
                missionText = "누군가를 위해 일하기";
            }
            else if (59 == taskTypeId)
            {
                missionText = "야생종자 " + requiredPoints + "개 심기";
            }
            else if (60 == taskTypeId && LicenceManager.manage.allLicences[16].hasALevelOneOrHigher())
            {
                missionText = "땅을 " + requiredPoints + "번 파기";
            }
            else if (3 == taskTypeId)
            {
                missionText = "곤충 " + requiredPoints + "번 잡기";
            }
            else if (WorldManager.manageWorld.day != 1 && 33 == taskTypeId)
            {
                if (!NPCManager.manage.NPCDetails[3].mySchedual.dayOff[WorldManager.manageWorld.day - 1])
                {
                    missionText = "곤충 " + requiredPoints + "번 팔기";
                }
            }
            else if (4 == taskTypeId)
            {
                missionText = "아이템 " + requiredPoints + "번 제작";
            }
            else if (5 == taskTypeId)
            {
                missionText = "무언가 먹기";
            }
            else if (WorldManager.manageWorld.day != 1 && 8 == taskTypeId && !NPCManager.manage.NPCDetails[1].mySchedual.dayOff[WorldManager.manageWorld.day - 1])
            {
                missionText = Inventory.inv.moneyItem.getInvItemName() + " " + requiredPoints + " 얻기";
            }
            else if (7 == taskTypeId)
            {
                missionText = Inventory.inv.moneyItem.getInvItemName() + " " + requiredPoints + " 소비";
            }
            else if (6 == taskTypeId)
            {
                missionText = "도보로 " + requiredPoints + "m 이동";
            }
            else if (62 == taskTypeId && LicenceManager.manage.allLicences[7].hasALevelOneOrHigher())
            {
                missionText = "탑승물로 " + requiredPoints + "m 이동";
            }
            else if (9 == taskTypeId)
            {
                missionText = "고기 " + requiredPoints + "개 요리";
            }
            else if (29 == taskTypeId)
            {
                missionText = "과일 " + requiredPoints + "개 요리";
            }
            else if (30 == taskTypeId)
            {
                missionText = "조리대에서 뭔가 요리하기";
            }
            else if (31 == taskTypeId)
            {
                missionText = "나무씨았 " + requiredPoints + "개 심기";
            }
            else if (10 == taskTypeId && LicenceManager.manage.allLicences[11].hasALevelOneOrHigher())
            {
                missionText = "작물종자 " + requiredPoints + "개 심기";
            }
            else if (11 == taskTypeId && LicenceManager.manage.allLicences[11].hasALevelOneOrHigher() && !WeatherManager.manage.raining)
            {
                missionText = "작물에 " + requiredPoints + "번 물주기";
            }
            else if (12 == taskTypeId && LicenceManager.manage.allLicences[1].hasALevelOneOrHigher())
            {
                missionText = "암석을 " + requiredPoints + "번 부수기";
            }
            else if (13 == taskTypeId && LicenceManager.manage.allLicences[1].hasALevelOneOrHigher())
            {
                missionText = "광석을 " + requiredPoints + "번 부수기";
            }
            else if (14 == taskTypeId && LicenceManager.manage.allLicences[1].hasALevelOneOrHigher())
            {
                missionText = "광물을 주괴로 녹이기";
            }
            else if (15 == taskTypeId && NPCManager.manage.npcStatus[1].checkIfHasMovedIn() && LicenceManager.manage.allLicences[1].hasALevelOneOrHigher())
            {
                missionText = "암석을 " + requiredPoints + "번 갈기";
            }
            else if (16 == taskTypeId && LicenceManager.manage.allLicences[2].hasALevelOneOrHigher())
            {
                missionText = "나무 " + requiredPoints + "그루 자르기";
            }
            else if (17 == taskTypeId && LicenceManager.manage.allLicences[2].hasALevelOneOrHigher())
            {
                missionText = "그루터기 " + requiredPoints + "개 정리";
            }
            else if (18 == taskTypeId && LicenceManager.manage.allLicences[2].hasALevelOneOrHigher())
            {
                missionText = "판자로 " + requiredPoints + "번 제재";
            }
            else if (19 == taskTypeId && LicenceManager.manage.allLicences[3].hasALevelOneOrHigher())
            {
                missionText = "물고기 " + requiredPoints + "번 잡기";
            }
            else if (WorldManager.manageWorld.day != 1 && 32 == taskTypeId && LicenceManager.manage.allLicences[3].hasALevelOneOrHigher())
            {
                if (!NPCManager.manage.NPCDetails[3].mySchedual.dayOff[WorldManager.manageWorld.day - 1])
                {
                    missionText = "물고기 " + requiredPoints + "번 팔기";
                }
            }
            else if (20 == taskTypeId && LicenceManager.manage.allLicences[2].hasALevelOneOrHigher())
            {
                missionText = "풀 " + requiredPoints + "번 정리";
            }
            else if (39 == taskTypeId && FarmAnimalManager.manage.isThereAtleastOneActiveAgent())
            {
                missionText = "동물 쓰다듬기";
            }
            else if (28 == taskTypeId && (bool)TownManager.manage.allShopFloors[6])
            {
                if (!NPCManager.manage.NPCDetails[4].mySchedual.dayOff[WorldManager.manageWorld.day - 1])
                {
                    missionText = "새로운 옷 구입";
                }
            }
            else if (23 == taskTypeId && (bool)TownManager.manage.allShopFloors[10])
            {
                if (!NPCManager.manage.NPCDetails[3].mySchedual.dayOff[WorldManager.manageWorld.day - 1])
                {
                    missionText = "새로운 가구 구입";
                }
            }
            else if (24 == taskTypeId && (bool)TownManager.manage.allShopFloors[10])
            {
                if (!NPCManager.manage.NPCDetails[3].mySchedual.dayOff[WorldManager.manageWorld.day - 1])
                {
                    missionText = "새로운 벽지 구입";
                }
            }
            else if (25 == taskTypeId && (bool)TownManager.manage.allShopFloors[10])
            {
                if (!NPCManager.manage.NPCDetails[3].mySchedual.dayOff[WorldManager.manageWorld.day - 1])
                {
                    missionText = "새로운 바닥재 구입";
                }
            }
            else if (WorldManager.manageWorld.getNoOfCompletedCrops() >= 4 && 27 == taskTypeId && LicenceManager.manage.allLicences[11].hasALevelOneOrHigher())
            {
                missionText = "작물 " + requiredPoints + "번 수확";
            }
            else if (WorldManager.manageWorld.day != 1 && WorldManager.manageWorld.getNoOfCompletedCrops() >= 4 && 35 == taskTypeId && LicenceManager.manage.allLicences[11].hasALevelOneOrHigher())
            {
                missionText = "작물 " + requiredPoints + "번 팔기";
            }
            else if (76 == taskTypeId && LicenceManager.manage.allLicences[11].getCurrentLevel() >= 2)
            {
                missionText = "무언가를 퇴비로 만들기";
            }
            else if (61 == taskTypeId)
            {
                missionText = "새로운 도구 제작";
            }
            else if (26 == taskTypeId && (bool)TownManager.manage.allShopFloors[11])
            {
                if (!NPCManager.manage.NPCDetails[0].mySchedual.dayOff[WorldManager.manageWorld.day - 1])
                {
                    missionText = "종자 " + requiredPoints + "개 구입";
                }
            }
            else if (22 == taskTypeId && LicenceManager.manage.allLicences[15].hasALevelOneOrHigher())
            {
                missionText = "동물 포획 및 운반";
            }
            else if (21 == taskTypeId && LicenceManager.manage.allLicences[4].hasALevelOneOrHigher())
            {
                missionText = "동물 " + requiredPoints + "번 사냥";
            }
            else if (51 == taskTypeId && !NPCManager.manage.NPCDetails[1].mySchedual.dayOff[WorldManager.manageWorld.day - 1])
            {
                missionText = "새로운 도구 구입";
            }
            else if (Inventory.inv.checkIfToolNearlyBroken() && 52 == taskTypeId)
            {
                missionText = "도구 부수기";
            }
            else if (36 == taskTypeId && LicenceManager.manage.allLicences[6].hasALevelOneOrHigher())
            {
                missionText = "묻힌 보물찾기";
            }
            else
            {
                missionText = "임무 세트 없음";
            }

            if (!string.IsNullOrEmpty(missionText))
                __instance.missionText = missionText;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PickUpNotification), "fillButtonPrompt")]
        public static void PickUpNotification_fillButtonPrompt_Patch(PickUpNotification __instance, string buttonPromptText)
        {
            string text = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.DynamicTextLocList, buttonPromptText.Trim());
            if (text.Contains("구입"))
            {
                text = text.Replace("구입", "") + " 구입";
            }
            if (text.Contains("대화하기"))
            {
                text = text.Replace("대화하기", "") + " 대화하기";
            }
            __instance.itemText.text = text.Trim();
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
            __result = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.DynamicTextLocList, __result);
        }

        [HarmonyPrefix, HarmonyPatch(typeof(SeasonAndTime), "capitaliseFirstLetter")]
        public static bool SeasonAndTime_capitaliseFirstLetter_Patch(ref string __result, string toChange)
        {
            Debug.Log($"SeasonAndTime_capitaliseFirstLetter_Patch 1:{toChange}");
            __result = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.DynamicTextLocList, toChange);
            Debug.Log($"SeasonAndTime_capitaliseFirstLetter_Patch 2:{__result}");
            return false;
        }

        // ADD
        [HarmonyPostfix, HarmonyPatch(typeof(NPCRequest), "getMissionText")]
        public static void NPCRequest_getMissionText_Patch(NPCRequest __instance, int npcId, ref string __result)
        {
            __result = "<sprite=12> " + NPCManager.manage.NPCDetails[npcId].NPCName + "에게 " + __instance.getDesiredItemName() + " 가져다주기";

            if (__instance.specificDesiredItem != -1)
            {
                string text = " [" + __instance.checkAmountOfItemsInInv() + "/" + __instance.desiredAmount + "]";
                if (__instance.checkAmountOfItemsInInv() >= __instance.desiredAmount)
                {
                    __result = "<sprite=13> " + __instance.getDesiredItemName() + " 수집하기" + text + "\n<sprite=12> " + NPCManager.manage.NPCDetails[npcId].NPCName + "에게 " + __instance.getDesiredItemName() + " 가져다주기";
                }

                __result = "<sprite=12> " + __instance.getDesiredItemName() + " 수집하기" + text + "\n<sprite=12> " + NPCManager.manage.NPCDetails[npcId].NPCName + "에게 " + __instance.getDesiredItemName() + " 가져다주기";
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(NPCRequest), "setRandomBugNoAndLocation")]
        public static void NPCRequest_setRandomBugNoAndLocation_Patch(NPCRequest __instance)
        {
            __instance.itemFoundInLocation = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.DynamicTextLocList, __instance.itemFoundInLocation);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(NPCRequest), "setRandomFishNoAndLocation")]
        public static void NPCRequest_setRandomFishNoAndLocation_Patch(NPCRequest __instance)
        {
            __instance.itemFoundInLocation = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.DynamicTextLocList, __instance.itemFoundInLocation);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(AnimalManager), "fillAnimalLocation")]
        public static void AnimalManager_fillAnimalLocation_Patch(ref string __result)
        {
            Debug.Log($"AnimalManager_fillAnimalLocation_Patch 1:{__result}");
            __result = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.DynamicTextLocList, __result);
            Debug.Log($"AnimalManager_fillAnimalLocation_Patch 2:{__result}");
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AnimalManager), "capitaliseFirstLetter")]
        public static bool AnimalManager_capitaliseFirstLetter_Patch(ref string __result, string toChange)
        {
            Debug.Log($"AnimalManager_capitaliseFirstLetter_Patch 1:{toChange}");
            __result = TextLocData.GetLoc(DinkumKoreanPlugin.Inst.DynamicTextLocList, toChange);
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
                    text2 += "에";
                    if (_this.allItems[itemId].placeable.tileObjectGrowthStages.growsInSummer)
                    {
                        text2 = "여름" + text2;
                    }
                    if (_this.allItems[itemId].placeable.tileObjectGrowthStages.growsInAutum)
                    {
                        if (text2 != "에")
                        {
                            text2 = text2.Insert(text2.Length - 2, "과 ");
                        }
                        text2 = text2.Insert(text2.Length - 2, "가을");

                    }
                    if (_this.allItems[itemId].placeable.tileObjectGrowthStages.growsInWinter)
                    {
                        if (text2 != "에")
                        {
                            text2 = text2.Insert(text2.Length - 2, "과 ");
                        }
                        text2 = text2.Insert(text2.Length - 2, "겨울");

                    }
                    if (_this.allItems[itemId].placeable.tileObjectGrowthStages.growsInSpring)
                    {
                        if (text2 != "에")
                        {
                            text2 = text2.Insert(text2.Length - 2, "과 ");
                        }
                        text2 = text2.Insert(text2.Length - 2, "봄");

                    }
                }
                if (_this.allItems[itemId].placeable.tileObjectGrowthStages.needsTilledSoil)
                {
                    text = text + "이것들은 " + text2 + " 자랍니다.";
                }
                if (_this.allItems[itemId].placeable.tileObjectGrowthStages.objectStages.Length != 0)
                {
                    if (_this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto)
                    {
                        text = string.Concat(new string[]
                        {
                        text,
                        "<b>",
                        _this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto.tileObjectGrowthStages.harvestSpots.Length.ToString(),
                        "</b>(으)로 <b>",
                        _this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto.tileObjectGrowthStages.harvestDrop.getInvItemName(),
                        "</b>(이)가 나오므로 주변에 공간이 필요합니다. 이 작물은 최대 4번의 파생물을 가질수 있는 기회가 있습니다"
                        });
                        text = KoreanCheck.ReplaceJosa(text);
                    }
                    else
                    {
                        text = string.Concat(new string[]
                        {
                        text,
                        _this.allItems[itemId].placeable.tileObjectGrowthStages.objectStages.Length.ToString(),
                        "일 동안 자랍니다. ",
                        _this.allItems[itemId].placeable.tileObjectGrowthStages.harvestSpots.Length.ToString(),
                        "개의 ",
                        _this.allItems[itemId].placeable.tileObjectGrowthStages.harvestDrop.getInvItemName(),
                        "(을)를 생산합니다"
                        });
                        text = KoreanCheck.ReplaceJosa(text);
                    }
                }
                if (!_this.allItems[itemId].placeable.tileObjectGrowthStages.diesOnHarvest && !_this.allItems[itemId].placeable.tileObjectGrowthStages.steamsOutInto)
                {
                    text = string.Concat(new string[]
                    {
                    text,
                    Mathf.Abs(_this.allItems[itemId].placeable.tileObjectGrowthStages.takeOrAddFromStateOnHarvest).ToString(),
                    "일마다 ",
                    _this.allItems[itemId].placeable.tileObjectGrowthStages.harvestSpots.Length.ToString(),
                    "개의 ",
                    _this.allItems[itemId].placeable.tileObjectGrowthStages.harvestDrop.getInvItemName(),
                    "(을)를 수확할 수 있습니다."
                    });
                    text = KoreanCheck.ReplaceJosa(text);
                }
                if (!WorldManager.manageWorld.allObjectSettings[_this.allItems[itemId].placeable.tileObjectId].walkable)
                {
                    text += " 아, 여기에 붙어서 자라기 위한 식물 받침대도 필요합니다.";
                }
            }
            __result = KoreanCheck.ReplaceJosa(text);
            return false;
        }

        //ADD
        [HarmonyPostfix, HarmonyPatch(typeof(InventoryItemDescription), "fillItemDescription")]
        public static void InventoryItemDescription_fillItemDescription_Patch(InventoryItemDescription __instance, InventoryItem item)
        {
            if (((bool)item.placeable && (bool)item.placeable.sprinklerTile) || ((bool)item.placeable && item.placeable.tileObjectId == 16))
            {
                __instance.reachTiles.SetActive(value: true);
                if (item.placeable.tileObjectId == 16)
                {
                    __instance.reachTileText.text = "최대 12개의 타일에 대해 특정 생산 장치의 속도를 높입니다";
                }
                else if (!item.placeable.sprinklerTile.isTank && !item.placeable.sprinklerTile.isSilo)
                {
                    __instance.reachTileText.text = item.placeable.sprinklerTile.verticlSize + "개의 타일에 도달합니다.\n<color=red>물탱크가 필요합니다</color>";
                }
                else if (item.placeable.sprinklerTile.isTank)
                {
                    __instance.reachTileText.text = "스프링클러에 물을 " + item.placeable.sprinklerTile.verticlSize + "타일 밖으로 제공합니다.";
                }
                else if (item.placeable.sprinklerTile.isSilo)
                {
                    __instance.reachTileText.text = "동물 사료통을 " + item.placeable.sprinklerTile.verticlSize + "타일 밖으로 채웁니다.\n<color=red>동물사료가 필요합니다</color>";
                }
            }
        }

        //ADD
        [HarmonyPostfix, HarmonyPatch(typeof(BugAndFishCelebration), "openWindow")]
        public static void BugAndFishCelebration_openWindow_Patch(BugAndFishCelebration __instance, int invItem)
        {
            string text = Inventory.inv.allItems[invItem].getInvItemName() + "(을)를 잡았다!";
            text = KoreanCheck.ReplaceJosa(text);
            __instance.celebrationText.text = text;
        }


    }
}