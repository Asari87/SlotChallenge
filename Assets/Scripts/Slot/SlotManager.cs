using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public enum SlotStatus { Idle, Spinning, Auto}
public class SlotManager : MonoBehaviour
{

    [Header("Reel Settings")]
    [SerializeField] private bool randomizeSpritesOnReel;
    [SerializeField, Tooltip("Specify where a reel should start")] 
    private Vector2[] reelPositions;
    [SerializeField] private float delayBetweenReels = .2f;

    [Header("Debug")]
    public bool debugMode;
    public List<int> resultList;

    private SlotSettingSO slotSetting;

    private WaitForSeconds reelDelay;
    private List<ReelController> reels;

    private SlotStatus currentStatus;
    public Action<SlotStatus> OnSlotStatusChanged;
    private bool waitingForReels = false;
    public int GetSpinCost() => slotSetting.spinCost;
    public int GetPrizePerSymbol() => slotSetting.PrizePerSymbol;

    private void Awake()
    {
        reelDelay = new WaitForSeconds(delayBetweenReels);
        slotSetting = Resources.Load<SlotSettingSO>("SlotSetting");
    }


    private void Start()
    {
        InitializeReels();
        ChangeSlotStatus(SlotStatus.Idle);
    }

    private void Update()
    {
        if(waitingForReels)
        {
            if (reels.All(r => !r.IsSpinning) && currentStatus != SlotStatus.Idle)
                ChangeSlotStatus(SlotStatus.Idle);
        }
    }

    private void ChangeSlotStatus(SlotStatus status)
    {
        currentStatus = status;
        OnSlotStatusChanged?.Invoke(currentStatus);
    }

    private void InitializeReels()
    {
        reels = new();
        foreach (Vector2 pos in reelPositions)
        {
            ReelController newReel = ReelFactory.Instance.GetReel(slotSetting.symbolsPerReel, randomizeSpritesOnReel);
            newReel.transform.SetParent(transform, false);
            newReel.transform.position = transform.TransformPoint(pos);
            reels.Add(newReel);
        }
    }

    [ContextMenu("Spin")]
    public void SpinReels()
    {
        ChangeSlotStatus(SlotStatus.Spinning);
        StartCoroutine(ToggleReels(true));
        waitingForReels = true;
    }
    [ContextMenu("Stop")]
    public void StopReels()
    {
        StartCoroutine(ToggleReels(false));
    }

    private IEnumerator ToggleReels(bool active)
    {

        for (int reelIndex = 0; reelIndex < reels.Count; reelIndex++)
        {
            if (active)
            {
                if (debugMode)
                    reels[reelIndex].Spin(resultList[reelIndex]);
                else
                    reels[reelIndex].Spin();
            }
            else
                reels[reelIndex].Stop();
            yield return reelDelay;
        }

    }

    private void OnDrawGizmos()
    {
        if (reelPositions == null || reelPositions.Length == 0) return;
        Gizmos.color = Color.red;
        foreach(Vector2 pos in reelPositions)
        {
            Gizmos.DrawSphere(transform.TransformPoint(pos), 10f);
        }
    }

    public void ToggleState()
    {
        switch (currentStatus)
        {
            case SlotStatus.Idle:
                SpinReels();
                break;
            case SlotStatus.Spinning:
                StopReels();
                break;
            case SlotStatus.Auto:
                StopReels();
                break;
        }
    }

    public ReelResult[,] GetResultMatrix()
    {
        ReelResult[,] results = new ReelResult[reels.Count, 3];
        for (int reelIndex = 0; reelIndex < results.GetLength(0); reelIndex++)
        {
            for (int rowIndex = 0; rowIndex < results.GetLength(1); rowIndex++)
            {
                results[reelIndex, rowIndex] = reels[reelIndex].GetRowResult(rowIndex);
            }
        }
        return results;
    }

    
 
}
