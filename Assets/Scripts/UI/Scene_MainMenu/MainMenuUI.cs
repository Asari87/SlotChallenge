using Game.Core;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;

    private void Awake()
    {
        playButton.onClick.AddListener(
            () =>
            {
                //TODO: Load slot assets here
                GameManager.Instance.SceneHandler.LoadScene(1);
            });
        optionsButton.onClick.AddListener(() => GameManager.Instance.ShowSoundOptions());
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveAllListeners();
        optionsButton.onClick.RemoveAllListeners();
    }
}
