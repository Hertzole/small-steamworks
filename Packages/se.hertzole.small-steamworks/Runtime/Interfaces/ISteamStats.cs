#nullable enable
using System;

namespace Hertzole.SmallSteamworks
{
	public interface ISteamStats : IDisposable
	{
		bool HasCurrentStats { get; }
		bool IsLoadingCurrentStats { get; }

		void RequestCurrentStats(UserStatsReceivedCallback? callback = null);

		void RequestUserStats(in SteamID steamID, UserStatsReceivedCallback? callback = null);

		void RequestGlobalStats(in int historyDays, GlobalStatsReceivedCallback? callback = null);

		bool StoreStats();

		bool ResetAllStats(in bool achievementsToo = false);

		void SetStatInt(in string statName, in int value, in bool shouldStore = true);

		void SetStatFloat(in string statName, in float value, in bool shouldStore = true);

		int GetStatInt(in string statName);

		float GetStatFloat(in string statName);

		int GetUserStatInt(in SteamID steamID, in string statName);

		float GetUserStatFloat(in SteamID steamID, in string statName);

		long GetGlobalStatInt(in string statName);

		double GetGlobalStatFloat(in string statName);

		long[] GetGlobalStatHistoryInt(in string statName, in int maxDays = 60);

		double[] GetGlobalStatHistoryFloat(in string statName, in int maxDays = 60);
		
		bool UpdateAvgRateStat(in string statName, in float countThisSession, in double sessionLength, bool shouldStore = true);
	}
}