using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReelController : MonoBehaviour 
{
    [SerializeField] private Image symbolTemplate;
    [SerializeField] [Range(5,9)]  private int numberOfSymbols;
    [SerializeField] private int numberOfSpins;
    [SerializeField] private int targetResult;


    private void Start()
    {
        for (int index = 0; index < numberOfSymbols; index++)
        {
            Instantiate(symbolTemplate, transform.position + index * Vector3.up * symbolTemplate.sprite.texture.height, Quaternion.identity, transform);
        }
    }
}
