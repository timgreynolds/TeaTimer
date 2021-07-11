// This file has been autogenerated from a class added in the UI designer.

using System;
using AppKit;
using Foundation;

namespace TeaTimer
{
    public partial class TeaViewController : NSViewController
    {
        #region Computed Properties
        public TeaModel Tea { get; set; }
        #endregion

        #region Constructors
        public TeaViewController (IntPtr handle) : base (handle)
		{	
		}
        #endregion

        #region Override Methods
        public override void ViewWillAppear()
        {
            base.ViewWillAppear();

            // Do any additional setup before loading the view.            
            TeaTextField.Changed += (sender, e) => TextFieldChanged(sender, e);
            SteepTimeTextField.Changed += (sender, e) => TextFieldChanged(sender, e);
            BrewTempTextField.Changed += (sender, e) => TextFieldChanged(sender, e);

            if (Tea != null)
            {
                TeaTextField.StringValue = Tea.Name;
                SteepTimeTextField.StringValue = Tea.SteepTime.ToString();
                BrewTempTextField.StringValue = Tea.BrewTemp.ToString();
            }
        }

        public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Do any additional setup after loading the view.
		}

        public override NSObject RepresentedObject
		{
			get => base.RepresentedObject;
            // Update the view, if already loaded.
            set => base.RepresentedObject = value;
		}
        #endregion

        #region Methods
        // TODO Need to update changes to an existing tea variety or add a new
        // one to the database
        partial void SaveButtonClicked(NSObject sender)
        {
            if(string.IsNullOrWhiteSpace(TeaTextField.StringValue) ||
                string.IsNullOrWhiteSpace(SteepTimeTextField.StringValue) ||
                string.IsNullOrWhiteSpace(BrewTempTextField.StringValue))
            {
                NSAlert alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Warning,
                    MessageText = "Cannot Save Tea Variety",
                    InformativeText = "You must specify Tea Name, Steep Time, and Brew Temperature."
                };
                alert.BeginSheet(View.Window);
            }
            else
            {
                bool success = false;
                if(Tea == null)
                {
                    try
                    {
                        success = TeaVarietiesDataSource.AddTea(new TeaModel(TeaTextField.StringValue, SteepTimeTextField.StringValue, BrewTempTextField.StringValue));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        success = TeaVarietiesDataSource.UpdateTea(new TeaModel(TeaTextField.StringValue, SteepTimeTextField.StringValue, BrewTempTextField.StringValue));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if(success)
                {
                    NSAlert alert = new NSAlert()
                    {
                        AlertStyle = NSAlertStyle.Informational,
                        MessageText = "Tea Saved!"
                    };
                    alert.BeginSheet(View.Window);
                }
            }
        }

        private void TextFieldChanged(Object sender, EventArgs e)
        {
            SaveButton.Enabled = true;
        }
        #endregion
    }
}
