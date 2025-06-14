using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BurnPlantsQuestStep : QuestStep, IInteractable
{

    private InputEvents _inputEvents => GameEventsManager.inputEvents;
    [Header("Input Settings")]
    [SerializeField] private InputEventContext requiredContext = InputEventContext.DEFAULT;

    [Header("Campfire Animation")]
    [SerializeField] private Animator campfireAnimator;
    [SerializeField] private string burnTrigger = "Burn";

    [Header("Crafting")]
    private CraftingUIManager craftingUIManager;
    [SerializeField] private string craftingLocation = "forest";

    private bool _playerInside;
    private bool _done;

    private void Awake()
    {
        if (campfireAnimator == null)
            campfireAnimator = GetComponent<Animator>();

        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnEnable()
    {
        // підписуємось на натиск «Submit» (E)
        _inputEvents.onSubmitPressed += OnSubmit;
    }

    private void OnDisable()
    {
        _inputEvents.onSubmitPressed -= OnSubmit;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _playerInside = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _playerInside = false;
    }

    private void OnSubmit(InputEventContext ctx)
    {
        // лише в потрібному контексті й коли гравець поруч
        if (!_done && ctx == requiredContext && _playerInside)
            Interact();
    }

    public void Interact()
    {
        // змінюємо контекст щоб гравець не рухався
        GameEventsManager.inputEvents.ChangeInputEventContext(InputEventContext.DIALOGUE);
        if (_done) return;

        _done = true;

        if (campfireAnimator)
        {
            campfireAnimator.SetTrigger(burnTrigger);
            Debug.Log("Triger is supposed to be set");
        }


        craftingUIManager = FindObjectOfType<CraftingUIManager>(true);

        if (craftingUIManager != null)
        {
            var grannyObj = FindObjectOfType<Granny>();
            if (grannyObj != null)
            {
              
                craftingUIManager.gameObject.SetActive(true);
                craftingUIManager.InitForLevel(craftingLocation, this, grannyObj);
            }
            else
            {
                Debug.LogWarning("Granny не знайдено на сцені.");
            }
        }
        else
        {
            Debug.LogError("CraftingUIManager не знайдено на сцені.");
        }
    }

    protected override void SetQuestStepState(string state)
    {
        if (state == "done")
            _done = true;
    }

    public void CompleteCraftingStep()
    {
        // змінюємо контекст щоб гравець міг знову рухатись
        //GameEventsManager.inputEvents.ChangeInputEventContext(InputEventContext.DEFAULT);
        FinishQuestStep();
    }

}
