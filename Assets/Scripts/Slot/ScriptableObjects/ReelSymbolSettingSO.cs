using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class ReelSymbolSettingSO : ScriptableObject
{
    public Image symbolTemplate;
    public List<Sprite> reelSprites;
    public int cyclesBeforeStopping;
}
