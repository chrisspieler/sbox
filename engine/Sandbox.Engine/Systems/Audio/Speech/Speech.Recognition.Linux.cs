#if !WIN

namespace Sandbox.Speech;

/// <summary>
/// A result from speech recognition.
/// </summary>
public struct SpeechRecognitionResult
{
	/// <summary>
	/// From 0-1 how confident are we that this is the correct result?
	/// </summary>
	public float Confidence { get; init; }

	/// <summary>
	/// The text result from speech recognition.
	/// </summary>
	public string Text { get; init; }

	/// <summary>
	/// Did we successfully find a match?
	/// </summary>
	public bool Success { get; init; }
}

public static class Recognition
{
	/// <summary>
	/// Called when we have a result from speech recognition.
	/// </summary>
	/// <param name="result"></param>
	public delegate void OnSpeechResult( SpeechRecognitionResult result );

	/// <summary>
	/// Whether or not we are currently listening for speech.
	/// </summary>
	public static bool IsListening => false;

	/// <summary>
	/// Whether or not speech recognition is supported and a language is available.
	/// </summary>
	public static bool IsSupported => false;

	/// <summary>
	/// Start listening for speech to recognize as text. When speech has been recognized the callback
	/// will be invoked, the callback will also be invoked if recognition fails.
	/// </summary>
	/// <param name="callback">
	/// A callback that will be invoked when recognition has finished.
	/// </param>
	/// <param name="choices">
	/// An array of possible choices. If specified, the closest match will be chosen and passed to
	/// the callback.
	/// </param>
	public static void Start( OnSpeechResult callback, IEnumerable<string> choices = null ) { }

	/// <summary>
	/// Stop any active listening for speech.
	/// </summary>
	public static void Stop() { }

	internal static void Reset() { }
}
#endif
