using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using com.mahonkin.tim.maui.TeaTimer.DataModel;
using SQLite;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    /// <summary>
    /// Implementation of <see cref="IDataService{T}">IDataService"</see> using
    /// SQLLite and a database of teas.
    /// </summary>
    public class TeaSqlService : IDataService<TeaModel>
    {
        #region Private Fields
        private static readonly string _appConfigFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static readonly string _appName = Assembly.GetExecutingAssembly().GetName().Name;
        private static readonly string _dbFileName = Path.Combine(_appConfigFolder, _appName, _appName + ".db3");
        private SQLiteAsyncConnection _asyncConnection;
        private bool _initialized;
        #endregion Private Fields

        #region Public Methods
        /// <summary>
        /// Ensures that the database exists and contains at least one tea
        /// variety.
        /// </summary>
        /// <remarks>
        /// Creates the DB file in the platform-specific <see
        /// cref="Environment.SpecialFolder.LocalApplicationData">Local
        /// Application Data</see> directory and populates it with 'Earl Grey'
        /// tea. If the DB file already exists and contains at least one entry
        /// this should be a no-op.
        /// </remarks>
        public void Initialize()
        {
            // The DbFile must be created, and populated with at least one initial tea variety.
            // The routines *should* all be non-destructive, relying on 'CreateIfNotExist' patterns, but I added some extra checks just to be sure.
            try
            {
                Directory.CreateDirectory(_appConfigFolder);
                Directory.CreateDirectory(Path.Combine(_appConfigFolder, _appName));
                using (SQLiteConnection connection = new SQLiteConnection(_dbFileName, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex))
                {
                    TableMapping mapping = connection.TableMappings.Where(m => m.TableName.Equals("TeaVarieties", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (mapping is null)
                    {
                        CreateTableResult createTableResult = connection.CreateTable<TeaModel>();
                    }
                    if (connection.Table<TeaModel>().Count() < 1)
                    {
                        connection.Insert(new TeaModel("Earl Grey"));
                    }
                    _initialized = true;
                }
                _asyncConnection = new SQLiteAsyncConnection(_dbFileName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex);
            }
            catch (SQLiteException ex)
            {
                throw SQLiteException.New(ex.Result, ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Use the async method if possible.
        /// </summary>
        /// <remarks>
        /// This wraps the async method in a continuation using ContinueWith.
        /// </remarks>
        public TeaModel Add(object obj)
        {
            TeaModel tea = null;
            AddAsync(obj).ContinueWith((t) => { tea = t.Result; })
                .ConfigureAwait(false);
            return tea;
        }

        /// <summary>
        /// Adds the given tea to the database in an asynchronous manner.
        /// </summary>
        /// <param name="obj">
        /// A <see cref="TeaModel">Tea</see> to add to the database.
        /// </param>
        /// <returns>
        /// A Task representing the add operation. The task result contains the
        /// tea as added to the database including its auto-assigned unique key.
        /// </returns>
        /// <exception cref="SQLiteException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<TeaModel> AddAsync(object obj)
        {
            TeaModel tea = TeaModel.ValidateTea((TeaModel)obj);
            if (_initialized == false)
            {
                Initialize();
            }
            try
            {
                await _asyncConnection.InsertAsync(tea).ConfigureAwait(false);
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
        /// Use the async method if possible.
        /// </summary>
        /// <remarks>
        /// This wraps the async method in a continuation using ContinueWith.
        /// </remarks>
        public TeaModel Update(object obj)
        {
            TeaModel tea = null;
            UpdateAsync(obj).ContinueWith((t) => tea = t.Result)
                .ConfigureAwait(false);
            return tea;
        }

        /// <summary>
        /// Updates all of the columns of a table using the given tea except
        /// for its primary key in an asynchronous manner. The object is 
        /// required to have a primary key.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="TeaModel">Tea</see> to be updated.
        /// </param>
        /// <returns>
        /// A Task representing the update operation. The task result contains
        /// the tea as it was updated.
        /// </returns>
        /// <exception cref="SQLiteException" />
        /// <exception cref="Exception" />
        public async Task<TeaModel> UpdateAsync(object obj)
        {
            TeaModel tea = TeaModel.ValidateTea((TeaModel)obj);
            if (_initialized == false)
            {
                Initialize();
            }
            try
            {
                await _asyncConnection.UpdateAsync(tea).ConfigureAwait(false);
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
        /// Use the async method if possible.
        /// </summary>
        /// <remarks>
        /// This wraps the async method in a continuation using ContinueWith.
        /// </remarks>
        public bool Delete(object obj)
        {
            bool deleted = false;
            DeleteAsync(obj).ContinueWith((t) => deleted = t.Result)
                .ConfigureAwait(false);
            return deleted;
        }

        /// <summary>
        /// Deletes the given tea from the database using its primary key in an
        /// asynchronous manner. The object is required to have a primary key.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="TeaModel">Tea</see> to be deleted.
        /// </param>
        /// <returns>
        /// A Task representing the delete operation. The task result contains
        /// true if the tea was deleted and false otherwise.
        /// </returns>
        /// <exception cref="SQLiteException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<bool> DeleteAsync(object obj)
        {
            TeaModel tea = TeaModel.ValidateTea((TeaModel)obj);
            bool deleted = false;
            if (_initialized == false)
            {
                Initialize();
            }
            try
            {
                if (await _asyncConnection.DeleteAsync(tea).ConfigureAwait(false) == 1)
                {
                    deleted = true;
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
            return deleted;
        }

        /// <summary>
        /// Use the async method if possible.
        /// </summary>
        /// <remarks>
        /// This wraps the async method in a continuation using ContinueWith.
        /// </remarks>
        public List<TeaModel> Get()
        {
            List<TeaModel> teas = new List<TeaModel>();
            GetAsync().ContinueWith((t) => teas = t.Result)
                .ConfigureAwait(false);
            return teas;
        }

        /// <summary>
        /// Gets all the teas from the database in an asynchronous manner.
        /// </summary>
        /// <returns>
        /// A Task representing the get operation. The task result contains a
        /// List of all the teas in the database.
        /// </returns>
        /// <exception cref="SQLiteException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<List<TeaModel>> GetAsync()
        {
            if (_initialized == false)
            {
                Initialize();
            }
            try
            {
                return await _asyncConnection.Table<TeaModel>().ToListAsync().ConfigureAwait(false);
            }
            catch (SQLiteException ex)
            {
                throw SQLiteException.New(ex.Result, ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Use the async method if possible.
        /// </summary>
        /// <remarks>
        /// This wraps the async method in a continuation using ContinueWith.
        /// </remarks>
        public TeaModel FindById(object id)
        {
            TeaModel tea = null;
            FindByIdAsync(id).ContinueWith((t) => tea = t.Result)
                .ConfigureAwait(false);
            return tea;
        }

        /// <summary>
        /// Attempts to retrieve the tea with the given primary key from the
        /// database in an asynchronous manner. Use of this method requires that
        /// the given tea have a primary key.
        /// </summary>
        /// <param name="obj">The primary key of the tea to retrieve.</param>
        /// <returns>
        /// A Task representing the retrieve operation. The task result contains
        /// the tea retrieved or null if not found.
        /// </returns>
        /// <exception cref="SQLiteException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<TeaModel> FindByIdAsync(object obj)
        {
            if (_initialized == false)
            {
                Initialize();
            }
            try
            {
                return await _asyncConnection.FindAsync<TeaModel>(obj).ConfigureAwait(false);
            }
            catch (SQLiteException ex)
            {
                throw SQLiteException.New(ex.Result, ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion Public Methods
    }
}