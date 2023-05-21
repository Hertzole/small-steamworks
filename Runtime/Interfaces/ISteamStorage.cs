#nullable enable
using System;

namespace Hertzole.SmallSteamworks
{
	public interface ISteamStorage : IDisposable
	{
		bool WriteFileSynchronous(string fileName, byte[] data);

		void WriteFile(string fileName, byte[] data, FileWrittenCallback? callback = null);

		FileReadResponse ReadFileSynchronous(string fileName);

		void ReadFile(string fileName, FileReadCallback? callback = null);

		bool FileExists(string fileName);

		bool FilePersisted(string fileName);

		bool ForgetFile(string fileName);

		bool DeleteFile(string fileName);

		int GetFileSize(string fileName);
		
		QuotaResponse GetQuota();

		SteamWriteBatch WriteBatch();

		SteamWriteStream WriteStream(string fileName);
		
		void ShareFile(string fileName, FileSharedCallback? callback = null);
		
		void DownloadSharedFile(SteamUGCHandle ugcHandle, uint priority = 0, UGCDownloadedCallback? callback = null);

		void DownloadSharedFileToLocation(SteamUGCHandle ugcHandle, string location, uint priority = 0, UGCDownloadedCallback? callback = null);
		
		byte[] ReadUGCFile(SteamUGCHandle ugcHandle, out int fileSize);
	}
}