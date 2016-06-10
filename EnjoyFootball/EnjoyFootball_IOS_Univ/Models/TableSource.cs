using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

namespace EnjoyFootball_IOS_Univ.Models
{
    public class TableSource: UITableViewSource
    {
        protected string[] tableItems;
        protected string cellIdentifier = "TableCell";
        public TableSource(string[] items)
        {
            tableItems = items;
        }
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // request a recycled cell to save memory
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
            // if there are no cells to reuse, create a new one
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            cell.TextLabel.Text = tableItems[indexPath.Row];
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return tableItems.Length;
        }
    }
}
