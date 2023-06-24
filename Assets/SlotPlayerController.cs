using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPlayerController : MonoBehaviour
{
    private PlayerData playerData;
    private SlotManager slotManager;
    private SlotSpinButton spinButton;

    private bool autoSpinMode = false;
    private void Awake()
    {
        playerData = Resources.Load<PlayerData>("PlayerData");
        spinButton = GetComponentInChildren<SlotSpinButton>();
        slotManager = GetComponentInChildren<SlotManager>();

        slotManager.OnSlotStatusChanged += HandleSlotStatusChange;
        spinButton.OnSpinPressed += HandleSpinPress;
    }
    private void OnDestroy()
    {
        slotManager.OnSlotStatusChanged -= HandleSlotStatusChange;
        spinButton.OnSpinPressed -= HandleSpinPress;

    }
    private void HandleSlotStatusChange(SlotStatus status)
    {
        if(autoSpinMode)
        {
            if(status == SlotStatus.Idle)
            {
                TrySpinningSlot();
            }

            return;
        }
        //else, update the button visual
        spinButton.HandleSlotState(status);
    }

    private void TrySpinningSlot()
    {
        if(playerData.GetCurrentBalance() >= slotManager.GetSpinCost())
        {
            playerData.TakeFromBalance(slotManager.GetSpinCost());
            slotManager.SpinReels();
        }
        else
        {
            Debug.LogWarning("Not enough money to spin");
            autoSpinMode = false;
        }
    }

    private void HandleSpinPress(bool longPress)
    {
        autoSpinMode = longPress && !autoSpinMode;


        if (autoSpinMode)
            spinButton.HandleSlotState(SlotStatus.Auto);
        else
            TrySpinningSlot();
    }



}
