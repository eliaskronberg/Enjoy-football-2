using EnjoyFootball.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyFootball.Models
{
    public class DataManager
    {
        FootballContext context;

        const string connectionString = "Data Source=ACADEMY030;Initial Catalog=EnjoyFootball2;Integrated Security=True;Pooling=False";

        List<Game> gamesList = new List<Game>();
        public static DataManager dataManager = new DataManager();
        List<Player> playerList = new List<Player>();
        List<Team> teamList = new List<Team>();
        List<Field> fieldList= new List<Field>();


        public List<Player> GetAllPlayers()
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Adding players to PlayerList
            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandText = "SELECT ID, FirstName, LastName, Age, Skill, City, Nickname FROM dbo.Players";
                SqlDataReader myreader = cmd.ExecuteReader();

                while (myreader.Read())
                {
                    Player newPlayer = new Player();
                    newPlayer.FirstName = (string)myreader["FirstName"];
                    newPlayer.LastName = (string)myreader["LastName"];
                    newPlayer.Nickname = (string)myreader["NickName"];
                    newPlayer.Skill = (int)myreader["Skill"];
                    newPlayer.Age = (int)myreader["Age"];
                    newPlayer.City = (string)myreader["City"];
                    newPlayer.Id = (string)myreader["ID"];

                    playerList.Add(newPlayer);
                }
                return playerList;
            }
            catch (Exception e)
            {
                throw e;
            }

            finally
            {
                myConnection.Close();
            }
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

        //Create a new Game
        public bool CreateGame(CreateGameVM createGameVm, string userId)
        {
            var fieldId = ListFields().Where(o => o.Name == createGameVm.Field).Select(o => o.Id).SingleOrDefault();

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Create_Game";
                cmd.Parameters.Add(new SqlParameter("@Description", createGameVm.Description));
                cmd.Parameters.Add(new SqlParameter("@Field", fieldId));
                cmd.Parameters.Add(new SqlParameter("@IsActive", createGameVm.IsActive));
                cmd.Parameters.Add(new SqlParameter("@IsPublic", createGameVm.IsPublic));
                cmd.Parameters.Add(new SqlParameter("@MaxSlots", createGameVm.MaxSlots));
                cmd.Parameters.Add(new SqlParameter("@Owner", userId));
                cmd.Parameters.Add(new SqlParameter("@StartTime", createGameVm.StartTime));


                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                myConnection.Close();
            }

            return true;

        }

        //public List<Match> ListGameOverviews()
        //{

        //}

        public List<Field> ListFields()
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Adding Games to GamesList
            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandText = "SELECT * FROM dbo.Fields";
                SqlDataReader myreader = cmd.ExecuteReader();

                while (myreader.Read())
                {
                    Field newField = new Field();
                    newField.Id = (int)myreader["ID"];
                    newField.Capacity = (int)myreader["Capacity"];
                    newField.Condition = (double)myreader["Condition"];
                    newField.Coordinates = (string)myreader["Coordinates"];
                    newField.Description = (string)myreader["Description"];
                    newField.Lighting = (bool)myreader["Lighting"];
                    newField.Name = (string)myreader["Name"];
                    newField.Turf = (string)myreader["Turf"];
                    newField.Votes = (int)myreader["Votes"];


                    fieldList.Add(newField);
                }
                return fieldList;
            }
            catch (Exception e)
            {
                throw e;
            }

            finally
            {
                myConnection.Close();
            }
        }

        public Field GetFieldById(int fieldId)
        {
            return ListFields()
                .Where(o => o.Id == fieldId).SingleOrDefault();
        }

        public string[] GetAllFieldNames()
        {
            return ListFields().Select(o=>o.Name).ToArray();
        }

        public string GetUserId(string Name)
        {

            var user = context.Users.Where(o => o.UserName == Name).SingleOrDefault();
            return user.Id;
        }

        public List<Game> GetAllGames()
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Adding Games to GamesList
            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandText = "SELECT * from dbo.Games";
                SqlDataReader myreader = cmd.ExecuteReader();

                while (myreader.Read())
                {
                    Game newGameOverview = new Game();
                    newGameOverview.IsActive = (bool)myreader["IsActive"];
                    newGameOverview.Id = (int)myreader["ID"];
                    newGameOverview.Field = (int)myreader["Field"];
                    newGameOverview.IsPublic = (bool)myreader["IsPublic"];
                    newGameOverview.MaxSlots = (int)myreader["MaxSlots"];
                    newGameOverview.StartTime = (DateTime)myreader["StartTime"];
                    newGameOverview.Owner = (string)myreader["Owner"];
                    newGameOverview.Description = (string)myreader["Description"];

                    gamesList.Add(newGameOverview);
                }
                return gamesList;
            }
            catch (Exception e)
            {
                throw e;
            }

            finally
            {
                myConnection.Close();
            }
        }
        public GameDetails getGameByID(int id)
        {
            var game = GetAllGames()
                .Where(o => o.Id == id)
                .Select(o => new GameDetails
                {
                    Id = o.Id,
                    Field = GetFieldById(o.Field).Name,
                    Owner = o.Owner,
                    OpenSlots = o.MaxSlots,
                    StartTime = o.StartTime,
                    PlayerList = PlayerByGameId(id)
                })
                .SingleOrDefault();

            return game;
        }
        public void AddPlayerToGame(string playerNameToAdd, int id)
        {
            //var player=GetAllPlayers().Where(o => o.Nickname == playerNameToAdd).SingleOrDefault();
            var playerId = GetSingleUserId(playerNameToAdd);

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Add_Player_ToGame";
                cmd.Parameters.Add(new SqlParameter("@playerId", playerId));
                cmd.Parameters.Add(new SqlParameter("@gameId", id));
                

                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                myConnection.Close();
            }

        }

        public void RemovePlayerFromGame(int id, string playerNameToRemove)
        {

            var player = GetAllPlayers().Where(o => o.Nickname == playerNameToRemove).SingleOrDefault();

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Remove_Player_FromGame";
                cmd.Parameters.Add(new SqlParameter("@playerId", player.Id));
                cmd.Parameters.Add(new SqlParameter("@gameId", id));


                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                myConnection.Close();
            }
        }

        public List<Player> PlayerByGameId(int gameId)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Players to specific game
            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandText = "SELECT dbo.Games.Id as gameID, dbo.Players.Id, dbo.Players.Age, dbo.Players.City, dbo.Players.FirstName, dbo.Players.LastName, dbo.Players.Nickname, dbo.Players.Skill, dbo.Games.Field FROM dbo.MidGamePlayer INNER JOIN dbo.Games ON dbo.MidGamePlayer.GameID = dbo.Games.Id INNER JOIN dbo.Players ON dbo.MidGamePlayer.PlayerID = dbo.Players.Id";
                SqlDataReader myreader = cmd.ExecuteReader();

                List<Player> AllPlayersInGame = new List<Player>();
                while (myreader.Read())
                {
                    if ((int)myreader["gameID"] == gameId)
                    {
                        Player newPlayer = new Player();
                        
                        newPlayer.FirstName =  DBNull.Value.Equals(myreader["FirstName"]) ? "": (string)myreader["FirstName"];
                        newPlayer.LastName = DBNull.Value.Equals(myreader["LastName"]) ? "": (string)myreader["LastName"];
                        newPlayer.Nickname = DBNull.Value.Equals(myreader["NickName"]) ? "": (string)myreader["NickName"];
                        newPlayer.Skill = DBNull.Value.Equals(myreader["Skill"])?0: (int)myreader["Skill"];
                        newPlayer.Age = DBNull.Value.Equals(myreader["Age"]) ? 0: (int)myreader["Age"];
                        newPlayer.City = DBNull.Value.Equals(myreader["City"]) ? "" : (string)myreader["City"];
                        newPlayer.Id = DBNull.Value.Equals(myreader["Id"]) ? "" : (string)myreader["Id"];

                    AllPlayersInGame.Add(newPlayer);
                    }
                }
                return AllPlayersInGame;
            }
            catch (Exception e)
            {
                throw e;
            }

            finally
            {
                myConnection.Close();
            }
        }

        public string GetSingleUserId(string userName)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Adding Games to GamesList
            try
            {
                string myId = "";
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar);
                cmd.Parameters["@UserName"].Value = userName;
                cmd.CommandText = "SELECT Id FROM dbo.AspNetUsers Where Username=@UserName";
                SqlDataReader myreader = cmd.ExecuteReader();

                while (myreader.Read())
                {
                   myId = (string)myreader["Id"];
                 
                }
                return myId;
            }
            catch (Exception e)
            {
                throw e;
            }

            finally
            {
                myConnection.Close();
            }
        }

        public void CreateNewPlayer(Player newPlayer)
        {

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Add_New_Player";
                cmd.Parameters.Add(new SqlParameter("@Age", newPlayer.Age));
                cmd.Parameters.Add(new SqlParameter("@Nickname", newPlayer.Nickname));
                cmd.Parameters.Add(new SqlParameter("@Id", newPlayer.Id));


                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                myConnection.Close();
            }
        }
    }
}
