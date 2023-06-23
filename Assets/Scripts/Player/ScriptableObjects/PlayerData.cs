using System;
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
    private const string BALANCEKEY = "PlayerBalance";

    public Action<int> OnBalanceChanged;

    public int currentBalance;

    // Thing that could be here
    // - player sprite
    // - player action history

    private void Awake()
    {
        if(PlayerPrefs.HasKey(BALANCEKEY))
        {
            currentBalance = PlayerPrefs.GetInt(BALANCEKEY);
        }
        else
        {
            currentBalance = 100000;
            PlayerPrefs.SetInt(BALANCEKEY, currentBalance);
        }
        OnBalanceChanged?.Invoke(currentBalance);
    }

    private void OnValidate()
    {
        OnBalanceChanged?.Invoke(currentBalance);
    }


    public void AddToBalance(int amount)
    {
        currentBalance += amount;
        OnBalanceChanged?.Invoke(currentBalance);
    }

    public int GetCurrentBalance() => currentBalance;

}
