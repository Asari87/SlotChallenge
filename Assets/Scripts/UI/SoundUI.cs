using Game.Core.Sounds;

using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI
{
    public class SoundUI : MonoBehaviour
    {
        private IToggleSounds soundToggler;
        [Header("UI Elements")]
        [SerializeField] private Button effectsBtn;
        [SerializeField] private Button musicBtn;

        [SerializeField] private Slider effectSlider;
        [SerializeField] private Slider musicSlider;

        [SerializeField] private Button backBtn;

        [Header("Button Sprites")]
        [SerializeField] private Sprite effectsOn;
        [SerializeField] private Sprite effectsOff;
        [SerializeField] private Sprite musicOn;
        [SerializeField] private Sprite musicOff;

        private Image effectsImage;
        private Image musicImage;

        private bool effectsMute = false;
        private bool musicMute = false;
        private void Awake()
        {
            effectsImage = effectsBtn.GetComponent<Image>();
            musicImage = musicBtn.GetComponent<Image>();
            if (PlayerPrefs.HasKey(SoundPrefsKeys.EffectMute.ToString()))
            {
                effectsMute = PlayerPrefs.GetInt(SoundPrefsKeys.EffectMute.ToString()) == 1;
                UpdateUI();
            }
            if (PlayerPrefs.HasKey(SoundPrefsKeys.MusicMute.ToString()))
            {
                musicMute = PlayerPrefs.GetInt(SoundPrefsKeys.MusicMute.ToString()) == 1;
                UpdateUI();
            }

            if (PlayerPrefs.HasKey(SoundPrefsKeys.EffectVolume.ToString()))
            {
                effectSlider.value = PlayerPrefs.GetFloat(SoundPrefsKeys.EffectVolume.ToString());
            }
            if (PlayerPrefs.HasKey(SoundPrefsKeys.MusicVolume.ToString()))
            {
                musicSlider.value = PlayerPrefs.GetFloat(SoundPrefsKeys.MusicVolume.ToString());
            }

            effectsBtn.onClick.AddListener(() =>
            {
                soundToggler.ToggleEffects(); 
                effectsMute = !effectsMute;
                UpdateUI();
            });
            musicBtn.onClick.AddListener(() =>
            {
                soundToggler.ToggleMusic();
                musicMute = !musicMute;
                UpdateUI();
            });

            effectSlider.onValueChanged.AddListener((value) => soundToggler.SetEffectsVolume(value));
            musicSlider.onValueChanged.AddListener((value) => soundToggler.SetMusicVolume(value));

            backBtn.onClick.AddListener(() => soundToggler.SaveChanges());
        }

        private void OnEnable()
        {
            System.Collections.Generic.IEnumerable<IToggleSounds> soundTogglers = FindObjectsOfType<MonoBehaviour>().OfType<IToggleSounds>();
            soundToggler = soundTogglers?.SingleOrDefault();
            if (soundToggler == null)
                throw new System.Exception($"UI could not find the sound manager");


        }

        private void UpdateUI()
        {
            effectsImage.sprite = effectsMute ? effectsOff : effectsOn;
            musicImage.sprite = musicMute ? musicOff : musicOn;

        }



    }
}