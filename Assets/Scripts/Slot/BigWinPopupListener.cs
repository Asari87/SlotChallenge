using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWinPopupListener : MonoBehaviour
{
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private AudioClip popupAudio;

    private SlotPlayerController slotPlayerController;

    private void Awake()
    {
        slotPlayerController = GetComponent<SlotPlayerController>();
        slotPlayerController.OnWinSequenceFound += HandleWin;
    }

    private void HandleWin(int sequence)
    {
        switch (sequence)
        {
            default:
                break;
            case 5:
                ShowPopup();
                break;
        }
    }

    private void ShowPopup()
    {
        StartCoroutine(DisplayPopup());
    }

    private IEnumerator DisplayPopup()
    {
        Debug.Log("BIG WIN!!!");
        yield return null;
    }
}
