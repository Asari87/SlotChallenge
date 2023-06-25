using Game.Core;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWinPopupListener : MonoBehaviour
{
    [SerializeField] private BigWinHandler popupPrefab;
    [SerializeField] private Transform popupParent;
    [SerializeField] private AudioClip popupAudio;

    private SlotPlayerController slotPlayerController;

    private void Awake()
    {
        slotPlayerController = GetComponent<SlotPlayerController>();
        slotPlayerController.OnWinSequenceFound += HandleWin;
    }

    private void HandleWin(int sequence, int prize)
    {
        switch (sequence)
        {
            default:
                break;
            case 5:
                ShowPopup(prize);
                break;
        }
    }

    private void ShowPopup(int prize)
    {
        Debug.Log("BIG WIN!!!");
        BigWinHandler popupHandle = Instantiate(popupPrefab, popupParent);
        popupHandle.SetPrizeText(prize);
        GameManager.Instance.SoundsPlayer.PlayEffect(popupAudio);
    }

}
