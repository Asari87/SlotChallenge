using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class BigWinHandler : MonoBehaviour
{
    [SerializeField] private Text prizeText;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button greatBtn;

    private void Awake()
    {
        closeBtn.onClick.AddListener(() => Destroy(gameObject));
        greatBtn.onClick.AddListener(() => Destroy(gameObject));
    }

    public void SetPrizeText(int prize)
    {
        prizeText.text = prize.ToString();
    }

    private void OnDestroy()
    {
        closeBtn.onClick.RemoveAllListeners();
        greatBtn.onClick.RemoveAllListeners();
    }
}
