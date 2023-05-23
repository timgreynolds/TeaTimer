using UIKit;

namespace com.mahonkin.tim.maui.TeaTimer;

public class Program
{
	// This is the main entry point of the application.
	static void Main(string[] args)
	{
		// if you want to use a different Application Delegate class from "AppDelegate"
		// you can specify it here.
		try
		{
			UIApplication.Main(args, null, typeof(AppDelegate));
		}
		catch(System.Exception ex)
		{
			System.Console.WriteLine($"Application level exception handler. Something went wrong.");
			System.Console.WriteLine(ex.Message);
			System.Console.WriteLine(ex.StackTrace);
		}
	}
}

