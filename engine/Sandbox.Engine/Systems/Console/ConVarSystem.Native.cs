namespace Sandbox;

internal static partial class ConVarSystem
{
	internal static void ClearNativeCommands()
	{
		if ( Members.Count == 0 )
			return;

		System.Collections.Generic.List<string> nativeKeys = null;

		foreach ( var (name, command) in Members )
		{
			// TODO: Detect whether Command or ConVar is native.
		}

		if ( nativeKeys is null )
			return;

		foreach ( var name in nativeKeys )
		{
			Members.Remove( name );
		}
	}
}

