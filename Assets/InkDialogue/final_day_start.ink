
=== FinalDayQuest ===
{ FinalDayQuestState :
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
Косулик: Доброго ранку бабусю! Нарешті тобі краще!
 ~ StartQuest(FinalDayQuestId)
 ~ FinishQuest(FinalDayQuestId)
-> END


= inProgress
- -> END

= canFinish
~ FinishQuest(FinalDayQuestId)
-> END


= finished
-> END