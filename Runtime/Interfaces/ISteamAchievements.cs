#nullable enable
using System;
using System.Collections.Generic;

namespace Hertzole.SmallSteamworks
{
	public interface ISteamAchievements : IDisposable
	{
		/// <summary>
		///     <para>Gets the number of achievements defined for the app on the Steamworks website.</para>
		///     <para>This is used for iterating through all of the achievements with <see cref="GetAchievementName" />.</para>
		///     <para>
		///         In general games should not need these functions because they should have a list of existing achievements
		///         compiled into them.
		///     </para>
		/// </summary>
		uint NumberOfAchievements { get; }

		/// <summary>
		///     Is true if global stats are available; false otherwise.
		/// </summary>
		bool HasGlobalStats { get; }

		/// <summary>
		///     Called when an achievement is unlocked.
		/// </summary>
		event AchievementUnlockCallback? OnAchievementUnlocked;

		/// <summary>
		///     <para>Resets the unlock status of an achievement.</para>
		///     <para>This is primarily only ever used for testing.</para>
		///     <para>
		///         You must have called <see cref="ISteamStats.RequestCurrentStats" /> and it needs to return successfully via
		///         its callback prior to calling this! However, this is usually done automatically.
		///     </para>
		/// </summary>
		/// <param name="achievementName">The 'API Name' of the achievement to reset.</param>
		/// <param name="shouldStore">
		///     If true, the unlock status will be sent to the Steam servers. Otherwise, you need to call
		///     <see cref="ISteamStats.StoreStats" /> after calling this.
		/// </param>
		void ResetAchievement(in string achievementName, in bool shouldStore = true);

		/// <summary>
		///     <para>Resets the unlock status of all achievements.</para>
		///     <para>This is primarily only ever used for testing.</para>
		///     <para>
		///         You must have called <see cref="ISteamStats.RequestCurrentStats" /> and it needs to return successfully via
		///         its callback prior to calling this! However, this is usually done automatically.
		///     </para>
		/// </summary>
		/// <param name="shouldStore">
		///     If true, the unlock status will be sent to the Steam servers. Otherwise, you need to call
		///     <see cref="ISteamStats.StoreStats" /> after calling this.
		/// </param>
		void ResetAllAchievements(in bool shouldStore = true);

		/// <summary>
		///     <para>Gets the unlock status of the achievement for the local user.</para>
		///     <para>To get the unlock status for others users, use <see cref="IsAchievementUnlockedForUser" />.</para>
		///     <para>
		///         You must have called <see cref="ISteamStats.RequestCurrentStats" /> and it needs to return successfully via
		///         its callback prior to calling this! However, this is usually done automatically.
		///     </para>
		/// </summary>
		/// <param name="achievementName">The 'API Name' of the achievement.</param>
		/// <param name="unlockTime">If the achievement is unlocked, returns the unlock time.</param>
		/// <returns>True if the achievement is unlocked; otherwise false.</returns>
		bool IsAchievementUnlocked(in string achievementName, out DateTime unlockTime);

		/// <summary>
		///     <para>Gets the unlock status of the achievement for a specific user.</para>
		///     <para>To get the unlock status for the local user, use <see cref="IsAchievementUnlocked" />.</para>
		///     <para>
		///         You must have called <see cref="ISteamStats.RequestUserStats" /> and it needs to return successfully via its
		///         callback prior to calling this!
		///     </para>
		/// </summary>
		/// <param name="steamId">The Steam ID of the user to get the achievement for.</param>
		/// <param name="achievementName">The 'API Name' of the achievement.</param>
		/// <param name="unlockTime">If the achievement is unlocked, returns the unlock time.</param>
		/// <returns>True if the achievement is unlocked; otherwise false.</returns>
		bool IsAchievementUnlockedForUser(in SteamID steamId, in string achievementName, out DateTime unlockTime);

		/// <summary>
		///     <para>Unlocks an achievement.</para>
		///     <para>
		///         You must have called <see cref="ISteamStats.RequestCurrentStats" /> and it needs to return successfully via
		///         its callback prior to calling this! However, this is usually done automatically.
		///     </para>
		///     <para>
		///         You can unlock an achievement multiple times so you don't need to worry about only unlocking achievements
		///         that aren't already unlocked.
		///     </para>
		/// </summary>
		/// <param name="achievementName">The 'API Name' of the achievement to unlock.</param>
		/// <param name="shouldStore">
		///     If true, the unlock status will be sent to the Steam servers. Otherwise, you need to call
		///     <see cref="ISteamStats.StoreStats" /> after calling this.
		/// </param>
		void UnlockAchievement(in string achievementName, in bool shouldStore = true);

		/// <summary>
		///     <para>Gets the 'API Name' for the achievement at the given index.</para>
		///     <para>
		///         This function must be used in conjunction with <see cref="NumberOfAchievements" /> to loop over the list of
		///         achievements.
		///     </para>
		///     <para>
		///         In general games should not need these functions as they should have the list of achievements compiled into
		///         them.
		///     </para>
		///     <para>
		///         You must have called <see cref="ISteamStats.RequestCurrentStats" /> and it needs to return successfully via
		///         its callback prior to calling this! However, this is usually done automatically.
		///     </para>
		/// </summary>
		/// <param name="index">The index of the achievement.</param>
		/// <returns>The 'API Name' of the achievement.</returns>
		/// <example>
		///     <code>
		///  uint numberOfAchievements = SteamManager.Achievements.NumberOfAchievements;
		///  for (uint i = 0; i &lt; numberOfAchievements; i++)
		///  {
		/// 		string achievementName = SteamManager.Achievements.GetAchievementName(i);
		/// 		Debug.Log(achievementName);
		///  }
		///  </code>
		/// </example>
		string GetAchievementName(in uint index);

		/// <summary>
		///     <para>Gets the localized achievement display name.</para>
		///     <para>
		///         This localization is provided based on the games language if it's set, otherwise it checks if a localization
		///         is available for the users Steam UI Language. If that fails too, then it falls back to English.
		///     </para>
		///     <para>
		///         You must have called <see cref="ISteamStats.RequestCurrentStats" /> and it needs to return successfully via
		///         its callback prior to calling this! However, this is usually done automatically.
		///     </para>
		///     <para>
		///         If you want to get display name, description, is hidden, and unlock status at once, you should use
		///         <see cref="GetAchievementInfo" /> instead.
		///     </para>
		/// </summary>
		/// <param name="achievementName">The 'API Name' of the achievement.</param>
		/// <returns>The localized display name for the achievement.</returns>
		string GetAchievementDisplayName(in string achievementName);

		/// <summary>
		///     <para>Gets the localized achievement description.</para>
		///     <para>
		///         This localization is provided based on the games language if it's set, otherwise it checks if a localization
		///         is available for the users Steam UI Language. If that fails too, then it falls back to English.
		///     </para>
		///     <para>
		///         You must have called <see cref="ISteamStats.RequestCurrentStats" /> and it needs to return successfully via
		///         its callback prior to calling this! However, this is usually done automatically.
		///     </para>
		///     <para>
		///         If you want to get display name, description, is hidden, and unlock status at once, you should use
		///         <see cref="GetAchievementInfo" /> instead.
		///     </para>
		/// </summary>
		/// <param name="achievementName">The 'API Name' of the achievement.</param>
		/// <returns>The localized description for the achievement.</returns>
		string GetAchievementDescription(in string achievementName);

		/// <summary>
		///     <para>Gets whether the achievement is hidden or not.</para>
		///     <para>
		///         You must have called <see cref="ISteamStats.RequestCurrentStats" /> and it needs to return successfully via
		///         its callback prior to calling this! However, this is usually done automatically.
		///     </para>
		///     <para>
		///         If you want to get display name, description, is hidden, and unlock status at once, you should use
		///         <see cref="GetAchievementInfo" /> instead.
		///     </para>
		/// </summary>
		/// <param name="achievementName">The 'API Name' of the achievement.</param>
		/// <returns>True if the achievement is hidden; otherwise false.</returns>
		bool IsAchievementHidden(in string achievementName);

		/// <summary>
		///     <para>Gets the localized achievement display name, description, hidden status, and unlock status as one struct.</para>
		///     <para>
		///         This localization is provided based on the games language if it's set, otherwise it checks if a localization
		///         is available for the users Steam UI Language. If that fails too, then it falls back to English.
		///     </para>
		///     <para>
		///         You must have called <see cref="ISteamStats.RequestCurrentStats" /> and it needs to return successfully via
		///         its callback prior to calling this! However, this is usually done automatically.
		///     </para>
		/// </summary>
		/// <param name="achievementName">The 'API Name' of the achievement.</param>
		/// <returns>The collected achievement info.</returns>
		SteamAchievement GetAchievementInfo(in string achievementName);

		/// <summary>
		///     <para>Asynchronously gets the icon for the achievement.</para>
		///     <para>The callback will be called once the icon has been downloaded.</para>
		///     <para>
		///         You must have called <see cref="ISteamStats.RequestCurrentStats" /> and it needs to return successfully via
		///         its callback prior to calling this! However, this is usually done automatically.
		///     </para>
		/// </summary>
		/// <param name="achievementName">The 'API Name' of the achievement.</param>
		/// <param name="onIconFetched">The callback to be called once the icon is downloaded.</param>
		void GetAchievementIcon(string achievementName, AchievementIconReceivedCallback? onIconFetched = null);

		/// <summary>
		///     <para>Returns the percentage of users who have unlocked the achievements.</para>
		///     <para>
		///         You must have called <see cref="ISteamAchievements.RequestGlobalAchievementStats" /> and it needs to return
		///         successfully via
		///         its callback prior to calling this!
		///     </para>
		/// </summary>
		/// <param name="achievementName">The 'API Name' of the achievement.</param>
		/// <returns>The percentage of people that have unlocked this achievement from 0 to 100.</returns>
		float GetGlobalAchievementPercent(in string achievementName);

		/// <summary>
		///     <para>Gets the info on the most achieved achievements for the game.</para>
		///     <para>
		///         You must have called <see cref="ISteamAchievements.RequestGlobalAchievementStats" /> and it needs to return
		///         successfully via
		///         its callback prior to calling this!
		///     </para>
		/// </summary>
		/// <returns>Returns an IEnumerable that you can enumerate through to get all the achievements info.</returns>
		IEnumerable<SteamGlobalAchievementInfo> GetMostAchievedAchievements();

		/// <summary>
		///     <para>Shows the user a pop-up notification with the current progress of the achievement.</para>
		///     <para>
		///         Calling this function will NOT set the progress or unlock the achievement, the game must do that manually by
		///         calling <see cref="ISteamStats.SetStatInt" /> or <see cref="UnlockAchievement" />.
		///     </para>
		///     <para>
		///         You must have called <see cref="ISteamStats.RequestCurrentStats" /> and it needs to return successfully via
		///         its callback prior to calling this! However, this is usually done automatically.
		///     </para>
		/// </summary>
		/// <param name="achievementName"></param>
		/// <param name="currentProgress"></param>
		/// <param name="maxProgress"></param>
		/// <returns>
		///     True if the progress was shown to the user; false if the the achievement is already unlocked or if
		///     currentProgress is more than maxProgress.
		/// </returns>
		bool IndicateAchievementProgress(in string achievementName, in uint currentProgress, in uint maxProgress);

		/// <summary>
		///     <para>
		///         Asynchronously fetch the data for the percentage of players who have received each achievement for the
		///         current game globally.
		///     </para>
		///     <para>
		///         You must have called <see cref="ISteamStats.RequestCurrentStats" /> and it needs to return successfully via
		///         its callback prior to calling this! However, this is usually done automatically.
		///     </para>
		/// </summary>
		/// <param name="onStatsFetched">The callback to be called once the stats have been fetched.</param>
		void RequestGlobalAchievementStats(GlobalAchievementStatsReceivedCallback? onStatsFetched = null);
	}
}