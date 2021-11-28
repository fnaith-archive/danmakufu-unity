using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Port.Audio;
using Adapter.Asset;

namespace Adapter.Audio
{
	public class UnitySfxPlayer : SfxPlayer
	{
		private const string GAME_OBJECT_NAME = "UnitySfxPlayer";
		private const float SPATIAL_BLEND = 0.5f;
		private const string TIME_SCALE_PROOF_AUDIO_MIXER_GROUP_NAME = "Sfx";
		private const string AUDIO_MIXER_GROUP_NAME = "TimeScaled";
		private const string VOLMUE_PARAMETER_NAME = "SfxVolume";
		private const string PITCH_PARAMETER_NAME = "TimeScaledPitch";

		private UnityAssetLoader unityAssetLoader;
		private AudioMixer mainMixer;
		private GameObject gameObject;
		private IDictionary<int, IList<AudioSource>> soundToAudioSources;
		private float volume;
		private bool mute;
		private float pitch;
		public UnitySfxPlayer(UnityAssetLoader unityAssetLoader, AudioMixer mixer)
		{
			this.unityAssetLoader = unityAssetLoader;
			mainMixer = mixer;
			gameObject = new GameObject();
			gameObject.name = GAME_OBJECT_NAME;
			soundToAudioSources = new Dictionary<int, IList<AudioSource>>();
			volume = 1;
			mute = false;
			pitch = 1;
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
		public void Play(int audioData, double cx, double cy, bool timeScaleProof)
		{
            AudioSource audioSource = CreateAudioSource(audioData);
            AudioClip audioClip = unityAssetLoader.GetAudio(audioData);
			audioSource.clip = audioClip;
			audioSource.spatialBlend = SPATIAL_BLEND;
			Vector3 position = audioSource.transform.position;
			position.Set((float)cx, (float)cy, 0);
			audioSource.transform.position = position;
			if (timeScaleProof)
			{
				audioSource.outputAudioMixerGroup = GetSfxGroup();
			}
			else
			{
				audioSource.outputAudioMixerGroup = GetTimeScaledGroup();
			}
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
		private AudioMixerGroup GetSfxGroup()
		{
			return mainMixer.FindMatchingGroups(TIME_SCALE_PROOF_AUDIO_MIXER_GROUP_NAME)[0];
		}
		private AudioMixerGroup GetTimeScaledGroup()
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
		public void Pause()
		{
			foreach (IList<AudioSource> audioSources in soundToAudioSources.Values)
			{
				foreach (AudioSource audioSource in audioSources)
				{
					audioSource.Pause();
				}
			}
		}
		public void Resume()
		{
			foreach (IList<AudioSource> audioSources in soundToAudioSources.Values)
			{
				foreach (AudioSource audioSource in audioSources)
				{
					audioSource.UnPause();
				}
			}
		}
		public void SetPitch(double pitch)
		{
			mainMixer.SetFloat(PITCH_PARAMETER_NAME, (float)pitch);
			this.pitch = (float)pitch;
		}
		public double GetPitch()
		{
			return pitch;
		}
	}
}
