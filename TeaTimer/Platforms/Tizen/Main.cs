using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace com.mahonkin.tim.maui.TeaTimer;

class Program : MauiApplication
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	static void Main(string[] args)
	{
		var app = new Program();
		app.Run(args);
	}
}

