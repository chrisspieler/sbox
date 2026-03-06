using Editor;
using Native;
using Sandbox.Diagnostics;
using Sandbox.Engine;
using System;
using System.Runtime.InteropServices;

namespace Sandbox;

public class QtAppSystem
{
	protected Logger log = new Logger( "AppSystem" );

	public virtual void Init()
	{
		// get the current exe folder
		string exeDir = AppContext.BaseDirectory;
		
		Api.Init();
		NetCore.InitializeInterop( exeDir );
		ErrorReporter.Initialize();
		Bootstrap.InitMinimal( exeDir );
		Editor.AssemblyInitialize.Initialize();
		IToolsDll.Current.Bootstrap();

		QApp.Initialize();
		ManagedTools.InitFilesystem();
		ManagedTools.InitQt();

		QDir.addSearchPath( "tools", $"{exeDir}/core/tools" );
		QApp.ReloadTabbedStyle();

		Editor.Application.ReloadStyles();
		ProcessEvents();
	}

	public void ProcessEvents()
	{
		QApp.processEvents();
	}

	public void Run()
	{
		Init();

		NativeEngine.EngineGlobal.Plat_SetCurrentFrame( 0 );


		QApp.exec();

		//while ( RunFrame() )
		//{
		//	BlockingLoopPumper.Run( () => RunFrame() );
		////}

		Shutdown();
	}

	protected virtual bool RunFrame()
	{
		return false;
	}

	public void Shutdown()
	{
		OnShutdown();

		IToolsDll.Current.Exiting();

		QApp.exit();
	}

	protected virtual void OnShutdown()
	{

	}
}
