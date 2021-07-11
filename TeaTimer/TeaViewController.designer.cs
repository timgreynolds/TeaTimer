// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace TeaTimer
{
	[Register ("TeaViewController")]
	partial class TeaViewController
	{
		[Outlet]
		AppKit.NSTextField BrewTempTextField { get; set; }

		[Outlet]
		AppKit.NSButton SaveButton { get; set; }

		[Outlet]
		AppKit.NSTextField SteepTimeTextField { get; set; }

		[Outlet]
		AppKit.NSTextField TeaTextField { get; set; }

		[Action ("SaveButtonClicked:")]
		partial void SaveButtonClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (BrewTempTextField != null) {
				BrewTempTextField.Dispose ();
				BrewTempTextField = null;
			}

			if (SteepTimeTextField != null) {
				SteepTimeTextField.Dispose ();
				SteepTimeTextField = null;
			}

			if (TeaTextField != null) {
				TeaTextField.Dispose ();
				TeaTextField = null;
			}

			if (SaveButton != null) {
				SaveButton.Dispose ();
				SaveButton = null;
			}
		}
	}
}
