using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.ViewModels
{
    public class UserToRemoveVM
    {
        public string UserId { get; set; }
        public int GameId { get; set; }
        public UserToRemoveVM(string uid, int gid)
        {
            UserId = uid;
            GameId = gid;
        }
    }
}
