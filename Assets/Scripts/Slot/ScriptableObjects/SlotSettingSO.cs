using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SlotSettingSO : ScriptableObject
{
    public int spinCost = 1000;
    [Range(5, 9)]
    public int symbolsPerReel = 5;
    public int PrizePerSymbol = 5000;
}
