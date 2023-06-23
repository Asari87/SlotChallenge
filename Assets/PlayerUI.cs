using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField, Tooltip("UI Text to be updated")] 
    private TMP_Text balanceText;
    
    [Header("Lerp animation settings")]
    [SerializeField, Tooltip("Controlls the lerp of the UI animation")] 
    private AnimationCurve lerpSpeedCurve;



    private PlayerData playerData;
    private int currentDisplayedBalance;
    private Coroutine balanceUpdateRoutine;
    private void Awake()
    {
        playerData = Resources.Load<PlayerData>("PlayerData");
        playerData.OnBalanceChanged += UpdateUI;

        //initialize
        UpdateUI(playerData.GetCurrentBalance());
    }
    private void OnDestroy()
    {
        playerData.OnBalanceChanged -= UpdateUI;
    }
    private void UpdateUI(int currentBalance)
    {
        if(balanceUpdateRoutine != null)
            StopCoroutine(balanceUpdateRoutine);
        balanceUpdateRoutine = StartCoroutine(AnimateBalanceCounter(currentBalance));
    }

    private IEnumerator AnimateBalanceCounter(int targetBalance)
    {
        float time = 0;
        while(time < 1)
        {
            currentDisplayedBalance = (int) Mathf.Lerp(currentDisplayedBalance, targetBalance, lerpSpeedCurve.Evaluate(time));
            time += Time.deltaTime;
            balanceText.text = currentDisplayedBalance.ToString();
            yield return null;
        }
        //last frame
        currentDisplayedBalance = targetBalance;
        balanceText.text = currentDisplayedBalance.ToString();
    }

    
}
