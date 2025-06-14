using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayEvents 
{
    public event Action OnDayEnd;

    public void EndDay()
    {
        OnDayEnd?.Invoke();
    }
}
