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
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextField TimerLabel { get; set; }

		[Action ("ButtonClicked:")]
		partial void ButtonClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (TimerLabel != null) {
				TimerLabel.Dispose ();
				TimerLabel = null;
			}
		}
	}
}
