using UnityEngine;
namespace Game.Core.Sounds
{
    public interface IPlaySounds
    {
        /// <summary>
        /// Play effect clip (one shot)
        /// </summary>
        /// <param name="clip"></param>
        public void PlayEffect(AudioClip clip);

        /// <summary>
        /// Play effect clip at a certain position and volume
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="volume"></param>
        public void PlayEffectAtPosition(AudioClip clip, Vector3 position, float volume = 1.0f);

        /// <summary>
        /// Play ambiance music
        /// </summary>
        /// <param name="clip"></param>
        public void PlayMusic(AudioClip clip);
    }
}