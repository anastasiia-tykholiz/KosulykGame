using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputEventContext
{
    DEFAULT,
    DIALOGUE
}

public class InputContextRouter : MonoBehaviour
{
    public static InputEventContext Current = InputEventContext.DEFAULT;
}