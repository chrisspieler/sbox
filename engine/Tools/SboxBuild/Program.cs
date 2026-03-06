using System.CommandLine;
using Facepunch.Pipelines;
using Facepunch.Steps;
using static Facepunch.Constants;

namespace Facepunch;

/// <summary>
/// Main entry point for the SboxBuild tool
/// </summary>
internal class Program
{
	static int Main( string[] args )
	{
		// Create root command
		var rootCommand = new RootCommand( "sboxbuild - Build and deployment tool for s&box\n\nRun this from your sbox project root." );

		AddBuildPipeline( rootCommand );
		AddFormatPipeline( rootCommand );
		AddTestStep( rootCommand );

		rootCommand.Invoke( args );
		return Environment.ExitCode;
	}

	private static void AddBuildPipeline( RootCommand rootCommand )
	{
		var buildCommand = new Command( "build", "Build managed & native code" );

		var cleanOption = new Option<bool>(
			"--clean",
			description: "Whether to do a clean build",
			getDefaultValue: () => false );

		var skipNativeOption = new Option<bool>(
			"--skip-native",
			description: "Skip building native code",
			getDefaultValue: () => false );

		var skipManagedOption = new Option<bool>(
			"--skip-managed",
			description: "Skip building managed code",
			getDefaultValue: () => false );

		buildCommand.AddOption( cleanOption );
		buildCommand.AddOption( skipNativeOption );
		buildCommand.AddOption( skipManagedOption );

		buildCommand.SetHandler( ( bool clean, bool skipNative, bool skipManaged ) =>
		{
			var pipeline = Build.Create( clean, skipNative, skipManaged );
			ExitCode result = pipeline.Run();
			Environment.ExitCode = (int)result;
		}, cleanOption, skipNativeOption, skipManagedOption );

		rootCommand.Add( buildCommand );
	}

	private static void AddFormatPipeline( RootCommand rootCommand )
	{
		var formatCommand = new Command( "format", "Format all code" );

		var verifyOption = new Option<bool>(
			"--verify",
			description: "Verify compliance without changes",
			getDefaultValue: () => false );

		formatCommand.AddOption( verifyOption );

		formatCommand.SetHandler( ( bool verifyOnly ) =>
		{
			var pipeline = FormatAll.Create( verifyOnly );
			ExitCode result = pipeline.Run();
			Environment.ExitCode = (int)result;
		}, verifyOption );
		rootCommand.Add( formatCommand );
	}

	private static void AddTestStep( RootCommand rootCommand )
	{
		var testsCommand = new Command( "test", "Run tests" );
		testsCommand.SetHandler( () =>
		{
			var step = new Test( "Run Tests" );
			ExitCode result = step.Run();
			Environment.ExitCode = (int)result;
		} );
		rootCommand.Add( testsCommand );
	}
}
