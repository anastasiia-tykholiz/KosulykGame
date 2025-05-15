using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }

    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool InteractInput { get; private set; }
    public bool EscapeInput { get; private set; }

    [SerializeField] private float _inputHoldTime = 0.2f;
    private float _jumpInputStartTime;
    private bool _canEscape = true;

    private void Update()
    {
        CheckJumpInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormalizedInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        NormalizedInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
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
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            InteractInput = true;
        }
        if (context.canceled)
        {
            InteractInput = false;
        }
    }
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
