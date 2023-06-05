#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System;

namespace Hertzole.SmallSteamworks
{
	/// <summary>
	///     Callback for uploading a score to a leaderboard.
	/// </summary>
	/// <param name="success">
	///     Was the call successful? May be false if the leaderboard is set to "Trusted" in the App Admin on
	///     the Steamworks website.
	/// </param>
	/// <param name="leaderboard">
	///     The leaderboard that this score was uploaded to. Will be
	///     <see cref="SteamLeaderboard.Invalid" /> if the call failed.
	/// </param>
	/// <param name="score">The score that was attempted to set.</param>
	/// <param name="scoreChanged">
	///     True if the score on the leaderboard changed; otherwise false if the existing score was
	///     better.
	/// </param>
	/// <param name="newRank">The new global rank of the user on this leaderboard.</param>
	/// <param name="previousRank">
	///     The previous global rank of the user on this leaderboard. Will be 0 if the user had no
	///     existing entry in the leaderboard.
	/// </param>
	public delegate void UploadScoreCallback(bool success, SteamLeaderboard leaderboard, int score, bool scoreChanged, int newRank, int previousRank);

	public readonly struct UploadScoreResponse : IEquatable<UploadScoreResponse>
	{
		/// <summary>
		///     Was the call successful? May be false if the leaderboard is set to "Trusted" in the App Admin on
		///     the Steamworks website.
		/// </summary>
		public bool Success { get; }
		/// <summary>
		///     The leaderboard that this score was uploaded to. Will be
		///     <see cref="SteamLeaderboard.Invalid" /> if the call failed.
		/// </summary>
		public SteamLeaderboard Leaderboard { get; }
		/// <summary>
		///     The score that was attempted to set.
		/// </summary>
		public int Score { get; }
		/// <summary>
		///     True if the score on the leaderboard changed; otherwise false if the existing score was
		///     better.
		/// </summary>
		public bool ScoreChanged { get; }
		/// <summary>
		///     The new global rank of the user on this leaderboard.
		/// </summary>
		public int NewRank { get; }
		/// <summary>
		///     The previous global rank of the user on this leaderboard. Will be 0 if the user had no
		///     existing entry in the leaderboard.
		/// </summary>
		public int PreviousRank { get; }

		internal UploadScoreResponse(bool success, SteamLeaderboard leaderboard, int score, bool scoreChanged, int newRank, int previousRank)
		{
			Success = success;
			Leaderboard = leaderboard;
			Score = score;
			ScoreChanged = scoreChanged;
			NewRank = newRank;
			PreviousRank = previousRank;
		}

		public bool Equals(UploadScoreResponse other)
		{
			return Success == other.Success && Leaderboard.Equals(other.Leaderboard) && Score == other.Score && ScoreChanged == other.ScoreChanged && NewRank == other.NewRank && PreviousRank == other.PreviousRank;
		}

		public override bool Equals(object obj)
		{
			return obj is UploadScoreResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Success.GetHashCode();
				hashCode = (hashCode * 397) ^ Leaderboard.GetHashCode();
				hashCode = (hashCode * 397) ^ Score;
				hashCode = (hashCode * 397) ^ ScoreChanged.GetHashCode();
				hashCode = (hashCode * 397) ^ NewRank;
				hashCode = (hashCode * 397) ^ PreviousRank;
				return hashCode;
			}
		}

		public static bool operator ==(UploadScoreResponse left, UploadScoreResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(UploadScoreResponse left, UploadScoreResponse right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"{nameof(UploadScoreResponse)} ({nameof(Success)}: {Success}, {nameof(Leaderboard)}: {Leaderboard}, {nameof(Score)}: {Score}, {nameof(ScoreChanged)}: {ScoreChanged}, {nameof(NewRank)}: {NewRank}, {nameof(PreviousRank)}: {PreviousRank})";
		}
	}
}