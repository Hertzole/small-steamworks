using System;

namespace Hertzole.SmallSteamworks
{
	public class SteamException : Exception
	{
		protected SteamException(string message) : base(message) { }
	}
}