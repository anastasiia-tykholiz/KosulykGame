
=== CollectSwampPlants ===
{ CollectSwampPlantsQuestState :
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
Косулик: Доброго ранку бабусю! Як ти сьогодні?
* [Що трапилося?]          -> step_what

= step_what
    Бабуся: Ох, Косулику мій... все ще погано.
    * [Можливо..]        -> step_flowers


= step_flowers
    Косулик: Можливо мені ще щось зібрати?
    Бабуся: 
    Косулик: 
    Бабуся: 
    * [Добре!]      -> step_ok


= step_ok
    Косулик: Я тоді одразу піду!!
    ~ StartQuest(CollectSwampPlantsQuestId)
    -> END


= inProgress
Бабуся: Так, Косулику?
* [Нагадай..]
    Косулик: Нагадай що потрібно зібрати та де сосновий ліс?
    Бабуся: Звіробою п'ять штук, жовті квіти, що ростуть в сосновосу лісі праворуч у горах.
* [Що робити з квітами?]
    Косулик: І що ж робити з тими квітами?
    Бабуся: Як збереш всі квіти, підійти до казана та натисни "E".
- -> END

= canFinish
Косулик: Я зробив все як було написано в книзі!
* [Віддати зілля] 
    Бабуся: Ти зварив своє треттє зілля! Дякую, Косулику.
    Косулик: Тобі ж стане легше?
    Бабуся: Побачимо вранці... Дякую, мій відважний помічнику. На добраніч.
    Косулик: Сподіваюсь, зілля допоможе.
    Косулик: На добраніч!
    ~ EndDay(FinalDayQuestId)
    ~ FinishQuest(CollectSwampPlantsQuestId)
    -> END


= finished
-> END


