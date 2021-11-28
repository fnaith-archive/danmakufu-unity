namespace Port.Audio
{
	public interface AudioControl
	{
		MusicPlayer GetMusicPlayer();
		SfxPlayer GetSfxPlayer();
		UiPlayer GetUiPlayer();
		void SetVolume(double volume);
		double GetVolume();
		void SetMute(bool mute);
		bool GetMute();
	}
}
