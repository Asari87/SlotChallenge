using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public enum SlotStatus { Idle, Spinning, Auto}
public class SlotManager : MonoBehaviour
{
    private SlotSettingSO slotSetting;
    [SerializeField, Tooltip("Specify where a reel should start")] 
    private Vector2[] reelPositions;
    [SerializeField] private float delayBetweenReels = .2f;
    private WaitForSeconds reelDelay;
    private List<ReelController> reels;

    private SlotStatus currentStatus;
    public Action<SlotStatus> OnSlotStatusChanged;

    private bool waitingForReels = false;
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
            if (reels.All(r => !r.IsSpinning))
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
            ReelController newReel = ReelFactory.Instance.GetReel(slotSetting.symbolsPerReel,false);
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
        foreach (ReelController reel in reels)
        {
            if (active)
                reel.Spin();
            else
                reel.Stop();
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
                SpinReels();
                break;
        }
    }

}
