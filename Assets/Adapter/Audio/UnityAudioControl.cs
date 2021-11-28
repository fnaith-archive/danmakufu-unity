using UnityEngine;
using UnityEngine.Audio;
using Port.Audio;

namespace Adapter.Audio
{
	public class UnityAudioControl : AudioControl
	{
		private const float UPPER_DECIBEL_BOUND = 20;
		private const float LOWER_DECIBEL_BOUND = -80;

		public static float volumeToDecibel(float volume)
		{
			if (0 < volume)
			{
				float decibel = UPPER_DECIBEL_BOUND * Mathf.Log10(volume);
				return decibel;
			}
			return LOWER_DECIBEL_BOUND;
		}

		private const string VOLMUE_PARAMETER_NAME = "MasterVolume";
		
		private AudioMixer mainMixer;
		UnityMusicPlayer unityMusicPlayer;
		UnitySfxPlayer unitySfxPlayer;
		UnityUiPlayer unityUiPlayer;
		private float volume;
		private bool mute;
		public UnityAudioControl(UnityMusicPlayer unityMusicPlayer, UnitySfxPlayer unitySfxPlayer, UnityUiPlayer unityUiPlayer, AudioMixer mixer)
		{
			mainMixer = mixer;
			this.unityMusicPlayer = unityMusicPlayer;
			this.unitySfxPlayer = unitySfxPlayer;
			this.unityUiPlayer = unityUiPlayer;
			volume = 1;
			mute = false;
		}
		public MusicPlayer GetMusicPlayer()
		{
			return unityMusicPlayer;
		}
		public SfxPlayer GetSfxPlayer()
		{
			return unitySfxPlayer;
		}
		public UiPlayer GetUiPlayer()
		{
			return unityUiPlayer;
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
	}
}
