using System;
using CoreFoundation;
using UIKit;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <summary>
/// This is the main entry point of the application.
/// </summary>
public class Program
{
    /// <inheritdoc cref="UIApplication.Main(string[], Type, Type)" />
    static void Main()
    {
        CoreFoundation.OSLog log = new(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, "Main");
        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        try
        {
            UIApplication.Main(null, null, typeof(AppDelegate));
        }
        catch (Exception ex)
        {
            log.Log(OSLogLevel.Fault, $"An application error has occurred.\n{ex.GetType().Name} - {ex.Message}\n{ex.StackTrace}");
        }
    }
}

