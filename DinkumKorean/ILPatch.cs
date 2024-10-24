﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace DinkumKorean
{
    public static class ILPatch
    {
        [HarmonyTranspiler, HarmonyPatch(typeof(AnimalHouseMenu), "fillData")]
        public static IEnumerable<CodeInstruction> AnimalHouseMenu_fillData_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, " Year", " 년");
            instructions = ReplaceIL(instructions, " Month", " 월");
            instructions = ReplaceIL(instructions, " Day", " 일");
            instructions = ReplaceIL(instructions, "s", "");
            instructions = ReplaceIL(instructions, "SELL ", "판매 ");
            return instructions;
        }

        //[HarmonyTranspiler, HarmonyPatch(typeof(AnimalHouseMenu), "openConfirm")]
        //public static IEnumerable<CodeInstruction> AnimalHouseMenu_openConfirm_Patch(IEnumerable<CodeInstruction> instructions)
        //{
        //    instructions = ReplaceIL(instructions, "Sell ", "판매 ");
        //    instructions = ReplaceIL(instructions, " for <sprite=11>", " 으로 <sprite=11>");
        //    return instructions;
        //}

        [HarmonyTranspiler, HarmonyPatch(typeof(BankMenu), "convertButton")]
        public static IEnumerable<CodeInstruction> BankMenu_convertButton_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Convert [<sprite=11> 500 for <sprite=15> 1]", "[<sprite=11> 500을 <sprite=15> 1로] 변환");
            return instructions;
        }

        //[HarmonyTranspiler, HarmonyPatch(typeof(BulletinBoard), "getMissionText")]
        //public static IEnumerable<CodeInstruction> BulletinBoard_getMissionText_Patch(IEnumerable<CodeInstruction> instructions)
        //{
        //    instructions = ReplaceIL(instructions, "<sprite=12> Speak to ", "<sprite=12> 대화 ");
        //    instructions = ReplaceIL(instructions, "<sprite=12> Hunt down the ", "<sprite=12> 사냥 ");
        //    instructions = ReplaceIL(instructions, " using its last know location on the map", " 지도에서 마지막으로 알려진 위치 사용");
        //    instructions = ReplaceIL(instructions, "<sprite=13> Visit the location on the map to investigate", "<sprite=13> 지도의 위치를 방문하여 조사");
        //    return instructions;
        //}

        [HarmonyTranspiler, HarmonyPatch(typeof(BulletinBoard), "showSelectedPost")]
        public static IEnumerable<CodeInstruction> BulletinBoard_showSelectedPost_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "EXPIRED", "만료됨");
            instructions = ReplaceIL(instructions, " Last Day", " 마지막날");
            instructions = ReplaceIL(instructions, " Days Remaining", " 일 남음");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(BullitenBoardPost), "getBoardRequestItem")]
        public static IEnumerable<CodeInstruction> BullitenBoardPost_getBoardRequestItem_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "any other furniture", "기타 모든 가구");
            instructions = ReplaceIL(instructions, "any other clothing", "기타 모든 의류");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(TownEventManager), "RunIslandDay", MethodType.Enumerator)]
        public static IEnumerable<CodeInstruction> TownEventManager_RunIslandDay_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "It's ", "오늘은 ");
            instructions = ReplaceIL(instructions, " Day!", " 입니다!");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(ConversationManager), "CheckLineForReplacement")]
        public static IEnumerable<CodeInstruction> ConversationManager_checkLineForReplacement_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "South City", "사우스 시티");
            instructions = ReplaceIL(instructions, "Animal Type", "동물 유형");
            instructions = ReplaceIL(instructions, "Animal Name", "동물 이름");
            instructions = ReplaceIL(instructions, "Dinks", "딩크");
            instructions = ReplaceIL(instructions, "Dink", "딩크");
            instructions = ReplaceIL(instructions, "Journal", "저널");
            instructions = ReplaceIL(instructions, "Licence", "면허증");
            instructions = ReplaceIL(instructions, "Licences", "면허증");
            instructions = ReplaceIL(instructions, "Shift", "이동");
            instructions = ReplaceIL(instructions, "Shifts", "이동");
            instructions = ReplaceIL(instructions, "Airship", "비행선");
            instructions = ReplaceIL(instructions, "Airships", "비행선");
            instructions = ReplaceIL(instructions, "Nomad", "방랑자");
            instructions = ReplaceIL(instructions, "Nomads", "방랑자");
            instructions = ReplaceIL(instructions, "Birthday", "생일");
            instructions = ReplaceIL(instructions, "Permit Points", "허가 점수");
            instructions = ReplaceIL(instructions, "Snag Sizzles", "소시지 구이");
            instructions = ReplaceIL(instructions, "Snag Sizzle", "소시지 구이");
            instructions = ReplaceIL(instructions, "Snag", "소시지");
            instructions = ReplaceIL(instructions, "Snags", "소시지");
            instructions = ReplaceIL(instructions, " Day", " 일");
            instructions = ReplaceIL(instructions, "Sky Fest", "하늘 축제");
            instructions = ReplaceIL(instructions, "Kite", "연");
            instructions = ReplaceIL(instructions, "Flying Lantern", "풍등");
            instructions = ReplaceIL(instructions, "Flying Lanterns", "풍등");
            instructions = ReplaceIL(instructions, "Kite Making Table", "연 제작대");
            instructions = ReplaceIL(instructions, "Aurora", "오로라");
            instructions = ReplaceIL(instructions, " Year", " 년");
            instructions = ReplaceIL(instructions, "reward", "보상");
            instructions = ReplaceIL(instructions, "Animal", "동물");
            instructions = ReplaceIL(instructions, "I just love the colours!", "난 그냥 이 색깔들 좋아해요！");
            instructions = ReplaceIL(instructions, "I love this one.", "난 이것을 좋아해요.");
            instructions = ReplaceIL(instructions, "The composition is wonderful", "이 구성이 훌륭합니다");
            instructions = ReplaceIL(instructions, "It speaks to me, you know?", "저한테 말을 걸어요, 알겠죠?");
            instructions = ReplaceIL(instructions, "It makes me feel something...", "뭔가 느낌이 있어요...");
            instructions = ReplaceIL(instructions, "Made by hand by yours truly!", "정말 직접만들었군요！");
            instructions = ReplaceIL(instructions, "Finished that one off today!", "오늘은 이것으로 끝！");
            instructions = ReplaceIL(instructions, "It feels just right for you, ", "당신에게 딱 맞는 것 같아요, ");
            instructions = ReplaceIL(instructions, "The colour is very powerful, y'know?", "색이 아주 강렬하죠?");
            instructions = ReplaceIL(instructions, "It will open your chakras, y'know?", "그것은 당신의 차크라를 열 것입니다.");
            instructions = ReplaceIL(instructions, "Do you feel the engery coming from it?", "거기서 오는 에너지가 느껴지나요?");
            instructions = ReplaceIL(instructions, "I feel good things coming to whoever buys it.", "사시는 분에게 좋은 일이 생길 것 같아요.");
            instructions = ReplaceIL(instructions, "The design just came to me, y'know?", "디자인이 딱 떠올랐죠?");
            instructions = ReplaceIL(instructions, "Y'know, that would look great on you, ", "그게 당신에게 잘어울립니다, ");
            instructions = ReplaceIL(instructions, "I put a lot of myself into this one.", "저는 이 작품에 많은 노력을 기울였습니다.");
            instructions = ReplaceIL(instructions, "darl", "딩딩");
            instructions = ReplaceIL(instructions, "love", "뽀삐");
            instructions = ReplaceIL(instructions, "possum", "쪼꼬미");
            instructions = ReplaceIL(instructions, "Bug Catching Comp", "곤충 잡기 대회");
            instructions = ReplaceIL(instructions, "Fishing Comp", "낚시 대회");
            instructions = ReplaceIL(instructions, "Comp Log Book", "대회 안내서");
            instructions = ReplaceIL(instructions, "Beginning...", "시작중...");
            instructions = ReplaceIL(instructions, "...Nothing happened...", "...아무일도 없습니다...");
            instructions = ReplaceIL(instructions, "s", "");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(ExhibitSign), "Start")]
        public static IEnumerable<CodeInstruction> ExhibitSign_Start_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "This exhibit is currently empty.", "이 전시품 현재 비어 있습니다.");
            instructions = ReplaceIL(instructions, "We look forward to future donations!", "앞으로의 기부를 기대합니다！");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(ExhibitSign), "updateMySign")]
        public static IEnumerable<CodeInstruction> ExhibitSign_updateMySign_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "In this exhibit:", "전시품：");
            return instructions;
        }

        // todo
        [HarmonyTranspiler, HarmonyPatch(typeof(FadeBlackness), "fadeInDateText", MethodType.Enumerator)]
        public static IEnumerable<CodeInstruction> FadeBlackness_fadeInDateText_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Year ", "연도 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(GiveNPC), "UpdateMenu", MethodType.Enumerator)]
        public static IEnumerable<CodeInstruction> GiveNPC_UpdateMenu_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Place", "배치");
            instructions = ReplaceIL(instructions, "Donate", "기부");
            instructions = ReplaceIL(instructions, "Sell", "판매");
            instructions = ReplaceIL(instructions, "Cancel", "취소");
            instructions = ReplaceIL(instructions, "Swap", "교환");
            instructions = ReplaceIL(instructions, "Give", "주기");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(InventoryItemDescription), "fillItemDescription")]
        public static IEnumerable<CodeInstruction> InventoryItemDescription_fillItemDescription_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "All year", "연간");
            instructions = ReplaceIL(instructions, "Summer", "여름");
            instructions = ReplaceIL(instructions, "Autumn", "가을");
            instructions = ReplaceIL(instructions, "Winter", "겨울");
            instructions = ReplaceIL(instructions, "Spring", "봄");
            instructions = ReplaceIL(instructions, "Bury", "묻기");
            //instructions = ReplaceIL(instructions, "Speeds up certain production devices for up to 12 tiles", "최대 12개의 타일에 대해 특정 생산장치 속도향상");
            //instructions = ReplaceIL(instructions, "Reaches ", "도달범위 ");
            //instructions = ReplaceIL(instructions, " tiles out.\n<color=red>Requires Water Tank</color>", " 타일 벗어남.\n<color=red>물 탱크 필요</color>");
            //instructions = ReplaceIL(instructions, "Provides water to sprinklers ", "@");
            //instructions = ReplaceIL(instructions, " tiles out.", " 개의 타일 밖에 있는 스프링클러에 물을 공급합니다.");
            //instructions = ReplaceIL(instructions, "Fills animal feeders ", "동물 먹이통 채우기 ");
            //instructions = ReplaceIL(instructions, " tiles out.\n<color=red>Requires Animal Food</color>", " 타일 벗어남.\n<color=red>동물 사료 필요</color>");

            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(MailManager), "getSentByName")]
        public static IEnumerable<CodeInstruction> MailManager_getSentByName_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Animal Research Centre", "동물 연구소");
            instructions = ReplaceIL(instructions, "Dinkum Dev", "Dinkum 개발자");
            instructions = ReplaceIL(instructions, "Fishin' Tipster", "낚시 가이드");
            instructions = ReplaceIL(instructions, "Bug Tipster", "곤충 가이드");
            instructions = ReplaceIL(instructions, "Unknown", "불명");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(MailManager), "showLetter")]
        public static IEnumerable<CodeInstruction> MailManager_showLetter_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "From ", "발신 ");
            instructions = ReplaceIL(instructions, "<size=18><b>To ", "<size=18><b>수신 ");
            instructions = ReplaceIL(instructions, "\n\n<size=18><b>From ", "\n\n<size=18><b>발신 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(NPCRequest), "getDesiredItemName")]
        public static IEnumerable<CodeInstruction> NPCRequest_getDesiredItemName_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "any bug", "아무 곤충");
            instructions = ReplaceIL(instructions, "any fish", "아무 생선");
            instructions = ReplaceIL(instructions, "something to eat", "먹을거리");
            instructions = ReplaceIL(instructions, "something you've made me at a cooking table", "조리대에서 요리한것");
            instructions = ReplaceIL(instructions, "furniture", "가구");
            instructions = ReplaceIL(instructions, "clothing", "옷");
            return instructions;
        }

        /// <summary>
        /// StringReturnPatch.cs 이동
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        //[HarmonyTranspiler, HarmonyPatch(typeof(NPCRequest), "getDesiredItemNameByNumber")]
        //public static IEnumerable<CodeInstruction> NPCRequest_getDesiredItemNameByNumber_Patch(IEnumerable<CodeInstruction> instructions)
        //{
        //    instructions = ReplaceIL(instructions, "a ", " ");
        //    return instructions;
        //}

        //[HarmonyTranspiler, HarmonyPatch(typeof(NPCRequest), "getMissionText")]
        //public static IEnumerable<CodeInstruction> NPCRequest_getMissionText_Patch(IEnumerable<CodeInstruction> instructions)
        //{
        //    instructions = ReplaceIL(instructions, "<sprite=12> Bring ", "<sprite=12> 가져다주기 ");
        //    instructions = ReplaceIL(instructions, "<sprite=13> Collect ", "<sprite=13> 수집하기 ");
        //    instructions = ReplaceIL(instructions, "\n<sprite=12> Bring ", "\n<sprite=12> 가져다주기 ");
        //    instructions = ReplaceIL(instructions, "<sprite=12> Collect ", "<sprite=12> 수집하기 ");
        //    return instructions;
        //}

        [HarmonyTranspiler, HarmonyPatch(typeof(NPCSchedual), "getDaysClosed")]
        public static IEnumerable<CodeInstruction> NPCSchedual_getDaysClosed_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Closed: ", "마감시간: ");
            instructions = ReplaceIL(instructions, " and ", " 그리고 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(NPCSchedual), "getNextDayOffName")]
        public static IEnumerable<CodeInstruction> NPCSchedual_getNextDayOffName_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "No Day off", "쉬는 날 없음");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(NPCSchedual), "getOpeningHours")]
        public static IEnumerable<CodeInstruction> NPCSchedual_getOpeningHours_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Open: ", "영업시간: ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(PlayerDetailManager), "switchToLevelWindow")]
        public static IEnumerable<CodeInstruction> PlayerDetailManager_switchToLevelWindow_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Resident for: ", "주민:");
            instructions = ReplaceIL(instructions, " days", " 일");
            instructions = ReplaceIL(instructions, " months", " 월");
            instructions = ReplaceIL(instructions, " years", " 년");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(PostOnBoard), "getPostedByName")]
        public static IEnumerable<CodeInstruction> PostOnBoard_getPostedByName_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Town Announcement", "마을 공지");
            instructions = ReplaceIL(instructions, "Animal Research Centre", "동물 연구소");
            return instructions;
        }

        //[HarmonyTranspiler, HarmonyPatch(typeof(Quest), "getMissionObjText")]
        //public static IEnumerable<CodeInstruction> Quest_getMissionObjText_Patch(IEnumerable<CodeInstruction> instructions)
        //{
        //instructions = ReplaceIL(instructions, "<sprite=12> Attract a total of 5 permanent residents to move to ", "<sprite=12> 다음으로 이주하기 위해 총 5 명의 영구 거주자를 유치: ");
        //instructions = ReplaceIL(instructions, "<sprite=12> Talk to ", "\n<sprite=12> 대화");
        //instructions = ReplaceIL(instructions, " once the Base Tent has been moved", "베이스 텐트 이동 후에");
        //instructions = ReplaceIL(instructions, "<sprite=13> Craft a ", "<sprite=13> ");
        //instructions = ReplaceIL(instructions, "at the crafting table in the Base Tent.\n<sprite=13> Place the  ", "(을)를 베이스 텐트 제작대에서 제작하고\n<sprite=13> ");
        //instructions = ReplaceIL(instructions, " down outside.\n<sprite=13> Place Tin Ore into ", "(을)를 바깥쪽에 배치합니다.\n<sprite=13> 주석 광석을");
        //instructions = ReplaceIL(instructions, " and wait for it to become ", "에 넣고 녹을 때까지 기다립니다");
        //instructions = ReplaceIL(instructions, ".\n<sprite=12> Take the ", ".\n<sprite=12> 잡고");
        //instructions = ReplaceIL(instructions, " to ", "에 ");
        //instructions = ReplaceIL(instructions, " down outside.\n<sprite=12> Place Tin Ore into ", "바깥쪽으로.\n<sprite=12> 주석 광석 놓기");
        //instructions = ReplaceIL(instructions, "at the crafting table in the Base Tent.\n<sprite=12> Place the  ", "베이스 텐트 제작대에서\n<sprite=12> 배치 ");
        //instructions = ReplaceIL(instructions, "<sprite=12> Craft a ", "<sprite=12> 제작 ");
        //instructions = ReplaceIL(instructions, "<sprite=13> Buy the ", "<sprite=13> 구입 ");
        //instructions = ReplaceIL(instructions, "\n<sprite=12> Talk to ", "\n<sprite=12> 대화 ");
        //instructions = ReplaceIL(instructions, "<sprite=12> Buy the ", "<sprite=12> 구입 ");
        //instructions = ReplaceIL(instructions, "[Optional] Complete Daily tasks\n<sprite=12> Place sleeping bag and get some rest.", "[선택 사항] 일일 작업 완료\n<sprite=12>침낭을 놓고 휴식을 취하세요.");
        //instructions = ReplaceIL(instructions, "<sprite=13> Find something to eat.\n<sprite=12> Talk to ", "<sprite=13> 먹을 것을 찾으세요.\n<sprite=12> 대화하기: ");
        //instructions = ReplaceIL(instructions, "<sprite=12> Find something to eat.\n<sprite=12> Talk to ", "<sprite=12> 먹을 것을 찾으세요.\n<sprite=12> 대화하기: ");
        //instructions = ReplaceIL(instructions, "<sprite=13> Collect the requested items.\n<sprite=12> Bring items to ", "<sprite=13> 의뢰 아이템 수집\n<sprite=12> 아이템 가져가기: ");
        //instructions = ReplaceIL(instructions, "<sprite=12> Collect the requested items.", "<sprite=12> 의뢰 아이템 수집");
        //instructions = ReplaceIL(instructions, "\n<sprite=12> Bring items to ", "\n<sprite=12> 아이템 가져가기: ");
        //instructions = ReplaceIL(instructions, "<sprite=12> Do some favours for John", "<sprite=12> John에게 호의를 베풀다");
        //instructions = ReplaceIL(instructions, "<sprite=13> Do some favours for John", "<sprite=13> John에게 호의를 베풀다");
        //instructions = ReplaceIL(instructions, "\n<sprite=12> Spend money or sell items in John's store", "\n<sprite=12> John의 상점에서 돈을 쓰거나 아이템 팔기");
        //instructions = ReplaceIL(instructions, "\n<sprite=13> Spend money or sell items in John's store", "\n<sprite=13> John의 상점에서 돈을 쓰거나 아이템 팔기");
        //instructions = ReplaceIL(instructions, "\n<sprite=12> Convince John to move in.", "\n<sprite=12> John이 이사오도록 설득하세요.");
        //instructions = ReplaceIL(instructions, "<sprite=12> Ask ", "<sprite=12> 물어보기 ");
        //instructions = ReplaceIL(instructions, " about the town to apply for the ", "신청할 마을에 대해");
        //instructions = ReplaceIL(instructions, "<sprite=12> Place the ", "<sprite=12> 배치 ");
        //instructions = ReplaceIL(instructions, "<sprite=12> Wait for construction of the  ", "<sprite=12> 건설 대기");
        //instructions = ReplaceIL(instructions, " to be completed", " 건설 완료");
        //instructions = ReplaceIL(instructions, "<sprite=12> Place the required items into the construction box at the deed site", "<sprite=12> 계약 장소의 건설 상자에 필요한 아이템을 넣습니다.");
        //instructions = ReplaceIL(instructions, "<sprite=12> Place ", "<sprite=12> 배치 ");
        //instructions = ReplaceIL(instructions, "<sprite=13> Place ", "<sprite=13> 배치 ");
        //    return instructions;
        //}

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "displayQuest")]
        public static IEnumerable<CodeInstruction> QuestTracker_displayQuest_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, " days remaining", " 일 남음");
            return instructions;
        }

        //[HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "displayRequest")]
        //public static IEnumerable<CodeInstruction> QuestTracker_displayRequest_Patch(IEnumerable<CodeInstruction> instructions)
        //{
        //    instructions = ReplaceIL(instructions, "Request for ", "요청을 ");
        //    instructions = ReplaceIL(instructions, " has asked you to get ", "(이)가 했습니다. 오늘 끝나기전에");
        //    instructions = ReplaceIL(instructions, "By the end of the day", "(을)를 하세요.");
        //    return instructions;
        //}

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "displayTrackingRecipe")]
        public static IEnumerable<CodeInstruction> QuestTracker_displayTrackingRecipe_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, " Recipe", " 제작법");
            instructions = ReplaceIL(instructions, " Construction Requirements", " 건설 요구사항");
            instructions = ReplaceIL(instructions, "These items required to start construction on ", "건설을 시작하려면 다음의 아이템이 필요 ");
            instructions = ReplaceIL(instructions, "These items are required to craft ", "제작에 다음의 아이템이 필요 ");
            instructions = ReplaceIL(instructions, "\n Unpin this to stop tracking recipe.", "\n 제작법 추적을 중지하려면 고정해제하세요");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "fillMissionTextForRecipe")]
        public static IEnumerable<CodeInstruction> QuestTracker_fillMissionTextForRecipe_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Crafting ", "제작중 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "pressPinRecipeButton")]
        public static IEnumerable<CodeInstruction> QuestTracker_pressPinRecipeButton_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, " Recipe", " 제작법");
            return instructions;
        }

        //[HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "updateLookingAtTask")]
        //public static IEnumerable<CodeInstruction> QuestTracker_updateLookingAtTask_Patch(IEnumerable<CodeInstruction> instructions)
        //{
        //    instructions = ReplaceIL(instructions, "<sprite=17> Pinned", "<sprite=17> 고정");
        //    instructions = ReplaceIL(instructions, "<sprite=16> Pinned", "<sprite=16> 고정");
        //    return instructions;
        //}

        //[HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "updatePinnedRecipeButton")]
        //public static IEnumerable<CodeInstruction> QuestTracker_updatePinnedRecipeButton_Patch(IEnumerable<CodeInstruction> instructions)
        //{
        //    instructions = ReplaceIL(instructions, "<sprite=13> Track Recipe Ingredients", "<sprite=13> 제작 재료 추적");
        //    instructions = ReplaceIL(instructions, "<sprite=12> Track Recipe Ingredients", "<sprite=12> 제작 재료 추적");
        //    return instructions;
        //}

        //[HarmonyTranspiler, HarmonyPatch(typeof(QuestTracker), "updatePinnedTask")]
        //public static IEnumerable<CodeInstruction> QuestTracker_updatePinnedTask_Patch(IEnumerable<CodeInstruction> instructions)
        //{
        //    instructions = ReplaceIL(instructions, "Request for ", "요청한 사람 ");
        //    return instructions;
        //}

        //[HarmonyTranspiler, HarmonyPatch(typeof(BugAndFishCelebration), "openWindow")]
        //public static IEnumerable<CodeInstruction> BugAndFishCelebration_openWindow_Patch(IEnumerable<CodeInstruction> instructions)
        //{
        //    instructions = ReplaceIL(instructions, "I caught a ", "잡았다 ");
        //    return instructions;
        //}

        [HarmonyTranspiler, HarmonyPatch(typeof(RealWorldTimeLight), "showTimeOnClock")]
        public static IEnumerable<CodeInstruction> RealWorldTimeLight_showTimeOnClock_Patch(IEnumerable<CodeInstruction> instructions)
        {
            //instructions = ReplaceIL(instructions, "<size=10>PM</size>", "<size=10>오후</size>");
            //instructions = ReplaceIL(instructions, "<size=10>AM</size>", "<size=10>오전</size>");
            instructions = ReplaceIL(instructions, "Late", "야간");
            return instructions;
        }

        /*---------------------------------*/
        /// <summary>
        /// IL에서 텍스트 바꾸기
        /// </summary>
        public static IEnumerable<CodeInstruction> ReplaceIL(IEnumerable<CodeInstruction> instructions, string target, string i18n)
        {
            bool success = false;
            var list = instructions.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var ci = list[i];
                if (ci.opcode == OpCodes.Ldstr)
                {
                    if ((string)ci.operand == target)
                    {
                        ci.operand = i18n;
                        success = true;
                    }
                }
            }
            if (!success)
            {
                Debug.LogWarning($"플러그인이 {target} => {i18n} 대체하는 데 실패, 대상을 찾을 수 없습니다.");
            }
            return list.AsEnumerable();
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(SaveSlotButton), "fillFromSaveSlot")]
        public static IEnumerable<CodeInstruction> SaveSlotButton_fillFromSaveSlot_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Year ", "연도 ");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(SeasonAndTime), "getLocationName")]
        public static IEnumerable<CodeInstruction> SeasonAndTime_getLocationName_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Everywhere", "어디서나");
            return instructions;
        }

        //[HarmonyTranspiler, HarmonyPatch(typeof(Task), MethodType.Constructor, new Type[] { typeof(int), typeof(int) })]
        //public static IEnumerable<CodeInstruction> Task_Constructor_Patch(IEnumerable<CodeInstruction> instructions)
        //{
        //    instructions = ReplaceIL(instructions, "Harvest ", "수확하기 ");
        //    instructions = ReplaceIL(instructions, "Catch ", "포획하기 ");
        //    instructions = ReplaceIL(instructions, " Bugs", " 곤충");
        //    return instructions;
        //}

        //[HarmonyTranspiler, HarmonyPatch(typeof(Task), "generateTask")]
        //public static IEnumerable<CodeInstruction> Task_generateTask_Patch(IEnumerable<CodeInstruction> instructions)
        //{
        //    instructions = ReplaceIL(instructions, "Harvest ", "수확 ");
        //    instructions = ReplaceIL(instructions, "Chat with ", "대화 ");
        //    instructions = ReplaceIL(instructions, " residents", " 주민");
        //    instructions = ReplaceIL(instructions, "Bury ", "묻기 ");
        //    instructions = ReplaceIL(instructions, " Fruit", " 과일");
        //    instructions = ReplaceIL(instructions, "Collect ", "채집 ");
        //    instructions = ReplaceIL(instructions, " Shells", " 껍질");
        //    instructions = ReplaceIL(instructions, "Sell ", "판매");
        //    instructions = ReplaceIL(instructions, "Do a job for someone", "누군가 위해 일하기");
        //    instructions = ReplaceIL(instructions, "Plant ", "재배 ");
        //    instructions = ReplaceIL(instructions, " Wild Seeds", " 야생종자");
        //    instructions = ReplaceIL(instructions, "Dig up dirt ", "흙 파내기 ");
        //    instructions = ReplaceIL(instructions, " times", "배");
        //    instructions = ReplaceIL(instructions, "Catch ", "포획 ");
        //    instructions = ReplaceIL(instructions, " Bugs", " 수집");
        //    instructions = ReplaceIL(instructions, "Craft ", "제작 ");
        //    instructions = ReplaceIL(instructions, " Items", " 아이템");
        //    instructions = ReplaceIL(instructions, "Eat something", "무언가 먹을것");
        //    instructions = ReplaceIL(instructions, "Make ", "획득 ");
        //    instructions = ReplaceIL(instructions, "Spend ", "소비 ");
        //    instructions = ReplaceIL(instructions, "Travel ", "이동 ");
        //    instructions = ReplaceIL(instructions, "m on foot.", "분（도보로）");
        //    instructions = ReplaceIL(instructions, "m by vehicle", "분（차량으로）");
        //    instructions = ReplaceIL(instructions, "Cook ", "요리 ");
        //    instructions = ReplaceIL(instructions, " meat", " 고기");
        //    instructions = ReplaceIL(instructions, " fruit", " 열매");
        //    instructions = ReplaceIL(instructions, "Cook something at the Cooking table", "조리대에서 무언가를 요리");
        //    instructions = ReplaceIL(instructions, " tree seeds", " 나무종자");
        //    instructions = ReplaceIL(instructions, "crop seeds", "작물종자");
        //    instructions = ReplaceIL(instructions, "Water ", "물 주기 ");
        //    instructions = ReplaceIL(instructions, " crops", " 작물");
        //    instructions = ReplaceIL(instructions, "Smash ", "캐기 ");
        //    instructions = ReplaceIL(instructions, " rocks", " 암석");
        //    instructions = ReplaceIL(instructions, " ore rocks", " 광석");
        //    instructions = ReplaceIL(instructions, "Smelt some ore into a bar", "약갼의 광석을 주괴로 제련");
        //    instructions = ReplaceIL(instructions, "Grind ", "연마 ");
        //    instructions = ReplaceIL(instructions, " stones", " 바위");
        //    instructions = ReplaceIL(instructions, "Cut down ", "벌목 ");
        //    instructions = ReplaceIL(instructions, " trees", " 목재");
        //    instructions = ReplaceIL(instructions, "Clear ", "정리 ");
        //    instructions = ReplaceIL(instructions, " tree stumps", " 그루터기");
        //    instructions = ReplaceIL(instructions, "Saw ", "톱질 ");
        //    instructions = ReplaceIL(instructions, " planks", " 판자");
        //    instructions = ReplaceIL(instructions, " Fish", " 생선");
        //    instructions = ReplaceIL(instructions, " grass", " 풀");
        //    instructions = ReplaceIL(instructions, "Pet an animal", "동물 쓰다듬기");
        //    instructions = ReplaceIL(instructions, "Buy some new clothes", "새 옷을 사기");
        //    instructions = ReplaceIL(instructions, "Buy some new furniture", "새 가구를 사기");
        //    instructions = ReplaceIL(instructions, "Buy some new wallpaper", "새 벽지를 사기");
        //    instructions = ReplaceIL(instructions, "Buy some new flooring", "새 바닥재를 사기");
        //    instructions = ReplaceIL(instructions, "Compost something", "퇴비만들기");
        //    instructions = ReplaceIL(instructions, "Craft a new tool", "새 도구 제작하기");
        //    instructions = ReplaceIL(instructions, "Buy ", "구매 ");
        //    instructions = ReplaceIL(instructions, " seeds", " 종자");
        //    instructions = ReplaceIL(instructions, "Trap an animal and deliver it", "동물 포획 및 운반하기");
        //    instructions = ReplaceIL(instructions, "Hunt ", "사냥 ");
        //    instructions = ReplaceIL(instructions, " animals", " 동물");
        //    instructions = ReplaceIL(instructions, "Buy a new tool", "새 도구를 사기");
        //    instructions = ReplaceIL(instructions, "Break a tool", "도구 부서뜨리기");
        //    instructions = ReplaceIL(instructions, "Find some burried treasure", "묻힌 보물찾기");
        //    instructions = ReplaceIL(instructions, "No mission set", "정해진 과제 없음");
        //    return instructions;
        //}

        [HarmonyTranspiler, HarmonyPatch(typeof(UseBook), "plantBookRoutine", MethodType.Enumerator)]
        public static IEnumerable<CodeInstruction> UseBook_plantBookRoutine_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, " Plant", " 재배");
            instructions = ReplaceIL(instructions, "Ready for harvest", "수확 준비 완료");
            instructions = ReplaceIL(instructions, "Mature in:\n", "성숙 시간:\n");
            instructions = ReplaceIL(instructions, " days.", " 일");
            instructions = ReplaceIL(instructions, " days", " 일");
            instructions = ReplaceIL(instructions, "Plant", "재매");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(WeatherManager), "GetWeatherDescription")]
        public static IEnumerable<CodeInstruction> WeatherManager_GetWeatherDescription_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "It is currently {0} ° and ", "현재 {0} °이고 ");
            instructions = ReplaceIL(instructions, "Storming", "폭풍우");
            instructions = ReplaceIL(instructions, "Raining", "비");
            instructions = ReplaceIL(instructions, "Foggy", "안개");
            instructions = ReplaceIL(instructions, "Fine", "맑은 날씨");
            instructions = ReplaceIL(instructions, ". With a", "와(과) 함께");
            instructions = ReplaceIL(instructions, " Strong", "강한");
            instructions = ReplaceIL(instructions, " Light", "약한");
            instructions = ReplaceIL(instructions, " Northern ", " 북");
            instructions = ReplaceIL(instructions, " Southern ", " 남");
            instructions = ReplaceIL(instructions, " Westernly ", " 서");
            instructions = ReplaceIL(instructions, " Easternly ", " 동");
            instructions = ReplaceIL(instructions, " Wind.", "풍이 붑니다.");
            return instructions;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(LoadingScreenImageAndTips), "OnEnable")]
        public static IEnumerable<CodeInstruction> LoadingScreenImageAndTips_OnEnable_Patch(IEnumerable<CodeInstruction> instructions)
        {
            instructions = ReplaceIL(instructions, "Photo by ", "촬영자 ");
            return instructions;
        }

        ///// <summary>
        ///// 由于作者把地名翻译了但是在图标这里还是用的写死的英文来判断名字，所以导致在地下的时候图标显示不了。
        ///// 临时做个处理，等作者把这修了，再删掉这个。
        ///// </summary>
        //[HarmonyTranspiler, HarmonyPatch(typeof(RenderMap), "ChangeWorldArea")]
        //public static IEnumerable<CodeInstruction> RenderMap_ChangeWorldArea_Patch(IEnumerable<CodeInstruction> instructions)
        //{
        //    instructions = ReplaceIL(instructions, "Mine", "광산");
        //    instructions = ReplaceIL(instructions, "Airport", "비행장");
        //    return instructions;
        //}

    }
}