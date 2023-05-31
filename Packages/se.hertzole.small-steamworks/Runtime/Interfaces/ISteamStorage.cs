#nullable enable
using System;

namespace Hertzole.SmallSteamworks
{
	public interface ISteamStorage : IDisposable
	{
		/// <summary>
		///     <para>
		///         Creates a new file, writes the bytes to the file, and then closes the file. If the target file already
		///         exists, it is overwritten.
		///     </para>
		///     <para>
		///         NOTE: This is a synchronous call and as such is a will block your calling thread on the disk IO, and will
		///         also block the SteamAPI, which can cause other threads in your application to block. To avoid "hitching", you
		///         should use <see cref="WriteFile" /> instead.
		///     </para>
		/// </summary>
		/// <param name="fileName">The name of the file to write to.</param>
		/// <param name="data">The bytes to write to the file.</param>
		/// <returns>Returns a response with if the write was successful or not.</returns>
		FileWrittenResponse WriteFileSynchronous(string fileName, byte[] data);

		/// <summary>
		///     <para>
		///         Creates a new file and asynchronously writes the raw byte data to the Steam Cloud, and then closes the file.
		///         If the target file already exists, it is overwritten.
		///     </para>
		/// </summary>
		/// <param name="fileName">The name of the file to write to.</param>
		/// <param name="data">The bytes to write to the file.</param>
		/// <param name="callback">The callback when the file writing has completed.</param>
		void WriteFile(string fileName, byte[] data, FileWrittenCallback? callback = null);

		/// <summary>
		///     <para>Opens a binary file, reads the contents of the file into a byte array, and then closes the file.</para>
		///     <para>
		///         NOTE: This is a synchronous call and as such is a will block your calling thread on the disk IO, and will
		///         also block the SteamAPI, which can cause other threads in your application to block. To avoid "hitching", you
		///         should use <see cref="ReadFile" /> instead.
		///     </para>
		/// </summary>
		/// <param name="fileName">The name of the file to read from.</param>
		/// <returns>Returns a response with if the read was successful and the data of the file.</returns>
		FileReadResponse ReadFileSynchronous(string fileName);

		/// <summary>
		///     Opens a binary file, reads the contents of the file asynchronously into a byte array, and then closes the file.
		/// </summary>
		/// <param name="fileName">The name of the file to read from.</param>
		/// <param name="callback">The callback when the file reading has completed.</param>
		void ReadFile(string fileName, FileReadCallback? callback = null);

		/// <summary>
		///     Check whether the specified file exists.
		/// </summary>
		/// <param name="fileName">The name of the file.</param>
		/// <returns>True if the file exists; otherwise false.</returns>
		bool FileExists(string fileName);

		/// <summary>
		///     Check whether the specified file is persisted in the Steam Cloud.
		/// </summary>
		/// <param name="fileName">The name of the file.</param>
		/// <returns>
		///     True if the file exists and is persisted in the Steam cloud; false if <see cref="ForgetFile" /> was called on
		///     it and is only available locally.
		/// </returns>
		bool FilePersisted(string fileName);

		/// <summary>
		///     Deletes the file from remote storage, but leaves it on the local disk and remains accessible from the API.
		/// </summary>
		/// <param name="fileName">The name of the file that will be forgotten.</param>
		/// <returns>True if the file exists and has been successfully forgotten; otherwise, false.</returns>
		bool ForgetFile(string fileName);

		/// <summary>
		///     Deletes a file from the local disk, and propagates that delete to the cloud.
		/// </summary>
		/// <param name="fileName">	The name of the file that will be deleted.</param>
		/// <returns>True if the file exists and has been successfully deleted; otherwise, false if the file did not exist.</returns>
		bool DeleteFile(string fileName);

		/// <summary>
		///     Gets the specified files size in bytes.
		/// </summary>
		/// <param name="fileName">	The name of the file.</param>
		/// <returns>The size of the file in bytes. Returns 0 if the file does not exist.</returns>
		int GetFileSize(string fileName);

		/// <summary>
		///     Gets the number of bytes available, and used on the users Steam Cloud storage.
		/// </summary>
		/// <returns>A response that contains the amount of bytes the user has, how many are used, and how many are available.</returns>
		QuotaResponse GetQuota();

		/// <summary>
		///     <para>
		///         Start doing multiple file operations in a single state change. For example, if saving game progress requires
		///         updating both savegame1.dat amd maxprogress.dat, wrap those operations with calls within this batch.
		///     </para>
		///     <para>
		///         These functions provide a hint to Steam which will help it manage the app's Cloud files. Using these
		///         functions is optional, however it will provide better reliability.
		///     </para>
		/// </summary>
		/// <returns>A write batch that should be used within a using statement.</returns>
		SteamWriteBatch WriteBatch();

		/// <summary>
		///     <para>
		///         Creates a new file output stream allowing you to stream out data to the Steam Cloud file in chunks. If the
		///         target file already exists, it is not overwritten until the stream has been closed.
		///     </para>
		///     <para>
		///         To write data to the stream, use <see cref="SteamWriteStream.WriteChunk" />. You can also cancel the stream
		///         using <see cref="SteamWriteStream.Cancel" /> to avoid writing a file.
		///     </para>
		/// </summary>
		/// <param name="fileName">The name of the file to write to.</param>
		/// <returns>A stream that should be used within a using statement.</returns>
		SteamWriteStream WriteStream(string fileName);

		/// <summary>
		///     Shares a file publicly that can be accessed by the community. You can use the file to, for example, to attach to a
		///     leaderboard entry.
		/// </summary>
		/// <param name="fileName">The name of the file to share.</param>
		/// <param name="callback">The callback when the share process has completed.</param>
		void ShareFile(string fileName, FileSharedCallback? callback = null);

		/// <summary>
		///     Downloads a shared file from the Steam Cloud.
		/// </summary>
		/// <param name="ugcHandle">The handle to download from.</param>
		/// <param name="priority">The priority of the file. 0 is the highest priority.</param>
		/// <param name="callback">The callback when the download has completed.</param>
		void DownloadSharedFile(SteamUGCHandle ugcHandle, uint priority = 0, UGCDownloadedCallback? callback = null);

		/// <summary>
		///     Downloads a shared file from the Steam Cloud to a specific location.
		/// </summary>
		/// <param name="ugcHandle">The handle to download from.</param>
		/// <param name="location">The location to put the file.</param>
		/// <param name="priority">The priority of the file. 0 is the highest priority.</param>
		/// <param name="callback">The callback when the download has completed.</param>
		void DownloadSharedFileToLocation(SteamUGCHandle ugcHandle, string location, uint priority = 0, UGCDownloadedCallback? callback = null);

		/// <summary>
		///     Reads a downloaded shared file. You must have downloaded the file first using <see cref="DownloadSharedFile" /> or
		///     <see cref="DownloadSharedFileToLocation" /> before calling this method.
		/// </summary>
		/// <param name="ugcHandle">The handle to read from.</param>
		/// <param name="fileSize">The file size.</param>
		/// <returns>A byte array with the contents of the file.</returns>
		byte[] ReadSharedFile(SteamUGCHandle ugcHandle, out int fileSize);
	}
}