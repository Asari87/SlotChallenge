using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This scriptable object contains the player data across the game
/// </summary>
/// <remarks>
/// There should be only one object of this type
/// </remarks>
[CreateAssetMenu()]
public class PlayerData : ScriptableObject
{
    public int currentBalance;

    // Thing that could be here
    // - player sprite
    // - player action history

}
