namespace Hertzole.SmallSteamworks
{
	public delegate void TimeTrialStatusCallback(AppID appID, bool isOffline, uint totalSeconds, uint secondsPlayed, uint secondsRemaining);
}