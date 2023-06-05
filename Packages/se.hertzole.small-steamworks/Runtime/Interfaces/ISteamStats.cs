#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

#nullable enable
using System;

namespace Hertzole.SmallSteamworks
{
	public interface ISteamStats : IDisposable
	{
		/// <summary>
		///     If the current user's stats have been received yet.
		/// </summary>
		bool HasCurrentStats { get; }
		/// <summary>
		///     If the current user's stats are currently being loaded.
		/// </summary>
		bool IsLoadingCurrentStats { get; }

		/// <summary>
		///     Requests the current user's statistics from the Steam server asynchronously.
		/// </summary>
		/// <param name="callback">
		///     Optional. A callback function to be called when the user's statistics are received. Pass null if
		///     no callback is needed.
		/// </param>
		/// <remarks>
		///     <para>
		///         Use this method to asynchronously request the current user's statistics from the Steam server. The user's
		///         statistics include various data such as achievement progress, playtime, and other tracked values.
		///     </para>
		///     <para>
		///         If you provide a <paramref name="callback" /> function, it will be called when the user's statistics are
		///         received. You can use the callback to handle the received data and perform further actions or updates in your
		///         application. Pass null if you don't need a callback.
		///     </para>
		///     <para>
		///         It's important to note that requesting statistics from the Steam server is an asynchronous operation that may
		///         take some time, depending on the network and server conditions. Therefore, it's recommended to use a callback
		///         or await the operation to handle the received data asynchronously and avoid blocking the main thread.
		///     </para>
		/// </remarks>
		void RequestCurrentStats(UserStatsReceivedCallback? callback = null);

		/// <summary>
		///     Requests the statistics of a specific user from the Steam server asynchronously.
		/// </summary>
		/// <param name="steamID">The Steam ID of the user whose statistics to request.</param>
		/// <param name="callback">
		///     Optional. A callback function to be called when the user's statistics are received. Pass null if
		///     no callback is needed.
		/// </param>
		/// <remarks>
		///     <para>
		///         Use this method to asynchronously request the statistics of a specific user from the Steam server. The user's
		///         statistics include various data such as achievement progress, playtime, and other tracked values.
		///     </para>
		///     <para>
		///         If you provide a <paramref name="callback" /> function, it will be called when the user's statistics are
		///         received. You can use the callback to handle the received data and perform further actions or updates in your
		///         application. Pass null if you don't need a callback.
		///     </para>
		///     <para>
		///         It's important to note that requesting statistics from the Steam server is an asynchronous operation that may
		///         take some time, depending on the network and server conditions. Therefore, it's recommended to use a callback
		///         or await the operation to handle the received data asynchronously and avoid blocking the main thread.
		///     </para>
		/// </remarks>
		void RequestUserStats(in SteamID steamID, UserStatsReceivedCallback? callback = null);

		/// <summary>
		///     Requests global statistics from the Steam server asynchronously.
		/// </summary>
		/// <param name="historyDays">
		///     The number of days of history to request for the global statistics. The maximum amount of
		///     days is 60.
		/// </param>
		/// <param name="callback">
		///     Optional. A callback function to be called when the global statistics are received. Pass null if
		///     no callback is needed.
		/// </param>
		/// <remarks>
		///     <para>
		///         Use this method to asynchronously request global statistics from the Steam server. Global statistics are
		///         cumulative data across all users of the application and can include various aggregated values such as playtime,
		///         number of achievements unlocked, and more.
		///     </para>
		///     <para>
		///         The <paramref name="historyDays" /> parameter specifies the number of days of history to request for the global
		///         statistics. This determines the range of historical data that will be retrieved.
		///     </para>
		///     <para>
		///         If you provide a <paramref name="callback" /> function, it will be called when the global statistics are
		///         received. You can use the callback to handle the received data and perform further actions or updates in your
		///         application. Pass null if you don't need a callback.
		///     </para>
		///     <para>
		///         It's important to note that requesting global statistics from the Steam server is an asynchronous operation
		///         that may take some time, depending on the network and server conditions. Therefore, it's recommended to use a
		///         callback or await the operation to handle the received data asynchronously and avoid blocking the main thread.
		///     </para>
		/// </remarks>
		void RequestGlobalStats(in int historyDays, GlobalStatsReceivedCallback? callback = null);

		/// <summary>
		///     Stores and submits the current user's statistics to the Steam server.
		/// </summary>
		/// <returns>
		///     <c>true</c> if the statistics were successfully stored and submitted; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>
		///     Use this method to store and submit the current user's statistics to the Steam server. The method returns
		///     <c>true</c> if the statistics were successfully stored and submitted, and <c>false</c> otherwise.
		///     Before calling this method, make sure you have appropriately set the values of the relevant statistics using the
		///     appropriate Steam API functions.
		///     It's recommended to call this method at appropriate points in your game's logic, such as after a match has ended or
		///     when a significant event has occurred that affects the player's statistics.
		/// </remarks>
		bool StoreStats();

		/// <summary>
		///     Resets all statistics and optionally achievements for the current user.
		/// </summary>
		/// <param name="achievementsToo">Optional. Specifies whether to reset achievements as well. Defaults to <c>false</c>.</param>
		/// <returns>
		///     <c>true</c> if the stats and/or achievements were successfully reset; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>
		///     Use this method to reset all statistics for the current user. By default, only the statistics are reset. If you
		///     want to reset both the statistics and the achievements, set the <paramref name="achievementsToo" /> parameter to
		///     <c>true</c>.
		///     It's recommended to prompt the user for confirmation before calling this method, as resetting the stats and/or
		///     achievements will result in permanent loss of progress.
		///     After calling this method, <see cref="StoreStats" /> will be called automatically to submit the reset stats and/or
		///     achievements to the Steam server.
		/// </remarks>
		bool ResetAllStats(in bool achievementsToo = false);

		/// <summary>
		///     Sets the value of an integer statistic for the current user.
		/// </summary>
		/// <param name="statName">The name of the statistic to set.</param>
		/// <param name="value">The value to set for the statistic.</param>
		/// <param name="shouldStore">
		///     Optional. Specifies whether the statistic should be stored immediately. Defaults to
		///     <c>true</c>.
		/// </param>
		/// <remarks>
		///     <para>
		///         Use this method to set the value of an integer statistic for the current user. The <paramref name="statName" />
		///         parameter specifies the name of the statistic, and the <paramref name="value" /> parameter specifies the value
		///         to set.
		///     </para>
		///     <para>
		///         By default, the statistic will be stored immediately, and you don't need to manually call
		///         <see cref="StoreStats" />. However, if you want to delay the storage of the statistic, set the
		///         <paramref name="shouldStore" /> parameter to <c>false</c>.
		///     </para>
		///     <para>
		///         If you did set <paramref name="shouldStore" /> to <c>false</c>, you must call <see cref="StoreStats" /> at an
		///         appropriate time later.
		///     </para>
		///     <para>
		///         Note that statistics are associated with the current user's Steam account and can be accessed by other users
		///         through Steam Community features.
		///     </para>
		/// </remarks>
		void SetStatInt(in string statName, in int value, in bool shouldStore = true);

		/// <summary>
		///     Sets the value of a floating-point statistic for the current user.
		/// </summary>
		/// <param name="statName">The name of the statistic to set.</param>
		/// <param name="value">The value to set for the statistic.</param>
		/// <param name="shouldStore">
		///     Optional. Specifies whether the statistic should be stored immediately. Defaults to
		///     <c>true</c>.
		/// </param>
		/// <remarks>
		///     <para>
		///         Use this method to set the value of a floating-point statistic for the current user. The
		///         <paramref name="statName" /> parameter specifies the name of the statistic, and the <paramref name="value" />
		///         parameter specifies the value to set.
		///     </para>
		///     <para>
		///         By default, the statistic will be stored immediately, and you don't need to manually call
		///         <see cref="StoreStats" />. However, if you want to delay the storage of the statistic, set the
		///         <paramref name="shouldStore" /> parameter to <c>false</c>.
		///     </para>
		///     <para>
		///         If you did set <paramref name="shouldStore" /> to <c>false</c>, you must call <see cref="StoreStats" /> at an
		///         appropriate time later.
		///     </para>
		///     <para>
		///         Note that statistics are associated with the current user's Steam account and can be accessed by other users
		///         through Steam Community features.
		///     </para>
		/// </remarks>
		void SetStatFloat(in string statName, in float value, in bool shouldStore = true);

		/// <summary>
		///     Retrieves the value of an integer statistic for the current user.
		/// </summary>
		/// <param name="statName">The name of the statistic to retrieve.</param>
		/// <returns>The value of the specified integer statistic.</returns>
		/// <remarks>
		///     <para>
		///         Use this method to retrieve the value of an integer statistic for the current user. The
		///         <paramref name="statName" /> parameter specifies the name of the statistic.
		///     </para>
		///     <para>
		///         It's important to note that the statistic must have been previously set using the appropriate Steam API
		///         function. If the statistic hasn't been set or doesn't exist, this method will return 0.
		///     </para>
		///     <para>
		///         The retrieved statistic value represents the current state of the statistic as stored on the Steam server.
		///     </para>
		/// </remarks>
		int GetStatInt(in string statName);

		/// <summary>
		///     Retrieves the value of a floating-point statistic for the current user.
		/// </summary>
		/// <param name="statName">The name of the statistic to retrieve.</param>
		/// <returns>The value of the specified floating-point statistic.</returns>
		/// <remarks>
		///     <para>
		///         Use this method to retrieve the value of a floating-point statistic for the current user. The
		///         <paramref name="statName" /> parameter specifies the name of the statistic.
		///     </para>
		///     <para>
		///         It's important to note that the statistic must have been previously set using the appropriate Steam API
		///         function. If the statistic hasn't been set or doesn't exist, this method will return 0.0f.
		///     </para>
		///     <para>
		///         The retrieved statistic value represents the current state of the statistic as stored on the Steam server.
		///     </para>
		/// </remarks>
		float GetStatFloat(in string statName);

		/// <summary>
		///     Retrieves the value of an integer statistic for a specified user.
		/// </summary>
		/// <param name="steamID">The Steam ID of the user to retrieve the statistic for.</param>
		/// <param name="statName">The name of the statistic to retrieve.</param>
		/// <returns>The value of the specified integer statistic for the specified user.</returns>
		/// <remarks>
		///     <para>
		///         Use this method to retrieve the value of an integer statistic for a specified user. The
		///         <paramref name="steamID" /> parameter specifies the Steam ID of the user, and the <paramref name="statName" />
		///         parameter specifies the name of the statistic.
		///     </para>
		///     <para>
		///         It's important to note that the statistic must have been previously set using <see cref="RequestUserStats" />.
		///         If the statistic hasn't been set or doesn't exist for the specified user, this method will return 0.
		///     </para>
		///     <para>
		///         The retrieved statistic value represents the current state of the statistic as stored on the Steam server for
		///         the specified user.
		///     </para>
		/// </remarks>
		int GetUserStatInt(in SteamID steamID, in string statName);

		/// <summary>
		///     Retrieves the value of a floating-point statistic for a specified user.
		/// </summary>
		/// <param name="steamID">The Steam ID of the user to retrieve the statistic for.</param>
		/// <param name="statName">The name of the statistic to retrieve.</param>
		/// <returns>The value of the specified floating-point statistic for the specified user.</returns>
		/// <remarks>
		///     <para>
		///         Use this method to retrieve the value of a floating-point statistic for a specified user. The
		///         <paramref name="steamID" /> parameter specifies the Steam ID of the user, and the <paramref name="statName" />
		///         parameter specifies the name of the statistic.
		///     </para>
		///     <para>
		///         It's important to note that the statistic must have been previously set using <see cref="RequestUserStats" />.
		///         If the statistic hasn't been set or doesn't exist for the specified user, this method will return
		///         0.0f.
		///     </para>
		///     <para>
		///         The retrieved statistic value represents the current state of the statistic as stored on the Steam server for
		///         the specified user.
		///     </para>
		/// </remarks>
		float GetUserStatFloat(in SteamID steamID, in string statName);

		/// <summary>
		///     Retrieves the value of an integer global statistic.
		/// </summary>
		/// <param name="statName">The name of the global statistic to retrieve.</param>
		/// <returns>The value of the specified integer global statistic.</returns>
		/// <remarks>
		///     <para>
		///         Use this method to retrieve the value of an integer global statistic. The <paramref name="statName" />
		///         parameter specifies the name of the statistic.
		///     </para>
		///     <para>
		///         Global statistics are stored on the Steam server and represent cumulative data across all users of the
		///         application. They can be used to track global achievements or other aggregated information.
		///     </para>
		///     <para>
		///         It's important to note that the global statistic must have been previously updated using
		///         <see cref="RequestGlobalStats" />. If the statistic hasn't been updated or doesn't exist, this method will
		///         return 0.
		///     </para>
		///     <para>
		///         The retrieved statistic value represents the current state of the global statistic as stored on the Steam
		///         server.
		///     </para>
		/// </remarks>
		long GetGlobalStatInt(in string statName);

		/// <summary>
		///     Retrieves the value of a floating-point global statistic.
		/// </summary>
		/// <param name="statName">The name of the global statistic to retrieve.</param>
		/// <returns>The value of the specified floating-point global statistic.</returns>
		/// <remarks>
		///     <para>
		///         Use this method to retrieve the value of a floating-point global statistic. The <paramref name="statName" />
		///         parameter specifies the name of the statistic.
		///     </para>
		///     <para>
		///         Global statistics are stored on the Steam server and represent cumulative data across all users of the
		///         application. They can be used to track global achievements or other aggregated information.
		///     </para>
		///     <para>
		///         It's important to note that the global statistic must have been previously updated using
		///         <see cref="RequestGlobalStats" />. If the statistic hasn't been updated or doesn't exist, this method will
		///         return 0.0.
		///     </para>
		///     <para>
		///         The retrieved statistic value represents the current state of the global statistic as stored on the Steam
		///         server.
		///     </para>
		/// </remarks>
		double GetGlobalStatFloat(in string statName);

		/// <summary>
		///     Retrieves the history of integer values for a global statistic within a specified time range.
		/// </summary>
		/// <param name="statName">The name of the global statistic to retrieve the history for.</param>
		/// <param name="maxDays">Optional. The maximum number of days to retrieve the history for. Defaults to 60.</param>
		/// <returns>An array of long values representing the history of the specified global statistic.</returns>
		/// <remarks>
		///     <para>
		///         Use this method to retrieve the history of integer values for a global statistic within a specified time range.
		///         The <paramref name="statName" /> parameter specifies the name of the statistic, and the
		///         <paramref name="maxDays" /> parameter specifies the maximum number of days to retrieve the history for (default
		///         is 60 days).
		///     </para>
		///     <para>
		///         Global statistics are stored on the Steam server and represent cumulative data across all users of the
		///         application. The history can be useful for analyzing trends and changes in the statistic over time.
		///     </para>
		///     <para>
		///         The retrieved history is returned as an array of long values, with each value representing the statistic value
		///         on a specific day. The size of the array will depend on the <paramref name="maxDays" /> parameter.
		///     </para>
		///     <para>
		///         It's important to note that the global statistic must have been previously updated using
		///         <see cref="RequestGlobalStats" />. If the statistic hasn't been updated or doesn't exist, an empty array will
		///         be returned.
		///     </para>
		/// </remarks>
		long[] GetGlobalStatHistoryInt(in string statName, in int maxDays = 60);

		/// <summary>
		///     Retrieves the history of floating-point values for a global statistic within a specified time range.
		/// </summary>
		/// <param name="statName">The name of the global statistic to retrieve the history for.</param>
		/// <param name="maxDays">Optional. The maximum number of days to retrieve the history for. Defaults to 60.</param>
		/// <returns>An array of double values representing the history of the specified global statistic.</returns>
		/// <remarks>
		///     <para>
		///         Use this method to retrieve the history of floating-point values for a global statistic within a specified time
		///         range. The <paramref name="statName" /> parameter specifies the name of the statistic, and the
		///         <paramref name="maxDays" /> parameter specifies the maximum number of days to retrieve the history for (default
		///         is 60 days).
		///     </para>
		///     <para>
		///         Global statistics are stored on the Steam server and represent cumulative data across all users of the
		///         application. The history can be useful for analyzing trends and changes in the statistic over time.
		///     </para>
		///     <para>
		///         The retrieved history is returned as an array of double values, with each value representing the statistic
		///         value on a specific day. The size of the array will depend on the <paramref name="maxDays" /> parameter.
		///     </para>
		///     <para>
		///         It's important to note that the global statistic must have been previously updated using
		///         <see cref="RequestGlobalStats" />. If the statistic hasn't been updated or doesn't exist, an empty array will
		///         be returned.
		///     </para>
		/// </remarks>
		double[] GetGlobalStatHistoryFloat(in string statName, in int maxDays = 60);

		/// <summary>
		///     Updates an average rate statistic with the count and session length for the current session.
		/// </summary>
		/// <param name="statName">The name of the average rate statistic to update.</param>
		/// <param name="countThisSession">The count of events occurred during the current session.</param>
		/// <param name="sessionLength">The duration of the current session in seconds.</param>
		/// <param name="shouldStore">
		///     Optional. Specifies whether the updated statistic should be stored on the Steam server.
		///     Defaults to true.
		/// </param>
		/// <returns>True if the average rate statistic was successfully updated; otherwise, false.</returns>
		/// <remarks>
		///     <para>
		///         Use this method to update an average rate statistic with the count and session length for the current session.
		///         The <paramref name="statName" /> parameter specifies the name of the statistic,
		///         <paramref name="countThisSession" /> represents the count of events occurred during the current session, and
		///         <paramref name="sessionLength" /> is the duration of the current session in seconds.
		///     </para>
		///     <para>
		///         Average rate statistics are useful for tracking events per unit of time. The method calculates the average rate
		///         by dividing the count by the session length.
		///     </para>
		///     <para>
		///         The <paramref name="shouldStore" /> parameter determines whether the updated statistic should be stored on the
		///         Steam server. By default, the updated statistic will be stored. Set it to false if you do not want to store the
		///         updated value on the server.
		///     </para>
		///     <para>
		///         It's important to note that the statistic must have been previously initialized using
		///         <see cref="RequestCurrentStats" />. If the statistic hasn't been initialized or doesn't exist, the method will
		///         return false.
		///     </para>
		/// </remarks>
		bool UpdateAvgRateStat(in string statName, in float countThisSession, in double sessionLength, bool shouldStore = true);
	}
}