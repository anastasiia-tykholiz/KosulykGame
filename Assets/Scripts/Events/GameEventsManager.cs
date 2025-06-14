using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }
   
    // тому що інакше воно null
    public static readonly InputEvents inputEvents = new InputEvents();
    public static readonly QuestEvents questEvents = new QuestEvents();
    public static readonly PlayerEvents playerEvents = new PlayerEvents();
    public static readonly DayEvents dayEvents = new DayEvents();

    public static readonly DialogueEvents dialogueEvents = new DialogueEvents();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        instance = this;

    }
}