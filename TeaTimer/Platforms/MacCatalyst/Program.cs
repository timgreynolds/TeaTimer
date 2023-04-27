using ObjCRuntime;
using UIKit;

namespace com.mahonkin.tim.maui.TeaTimer;

public class Program
{
    /// <summary>This is the main entry point of the application.</summary>
    static void Main(string[] args)
	{
		// if you want to use a different Application Delegate class from "AppDelegate"
		// you can specify it here.
		UIApplication.Main(args, null, typeof(AppDelegate));
	}
}

