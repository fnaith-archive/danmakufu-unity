using UnityEngine;
using UnityEngine.Audio;
using Port.Audio;
using Adapter.Asset;

namespace Adapter.Audio
{
	public class UnityMusicPlayer : MusicPlayer
	{
		private const string GAME_OBJECT_NAME = "UnityMusicPlayer";
		private const string AUDIO_MIXER_GROUP_NAME = "Music";
		private const string VOLMUE_PARAMETER_NAME = "MusicVolume";

		private AudioMixer mainMixer;
		private UnityAssetLoader unityAssetLoader;
		private GameObject gameObject;
		private AudioSource audioSource;
		private int audioData;
		private float volume;
		private bool mute;
		public UnityMusicPlayer(UnityAssetLoader unityAssetLoader, AudioMixer mixer)
		{
			this.unityAssetLoader = unityAssetLoader;
			mainMixer = mixer;
			gameObject = new GameObject();
			gameObject.name = GAME_OBJECT_NAME;
			audioSource = gameObject.AddComponent<AudioSource>();
			volume = 1;
			mute = false;
		}
		private AudioMixerGroup GetMusicGroup()
		{
			return mainMixer.FindMatchingGroups(AUDIO_MIXER_GROUP_NAME)[0];
		}
		public void SetMusic(int audioData)
		{
			AudioClip audioClip = unityAssetLoader.GetAudio(audioData);
			audioSource.clip = audioClip;
			audioSource.loop = GetLoop();
			this.audioData = audioData;
		}
		public int GetMusic()
		{
			return audioData;
		}
		public void SetVolume(double volume)
		{
			mainMixer.SetFloat(VOLMUE_PARAMETER_NAME, UnityAudioControl.volumeToDecibel((float)volume));
			this.volume = (float)volume;
		}
		public double GetVolume()
		{
			return volume;
		}
		public void SetMute(bool mute)
		{
			float volume = (mute ? 0 : this.volume);
			mainMixer.SetFloat(VOLMUE_PARAMETER_NAME, volume);
			this.mute = mute;
		}
		public bool GetMute()
		{
			return mute;
		}
		public void Play()
		{
			audioSource.Play();
            audioSource.outputAudioMixerGroup = GetMusicGroup();
        }
		public void Stop()
		{
			audioSource.Stop();
		}
		public void Pause()
		{
			audioSource.Pause();
		}
		public void Resume()
		{
			audioSource.UnPause();
		}
		public void SetLoop(bool loop)
		{
			audioSource.loop = loop;
		}
		public bool GetLoop()
		{
			return audioSource.loop;
		}
	}
}
