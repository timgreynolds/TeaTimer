using System;
using System.IO;
using AppKit;
using Foundation;
using SQLite;

namespace TeaTimer
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        private string _dbPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create) + Path.DirectorySeparatorChar + "TeaTimer" + Path.DirectorySeparatorChar + "TeaTimer.db3";

        public AppDelegate()
        {
        }

        public override void WillFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application

            // Get the database connection
            try
            {
                InitializeDatabase();
            }
            catch (Exception ex)
            {
                File.Delete(_dbPath);
                Console.WriteLine(ex.Message);
            }
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }

        private void InitializeDatabase()
        {
            if (File.Exists(_dbPath) == false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_dbPath));
                File.Create(_dbPath);
                SQLiteConnection conn = new SQLiteConnection(_dbPath);
                TeaModel earlGrey = new TeaModel("Earl Grey", new TimeSpan(0, 1, 20), 185);
                conn.CreateTable<TeaModel>();
                conn.Insert(earlGrey);
            }
        }

    }
}
