using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SlotSettingSO : ScriptableObject
{
    public int spinCost = 1000;
    public float spinSpeed;
    public int cyclesBeforeStopping;
    [Range(5, 9)]
    public int symbolsPerReel;
}
