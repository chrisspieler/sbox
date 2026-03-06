#if !WIN
using System.Collections.ObjectModel;

namespace Sandbox.Speech;

/// <summary>
/// A speech synthesis stream. Lets you write text into speech and output it to a <see cref="SoundHandle"/>.
/// </summary>
public sealed partial class Synthesizer
{
	public record struct InstalledVoice( string Name, string Gender, string Age );

	/// <summary>
	/// Return an empty list.
	/// </summary>
	public ReadOnlyCollection<InstalledVoice> InstalledVoices => [];

	/// <summary>
	/// Return this object without doing anything.
	/// </summary>
	public string CurrentVoice => null;

	/// <summary>
	/// Return this object without doing anything.
	/// </summary>
	/// <param name="voiceName"></param>
	/// <returns></returns>
	public Synthesizer TrySetVoice( string voiceName ) => this;

	/// <summary>
	/// Return this object without doing anything.
	/// </summary>
	/// <param name="gender"></param>
	/// <param name="age"></param>
	/// <returns></returns>
	public Synthesizer TrySetVoice( string gender = "Male", string age = null ) => this;

	/// <summary>
	/// Return this object without doing anything.
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public Synthesizer WithText( string input ) => this;

	/// <summary>
	/// Return this object without doing anything.
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	public Synthesizer OnVisemeReached( Action<int, TimeSpan> action ) => this;

	/// <summary>
	/// Return this object without doing anything.
	/// </summary>
	/// <param name="rate"></param>
	/// <returns></returns>
	public Synthesizer WithRate( int rate ) => this;

	/// <summary>
	/// Return this object without doing anything.
	/// </summary>
	/// <returns></returns>
	public Synthesizer WithBreak() => this;

	/// <summary>
	/// No-op stub for Linux.
	/// </summary>
	/// <returns></returns>
	public SoundHandle Play => SoundHandle.Empty;
}
#endif
