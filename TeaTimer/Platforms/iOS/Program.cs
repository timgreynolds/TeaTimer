using System;
using System.Runtime.CompilerServices;
using com.mahonkin.tim.logging;
using Foundation;
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
        nint logPtr = OSLogger.Create(typeof(TeaTimerApp).FullName, typeof(Program).FullName);
        OSLogger.LogTrace(logPtr, $"Application starting using delegate {typeof(AppDelegate).FullName}.");
        
        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        try
        {
            UIApplication.Main(null, null, typeof(AppDelegate));
        }
        catch (Exception ex)
        {
            OSLogger.LogCritical(logPtr, $"An exception occurred: {ex.GetType().Name} - {ex.Message}\n{ex.StackTrace}");
            throw new Exception(ex.Message, ex);
        }
    }
}

