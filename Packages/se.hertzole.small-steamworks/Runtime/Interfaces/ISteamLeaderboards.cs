#nullable enable
using System;

namespace Hertzole.SmallSteamworks
{
	public interface ISteamLeaderboards : IDisposable
	{
		/// <summary>
		///     <para>Gets a leaderboard by name.</para>
		///     <para>
		///         You must call either this or <see cref="FindOrCreateLeaderboard" /> to obtain a
		///         <see cref="SteamLeaderboard" /> that you can use to interact with leaderboards.
		///     </para>
		/// </summary>
		/// <param name="leaderboardName">The name of the leaderboard to find. Must not be longer than 128 bytes.</param>
		/// <param name="callback">Called when the leaderboard has been found.</param>
		void FindLeaderboard(in string leaderboardName, FindLeaderboardCallback? callback = null);

		/// <summary>
		///     <para>Gets a leaderboard by name, or create one if it's not found.</para>
		///     <para>
		///         You must call either this or <see cref="FindLeaderboard" /> to obtain a
		///         <see cref="SteamLeaderboard" /> that you can use to interact with leaderboards.
		///     </para>
		///     <para>
		///         Leaderboards created with this function will not automatically show up in the Steam Community. You must
		///         manually set the Community Name field in the App Admin panel of the Steamworks website. As such, it's generally
		///         recommend to prefer creating the leaderboards in the App Admin panel on the Steamworks website and use
		///         <see cref="FindLeaderboard" />, unless you're expected to have a large amount of dynamically created
		///         leaderboards.
		///     </para>
		/// </summary>
		/// <param name="leaderboardName">The name of the leaderboard to find or create. Must not be longer than 128 bytes.</param>
		/// <param name="sortMethod">The sort order of the new leaderboard if it's created.</param>
		/// <param name="displayType">
		///     The display type (used by the Steam Community website) of the new leaderboard if it's
		///     created.
		/// </param>
		/// <param name="callback">Called when the leaderboard has been found or created.</param>
		void FindOrCreateLeaderboard(in string leaderboardName, in LeaderboardSortMethod sortMethod, in LeaderboardDisplayType displayType, FindLeaderboardCallback? callback = null);

		/// <summary>
		///     <para>Uploads a score to a specified leaderboard.</para>
		///     <para>
		///         Details are optional game-defined information which outlines how the user got that score. For example if it's
		///         a racing style time based leaderboard you could store the timestamps when the player hits each checkpoint. If
		///         you have collectibles along the way you could use bit fields as booleans to store the items the player picked
		///         up in the playthrough.
		///     </para>
		///     <para>
		///         Uploading scores to Steam is rate limited to 10 uploads per 10 minutes and you may only have one outstanding
		///         call to this function at a time.
		///     </para>
		/// </summary>
		/// <param name="leaderboard">
		///     The leaderboard obtained from <see cref="FindLeaderboard" /> or
		///     <see cref="FindOrCreateLeaderboard" />.
		/// </param>
		/// <param name="uploadScoreMethod">Do you want to force the score to change, or keep the previous score if it was better?</param>
		/// <param name="score">The score to upload.</param>
		/// <param name="scoreDetails">A optional array containing the details surrounding the unlocking of this score.</param>
		/// <param name="callback">Called when the score has been uploaded.</param>
		void SubmitScore(in SteamLeaderboard leaderboard, in LeaderboardUploadScoreMethod uploadScoreMethod, in int score, in int[]? scoreDetails = null, UploadScoreCallback? callback = null);

		/// <summary>
		///     <para>Attaches a piece of user generated content to the current user's entry on a leaderboard.</para>
		///     <para>
		///         This content could be a replay of the user achieving the score or a ghost to race against. The attached
		///         handle will be available when the entry is retrieved and can be found in <see cref="SteamLeaderboardEntry" />
		///         which contains <see cref="SteamUGCHandle" />.
		///     </para>
		/// </summary>
		/// <param name="leaderboard">
		///     The leaderboard obtained from <see cref="FindLeaderboard" /> or
		///     <see cref="FindOrCreateLeaderboard" />.
		/// </param>
		/// <param name="ugcHandle">The UGC handle to share.</param>
		/// <param name="callback">Called when the UGC is attached.</param>
		void AttachLeaderboardUGC(in SteamLeaderboard leaderboard, in SteamUGCHandle ugcHandle, AttachLeaderboardUGCCallback? callback = null);

		/// <summary>
		///     <para>Fetches a series of leaderboard entries for a specified leaderboard.</para>
		///     <para>You can ask for more entries than exist, then this will return as many as do exist.</para>
		/// </summary>
		/// <param name="leaderboard">
		///     The leaderboard obtained from <see cref="FindLeaderboard" /> or
		///     <see cref="FindOrCreateLeaderboard" />.
		/// </param>
		/// <param name="count">How many entries to request.</param>
		/// <param name="offset"></param>
		/// <param name="callback">Called when the scores have been retrieved.</param>
		void GetScores(in SteamLeaderboard leaderboard, in int count, in int offset = 0, GetScoresCallback? callback = null);

		/// <summary>
		///     <para>Fetches all the leaderboard entries for friends of the current user.</para>
		/// </summary>
		/// <param name="leaderboard">
		///     The leaderboard obtained from <see cref="FindLeaderboard" /> or
		///     <see cref="FindOrCreateLeaderboard" />.
		/// </param>
		/// <param name="callback">Called when the scores have been retrieved.</param>
		void GetScoresFromFriends(in SteamLeaderboard leaderboard, GetScoresCallback? callback = null);

		/// <summary>
		///     <para>Fetches a series of leaderboard entries around the current user.</para>
		///     <para>
		///         The <see cref="rangeStart" /> will be X amount above the user and <see cref="rangeEnd" /> will be X amount
		///         beneath the user. For example, if the user is at #10 and start is set to -2 and end is set to 2, Steam will
		///         return position 8, 9, 10, 11, and 12.
		///     </para>
		/// </summary>
		/// <param name="leaderboard">
		///     The leaderboard obtained from <see cref="FindLeaderboard" /> or
		///     <see cref="FindOrCreateLeaderboard" />.
		/// </param>
		/// <param name="rangeStart">The index to start downloading entries.</param>
		/// <param name="rangeEnd">The last index to retrieve entries.</param>
		/// <param name="callback">Called when the scores have been retrieved.</param>
		void GetScoresAroundUser(in SteamLeaderboard leaderboard, in int rangeStart = 10, in int rangeEnd = 10, GetScoresCallback? callback = null);
	}
}