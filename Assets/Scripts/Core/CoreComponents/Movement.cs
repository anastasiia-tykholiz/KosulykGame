using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Movement : CoreComponent
{
    public Vector2 CurrentVelocity { get; private set; }

    public int FacingDirection { get; private set; }

    public Rigidbody2D Rigidbody2d { get; private set; }

    private Vector2 workspace;

    protected override void Awake()
    {
        base.Awake();

        FacingDirection = 1;

        Rigidbody2d = GetComponentInParent<Rigidbody2D>();
    }

    public override void LogicUpdate()
    {
        CurrentVelocity = Rigidbody2d.velocity;
    }

    public float GetVelocityY()
    {
        return Rigidbody2d.velocity.y;
    }
    public void SetVelocityZero()
    {
        Rigidbody2d.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        Rigidbody2d.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, Rigidbody2d.velocity.y);
        Rigidbody2d.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetVelocityY(float velocity)
    {
        workspace.Set(Rigidbody2d.velocity.x, velocity);
        Rigidbody2d.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    private void Flip()
    {
        FacingDirection *= -1;
        Rigidbody2d.transform.Rotate(0, 180, 0);
    }
}
