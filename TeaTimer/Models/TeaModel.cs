﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace com.mahonkin.tim.maui.TeaTimer.DataModel
{
    /// <summary>
    /// Class that defines the tea variety.
    /// </summary>
    [Table("TeaVarieties")]
    public class TeaModel
    {
        #region Private Fields
        private int _id;
        private int _brewTemp;
        private string _name;
        private TimeSpan _steepTime;
        #endregion Private Fields      

        #region Public Properties
        /// <summary>
        /// The database-assigned unique ID for this tea record. There should be no reason to set it in code.
        /// </summary>
        [Column("ID"), PrimaryKey, AutoIncrement]
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        /// <summary>
        /// The name of the tea variety. Could be a traditional name or a commercial name.
        /// <br>
        /// Essentially this is whatever the user will identify the tea variety by. For example, 'Earl Grey' or 'SleepyTime'.
        /// </br>
        /// </summary>
        [Column("Name"), Unique, NotNull]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        /// <summary>
        /// The amount of time to allow the tea to steep in the mug or cup.
        /// <br>If the user tries to set the steep time to a value of 30 minutes or longer an ArgumentException will be thrown.</br>
        /// </summary>
        [Column("Steeptime"), NotNull]
        public TimeSpan SteepTime
        {
            get => _steepTime;
            set => _steepTime = value;
        }

        /// <summary>
        /// The water temperature in farenheit degrees at which to steep the tea.
        /// <br>If the user attempts to set a temperature value greater than boiling (212 degrees farenheit) the value will be set to 212.</br>
        /// </summary>
        [Column("Brewtemp"), NotNull]
        public int BrewTemp
        {
            get => _brewTemp;
            set => _brewTemp = value;
        }
        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Do not use in your code.
        /// <br>Public, parameterless constructor required by the SQLite serialization/deserialization routines. If you try to use it you will almost
        /// undoubtedly get an exception.</br>
        /// </summary>
        public TeaModel()
        {
        }

        /// <summary>
        /// Creates a new Tea Variety object.
        /// <br>The user must specify a value for variety name and may specify optional values for steep time and brew temperature with these limitations:</br>
        /// <list type="bullet">
        /// <item>Steep time must be provided as a string in standard 'mm:ss' format or an ArgumentException will be thrown.</item>
        /// <item>Providing a steep time of greater than 30 minutes will throw an ArgumentException.</item>
        /// <item>Providing a brew temperature of greater than boiling (212 degrees farenheit) will result in the temperature being set to boiling.</item>
        /// <item>Not providing a step time will result in a steep time of 2 minutes.</item>
        /// <item>Not providing a brew temperature will result in the brew temperature being set to boiling (212 degrees farenheit).</item>
        /// </list>
        /// </summary>
        /// <param name="name">The user-identified name of the tea variety. May be a common name like 'Earl Grey' or a commercial name like 'SleepyTime'.</param>
        /// <param name="steepTime">The amount of time, expressed as a string in standard 'mm:ss' format, the tea should steep. If set to a time greater than 30 minutes an ArgumentException is thrown.</param>
        /// <param name="brewTemp">The temperature in degrees farenheit at which the tea should steep. If attempting to set to a value greater than 212 (boiling) it will be set to 212.</param>
        /// <exception cref="ArgumentException">ArgumentException</exception>
        internal TeaModel(string name, string steepTime = "02:00", int brewTemp = 212)
        {
            TimeSpan time = TimeSpan.MaxValue;
            if (TimeSpan.TryParseExact(steepTime, @"mm\:ss", null, out time) == false)
            {
                throw new ArgumentException($"Could not parse provided steep time {steepTime}", nameof(steepTime));
            }
            SteepTime = time <= new TimeSpan(0, 30, 0) ? time : throw new ArgumentException($"Steep times of greater than 30 minutes ({steepTime}) really don't make sense.", nameof(steepTime));
            Name = name;
            BrewTemp = brewTemp <= 212 ? brewTemp : 212;
        }

        /// <summary>
        /// Creates a new Tea Variety object.
        /// <br>The user must specify values for variety name and steep time and may specify an optional brew temperature with these limitations:</br>
        /// <list type="bullet">
        /// <item>Providing a steep time of greater than 30 minutes will throw an ArgumentException.</item>
        /// <item>Providing a brew temperature of greater than boiling (212 degrees farenheit) will result in the temperature being set to boiling.</item>
        /// <item>Not providing a brew temperature will result in the brew temperature being set to boiling (212 degrees farenheit).</item>
        /// </list>
        /// </summary>
        /// <param name="name">The user-identified name of the tea variety. May be a common name like 'Earl Grey' or a commercial name like 'SleepyTime'.</param>
        /// <param name="steepTime">The amount of time, expressed as a TimeSpan object, the tea should steep. If set to a time greater than 30 minutes an ArgumentException is thrown.</param>
        /// <param name="brewTemp">The temperature in degrees farenheit at which the tea should steep. If attempting to set to a value greater than 212 (boiling) it will be set to 212.</param>
        /// <exception cref="ArgumentException">ArgumentException</exception>
        internal TeaModel(string name, TimeSpan steepTime, int brewTemp = 212)
        {
            Name = name;
            SteepTime = steepTime <= new TimeSpan(0, 30, 0) ? steepTime : throw new ArgumentException($"Steep times greater than 30 minutes ({steepTime}) don't really make sense.", nameof(steepTime));
            BrewTemp = brewTemp <= 212 ? brewTemp : 212;
        }
        #endregion Constructors

        #region Public Methods
        public static TeaModel ValidateTea(TeaModel tea)
        {
            tea.Name = tea.Name.Trim();
            if (string.IsNullOrEmpty(tea.Name) || string.IsNullOrWhiteSpace(tea.Name))
            {
                throw new ArgumentNullException(nameof(tea.Name), "Tea variety must have a name.");
            }
            if (tea.BrewTemp > 212)
            {
                tea.BrewTemp = 212;
            }
            if (tea.SteepTime > new TimeSpan(0, 30, 0) || tea.SteepTime <= new TimeSpan(0))
            {
                throw new ArgumentOutOfRangeException(nameof(tea.SteepTime), "Steep Time must be more than zero seconds and less than 30 minutes.");
            }

            return tea;
        }
        #endregion Public Methods
    }
}
