using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

using UnityEngine;
using UnityEngine.UI;

public class ReelController : MonoBehaviour 
{
    [SerializeField] private int targetResult;
    [SerializeField] private float spinCounter;

    private ReelSettingSO reelSetting;
    private Transform[] symbols;
    
    private float spriteHeight;
    
    private float currentSpinSpeed = 0;
    private float startTime;
    private float reelLength;
    private Coroutine spinRoutine;
    public bool IsSpinning { get; private set; } = false;
    private bool spinSequence = false;

    private void Awake()
    {
        reelSetting = Resources.Load<ReelSettingSO>("ReelSetting");
    }
    public void ResetReel()
    {
        symbols = new Transform[transform.childCount];
        for (int index = 0; index < transform.childCount; index++)
        {
            symbols[index] = transform.GetChild(index);
        }

        Transform firstChild = transform.GetChild(0);
        Image SpriteImage = firstChild.GetComponent<Image>();
        spriteHeight = SpriteImage.rectTransform.sizeDelta.y;
        reelLength = spriteHeight * symbols.Length;
    }

    private void Update()
    {
        if (!IsSpinning) return;
        SpinLogic();
    }

    private void SpinLogic()
    {
        Vector3 movementDelta = currentSpinSpeed * Time.deltaTime * Vector3.up;

        MoveSymbols(movementDelta);
        CountSpins(movementDelta);
        CheckforOutOfBounds();
    }

    private void MoveSymbols(Vector3 movementDelta)
    {
        foreach (Transform child in transform)
        {
            child.position -= movementDelta;
        }
    }

    private void CountSpins(Vector3 movementDelta)
    {
        if (spinSequence)
        {
            spinCounter += movementDelta.y;
            if (spinCounter / reelLength > reelSetting.numberOfSpins)
                Stop();
        }
    }

    private void CheckforOutOfBounds()
    {
        //Check for out of bounds
        Transform bottomSymbol = transform.GetChild(0);
        if (bottomSymbol.position.y < spriteHeight / 2)
        {
            Vector3 pos = bottomSymbol.position;
            pos.y = transform.GetChild(transform.childCount - 1).position.y + spriteHeight / 2;
            bottomSymbol.position = pos;
            bottomSymbol.SetAsLastSibling();
        }
    }

    [ContextMenu("Spin")]
    public void Spin()
    {
        targetResult = UnityEngine.Random.Range(0,symbols.Length);
        ToggleSpin(true);

    }

    public void Spin(int targetResult)
    {
        this.targetResult = Mathf.Clamp(targetResult,0,symbols.Length);
        ToggleSpin(true);
    }

    [ContextMenu("Stop")]
    public void Stop()
    {

        ToggleSpin(false);
    }


    private void ToggleSpin(bool shouldSpin)
    {
        if (spinRoutine != null)
        {
            StopCoroutine(spinRoutine);
        }
        spinSequence = shouldSpin;
        if (shouldSpin)
        {
            spinCounter = 0;
            IsSpinning = true;
        }


        startTime = Time.time;
        spinRoutine = StartCoroutine(AdjustReelSpeed(shouldSpin));
    }



    private IEnumerator AdjustReelSpeed(bool shouldSpin)
    {
        float targetSpeed = shouldSpin ? reelSetting.spinSpeed : 0;
        while (currentSpinSpeed != targetSpeed)
        {
            if(shouldSpin)  //free spin
                currentSpinSpeed = Mathf.Lerp(currentSpinSpeed, targetSpeed, reelSetting.spinSpeedCurve.Evaluate(Time.time / startTime));
            else            //stop at target
            {
                float distanceToTarget = Mathf.Abs(symbols[targetResult].position.y - transform.position.y);
                currentSpinSpeed = Mathf.Lerp(currentSpinSpeed, targetSpeed, distanceToTarget);
            }

            yield return null;
        }

        if (!shouldSpin)
            CorrectPositions();

        yield return null;
        IsSpinning = false;
     }

    private void CorrectPositions()
    {
        symbols[targetResult].position = transform.position;
        int spriteCount = 0;
        int index;
        for (index = targetResult; index < symbols.Length; index++)
        {
            if(index == targetResult)
            {
                spriteCount++;
                continue;
            }
            symbols[index].position = transform.position + Vector3.up * (spriteCount++) * spriteHeight / 2;
            symbols[index].SetAsLastSibling();
            CheckforOutOfBounds();
        }
        for (index = 0; index < targetResult; index++)
        {
            symbols[index].position = transform.position + Vector3.up * (spriteCount++) * spriteHeight / 2;
            symbols[index].SetAsLastSibling();
            CheckforOutOfBounds();
        }
    }

    public ReelResult GetRowResult(int row)
    {
        Transform symbol = transform.GetChild(row);
        return new ReelResult(symbol.position, int.Parse(symbol.name));
    }
}


public struct ReelResult
{
    public Vector3 spritePosition;
    public int spriteIndex;

    public ReelResult(Vector3 spritePosition, int spriteIndex)
    {
        this.spritePosition = spritePosition;
        this.spriteIndex = spriteIndex;
    }
}

