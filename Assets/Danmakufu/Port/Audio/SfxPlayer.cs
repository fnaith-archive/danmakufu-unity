namespace Port.Audio
{
	public interface SfxPlayer
	{
		void SetVolume(double volume);
		double GetVolume();
		void SetMute(bool mute);
		bool GetMute();
		void Play(int audioData, double cx, double cy, bool timeScaleProof);
		void Stop();
		void Pause();
		void Resume();
		void SetPitch(double pitch);
		double GetPitch();
	}
}
