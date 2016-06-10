using EnjoyFootball.Models;
using System;
using System.Collections.Generic;
using UIKit;
using System.Linq;
using EnjoyFootball_IOS_Univ.Models;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace EnjoyFootball_IOS_Univ
{
    public partial class ViewController : UIViewController
    {
        GetWebApi apiCaller = new GetWebApi();
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var gameName = await getGames();
            //var gameNames = new string[] { "anna", "victor", "jesus" };
            //var webclient = new WebClient();
            //var url = new Uri("http://localhost:23718/api/home/index");
            //webclient.DownloadStringAsync(url);
            //var Json = "";
            //webclient.DownloadStringCompleted += (s, e) => { Json = e.Result; };
            //var games = JsonConvert.DeserializeObject<List<Game>>(Json);
            //var gameName = games.Select(o => o.Description).ToArray();
            GamesTable.Source = new TableSource(gameName);
            //Add(GamesTable);
        }
        public async Task<string[]> getGames()
        {
            List<Game> games = await apiCaller.GetAllGames();
            return games.Select(o => o.Description).ToArray();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}