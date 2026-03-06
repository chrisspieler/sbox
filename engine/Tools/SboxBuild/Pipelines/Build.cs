using System.IO;
using Facepunch;
using Facepunch.Steps;

namespace Facepunch.Pipelines;

internal class Build
{
	public static Pipeline Create(
								 bool clean = false,
								 bool skipNative = false,
								 bool skipManaged = false )
	{
		var builder = new PipelineBuilder( "Build" );

		// Always add interop gen
		builder.AddStep( new Steps.InteropGen( "Interop Gen", skipNative ) );

		// Add native build step if not skipped
		if ( !skipNative )
		{
			// TODO: Add native build step.
		}

		// Add managed build step if not skipped
		if ( !skipManaged )
		{
			builder.AddStep( new BuildManaged( "Build Managed", clean ) );
		}

		return builder.Build();
	}
}
