using EnjoyFootball.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.Models
{
    public class DataManager
    {
        FootballContext context;

        static List<Game> listOfMatches = new List<Game>();

        public DataManager(FootballContext context)
        {
            this.context = context;
            listOfMatches.Add(new Game { Id = 1, Owner = "Martin(alltid)", PitchName = "Vasaparken", MaxSlots = 10, StartTime = DateTime.Now, PlayerList = new List<Player>() });
            listOfMatches.Add(new Game { Id = 2, Owner = "Martin(alltid)", PitchName = "GrimstaIp", MaxSlots = 7, StartTime = DateTime.Now, PlayerList = new List<Player>() });
            listOfMatches.Add(new Game { Id = 3, Owner = "Martin(alltid)", PitchName = "GrimstaBeach", MaxSlots = 1, StartTime = DateTime.Now, PlayerList = new List<Player>() });
            listOfMatches.Add(new Game { Id = 4, Owner = "Martin(alltid)", PitchName = "Husby", MaxSlots = 3, StartTime = DateTime.Now, PlayerList = new List<Player>() });
        }

        // Only creates the field if there's no other field with the same name
        public bool CreateField(CreateFieldVM viewModel)
        {
            var result = context.Fields.Where(x => x.Name == viewModel.Name).SingleOrDefault();
            if (result == null)
            {
                context.Fields.Add(
                    new Field
                    {
                        Capacity = viewModel.Capacity,
                        Coordinates = viewModel.Coordinates,
                        Description = viewModel.Description,
                        Lighting = viewModel.Lighting,
                        Name = viewModel.Name,
                        Turf = viewModel.Turf
                    });

                context.SaveChanges();
                return true;
            }
            return false;
        }


        public bool CreateGame(CreateGameVM createGameVm, string userId)
        {
            var fieldId = context.Fields.Where(o => o.Name == createGameVm.Field).Select(o => o.Id).SingleOrDefault();
            try
            {
                context.Games.Add(new Game
                {
                    Description = createGameVm.Description,
                    IsActive = createGameVm.IsActive,
                    IsPublic = createGameVm.IsPublic,
                    Field = fieldId,
                    MaxSlots = createGameVm.MaxSlots,
                    Owner = userId,
                    StartTime = createGameVm.StartTime
                });
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;

        }


        public List<FieldVM> ListFields()
        {

            return context.Fields.Select(o => new FieldVM
            {
                Id = o.Id,
                Capacity = o.Capacity,
                Coordinates = o.Coordinates,
                Description = o.Description,
                Lighting = o.Lighting,
                Name = o.Name,
                Turf = o.Turf,
                Condition = o.Condition,
                Votes = o.Votes
            }).ToList();
        }

        public string[] GetAllFieldNames()
        {
            return context.Fields.Select(o => o.Name).ToArray();
        }

        public string GetUserId(string Name)
        {
            var user = context.Users.Where(o => o.UserName == Name).SingleOrDefault();
            return user.Id;
        }
        public List<Game> GetAllGames()
        {
            return listOfMatches;
        }
        public GameDetails getMatchByID(int id)
        {
            var game = listOfMatches
                .Where(o => o.Id == id)
                .Select(o => new GameDetails
                {
                    Id=o.Id,
                    Field = o.PitchName,
                    Owner = o.Owner,
                    OpenSlots = o.MaxSlots,
                    StartTime = o.StartTime,
                    PlayerList=o.PlayerList
                })
                .First();

            return game;
        }
        public void AddPlayerToGame(int id, Player playerToAdd)
        {
            var game = GetAllGames()
                .Where(o => o.Id == id)
                .First();
            game.PlayerList.Add(playerToAdd);
        }
    }
}
