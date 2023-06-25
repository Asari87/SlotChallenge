using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum SpinMode { Spin, Stop, Auto}

public class SlotSpinButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public SpinMode currentSpinMode;

    [Header("Visual Referencess")]
    [SerializeField] private Image buttonBackground;
    [SerializeField] private TMP_Text mainText;
    [SerializeField] private TMP_Text subText;
    [SerializeField] private Image autoSpinIndicator;

    [Header("Button Colors")]
    [SerializeField] private Color buttonRegularColor;
    [SerializeField] private Color buttonAutoSpinColor;

    [Header("Auto Spin indicator Colors")]
    [SerializeField] private Color indicatorInactiveColor;
    [SerializeField] private Color indicatorActiveColor;

    [Header("State Change Settings")]
    [SerializeField] private float buttonHoldTime = 1;
    [SerializeField] List<SpinButtonMode> buttonModes = new();

    private float timeSincePressed = 0;
    private bool isPressed = false;


    public delegate void SpinButtonEvent(bool longPress);
    public event SpinButtonEvent OnSpinPressed;
    private void Awake()
    {
        timeSincePressed = 0;
        SwitchState(currentSpinMode);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        bool isLongPress = timeSincePressed >= buttonHoldTime;
        isPressed = false;

        OnSpinPressed?.Invoke(isLongPress);
    }

    private void Update()
    {
        if (isPressed)
            timeSincePressed += Time.deltaTime;

        else
            timeSincePressed = 0;
    }

    public void HandleSlotState(SlotStatus status)
    {
        switch (status)
        {
            case SlotStatus.Idle:
                SwitchState(SpinMode.Spin);
                break;
            case SlotStatus.Spinning:
                SwitchState(SpinMode.Stop);
                break;
            case SlotStatus.Auto:
                SwitchState(SpinMode.Auto);
                break;
        }
    }

    private void SwitchState(SpinMode selectedMode)
    {

        currentSpinMode = selectedMode;
        SpinButtonMode relevantMode = buttonModes.Single(m => m.mode == selectedMode);
        mainText.text = relevantMode.buttonText;
        subText.text = relevantMode.subText;
        autoSpinIndicator.color = relevantMode.shouldIndicatorLightUp ? indicatorActiveColor : indicatorInactiveColor;
        buttonBackground.color = selectedMode == SpinMode.Auto ? buttonAutoSpinColor : buttonRegularColor;

    }



}

[System.Serializable]
public class SpinButtonMode
{
    public SpinMode mode;
    public string buttonText;
    public string subText;
    public bool shouldIndicatorLightUp;
}