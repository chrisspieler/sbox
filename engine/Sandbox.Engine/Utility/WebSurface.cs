using Sandbox.Utility;

namespace Sandbox;


/// <summary>
/// Enables rendering and interacting with a webpage
/// </summary>
public sealed class WebSurface : IDisposable
{
	static Dictionary<uint, WeakReference<WebSurface>> All;

	Action _initQueue;

	uint BrowserId;

	public bool IsLimited { get; private set; }

	static WebSurface GetBrowser( uint handle )
	{
		lock ( All )
		{
			if ( All.TryGetValue( handle, out var v ) && v.TryGetTarget( out var target ) )
			{
				return target;
			}

			return null;
		}
	}

	public delegate void TextureChangedDelegate( ReadOnlySpan<byte> span, Vector2 size );

	/// <summary>
	/// Called when the texture has changed and should be updated
	/// </summary>
	public TextureChangedDelegate OnTexture { get; set; }

	internal WebSurface( bool isLimited )
	{
		IsLimited = isLimited;
	}

	~WebSurface()
	{
		Dispose();
	}

	public void Dispose()
	{
	}

	string currentUrl;


	public string PageTitle { get; private set; }

	/// <summary>
	/// The current Url
	/// </summary>
	public string Url { get; set; }

	Vector2 _size;

	/// <summary>
	/// The size of the browser
	/// </summary>
	public Vector2 Size { get; set; }

	public string Cursor { get; private set; }

	/// <summary>
	/// Tell the browser the mouse has moved
	/// </summary>
	public void TellMouseMove( Vector2 position )
	{
		
	}

	/// <summary>
	/// Tell the browser the mouse wheel has moved
	/// </summary>
	/// <param name="delta"></param>
	public void TellMouseWheel( int delta )
	{

	}

	/// <summary>
	/// Tell the browser a mouse button has been pressed
	/// </summary>
	public void TellMouseButton( MouseButtons button, bool state )
	{
		
	}

	/// <summary>
	/// Tell the browser a unicode key has been pressed
	/// </summary>
	public void TellChar( uint unicodeKey, KeyboardModifiers modifiers )
	{
		
	}

	/// <summary>
	/// Tell the browser a key has been pressed or released
	/// </summary>
	public void TellKey( uint virtualKeyCode, KeyboardModifiers modifiers, bool state )
	{

	}

	bool _keyFocus;

	/// <summary>
	/// Tell the html control if it has key focus currently, controls showing the I-beam cursor in text controls amongst other things
	/// </summary>
	public bool HasKeyFocus { get; set; }

	float _scaleFactor = 1.0f;

	/// <summary>
	/// DPI Scaling factor
	/// </summary>
	public float ScaleFactor { get; set; }

	bool _backgroudMode;

	/// <summary>
	/// Enable/disable low-resource background mode, where javascript and repaint timers are throttled, resources are
	/// more aggressively purged from memory, and audio/video elements are paused. When background mode is enabled,
	/// all HTML5 video and audio objects will execute ".pause()" and gain the property "._steam_background_paused = 1".
	/// When background mode is disabled, any video or audio objects with that property will resume with ".play()".
	/// </summary>
	public bool InBackgroundMode { get; set; }
}
