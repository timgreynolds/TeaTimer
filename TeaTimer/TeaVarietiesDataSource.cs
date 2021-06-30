using System;
using System.Collections.Generic;
using System.IO;
using AppKit;
using Foundation;
using Newtonsoft.Json;

namespace TeaTimer
{
    [Register("TeaVarietiesDataSourcce")]
    public class TeaVarietiesDataSource : NSComboBoxDataSource
    {
        #region Private Variables
        // First tea to be used when initializing the tea database.
        // TODO: May want to expand this to a larger list in the future.
        private static List<TeaModel> _teas = new List<TeaModel> { new TeaModel("Earl Grey", 120, 212) };

        // Probably don't really need this. Can be inferred from the length of List<TeaModel>
        private static nint _recordCount = _teas.Count;

        // No need for a real database. Save the teas as Json file.
        private static readonly string _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TeaTimer", "TeaVarieties.json");        
        #endregion

        #region Computed Properties
        public static nint RecordCount
        {
            get => _recordCount;
        }

        public static List<TeaModel> Teas
        {
            get => _teas;
            set => _teas = value;
        }
        #endregion

        #region Override Methods
        public override nint ItemCount(NSComboBox comboBox)
        {
            return RecordCount;
        }

        public override NSObject ObjectValueForItem(NSComboBox comboBox, nint index)
        {
            throw new NotImplementedException();
        }

        public override nint IndexOfItem(NSComboBox comboBox, string value)
        {
            throw new NotImplementedException();
        }

        public override string CompletedString(NSComboBox comboBox, string uncompletedString)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region methods
        public static List<TeaModel> InitializeDatabase()
        {
            if (File.Exists(_dbPath))
            {
                // Tea database already exists. Assume the Json contains a List of teas and return it.
                _teas = JsonConvert.DeserializeObject<List<TeaModel>>(File.ReadAllText(_dbPath));
                _recordCount = _teas.Count;
                return _teas;
            }
            else
            {
                // Tea database doesn't already exist so it needs to be created.
                Directory.CreateDirectory(Path.GetDirectoryName(_dbPath));
                
                using (StreamWriter writer = new StreamWriter(_dbPath, append: true))
                {
                    writer.WriteLine(JsonConvert.SerializeObject(_teas));
                }
                // Return the contents of the newly created Json file. This helps to ensure future deserialization will be successful.
                _teas = JsonConvert.DeserializeObject<List<TeaModel>>(File.ReadAllText(_dbPath));
                _recordCount = _teas.Count;
                return _teas;
            }
        }
        #endregion
    }
}
