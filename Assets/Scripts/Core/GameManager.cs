using Game.Core.SceneManagment;
using Game.Core.Sounds;

using UnityEngine;

namespace Game.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public IPlaySounds SoundsPlayer;
        public IHandleScenes SceneHandler;
        public Canvas soundOptionsCanvas;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

            SoundsPlayer = GetComponentInChildren<IPlaySounds>();
            SceneHandler = GetComponentInChildren<IHandleScenes>();
        }

        [ContextMenu("Show Sound Menu")]
        public void ShowSoundOptions()
        {
            soundOptionsCanvas.enabled = true;
        }
    }
}