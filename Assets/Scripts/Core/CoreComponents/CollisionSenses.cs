using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    public Transform GroundCheck { get => _groundCheck; private set => _groundCheck = value; }
    public Transform WallCheck1 { get => _wallCheck1; private set => _wallCheck1 = value; }
    public Transform WallCheck2 { get => _wallCheck2; private set => _wallCheck2 = value; }
    public Transform LedgeCheck { get => _ledgeCheck; private set => _ledgeCheck = value; }
    public float GroundCheckRadius { get => _groundCheckRadius; set => _groundCheckRadius = value; }
    public float WallCheckDistance { get => _wallCheckDistance; set => _wallCheckDistance = value; }
    public LayerMask GroundLayer { get => groundLayer; set => groundLayer = value; }
    public bool IsInteracting { get => _canInteracting; private set => _canInteracting = value; }

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _wallCheck1;
    [SerializeField] private Transform _wallCheck2;
    [SerializeField] private Transform _ledgeCheck;

    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private float _interactCheckRadius;
    [SerializeField] private float _wallCheckDistance;

    private IInteractable _interactable;

    [SerializeField] private LayerMask groundLayer;

    private bool _isTouchingWall1;
    private bool _isTouchingWall2;
    private bool _canInteracting;
    private bool _interactInput;

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, groundLayer);
    }

    public bool IsTouchingLedge()
    {
        return Physics2D.Raycast(_ledgeCheck.position, Vector2.right * core.Movement.FacingDirection, _wallCheckDistance, groundLayer);
    }

    public bool IsTouchingWall()
    {
        _isTouchingWall1 = Physics2D.Raycast(_wallCheck1.position, Vector2.right * core.Movement.FacingDirection, _wallCheckDistance, groundLayer);
        _isTouchingWall2 = Physics2D.Raycast(_wallCheck2.position, Vector2.right * core.Movement.FacingDirection, _wallCheckDistance, groundLayer);
        if (_isTouchingWall1 == true && _isTouchingWall2 == true)
            return true;
        else
            return false;
    }
    public bool IsTouchingWallBack()
    {
        _isTouchingWall1 = Physics2D.Raycast(_wallCheck1.position, Vector2.right * -core.Movement.FacingDirection, _wallCheckDistance, groundLayer);
        _isTouchingWall2 = Physics2D.Raycast(_wallCheck2.position, Vector2.right * -core.Movement.FacingDirection, _wallCheckDistance, groundLayer);
        if (_isTouchingWall1 == true && _isTouchingWall2 == true)
            return true;
        else
            return false;
    }

    public void CanInteract()
    {
        _interactInput = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IInteractable interactable))
        {
            _canInteracting = true;
            _interactable = interactable;
        }
    }
    private void Update()
    {
        TryInteract();
    }
    private void TryInteract()
    {
        if (_canInteracting == true && _interactInput == true)
        {
            _interactable.Interact();
            _interactInput = false;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IInteractable interactable))
        {
            _canInteracting = false;          
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GroundCheck.position, _groundCheckRadius);
    }
}
