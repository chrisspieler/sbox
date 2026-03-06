using System.Diagnostics;
using System.Text;
using Facepunch.Steps;
using static Facepunch.Constants;

namespace Facepunch.Pipelines;

internal class Pipeline
{
	private readonly List<Step> steps = new();
	private readonly Dictionary<Step, bool> continueOnFailure = new();
	private readonly Dictionary<Step, ExitCode> stepResults = new();
	private readonly string name;

	private Platform platform = Platform.Create();

	// Track execution times
	private readonly Stopwatch pipelineStopwatch = new();

	// GitHub API limits
	private const int MAX_GITHUB_FIELD_LENGTH = 65535;
	private const int SUMMARY_LINE_COUNT = 32;

	public Pipeline( string name )
	{
		this.name = name;
	}

	/// <summary>
	/// Register a step to be run as part of this pipeline
	/// </summary>
	/// <param name="step">The step to register</param>
	/// <param name="continueOnFailure">Whether to continue if this step fails</param>
	public void RegisterStep( Step step, bool continueOnFailure = false )
	{
		steps.Add( step );
		this.continueOnFailure[step] = continueOnFailure;
	}

	/// <summary>
	/// Run the pipeline with all registered steps
	/// </summary>
	/// <returns>Success if all steps succeeded, Failure otherwise</returns>
	public ExitCode Run()
	{
		Log.Info( $"Running pipeline: {name}" );

		// Start the pipeline stopwatch
		pipelineStopwatch.Start();

		bool pipelineStopped = false;

		// Run all steps
		foreach ( var step in steps )
		{
			if ( pipelineStopped )
			{
				// Skip running the step if the pipeline has been stopped
				continue;
			}

			var result = RunStep( step );

			// If the step failed and we shouldn't continue on failure, stop the pipeline
			if ( result != ExitCode.Success && !continueOnFailure[step] )
			{
				Log.Warning( $"Pipeline {name} is stopping due to step failure after {FormatElapsedTime( pipelineStopwatch.Elapsed )}" );
				pipelineStopped = true;
			}
		}

		// Stop the pipeline stopwatch
		pipelineStopwatch.Stop();

		// Log total execution time
		Log.Info( $"Pipeline '{name}' completed in {FormatElapsedTime( pipelineStopwatch.Elapsed )}" );

		// Check if any steps failed
		return stepResults.Any( x => x.Value == ExitCode.Failure ) ? ExitCode.Failure : ExitCode.Success;
	}

	/// <summary>
	/// Format a timespan as minutes and seconds
	/// </summary>
	private string FormatElapsedTime( TimeSpan elapsed )
	{
		if ( elapsed.TotalHours >= 1 )
		{
			return $"{(int)elapsed.TotalHours}h {elapsed.Minutes}m {elapsed.Seconds}s";
		}
		else if ( elapsed.TotalMinutes >= 1 )
		{
			return $"{elapsed.Minutes}m {elapsed.Seconds}s";
		}
		else
		{
			return $"{elapsed.Seconds}s";
		}
	}

	/// <summary>
	/// Run a single step and record its result
	/// </summary>
	/// <param name="step">The step to run</param>
	/// <returns>The result of running the step</returns>
	private ExitCode RunStep( Step step )
	{
		// Set up console output capture
		var originalOut = Console.Out;
		var originalError = Console.Error;

		using ConsoleOutputCapture outputCapture = new ConsoleOutputCapture( originalOut, 2000 );
		using ConsoleOutputCapture errorCapture = new ConsoleOutputCapture( originalError, 2000 );

		// Redirect console output
		Console.SetOut( outputCapture );
		Console.SetError( errorCapture );

		// Start timing the step
		var stepStopwatch = Stopwatch.StartNew();

		ExitCode stepResult;
		try
		{
			// Run the step
			stepResult = step.Run();
			stepResults[step] = stepResult;
		}
		finally
		{
			// Stop timing
			stepStopwatch.Stop();

			// Restore original console writers
			Console.SetOut( originalOut );
			Console.SetError( originalError );
		}

		return stepResult;
	}
}

/// <summary>
/// Builder pattern for creating pipeline with steps
/// </summary>
internal class PipelineBuilder
{
	private readonly List<Step> steps = new();
	private readonly Dictionary<Step, bool> continueOnFailure = new();
	private readonly string name;

	public PipelineBuilder( string name )
	{
		this.name = name;
	}

	/// <summary>
	/// Add a step to the pipeline
	/// </summary>
	/// <param name="step">The step to add</param>
	/// <param name="continueOnFailure">Whether to continue if this step fails</param>
	/// <returns>The builder for chaining</returns>
	public PipelineBuilder AddStep( Step step, bool continueOnFailure = false )
	{
		steps.Add( step );
		this.continueOnFailure[step] = continueOnFailure;
		return this;
	}

	public PipelineBuilder AddStepGroup( string groupName, IEnumerable<Step> groupSteps, bool continueOnFailure = false )
	{
		var group = new StepGroup( groupName, groupSteps.ToList(), continueOnFailure );
		steps.Add( group );
		this.continueOnFailure[group] = continueOnFailure;
		return this;
	}

	/// <summary>
	/// Build the pipeline with all registered steps
	/// </summary>
	/// <returns>A pipeline with the configured steps</returns>
	public Pipeline Build()
	{
		var pipeline = new Pipeline( name );

		// Register all steps with the pipeline
		foreach ( var step in steps )
		{
			pipeline.RegisterStep( step, continueOnFailure[step] );
		}

		return pipeline;
	}
}
