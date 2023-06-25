using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPlayerController : MonoBehaviour
{
    private PlayerData playerData;
    private SlotManager slotManager;
    private SlotSpinButton spinButton;

    private List<LineRenderer> winLines;
    private bool autoSpinMode = false;
    public Action<int> OnWinSequenceFound;
    private void Awake()
    {
        winLines = new();
        playerData = Resources.Load<PlayerData>("PlayerData");
        spinButton = GetComponentInChildren<SlotSpinButton>();
        slotManager = GetComponentInChildren<SlotManager>();

    }
    private void Start()
    {

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
        if (status == SlotStatus.Idle)
        {
            CheckResults();
            if (autoSpinMode)
            {
                TrySpinningSlot();
                return;
            }
        }
        //else, update the button visual
        spinButton.HandleSlotState(status);
    }

    private void CheckResults()
    {
        ReelResult[,] results = slotManager.GetResultMatrix();
        for (int row = 0; row < results.GetLength(1); row++)
        {
            //saved max values
            ReelResult maxSequnceStartReel = results[0,row];
            int maxSquence = 0;

            //Init loop parameters
            ReelResult SequnceStartReel = results[0, row];
            int sequenceCount = 1;
            int currentSymbol = 0;

            //Loop through all reels in the row
            for (int reel = 0; reel < results.GetLength(0); reel++)
            {
                //if first reel, reset all data to the first element
                if (reel == 0)
                {
                    SequnceStartReel = results[reel, row];
                    currentSymbol = SequnceStartReel.spriteIndex;
                    sequenceCount = 1;
                    if(maxSquence < sequenceCount)
                        maxSquence = sequenceCount;
                    continue;
                }
                //check for sequence in the following reel
                if (currentSymbol == results[reel, row].spriteIndex)
                {
                    sequenceCount++;
                    if (maxSquence < sequenceCount)
                    {
                        maxSquence = sequenceCount;
                        maxSequnceStartReel = SequnceStartReel;
                    }
                }
                //reset progess
                else
                {
                    SequnceStartReel = results[reel, row];
                    currentSymbol = SequnceStartReel.spriteIndex;
                    sequenceCount = 1;
                }
            }
            Debug.Log($"Results from row #{row} = {maxSquence} starting at {maxSequnceStartReel.spritePosition}");
            if(maxSquence >= 3)
            {
                Vector3 startPosition = maxSequnceStartReel.spritePosition;
                Vector3 endPosition = startPosition;
                endPosition.x += 80 * (maxSquence - 1);
                LineRenderer renderer = Utilities.CreateLineRenderer("Win", startPosition, endPosition, default, 5);
                winLines.Add(renderer);
                renderer.transform.SetParent(slotManager.transform, false);

                playerData.AddToBalance(maxSquence * slotManager.GetPrizePerSymbol());
                OnWinSequenceFound?.Invoke(maxSquence);

            }



        }
    }

    private void TrySpinningSlot()
    {
        ClearPreviousLines();
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

    private void ClearPreviousLines()
    {
        foreach(LineRenderer renderer in winLines)
            Destroy(renderer.gameObject);
        winLines.Clear();
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
