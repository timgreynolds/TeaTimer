using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppKit;
using Foundation;
using Newtonsoft.Json;

namespace TeaTimer
{
    [Register("TeaVarietiesDataSourcce")]
    public class TeaVarietiesDataSource : NSComboBoxDataSource
    {
        #region Private Variables
        private static List<TeaModel> _teas;

        // No need for a real database. Save the teas as Json file.
        private static readonly string _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TeaTimer", "TeaVarieties.json");
        #endregion

        #region Computed Properties
        public List<TeaModel> Teas
        {
            get => _teas;
        }
        #endregion

        #region Constructors
        public TeaVarietiesDataSource()
        {
            InitializeDatabase();
        }
        #endregion

        #region Override Methods
        public override nint ItemCount(NSComboBox comboBox)
        {
            return _teas.Count;
        }

        public override NSObject ObjectValueForItem(NSComboBox comboBox, nint index)
        {
            return FromObject(_teas[(int)index].Name);
        }

        public override nint IndexOfItem(NSComboBox comboBox, string value)
        {
            return _teas.FindIndex(tea => tea.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase));
        }

        public override string CompletedString(NSComboBox comboBox, string uncompletedString)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        private void InitializeDatabase()
        {
            if (File.Exists(_dbPath))
            {
                // Tea database already exists. Assume the Json contains a List of teas and set _teas.
                try
                {
                    ReloadTeas();
                }
                catch (Exception ex)
                {
                    ShowAlert(ex, "An error occurred trying to read the tea varieties database.");
                }
            }
            else
            {
                // Tea database doesn't already exist so it needs to be created.
                try
                {
                    Directory.CreateDirectory(_dbPath);

                    using (StreamWriter writer = new StreamWriter(_dbPath, false))
                    {
                        writer.WriteLine(JsonConvert.SerializeObject(new List<TeaModel> { new TeaModel("Earl Grey", 120) }));
                    }
                    // Reset _teas to the contents of the newly created Json file. This helps to ensure future deserialization will be successful.
                    ReloadTeas();
                }
                catch (Exception ex)
                {
                    ShowAlert(ex, "An error occurred trying to create the initial tea varieties database.");
                }
            }
        }

        private static void ShowAlert(Exception ex, string messageText)
        {
            NSAlert alert = new NSAlert()
            {
                AlertStyle = NSAlertStyle.Critical,
                MessageText = messageText,
                InformativeText = $"Message: {ex.Message}"
            };
            alert.RunModal();
        }

        private static void ReloadTeas()
        {
            _teas = JsonConvert.DeserializeObject<List<TeaModel>>(File.ReadAllText(_dbPath));
        }
        #endregion

        #region Public Methods
        internal static bool UpdateTea(TeaModel tea)
        {            
            List<TeaModel> old = _teas.Where(o => o.Name.Equals(tea.Name, StringComparison.InvariantCultureIgnoreCase)).ToList();
            foreach(TeaModel t in old)
            {
                _teas.Remove(t);
            }
            _teas.Add(tea);
            using (StreamWriter writer = new StreamWriter(_dbPath, false))
            {
                writer.WriteLine(JsonConvert.SerializeObject(_teas));
            }
            ReloadTeas();
            return _teas.FindIndex(t => t.Name.Equals(tea.Name, StringComparison.InvariantCultureIgnoreCase)) > -1;
        }

        internal static bool AddTea(TeaModel tea)
        {
            bool success = UpdateTea(tea);
            ReloadTeas();
            return success;
        }
        #endregion
    }
}
