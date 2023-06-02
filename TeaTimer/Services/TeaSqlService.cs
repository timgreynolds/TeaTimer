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
    ///<inheritdoc cref="IDataService{T}"/>
    public class TeaSqlService : IDataService<TeaModel>
    {
        #region Private Fields
        private static readonly string _appConfigFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string _appName = Assembly.GetExecutingAssembly().GetName().Name;
        private static readonly string _dbFileName = Path.Combine(_appConfigFolder, _appName, _appName + ".db3");
        private SQLiteAsyncConnection _asyncConnection;
        private bool _initialized;
        #endregion Private Fields

        #region Public Methods
        /// <inheritdoc cref="IDataService{T}.Initialize()"/>
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

        /// <inheritdoc cref="IDataService{T}.Add(object)"/>
        public TeaModel Add(object obj)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IDataService{T}.AddAsync(object)" />
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

        /// <inheritdoc cref="IDataService{T}.Update(object)" />
        public TeaModel Update(object obj)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IDataService{T}.UpdateAsync(object)" />
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

        /// <inheritdoc cref="IDataService{T}.Delete(object)" />
        public bool Delete(object obj)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IDataService{T}.DeleteAsync(object)" />
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

        /// <inheritdoc cref="IDataService{T}.Get()" />
        public List<TeaModel> Get()
        {
            if (_initialized == false)
            {
                Initialize();
            }
            using (SQLiteConnection connection = new SQLiteConnection(_dbFileName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex))
            {
                return connection.Table<TeaModel>().ToList();
            }
        }

        /// <inheritdoc cref="IDataService{T}.GetAsync()" />
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

        /// <inheritdoc cref="IDataService{T}.FindById(object)" />
        public TeaModel FindById(object id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IDataService{T}.FindByIdAsync(object)" />
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