﻿using System;
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
            Console.WriteLine($"Application exception {ex.Message}\n{ex.StackTrace}");
            throw new Exception(ex.Message, ex);
        }
    }
}

