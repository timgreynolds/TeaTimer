using System;
using AppKit;
using Foundation;

namespace TeaTimer
{
    [Register("TeaVarietiesDataSourcce")]
    public class TeaVarietiesDataSource : NSComboBoxDataSource
    {
        #region Private Variables
        private nint _recordCount = 0;
        #endregion

        #region Computed Properties
        public nint RecordCount
        {
            get => _recordCount;
        }
        #endregion

        #region Constructors
        public TeaVarietiesDataSource()
        {
            
        }

        #endregion

        #region Override Methods
        public override nint ItemCount(NSComboBox comboBox)
        {
            return _recordCount;
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
    }
}
