using Game.Core;

using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

public class SlotPlayerController : MonoBehaviour
{
    private PlayerData playerData;
    private SlotManager slotManager;
    private SlotSpinButton spinButton;

    private List<LineRenderer> winLines;
    private bool autoSpinMode = false;
    public Action<int, int> OnWinSequenceFound;

    [Header("UI")]
    [SerializeField] private Button returnBtn;

    [Header("Win lines")]
    [SerializeField] private Material winLineMaterial;
    private void Awake()
    {
        winLines = new();
        playerData = Resources.Load<PlayerData>("PlayerData");
        spinButton = GetComponentInChildren<SlotSpinButton>();
        slotManager = GetComponentInChildren<SlotManager>();

        returnBtn.onClick.AddListener(() => GameManager.Instance.SceneHandler.LoadScene(0));
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
        returnBtn.onClick.RemoveAllListeners();
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

            #region Row Scan
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
            #endregion
            Debug.Log($"Results from row #{row} = {maxSquence} starting at {maxSequnceStartReel.spritePosition}");
            if(maxSquence >= 3)
            {
                DrawWinLine(maxSequnceStartReel, maxSquence);
                int prize = maxSquence * slotManager.GetPrizePerSymbol();
                playerData.AddToBalance(prize);
                OnWinSequenceFound?.Invoke(maxSquence, prize);
            }
        }
    }

    private void DrawWinLine(ReelResult maxSequnceStartReel, int maxSquence)
    {
        Vector3 startPosition = maxSequnceStartReel.spritePosition;
        Vector3 endPosition = startPosition;
        endPosition.x += 80 * (maxSquence - 1);
        LineRenderer renderer = Utilities.CreateLineRenderer("WinLine", startPosition, endPosition, winLineMaterial, 5);
        winLines.Add(renderer);
        renderer.transform.SetParent(slotManager.transform, false);

    }

    private void TrySpinningSlot()
    {
        ClearPreviousLines();
        if (slotManager.IsSlotBusy())
        {
            slotManager.StopReels();
            return;
        }
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

        TrySpinningSlot();
    }



}
