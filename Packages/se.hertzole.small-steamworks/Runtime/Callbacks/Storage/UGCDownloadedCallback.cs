namespace Hertzole.SmallSteamworks
{
	public delegate void UGCDownloadedCallback(bool success, SteamUGCHandle ugcHandle, AppId appId, int sizeInBytes, string fileName, SteamID ownerId);
}