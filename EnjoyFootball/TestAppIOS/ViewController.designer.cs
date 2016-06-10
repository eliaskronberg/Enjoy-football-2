// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TestAppIOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton TestBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TestLabel { get; set; }

		[Action ("DownClick:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void DownClick (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (TestBtn != null) {
				TestBtn.Dispose ();
				TestBtn = null;
			}
			if (TestLabel != null) {
				TestLabel.Dispose ();
				TestLabel = null;
			}
		}
	}
}
