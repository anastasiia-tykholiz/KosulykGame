using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public Movement Movement { get; private set; }
    public Stats Stats { get; private set; }
    public CollisionSenses CollisionSenses { get; private set; }

    private List<ILogicUpdate> components = new List<ILogicUpdate>();

    private void Awake()
    {
        Movement = GetComponentInChildren<Movement>();
        Stats = GetComponentInChildren<Stats>();
        CollisionSenses = GetComponentInChildren<CollisionSenses>();

        if (Movement == null)
        {
            Debug.LogError("Missing Core component");
        }
    }

    public void LogicUpdate()
    {
        foreach (ILogicUpdate component in components)
        {
            component.LogicUpdate();
        }
    }

    public void AddComponent(ILogicUpdate component)
    {
        if (components.Contains(component) == false)
        {
            components.Add(component);
        }
    }
}
