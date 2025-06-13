using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }

    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool InteractInput { get; private set; }
    public bool SubmitInput { get; private set; }
    public bool EscapeInput { get; private set; }

    [SerializeField] private float _inputHoldTime = 0.2f;
    private float _jumpInputStartTime;
    private bool _canEscape = true;


    private Player _player;
    private InputEvents InputEvents => GameEventsManager.inputEvents;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
    }
    
    /*==================== Move ====================*/
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        bool inDialogue = GameEventsManager.inputEvents.inputEventContext == InputEventContext.DIALOGUE;

        // ── читаємо вектор ОДИН раз
        Vector2 input = context.ReadValue<Vector2>();

        // ── 1. надсилаємо його у UI (навіть якщо персонаж зупинений)
        if ((context.performed || context.canceled) && InputEvents != null)
            InputEvents.MovePressed(input);

        // ── 2. блокуємо фізичний рух для персонажа, якщо не можна рухатись
        if (_player != null && (!_player.CanMove() || inDialogue))
            input = Vector2.zero;

        // ── записуємо для решти систем гри
        RawMovementInput = input;
        NormalizedInputX = (int)(input * Vector2.right).normalized.x;
        NormalizedInputY = (int)(input * Vector2.up).normalized.y;


        //// блокуємо фізичний рух завжди, якщо Player заблокований
        //if (_player != null && !_player.CanMove())
        //{
        //    RawMovementInput = Vector2.zero;
        //    NormalizedInputX = 0;
        //    NormalizedInputY = 0;
        //    // але ДЛЯ ДІАЛОГУ все-одно надсилаємо MovePressed
        //    if (!inDialogue) return;

        //}

        //RawMovementInput = context.ReadValue<Vector2>();

        //NormalizedInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        //NormalizedInputY = (int)(RawMovementInput * Vector2.up).normalized.y;

        //if ((context.performed || context.canceled) && InputEvents != null)
        //    InputEvents.MovePressed(RawMovementInput);
    }

    /*==================== Jump ====================*/
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (_player != null && !_player.CanMove()) return;

        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            _jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    /*=============== Interact (E) =================*/
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (_player != null && !_player.CanMove()) return;

        if (context.started)
        {
            InteractInput = true;
            InputEvents.InteractPressed();
        }
        if (context.canceled)
        {
            InteractInput = false;
        }
    }

    /*===============  Submit (Enter/F) =================*/
    public void OnSubmitInput(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        SubmitInput = true;
        // якщо зараз щось вибране у EventSystem – діалог/меню
        GameObject sel = EventSystem.current.currentSelectedGameObject;
        // ігноруємо Submit, якщо вибрано щось чуже діалогові (меню, поле вводу тощо)
        if (sel != null && sel.GetComponent<DialogueChoiceButton>() == null)
            return;
        InputEvents?.SubmitPressed();
    }

    /*=================== Escape ===================*/
    public void OnEscapeInput(InputAction.CallbackContext context)
    {
        if (context.started && _canEscape == true)
        {
            EscapeInput = true;
            _canEscape = false;
            SceneTransition.SwitchToScene("MainMenu");
        }
        if (context.canceled)
        {
            EscapeInput = false;
        }
    }

    /*=============== Methods for FSM ==============*/
    public void UseJumpInput() 
    { 
        JumpInput = false;
    }
    public void UseInteractInput() 
    { 
        InteractInput = false;
    }

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= _jumpInputStartTime + _inputHoldTime)
        {
            JumpInput = false;
        }
    }
}
