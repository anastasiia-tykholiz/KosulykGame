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
        _done = true;

        // анімація (поки чомусь не робоча)
        if (campfireAnimator)
        {
            campfireAnimator.SetTrigger(burnTrigger);
            Debug.Log("Triger is suppoued to be set");
        }

        Debug.Log("Крок з казаном виконано");
        FinishQuestStep();
    }

    protected override void SetQuestStepState(string state)
    {
        if (state == "done")
            _done = true;
    }
}
