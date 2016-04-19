using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.ViewModels
{
    public class GameBoolToEditVM
    {
        public int GameId { get; set; }
        public bool BoolToEdit { get; set; }

        public GameBoolToEditVM(int gameId)
        {
            GameId = gameId;
        }
    }
}
