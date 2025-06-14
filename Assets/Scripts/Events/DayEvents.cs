using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayEvents 
{
    public event Action<string> OnDayEnd;

    public void EndDay(string nextQuestId)
    {
        Debug.Log("Day Events End Day next Quest Id " + nextQuestId);
        OnDayEnd?.Invoke(nextQuestId);

    }
}
