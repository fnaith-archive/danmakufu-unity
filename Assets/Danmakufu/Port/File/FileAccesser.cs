namespace Port.File
{
	public interface FileAccesser
	{
		string Read(string fileName);
		void Write(string fileName, string content);
	}
}
