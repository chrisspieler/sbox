using Sandbox.Engine;

namespace Sandbox.Network;


internal static class NetworkConsoleCommands
{
	[ConCmd( "host", ConVarFlags.Protected )]
	public static void StartServer()
	{
		if ( Networking.IsActive )
		{
			Log.Warning( "You are already connected to a server." );
			return;
		}

		Networking.CreateLobby( new() );
	}

	[ConCmd( "connect", ConVarFlags.Protected )]
	public static void ConnectToServer( string target )
	{
		if ( Networking.IsActive )
		{
			Log.Warning( "You are already connected to a server." );
			return;
		}

		Networking.Connect( target );
	}

	[ConCmd( "disconnect", ConVarFlags.Protected )]
	public static void Disconnect()
	{
		IGameInstanceDll.Current.Disconnect();
	}

	[ConCmd( "reconnect", ConVarFlags.Protected )]
	public static void Reconnect()
	{
		if ( string.IsNullOrWhiteSpace( Networking.LastConnectionString ) )
		{
			Log.Warning( "You were never or are not currently connected to a server." );
			return;
		}

		Networking.Connect( Networking.LastConnectionString );
	}
}
