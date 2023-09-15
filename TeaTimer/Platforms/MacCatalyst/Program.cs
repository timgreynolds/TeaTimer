using System.Reflection;
using UIKit;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <summary>
/// This is the main entry point of the application.
/// </summary>
public class Program
{
    private static readonly CoreFoundation.OSLog _logger = new CoreFoundation.OSLog(Assembly.GetExecutingAssembly().GetName().Name, "iOS");

    /// <summary>This is the main entry point of the application.</summary>
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
            _logger.Log(CoreFoundation.OSLogLevel.Fault, $"Application level exception handler. Something went wrong.\n" +
                $"{ex.Message}\n{ex.StackTrace}");
        }
	}
}

