using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables

    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerStartIdleState StartIdleState { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }  
    public PlayerStartMoveState StartMoveState { get; private set; }  
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerInteractionState InteractionState { get; private set; }
    public PlayerTakeDamageState TakeDamageState { get; private set; }
    public PlayerFaintState FaintState { get; private set; }

    #endregion

    #region Components

    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public Rigidbody2D Rigidbody2d { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }

    [SerializeField] private PlayerData _playerData;



    private bool _canMove = true;

    #endregion

    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        StateMachine = new PlayerStateMachine();

        StartIdleState = new PlayerStartIdleState(this, StateMachine, _playerData, "startIdle");
        IdleState = new PlayerIdleState(this, StateMachine, _playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, _playerData, "move");
        StartMoveState = new PlayerStartMoveState(this, StateMachine, _playerData, "startMove");
        JumpState = new PlayerJumpState(this, StateMachine, _playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, _playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, _playerData, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, _playerData, "wallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, _playerData, "wallJump");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, _playerData, "ledgeClimbState");
        InteractionState = new PlayerInteractionState(this, StateMachine, _playerData, "interact");
        TakeDamageState = new PlayerTakeDamageState(this, StateMachine, _playerData, "takeDamage");
        FaintState = new PlayerFaintState(this, StateMachine, _playerData, "faint");
    }


    private void OnEnable()
    {
        GameEventsManager.playerEvents.onDisableMovement += DisableMovement;
        GameEventsManager.playerEvents.onEnableMovement += EnableMovement;
    }

    private void OnDisable()
    {
        GameEventsManager.playerEvents.onDisableMovement -= DisableMovement;
        GameEventsManager.playerEvents.onEnableMovement -= EnableMovement;
    }


    private void Start()
    {
        Anim = GetComponent<Animator>();
        Rigidbody2d = GetComponent<Rigidbody2D>();
        InputHandler = GetComponent<PlayerInputHandler>();
        
        StateMachine.Initialize(IdleState);

    }

    private void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();

        _playerData.canWallJump = PlayerPrefs.GetString("canWallJump", "false");
        _playerData.amountOfJumps = PlayerPrefs.GetInt("amountOfJumps", 1);
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    private void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }  

    private void DisableMovement()
    {
        _canMove = false;
    }

    private void EnableMovement()
    {
        _canMove = true;
    }

    public bool CanMove() => _canMove;
}
