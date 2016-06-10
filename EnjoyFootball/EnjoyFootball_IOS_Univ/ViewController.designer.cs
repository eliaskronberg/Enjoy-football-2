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

namespace EnjoyFootball_IOS_Univ
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView GamesTable { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (GamesTable != null) {
				GamesTable.Dispose ();
				GamesTable = null;
			}
		}
	}
}
