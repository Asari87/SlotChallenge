using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ReelFactory
{
    private static ReelFactory _instance;

    public static ReelFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ReelFactory();
            }
            return _instance;


        }
    }

    private ReelSymbolSettingSO symbolSettings;
    private ReelFactory()
    {
        symbolSettings = Resources.Load<ReelSymbolSettingSO>("SymbolSetting");
    }

    public ReelController GetReel(int numberOfSymbols, bool randomize = false)
    {
        GameObject reelObject = new("Reel");
        ReelController reel = reelObject.AddComponent<ReelController>();
        List<int> indexList = CreateIndexList(numberOfSymbols, randomize);


        for (int index = 0; index < numberOfSymbols; index++)
        {
            Vector3 spritePosition = reel.transform.position;
            spritePosition.y += index * symbolSettings.symbolTemplate.rectTransform.sizeDelta.y;
            Image reelSymbol = GameObject.Instantiate<Image>(symbolSettings.symbolTemplate, spritePosition, Quaternion.identity, reel.transform);
            reelSymbol.sprite = symbolSettings.reelSprites[indexList[index]];
            reelSymbol.name = reelSymbol.sprite.name;
        }
        reel.ResetReel();
        return reel;
    }

    private List<int> CreateIndexList(int numberOfSymbols, bool randomize)
    {
        List<int> list = new();
        for (int i = 0; i < numberOfSymbols; i++)
            list.Add(i);
        
        if (randomize)
            Shuffle(list);

        return list;
    }


    /// <summary>
    /// Shuffle function from stackoverfloat
    /// https://stackoverflow.com/questions/273313/randomize-a-listt
    /// </summary>
    /// <param name="list"></param>
    public void Shuffle(IList<int> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}


