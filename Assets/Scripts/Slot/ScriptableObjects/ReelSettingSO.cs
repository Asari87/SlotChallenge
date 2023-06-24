using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class ReelSettingSO : ScriptableObject
{
    public float spinSpeed = 100;
    [Min(1)] public int numberOfSpins;
    public AnimationCurve spinSpeedCurve;

}


