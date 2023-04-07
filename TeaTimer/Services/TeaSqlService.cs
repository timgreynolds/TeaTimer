using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using com.mahonkin.tim.maui.TeaTimer.DataModel;
using Microsoft.Maui.Storage;
using SQLite;

namespace com.mahonkin.tim.maui.TeaTimer.Services
{
    public class TeaSqlService : IDataService<TeaModel>
    {
        #region Private Fields
        private static readonly string _appConfigFolder = FileSystem.AppDataDirectory;
        private static readonly string _appName = Assembly.GetExecutingAssembly().GetName().Name;
        private static readonly string _dbFileName = Path.Combine(_appConfigFolder, _appName, _appName + ".db3");
        private readonly SQLiteAsyncConnection _asyncConnection;
        private bool _initialized;
        #endregion Private Fields

        #region Constructors
        public TeaSqlService()
        {
            _asyncConnection = new SQLiteAsyncConnection(_dbFileName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex);
        }
        #endregion Constructors

        #region Public Methods
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

        public TeaModel Add(object obj)
        {
            throw new NotImplementedException();
        }

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

        public TeaModel Update(object obj)
        {
            throw new NotImplementedException();
        }

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

        public bool Delete(object obj)
        {
            throw new NotImplementedException();
        }

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

        public List<TeaModel> Get()
        {
            List<TeaModel> teas = new List<TeaModel>();
            if (_initialized == false)
            {
                Initialize();
            }
            using (SQLiteConnection connection = new SQLiteConnection(_dbFileName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex))
            {
                teas = connection.Table<TeaModel>().ToList();
            }
            return teas;
        }

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

        public TeaModel FindById(object id)
        {
            throw new NotImplementedException();
        }

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