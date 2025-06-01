using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/PlayerData/BaseData")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    //public float movementVelocity = 4.5f;
    public float movementVelocity = 7f;

    [Header("Jump State")]
    public float jumpVelocity = 11.5f;
    public int amountOfJumps = 1;

    [Header("Wall Jump State")]
    public float wallJumpVelocity = 15f;
    public float wallJumpTime = 0.3f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);
    public string canWallJump = "false";

    [Header("In Air State")]
    public float coyoteTime = 0.1f;
    public float jumpBuffer = 0.2f;

    [Header("Wall Slide State")]
    public float wallSlideVelocity = 1.5f;

    [Header("Ledge Climb State")]
    public Vector2 startOffset;
    public Vector2 stopOffset;

}
