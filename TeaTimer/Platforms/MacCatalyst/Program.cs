using System;
using Microsoft.Extensions.Logging;
using UIKit;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <summary>
/// This is the main entry point of the application.
/// </summary>
public class Program
{
    /// <summary>This is the main entry point of the application.</summary>
    static void Main(string[] args)
    {
        try
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, typeof(AppDelegate));
        }
        catch (Exception ex)
        {
            // Console.WriteLine($"Applcation exception {ex.Message}\n{ex.StackTrace}");
            MauiProgram.Logger.LogCritical("Applcation exception {Message}\n{StackTrace}", ex.Message, ex.StackTrace);
        }
    }
}

