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
        private static readonly string _appConfigFolder = FileSystem.AppDataDirectory;
        private static readonly string _appName = Assembly.GetExecutingAssembly().GetName().Name;
        private static readonly string _dbFileName = Path.Combine(_appConfigFolder, _appName, _appName + ".db3");
        private readonly bool _initialized;

        public TeaSqlService()
        {
            if (File.Exists(_dbFileName))
            {
                _initialized = true;
            }
            else
            {
                _initialized = Initialize();
            }
        }

        public TeaModel Add(object obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(object obj)
        {
            throw new NotImplementedException();
        }

        public List<TeaModel> Get()
        {
            List<TeaModel> teas = new List<TeaModel>();
            if (_initialized == false)
            {
                Initialize();
            }
            using (SQLiteConnection connection = new SQLiteConnection(_dbFileName, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex))
            {
                teas = connection.Table<TeaModel>().ToList();
            }
            return teas;
        }

        public TeaModel FindById(object id)
        {
            throw new NotImplementedException();
        }

        public bool Initialize()
        {
            bool initialized = false;
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
                    initialized = true;
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
            return initialized;
        }

        public TeaModel Update(object obj)
        {
            throw new NotImplementedException();
        }
    }
}