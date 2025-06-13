using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    [Header("V-Sync Settings")]
    
    [Range(0, 2)]
    [Tooltip("0 = off, 1 = sync ������� V-Blank (��������� V-Sync), 2 = sync ������� ������� V-Blank")]
    public int vSyncCount = 0;

    [Header("Frame Rate Limit")]
    [Tooltip("����������� ������� ����� �� �������. ������ ���� ���� V-Sync �������� (vSyncCount = 0)")]
    public int targetFrameRate = 90;

    void Awake()
    {
        QualitySettings.vSyncCount = vSyncCount;
        Application.targetFrameRate = targetFrameRate;
    }
}
