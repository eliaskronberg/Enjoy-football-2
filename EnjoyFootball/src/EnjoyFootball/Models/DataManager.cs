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

        public DataManager(FootballContext context)
        {
            this.context = context;
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
    }
}
