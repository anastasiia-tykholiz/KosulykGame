using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TalkToGrannyQuestStep : QuestStep, IInteractable
{
    [Header("Granny settings")]
    [SerializeField] private Transform grannyTransform;   // �� ����� ������

    private bool _playerNearby;
    //private bool _done;

    private InputEvents _inputEvents => GameEventsManager.inputEvents;

    private void OnEnable()
    {
        // ������� ������ Submit
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
        // ���������� �������� �� �������� ������ �����
        if (ctx == InputEventContext.DEFAULT && _playerNearby)
            Interact();
    }

    public void Interact()
    {
        //_done = true;
        Debug.Log("���������� � �������");
        FinishQuestStep();
    }

    protected override void SetQuestStepState(string state)
    {
        // ���� ��������� ����������, �������, �� ��� ��������
        //if (state == "done")
        //    _done = true;
    }
}
