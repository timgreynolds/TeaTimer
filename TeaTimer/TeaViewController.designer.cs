// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using AppKit;
using Foundation;

namespace TeaTimer
{
    [Register ("TeaViewController")]
	partial class TeaViewController
	{
		[Outlet]
		NSTextField BrewTempTextField { get; set; }

		[Outlet]
		NSTextField SteepTimeTextField { get; set; }

		[Outlet]
		NSTextField TeaTextField { get; set; }

		[Action ("SaveButtonClicked:")]
		partial void SaveButtonClicked (NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (TeaTextField != null) {
				TeaTextField.Dispose ();
				TeaTextField = null;
			}

			if (SteepTimeTextField != null) {
				SteepTimeTextField.Dispose ();
				SteepTimeTextField = null;
			}

			if (BrewTempTextField != null) {
				BrewTempTextField.Dispose ();
				BrewTempTextField = null;
			}
		}
	}
}
