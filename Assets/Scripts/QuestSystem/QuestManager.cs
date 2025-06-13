using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool loadQuestState = true;

    public static bool SkipSaveOnDisable = false;

    private Dictionary<string, Quest> questMap;
    public IEnumerable<Quest> AllQuests => questMap.Values;


    // quest start requirements
    private int currentPlayerLevel;
    private Stats playerStats;

    public static QuestManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        SkipSaveOnDisable = false;
        DontDestroyOnLoad(gameObject);      // забезпечує існування між сценами

        questMap = CreateQuestMap();

        playerStats = FindObjectOfType<Player>()?.GetComponentInChildren<Stats>(true);
        if (playerStats == null)
            Debug.LogError("Stats component not found in Player hierarchy!");

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        GameEventsManager.questEvents.onStartQuest += StartQuest;
        GameEventsManager.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.questEvents.onFinishQuest += FinishQuest;

        GameEventsManager.questEvents.onQuestStepStateChange += QuestStepStateChange;
        GameEventsManager.playerEvents.onPlayerLevelChange += PlayerLevelChange;
    }

    private void OnDisable()
    {
        if (Instance != this) return;

        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameEventsManager.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.questEvents.onFinishQuest -= FinishQuest;

        GameEventsManager.questEvents.onQuestStepStateChange -= QuestStepStateChange;
        GameEventsManager.playerEvents.onPlayerLevelChange -= PlayerLevelChange;

        if (!SkipSaveOnDisable) { 
            foreach (var q in questMap.Values) SaveQuest(q);
        }
        Instance = null;
    }
    private void Start()
    {
        PlayerPrefs.DeleteKey("TaskComplete");
        PlayerPrefs.DeleteKey("TotalPlants");
        PlayerPrefs.DeleteKey("TaskValue");
        PlayerPrefs.Save();

        foreach (Quest quest in questMap.Values)
        {
            // initialize any loaded quest steps
            if (quest.state == QuestState.IN_PROGRESS)
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            // broadcast the initial state of all quests on startup
            GameEventsManager.questEvents.QuestStateChange(quest);
        }
    }

    private void Update()
    {
        // loop through ALL quests
        foreach (Quest quest in questMap.Values)
        {
            // if we're now meeting the requirements, switch over to the CAN_START state
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        Quest quest = GetQuestById(id);
        quest.StoreQuestStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.state);
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;

        PlayerPrefs.SetInt($"quest_{id}_state", (int)state);
        PlayerPrefs.Save(); // зберігаємо стан у PlayerPrefs

        SaveQuest(quest);
        GameEventsManager.questEvents.QuestStateChange(quest);
    }

    private void StartQuest(string id)
    {
        Debug.Log("Start Quest:" + id);
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Debug.Log("Advance Quest:" + id);
        Quest quest = GetQuestById(id);

        quest.MoveToNextStep();

        // if there are more steps, instantiate the next one
        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        // if there are no more steps, then we've finished all of them for this quest
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Debug.Log("Finish Quest:" + id);
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }


    private Dictionary<string, Quest> CreateQuestMap()
    {
       
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, LoadQuest(questInfo));
        }
        return idToQuestMap;
    }

    public Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }
        return quest;
    }

    private void ClaimRewards(Quest quest)
    {
        //Debug.Log($"---- ClaimRewards ---- quest={quest} info={quest?.info} abilitiesList={quest?.info?.abilitiesToUnlock}");
        //Debug.Log($"playerStats = {playerStats}");


        QuestInfoSO info = quest.info;

        /*  EXP  */
        if (playerStats != null && quest.info.expReward > 0)
        {
            //Debug.Log("Поточне EXP " + playerStats.EXP);
            playerStats.AddEXP(quest.info.expReward);
            //Debug.Log("Нове EXP " + playerStats.EXP);
        }
        else
        {
            //Debug.LogWarning("playerStats == null, EXP не нараховано");
        }

        /*  Abilities  */
        var list = quest.info.abilitiesToUnlock;
        if (list != null)
        {
            foreach (var ab in list)
            {
                UnlockAbility(ab);
                //Debug.Log("Нове Абіліті " + ab);
            }
        }
        PlayerPrefs.Save();
    }
    private void UnlockAbility(AbilityID id)
    {
        switch (id)
        {
            case AbilityID.DoubleJump: PlayerPrefs.SetInt("amountOfJumps", 2); break;
            case AbilityID.WallJump: PlayerPrefs.SetString("canWallJump", "true"); break;
            case AbilityID.SceneTravel: PlayerPrefs.SetString("canSceneTravel", "true"); break;
        }
        PlayerPrefs.Save();
    }
    private void PlayerLevelChange(int newLevel)
    {
        currentPlayerLevel = newLevel;

        foreach (var quest in questMap.Values)
        {
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET &&
                newLevel >= quest.info.levelRequirement)
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        // start true and prove to be false
        bool meetsRequirements = true;

        // check player level requirements
        if (currentPlayerLevel < quest.info.levelRequirement)
        {
            meetsRequirements = false;
        }

        // check quest prerequisites for completion
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void OnApplicationQuit()
    {
        foreach (Quest quest in questMap.Values)
        {
            SaveQuest(quest);
        }
    }

    private void SaveQuest(Quest quest)
    {
        try
        {
            QuestData questData = quest.GetQuestData();
            // можна ще використати JSON.NET
            string serializedData = JsonUtility.ToJson(questData);

            // краще збергіати в іншому місці
            PlayerPrefs.SetString(quest.info.id, serializedData);

            //Debug.Log(serializedData);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save quest with id " + quest.info.id + ": " + e);
        }
    }
    private Quest LoadQuest(QuestInfoSO questInfo)
    {
        Quest quest = null;
        try
        {
            // load quest from saved data
            if (PlayerPrefs.HasKey(questInfo.id) && loadQuestState)
            {
                string serializedData = PlayerPrefs.GetString(questInfo.id);
                QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                quest = new Quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
            }
            // otherwise, initialize a new quest
            else
            {
                quest = new Quest(questInfo);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load quest with id " + quest.info.id + ": " + e);
        }
        return quest;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var quest in questMap.Values)
            GameEventsManager.questEvents.QuestStateChange(quest);

    }
}
