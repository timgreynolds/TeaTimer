using System;
using AppKit;
using Foundation;

namespace TeaTimer
{
    [Register("Tea")]
    public class Tea : NSComboBoxDataSource
    {
        private string _name;
        private TimeSpan _steepTime;
        private int _brewTemp;

        [Export("Name")]
        public string Name
        {
            get => _name;
            set
            {
                WillChangeValue("Name");
                _name = value;
                DidChangeValue("Name");
            }
        }

        [Export("SteepTime")]
        public TimeSpan SteepTime
        {
            get => _steepTime;
            set
            {
                WillChangeValue("SteepTime");
                _steepTime = value;
                DidChangeValue("SteepTime");
            }
        }

        [Export("BrewTemp")]
        public int BrewTemp
        {
            get => _brewTemp;
            set
            {
                WillChangeValue("BrewTemp");
                _brewTemp = value;
                DidChangeValue("BrewTemp");
            }
        }

        public Tea(string name, TimeSpan steepTime, int brewTemp)
        {
            _name = name;
            _steepTime = steepTime;
            _brewTemp = brewTemp;
        }
    }
}
