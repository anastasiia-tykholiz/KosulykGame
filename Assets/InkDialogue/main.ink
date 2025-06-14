EXTERNAL StartQuest(questid)
EXTERNAL AdvanceQuest(questid)
EXTERNAL FinishQuest(questid)
EXTERNAL EndDay(nextQuestId)

// quest ids
VAR CollectApplesTutorialId = "CollectApplesTutorial"
VAR CollectForestPlantsQuestId = "CollectForestPlantsQuest"
VAR CollectPineForestPlantsQuestId = "CollectPineForestPlantsQuest"
VAR CollectSwampPlantsQuestId = "CollectSwampPlantsQuest"

VAR FinalDayQuestId = "FinalDayQuest"

// quest states (quest id + "State")
VAR CollectApplesTutorialState = "REQUIREMENTS_NOT_MET"
VAR CollectForestPlantsQuestState = "REQUIREMENTS_NOT_MET"
VAR CollectPineForestPlantsQuestState = "REQUIREMENTS_NOT_MET"
VAR CollectSwampPlantsQuestState = "REQUIREMENTS_NOT_MET"

VAR FinalDayQuestState = "REQUIREMENTS_NOT_MET"

// ink files
INCLUDE tutorial.ink
INCLUDE collect_forest_start.ink
INCLUDE collect_pineforest_start.ink
INCLUDE collect_swamp_start.ink
INCLUDE final_day_start.ink
