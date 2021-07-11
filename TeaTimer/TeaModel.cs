using System;
using Newtonsoft.Json;

namespace TeaTimer
{
    public class TeaModel
    {
        #region Properties
        public int Id { get; set; }

        public string Name { get; set; }
        
        public TimeSpan SteepTime { get; set; }
        
        public int BrewTemp { get; set; }
        #endregion

        #region Constructors
        [JsonConstructor]
        public TeaModel(string name, TimeSpan steepTime, int brewTemp)
        {
            Name = name;
            SteepTime = steepTime;
            BrewTemp = brewTemp;
        }

        public TeaModel(string name, string steepTime, string brewTemp)
        {
            Name = name;
            if(Convert.ToInt32(brewTemp) > 212)
            {
                BrewTemp = 212;
            }
            else
            {
                BrewTemp = Convert.ToInt32(brewTemp);
            }
            string[] hhmmss = steepTime.Split(":");
            SteepTime = new TimeSpan(Convert.ToInt32(hhmmss[0]), Convert.ToInt32(hhmmss[1]), Convert.ToInt32(hhmmss[2]));
        }

        public TeaModel(string name, TimeSpan steepTime)
        {
            Name = name;
            SteepTime = steepTime;
            BrewTemp = 212;
        }

        public TeaModel(string name, int steepSeconds)
        {
            Name = name;
            SteepTime = new TimeSpan(0, 0, steepSeconds);
            BrewTemp = 212;
        }
        #endregion
    }
}