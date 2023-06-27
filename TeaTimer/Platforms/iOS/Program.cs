using UIKit;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <summary>
/// This is the main entry point of the application.
/// </summary>
public class Program
{
	private static CoreFoundation.OSLog _logger = new CoreFoundation.OSLog(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, "Crash");

    /// <inheritdoc cref="UIApplication.Main(string[], System.Type, System.Type)" />
    static void Main()
	{
		// if you want to use a different Application Delegate class from "AppDelegate"
		// you can specify it here.
		try
		{
			UIApplication.Main(null, null, typeof(AppDelegate));
		}
		catch(System.Exception ex)
		{
            _logger.Log(CoreFoundation.OSLogLevel.Fault, $"Application level exception handler. Something went wrong.\n" +
                $"{ex.Message}\n{ex.StackTrace}");
		}
	}
}

