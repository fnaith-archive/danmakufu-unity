namespace Port.Audio
{
	public interface UiPlayer
	{
		void SetVolume(double volume);
		double GetVolume();
		void SetMute(bool mute);
		bool GetMute();
		void Play(int audioData);
		void Stop();
	}
}
