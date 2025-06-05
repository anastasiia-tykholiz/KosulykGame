EXTERNAL StartQuest(questid)
EXTERNAL AdvanceQuest(questid)
EXTERNAL FinishQuest(questid)

// quest ids
VAR CollectApplesTutorialId = "CollectApplesTutorial"

// quest states (quest id + "State")
VAR CollectApplesTutorialState = "REQUIREMENTS_NOT_MET"

=== StartTutorial ===
{ CollectApplesTutorialState :
    - "REQUIREMENTS_NOT_MET" : -> requirementsNotMet
    - "CAN_START" : -> canStart
    - "IN_PROGRESS" : -> inProgress
    - "CAN_FINISH" : -> canFinish
    - "FINISHED" : -> finished
    - else: -> END
}

= requirementsNotMet
-> END

= canStart
Хм...
Здається я щось забув..
* [Намагатися згадати]
    Точно! Я ж хотів зібрати бабусі яблука в садку ліворуч!
    ~ StartQuest(CollectApplesTutorialId)
* [Байдуже]
    Можливо потім згадаю..
- -> END

= inProgress
Я ще не зібрав достатньо яблук.
-> END

= canFinish
О! Наче вистачить, можна пригощати бабусю.
* [Віддати яблука]
    ~ FinishQuest(CollectApplesTutorialId)
-> END

= finished
-> END



