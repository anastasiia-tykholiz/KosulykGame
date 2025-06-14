EXTERNAL StartQuest(questid)
EXTERNAL AdvanceQuest(questid)
EXTERNAL FinishQuest(questid)
EXTERNAL EndDay()

// quest ids
VAR CollectApplesTutorialId = "CollectApplesTutorial"
VAR CollectForestPlantsQuestId = "CollectForestPlantsQuest"

// quest states (quest id + "State")
VAR CollectApplesTutorialState = "REQUIREMENTS_NOT_MET"
VAR CollectForestPlantsQuestState = "REQUIREMENTS_NOT_MET"


// ink files
INCLUDE tutorial.ink
INCLUDE collect_forest_start.ink
INCLUDE collect_pineforest_start.ink
