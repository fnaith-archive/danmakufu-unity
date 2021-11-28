using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Port.Audio;
using Adapter.Asset;

namespace Adapter.Audio
{
	public class UnityUiPlayer : UiPlayer
	{
		private const string GAME_OBJECT_NAME = "UnityUiPlayer";
		private const string AUDIO_MIXER_GROUP_NAME = "Ui";
		private const string VOLMUE_PARAMETER_NAME = "UiVolume";

		private UnityAssetLoader unityAssetLoader;
		private AudioMixer mainMixer;
		private GameObject gameObject;
		private IDictionary<int, IList<AudioSource>> soundToAudioSources;
		private float volume;
		private bool mute;
		public UnityUiPlayer(UnityAssetLoader unityAssetLoader, AudioMixer mixer)
		{
			this.unityAssetLoader = unityAssetLoader;
			mainMixer = mixer;
			gameObject = new GameObject();
			gameObject.name = GAME_OBJECT_NAME;
			soundToAudioSources = new Dictionary<int, IList<AudioSource>>();
			volume = 1;
			mute = false;
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
		public void Play(int audioData)
		{
			AudioClip audioClip = unityAssetLoader.GetAudio(audioData);
			AudioSource audioSource = CreateAudioSource(audioData);
			audioSource.clip = audioClip;
			audioSource.outputAudioMixerGroup = GetUiGroup();
			audioSource.PlayOneShot(audioClip);
		}
		private AudioSource CreateAudioSource(int audioData)
		{
			IList<AudioSource> audioSources = GetAudioSources(audioData);
			AudioSource newAudioSource = null;
			foreach (AudioSource audioSource in audioSources)
			{
				if (!audioSource.isPlaying)
				{
					newAudioSource = audioSource;
					break;
				}
			}
			if (null == newAudioSource)
			{
				newAudioSource = gameObject.AddComponent<AudioSource>();
				audioSources.Add(newAudioSource);
			}
			return newAudioSource;
		}
		private IList<AudioSource> GetAudioSources(int audioData)
		{
			IList<AudioSource> audioSources;
			if (!soundToAudioSources.TryGetValue(audioData, out audioSources))
			{
				audioSources = new List<AudioSource>();
				soundToAudioSources.Add(audioData, audioSources);
			}
			return audioSources;
		}
		private AudioMixerGroup GetUiGroup()
		{
			return mainMixer.FindMatchingGroups(AUDIO_MIXER_GROUP_NAME)[0];
		}
		public void Stop()
		{
			foreach (IList<AudioSource> audioSources in soundToAudioSources.Values)
			{
				foreach (AudioSource audioSource in audioSources)
				{
					audioSource.Stop();
				}
			}
		}
	}
}
