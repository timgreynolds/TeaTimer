using Microsoft.Maui.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using SQLite;
using System.Windows.Input;

namespace com.mahonkin.tim.maui.TeaTimer.DataModel
{
    /// <summary>
    /// Class that defines the tea variety along with some useful SQLite methods for managing the tea variety database.
    /// </summary>
    [Table("TeaVarieties")]
    public class TeaModel
    {
        #region Private Fields
        private int _id;
        private string _name;
        private TimeSpan _steepTime;
        private int _brewTemp;
        private static readonly string _appDataFolder = FileSystem.AppDataDirectory;
        private static readonly string _appConfigFolder = Path.Combine(_appDataFolder, "TeaTimer");
        private static readonly string _dbFileName = Path.Combine(_appConfigFolder, "TeaVarieties.db3");
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

        #region Non-SQL Public Properties
        /// <summary>
        /// The list of all tea varieties in the current tea database.
        /// <br>Primarily to be used for data binding the selection/picker.</br>
        /// </summary>
        [SQLite.Ignore]
        public static IList Teas
        {
            get => GetTeas();
        }
        #endregion Non-SQL Properties

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

        #region Private Methods
        private static List<TeaModel> CreateDatabase()
        {
            List<TeaModel> teas = new List<TeaModel>();

            // The DbFile must be created, and populated with at least one initial tea variety.
            // The routines *should* all be non-destructive, relying on 'CreateIfNotExist' patterns, but I added some extra checks just to be sure.
            try
            {
                Directory.CreateDirectory(_appConfigFolder);
                using (SQLiteConnection connection = new SQLiteConnection(_dbFileName, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex))
                {
                    TableMapping mapping = connection.TableMappings.Where(m => m.TableName.Equals("TeaVarieties", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (mapping is null)
                    {
                        connection.CreateTable<TeaModel>();
                    }
                    if (connection.Table<TeaModel>().Count() < 1)
                    {
                        connection.Insert(new TeaModel("Earl Grey"));
                    }
                    teas = connection.Table<TeaModel>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                throw SQLiteException.New(ex.Result, ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return teas;
        }

        private static TeaModel ValidateTea(TeaModel tea)
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

        private static List<TeaModel> GetTeas()
        {
            if (File.Exists(_dbFileName) == false)
            {
                return CreateDatabase();
            }
            else
            {
                using (SQLiteConnection connection = new SQLiteConnection(_dbFileName))
                {
                    return connection.Table<TeaModel>().ToList();
                }
            }
        }
        #endregion Private Methods

        #region Internal Methods
        /// <summary>
        /// Gets the tea from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier assigned by the database.</param>
        /// <returns>The tea identified by the supplied unique identifier.</returns>
        /// <exception cref="SQLiteException"></exception>
        /// <exception cref="Exception"></exception>
        internal static TeaModel GetById(int id)
        {
            TeaModel tea = null;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_dbFileName))
                {
                    tea = connection.Get<TeaModel>(id);
                }
            }
            catch (SQLiteException ex)
            {
                throw SQLiteException.New(ex.Result, ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return tea;
        }

        /// <summary>
        /// Adds the supplied tea object to the Tea Varieties database.
        /// </summary>
        /// <param name="tea">The specific tea to add to the database.</param>
        /// <returns>The newly added tea with its unique database ID filled in.</returns>
        /// <exception cref="Exception"></exception>
        internal static async Task<TeaModel> AddAsync(TeaModel tea)
        {
            tea = ValidateTea(tea);
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection(_dbFileName);
            try
            {
                if (await connection.InsertAsync(tea) < 1)
                {
                    throw new Exception("Could not add the tea to the database.");
                }
            }
            catch (SQLiteException ex)
            {
                throw SQLiteException.New(ex.Result, ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                await connection.CloseAsync();
            }
            return tea;
        }

        /// <summary>
        /// Updates the database entry with the same unique ID as the provided tea.
        /// </summary>
        /// <param name="tea">The tea with updated values.</param>
        /// <returns>True if the database is updated and false otherwise.</returns>
        /// <exception cref="Exception"></exception>
        internal static async Task<bool> UpdateAsync(TeaModel tea)
        {
            bool success = false;
            tea = ValidateTea(tea);
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection(_dbFileName);
            try
            {
                if (await connection.UpdateAsync(tea) > 0)
                {
                    success = true;
                }
            }
            catch (SQLiteException ex)
            {
                throw SQLiteException.New(ex.Result, ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                await connection.CloseAsync();
            }
            return success;
        }

        /// <summary>
        /// Deletes the database entry with the same unique ID as the provided tea.
        /// </summary>
        /// <param name="tea">The tea to be deleted.</param>
        /// <returns>True if the database is updated and false otherwise.</returns>
        /// <exception cref="Exception"></exception>
        internal static async Task<bool> DeleteAsync(TeaModel tea)
        {
            bool success = false;
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection(_dbFileName);
            try
            {
                if (await connection.DeleteAsync(tea) > 0)
                {
                    success = true;
                }
            }
            catch (SQLiteException ex)
            {
                throw SQLiteException.New(ex.Result, ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                await connection.CloseAsync();
            }
            return success;
        }
        #endregion Internal Methods
    }

}
