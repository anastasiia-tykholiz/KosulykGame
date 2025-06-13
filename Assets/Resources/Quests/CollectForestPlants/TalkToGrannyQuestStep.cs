using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TalkToGrannyQuestStep : QuestStep, IInteractable
{
    [Header("Granny settings")]
    [SerializeField] private Transform grannyTransform;   // де стоїть бабуся

    private bool _playerNearby;
    //private bool _done;

    private InputEvents _inputEvents => GameEventsManager.inputEvents;

    private void OnEnable()
    {
        // Слухаємо натиск Submit
        _inputEvents.onSubmitPressed += OnSubmit;
    }
    private void OnDisable()
    {
        _inputEvents.onSubmitPressed -= OnSubmit;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _playerNearby = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _playerNearby = false;
    }

    private void OnSubmit(InputEventContext ctx)
    {
        // перевіряємо контекст та наявність гравця поруч
        if (ctx == InputEventContext.DEFAULT && _playerNearby)
            Interact();
    }

    public void Interact()
    {
        //_done = true;
        Debug.Log("Поговорили з бабусею");
        FinishQuestStep();
    }

    protected override void SetQuestStepState(string state)
    {
        // якщо вантажимо збереження, відмічаємо, що вже зроблено
        //if (state == "done")
        //    _done = true;
    }
}
