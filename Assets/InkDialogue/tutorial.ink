
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
Косулик: Хм... щоб думати далі потрібно натиснути F або Enter наче..
Косулик: Здається я щось забув..
* [Намагатися згадати]
    Косулик: Точно! Я ж хотів зібрати бабусі яблука в садку ліворуч!
    ~ StartQuest(CollectApplesTutorialId)
* [Байдуже]
    Косулик: Можливо потім згадаю..
    
- -> END

= inProgress
Косулик: Я ще не зібрав достатньо яблук.
-> END

= canFinish
Косулик: О! Наче вистачить, можна пригощати бабусю.
* [Віддати яблука]
    ~ FinishQuest(CollectApplesTutorialId)
-> END

= finished
-> END



