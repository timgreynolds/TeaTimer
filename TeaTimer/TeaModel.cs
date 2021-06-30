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

        public TeaModel(string name, int steepSeconds, int brewTemp)
        {
            Name = name;
            SteepTime = new TimeSpan(0, 0, steepSeconds);
            BrewTemp = brewTemp;
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

        #region methods

        #endregion
    }
}