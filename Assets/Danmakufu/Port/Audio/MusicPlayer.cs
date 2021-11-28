namespace Port.Audio
{
	public interface MusicPlayer
	{
		void SetMusic(int audioData);
		int GetMusic();
		void SetVolume(double volume);
		double GetVolume();
		void SetMute(bool mute);
		bool GetMute();
		void Play();
		void Stop();
		void Pause();
		void Resume();
		void SetLoop(bool loop);
		bool GetLoop();
	}
}
