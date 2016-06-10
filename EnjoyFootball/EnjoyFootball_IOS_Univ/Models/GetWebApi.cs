using EnjoyFootball.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace EnjoyFootball_IOS_Univ
{
    class GetWebApi
    {
       HttpClient httpClient;
        public async Task<string> getWebApi(string urlaction)
        {
            var localhost = System.Net.Dns.GetHostName();
            var localhost1= System.Net.Dns.GetHostAddresses(localhost);
            httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri("http://localhost:23718/api/");
            //var response = await httpClient.GetAsync(urlaction);
            //var result = response.Content.ReadAsStringAsync().Result;
            var url = "http://localhost:23718/api/" + urlaction;
            var Json = await httpClient.GetStringAsync(url);
            return Json;

        }

        //public async Task<PlayerDetailsVM> GetPlayerInfo(string id)
        //{
        //    var Json = await getWebApi($"player/index/{id}");
        //    var player = JsonConvert.DeserializeObject<Player>(Json);

        //    PlayerDetailsVM playerPDW = new PlayerDetailsVM();
        //    playerPDW.Id = player.Id;
        //    playerPDW.Age = player.Age;
        //    playerPDW.FirstName = player.FirstName;
        //    playerPDW.LastName = player.LastName;
        //    playerPDW.Nickname = player.Nickname;
        //    playerPDW.Skill = player.Skill;
        //    playerPDW.City = player.City;

        //    return playerPDW;

        //}

        //public async Task<string> GetCurrentUser()
        //{
        //    var Json = await getWebApi("account/GetCurrentUser");
        //    var result = JsonConvert.DeserializeObject<string>(Json);
        //    return result;
        //}

        //// Only creates the field if there's no other field with the same name
        //public async Task<bool> CreateField(CreateFieldVM viewModel)
        //{
        //    var Json = await getWebApi("Field/createField");
        //    var result = JsonConvert.DeserializeObject<bool>(Json);
        //    return result;
        //}

        //internal async Task<SignInResult> signInUser(LoginVM loginvm)
        //{
        //    Login user = new Login();
        //    user.Email = loginvm.EMail;
        //    user.Password = loginvm.Password;

        //    var userSer = user.Email + ":" + user.Password;
        //    var Json = await getWebApi($"account/login/{userSer}");
        //    var result = JsonConvert.DeserializeObject<SignInResult>(Json);

        //    return result;
        //}

        internal async void ToggleActive(int gameId, bool isActive)
        {
            await getWebApi($"game/ChangeActive/{gameId}/{isActive}");
        }

        internal async void TogglePublic(int gameId, bool isPublic)
        {
            await getWebApi($"game/ChangePublic/{gameId}/{isPublic}");
        }

        public async Task<List<Field>> ListFields()
        {
            var Json = await getWebApi($"field/index");
            var result = JsonConvert.DeserializeObject<List<Field>>(Json);
            return result;
        }

        public async Task<string> GetFieldById(int fieldId)
        {
            var fieldName = await getWebApi($"game/GetFieldName/{fieldId}");
            return fieldName;
        }

        public async Task<string[]> GetAllFieldNames()
        {
            var fields = await ListFields();
            return fields.Select(o => o.Name).ToArray();

        }


        public async Task<List<Game>> GetAllGames()
        {
            var gameList = await getWebApi("home/index");
            var result = JsonConvert.DeserializeObject<List<Game>>(gameList);
            return result;
        }
        //public async Task<GameDetailsVM> getGameByID(int id)
        //{
        //    var game = await getWebApi($"game/index/{id}");
        //    var result = JsonConvert.DeserializeObject<Game>(game);
        //    GameDetailsVM gdv = new GameDetailsVM();
        //    gdv.Description = result.Description;
        //    gdv.Field = await GetFieldById(result.Field);
        //    gdv.Id = result.Id;
        //    gdv.IsActive = result.IsActive;
        //    gdv.IsPublic = result.IsPublic;
        //    gdv.OpenSlots = result.MaxSlots;
        //    gdv.Owner = result.Owner;
        //    gdv.StartTime = result.StartTime;
        //    gdv.PlayerList = await PlayerByGameId(id);
        //    return gdv;
        //}


        public async void AddPlayerToGame(string playerName, int id)
        {
            await getWebApi($"game/addplayer/{id}/{playerName}");
        }

        public async void RemovePlayerFromGame(int id, string playerIdToRemove)
        {
            await getWebApi($"game/removeplayer/{playerIdToRemove}/{id}");
        }

        public async Task<List<Player>> PlayerByGameId(int id)
        {
            var playerList = await getWebApi($"game/GetPlayersByGameId/{id}");
            var result = JsonConvert.DeserializeObject<List<Player>>(playerList);
            return result;
        }

        public async Task<string> GetSingleUserId(string userName)
        {
            var result = await getWebApi($"Player/GetPlayerIdByUsername/{userName}");
            return result;
        }

        public async void CreateNewPlayer(User newPlayer)
        {
            await getWebApi($"player/createNewPlayer{newPlayer}");
        }

        public async void AddPlayerToOwner(int gameId, string userId)
        {
            await getWebApi($"game/makeowner/{userId}/{gameId}");
        }

        public async void RemoveOwner(int gameId, string userId)
        {
            await getWebApi($"game/MakePeasant/{userId}/{gameId}");
        }

    }
}
