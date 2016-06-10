using EnjoySportsAPI2.Models;
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

        const string connectionString = "Data Source=ACADEMY030;Initial Catalog=EnjoyFootball2;Integrated Security=True;Pooling=False";

        List<Game> gamesList = new List<Game>();
        public static DataManager dataManager = new DataManager();
        List<Player> playerList = new List<Player>();
        List<Team> teamList = new List<Team>();
        List<Field> fieldList = new List<Field>();
        List<Tournament> tournamentList = new List<Tournament>();

        public List<Player> getTeamPlayersByTeamId(int teamId)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Adding Games to GamesList
            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.Parameters.Add("@teamId", SqlDbType.Int);
                cmd.Parameters["@teamId"].Value = teamId;
                cmd.CommandText = "SELECT * FROM dbo.MidTeamPlayer Where TeamId=@teamId";
                SqlDataReader myreader = cmd.ExecuteReader();

                Team theTeam = new Team();
                List<string> playerIds = new List<string>();
         
                while (myreader.Read())
                {
                    playerIds.Add((string)myreader["PlayerId"]);
                }

                foreach (var playerId in playerIds)
                {
                    theTeam.Players.Add(GetPlayerInfo(playerId));
                }
                return theTeam.Players;
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

        public Team GetTeamById(int teamId)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Adding Games to GamesList
            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.Parameters.Add("@teamId", SqlDbType.Int);
                cmd.Parameters["@teamId"].Value = teamId;
                cmd.CommandText = "SELECT * FROM dbo.Teams Where Id=@teamId";
                SqlDataReader myreader = cmd.ExecuteReader();

                Team theTeam = new Team();

                while (myreader.Read())
                {
                    theTeam.Name = (string)myreader["Name"];
                    theTeam.Id = (int)myreader["Id"];
                }
                return theTeam;
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
                    //newPlayer.FirstName = (string)myreader["FirstName"];
                    //newPlayer.LastName = (string)myreader["LastName"];
                    //newPlayer.Nickname = (string)myreader["NickName"];
                    //newPlayer.Skill = (int)myreader["Skill"];
                    //newPlayer.Age = (int)myreader["Age"];
                    //newPlayer.City = (string)myreader["City"];
                    //newPlayer.Id = (string)myreader["ID"];

                    newPlayer.FirstName = DBNull.Value.Equals(myreader["FirstName"]) ? "" : (string)myreader["FirstName"];
                    newPlayer.LastName = DBNull.Value.Equals(myreader["LastName"]) ? "" : (string)myreader["LastName"];
                    newPlayer.Nickname = DBNull.Value.Equals(myreader["NickName"]) ? "" : (string)myreader["NickName"];
                    newPlayer.Skill = DBNull.Value.Equals(myreader["Skill"]) ? 0 : (int)myreader["Skill"];
                    newPlayer.Age = DBNull.Value.Equals(myreader["Age"]) ? 0 : (int)myreader["Age"];
                    newPlayer.City = DBNull.Value.Equals(myreader["City"]) ? "" : (string)myreader["City"];
                    newPlayer.Id = DBNull.Value.Equals(myreader["Id"]) ? "" : (string)myreader["Id"];

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

        internal List<Tournament> getTournamentByTeamId(int teamId)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Adding Games to GamesList
            try
            {
                List<int> tournamentIds = new List<int>();
                List<Tournament> theTournaments = new List<Tournament>();
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.Parameters.Add("@teamId", SqlDbType.NVarChar);
                cmd.Parameters["@teamId"].Value = teamId;
                cmd.CommandText = "SELECT TournamentId FROM dbo.MidTournamentTeam Where TeamId=@teamId";
                SqlDataReader myreader = cmd.ExecuteReader();

                while (myreader.Read())
                {
                    tournamentIds.Add((int)myreader["TournamentId"]);

                }
                foreach (var item in tournamentIds)
                {
                    theTournaments.Add(GetTournamentById(item));
                }

                return theTournaments;
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

        public void SetTeamsInTourGame(int matchId, Team team)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Set_Teams_TournamentGame";
                cmd.Parameters.Add(new SqlParameter("@TeamOne", team.Id));
                cmd.Parameters.Add(new SqlParameter("@gameId", matchId));

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

        public List<int> getGamesByPlayerId(string id)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Adding Games to GamesList
            try
            {
                List<int> gameIds = new List<int>();
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.Parameters.Add("@playerId", SqlDbType.NVarChar);
                cmd.Parameters["@playerId"].Value = id;
                cmd.CommandText = "SELECT GameId FROM dbo.MidGamePlayer Where PlayerId=@playerId";
                SqlDataReader myreader = cmd.ExecuteReader();

                while (myreader.Read())
                {
                    gameIds.Add((int)myreader["GameId"]);

                }
                return gameIds;
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

        public Player GetPlayerInfo(string playerId)
        {
            var playerToDisplay = GetAllPlayers()
                .Where(o => o.Id == playerId)
                .Select(o => new Player
                {
                    Id = o.Id,
                    Age = o.Age,
                    City = o.City,
                    FirstName = o.FirstName,
                    LastName = o.LastName,
                    Nickname = o.Nickname,
                    Skill = o.Skill
                })
                .SingleOrDefault();
            return playerToDisplay;
        }

        // Only creates the field if there's no other field with the same name
        public bool CreateField(Field viewModel)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            try
            {
                myConnection.Open();
                double dub = 0.0;
                int votes = 0;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Create_Field";
                cmd.Parameters.Add(new SqlParameter("@Capacity", viewModel.Capacity));
                cmd.Parameters.Add(new SqlParameter("@Condition", dub));
                cmd.Parameters.Add(new SqlParameter("@Coordinates", string.IsNullOrWhiteSpace(viewModel.Coordinates) ? "" : viewModel.Coordinates));
                cmd.Parameters.Add(new SqlParameter("@Description", string.IsNullOrWhiteSpace(viewModel.Description) ? "" : viewModel.Description));
                cmd.Parameters.Add(new SqlParameter("@Lighting", viewModel.Lighting));
                cmd.Parameters.Add(new SqlParameter("@Name", viewModel.Name));
                cmd.Parameters.Add(new SqlParameter("@Turf", viewModel.Turf));
                cmd.Parameters.Add(new SqlParameter("@Votes", votes));
                cmd.Parameters.Add(new SqlParameter("@City", viewModel.City));


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

        internal void ToggleActive(int gameId, bool isActive)
        {
            var game = getGameByID(gameId);

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "EditActive";
                cmd.Parameters.Add(new SqlParameter("@paramIsActive", isActive));
                cmd.Parameters.Add(new SqlParameter("@paramGameId", gameId));

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

        internal void TogglePublic(int gameId, bool isPublic)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "EditPublic";
                cmd.Parameters.Add(new SqlParameter("@paramIsPublic", isPublic));
                cmd.Parameters.Add(new SqlParameter("@paramGameId", gameId));
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

        //Create a new Game
        public int CreateGame(Game gameModel, string userId)
        {

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Create_Game";
                cmd.Parameters.Add(new SqlParameter("@Description", gameModel.Description));
                cmd.Parameters.Add(new SqlParameter("@Field", gameModel.Field));
                cmd.Parameters.Add(new SqlParameter("@IsActive", gameModel.IsActive));
                cmd.Parameters.Add(new SqlParameter("@IsPublic", gameModel.IsPublic));
                cmd.Parameters.Add(new SqlParameter("@MaxSlots", gameModel.MaxSlots));
                cmd.Parameters.Add(new SqlParameter("@Owner", userId));
                cmd.Parameters.Add(new SqlParameter("@StartTime", gameModel.StartTime));


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

            return GetAllGames().Last().Id;

        }

        public int CreateTournament(string hostPlayerId, string tournamentName, Team hostPlayersTeam, string description, int tournamentsize)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Create_Tournament";
                cmd.Parameters.Add(new SqlParameter("@Name", tournamentName));
                cmd.Parameters.Add(new SqlParameter("@hostPlayerId", hostPlayerId));
                cmd.Parameters.Add(new SqlParameter("@description", description));
                cmd.Parameters.Add(new SqlParameter("@tournamentsize", tournamentsize));

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

            return GetAllTournaments().Last().Id;
        }

        public void UpdateGame(Game gameToChange)
        {
            
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Update_Game";
                cmd.Parameters.Add(new SqlParameter("@Description", string.IsNullOrWhiteSpace(gameToChange.Description) ? "" : gameToChange.Description));
                cmd.Parameters.Add(new SqlParameter("@Field", gameToChange.Field));

                cmd.Parameters.Add(new SqlParameter("@MaxSlots", gameToChange.MaxSlots > -1 ? gameToChange.MaxSlots : 0));
                //cmd.Parameters.Add(new SqlParameter("@Owner", gameToChange.Owner));
                cmd.Parameters.Add(new SqlParameter("@StartTime", gameToChange.StartTime));
                cmd.Parameters.Add(new SqlParameter("@gameId", gameToChange.Id));


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

        //public void UpdateTournamentGame(TournamentMatch gameToChange)
        //{

        //    SqlConnection myConnection = new SqlConnection();
        //    myConnection.ConnectionString = connectionString;

        //    try
        //    {
        //        myConnection.Open();

        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = myConnection;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "Update_TournamentGame";
        //        cmd.Parameters.Add(new SqlParameter("@IsPlayed", gameToChange.isPlayed));
        //        cmd.Parameters.Add(new SqlParameter("@IsTeamOneWinner", gameToChange.isTeamOneWinner));
        //        cmd.Parameters.Add(new SqlParameter("@gameId", gameToChange.Id > -1 ? gameToChange.Id : 0));

        //        cmd.ExecuteNonQuery();

        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        myConnection.Close();
        //    }

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
                    newField.City = (string)myreader["City"];

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
            return ListFields().Select(o => o.Name).ToArray();
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

        public List<Tournament> GetAllTournaments()
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Adding Games to GamesList
            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandText = "SELECT * from dbo.Tournaments";
                SqlDataReader myreader = cmd.ExecuteReader();

                while (myreader.Read())
                {
                    Tournament newTournament = new Tournament();
                    newTournament.hostPlayerID = (string)myreader["hostPlayerId"];
                    newTournament.Name = (string)myreader["Name"];
                    newTournament.Id = (int)myreader["Id"];
                    newTournament.TournamentSize = (int)myreader["Tournamentsize"];

                    tournamentList.Add(newTournament);
                }
                return tournamentList;
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
        public Game getGameByID(int id)
        {
            var game = GetAllGames()
                .Where(o => o.Id == id)
                .Select(o => new Game
                {
                    Description = o.Description,
                    Id = o.Id,
                    IsActive = o.IsActive,
                    IsPublic = o.IsPublic,
                    Field = o.Field,
                    Owner = o.Owner,
                    MaxSlots = o.MaxSlots,
                    StartTime = o.StartTime
                    //PlayerList = PlayerByGameId(id)
                })
                .SingleOrDefault();

            return game;
        }
        public void AddPlayerToGame(string playerNameToAdd, int id)
        {

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Add_Player_ToGame";
                cmd.Parameters.Add(new SqlParameter("@playerId", playerNameToAdd));
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

        public void AddTeamToTournament(int teamId, int tournamentId)
        {

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Add_Team_To_Tournament";
                cmd.Parameters.Add(new SqlParameter("@tournamentId", tournamentId));
                cmd.Parameters.Add(new SqlParameter("@teamId", teamId));


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

        public void RemovePlayerFromGame(int id, string playerIdToRemove)
        {

            var player = GetAllPlayers().Where(o => o.Id == playerIdToRemove).SingleOrDefault();

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

                        newPlayer.FirstName = DBNull.Value.Equals(myreader["FirstName"]) ? "" : (string)myreader["FirstName"];
                        newPlayer.LastName = DBNull.Value.Equals(myreader["LastName"]) ? "" : (string)myreader["LastName"];
                        newPlayer.Nickname = DBNull.Value.Equals(myreader["NickName"]) ? "" : (string)myreader["NickName"];
                        newPlayer.Skill = DBNull.Value.Equals(myreader["Skill"]) ? 0 : (int)myreader["Skill"];
                        newPlayer.Age = DBNull.Value.Equals(myreader["Age"]) ? 0 : (int)myreader["Age"];
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

        public void CreateNewPlayer(User user)
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
                cmd.Parameters.Add(new SqlParameter("@Age", user.Age));
                cmd.Parameters.Add(new SqlParameter("@Nickname", user.Nickname));
                cmd.Parameters.Add(new SqlParameter("@Id", user.Id));


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

        public void AddPlayerToOwner(int gameId, string userId)
        {
            //var gameId = getGameByID(utr.GameId).Id;

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            string myId = "";
            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.Parameters.Add("@gameId", SqlDbType.Int);
                cmd.Parameters["@gameId"].Value = gameId;
                cmd.CommandText = "SELECT Owner FROM dbo.Games Where Id=@gameId";
                SqlDataReader myreader = cmd.ExecuteReader();

                while (myreader.Read())
                {
                    myId = (string)myreader["Owner"];

                }

                myId += ";" + userId;



            }
            catch (Exception e)
            {
                throw e;
            }

            finally
            {
                myConnection.Close();

            }
            EditOwnerInjection(myId, gameId);

        }

        public void EditOwnerInjection(string myId, int gameId)
        {
            SqlConnection myConnection1 = new SqlConnection();
            myConnection1.ConnectionString = connectionString;

            try
            {
                myConnection1.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection1;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "EditOwner";
                cmd.Parameters.Add(new SqlParameter("@paramOwner", myId));
                cmd.Parameters.Add(new SqlParameter("@paramGameId", gameId));

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                myConnection1.Close();
            }
        }

        public void RemoveOwner(int gameId, string userId)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            string myId = "";
            string tempOwners = "";
            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.Parameters.Add("@gameId", SqlDbType.Int);
                cmd.Parameters["@gameId"].Value = gameId;
                cmd.CommandText = "SELECT Owner FROM dbo.Games Where Id=@gameId";
                SqlDataReader myreader = cmd.ExecuteReader();

                while (myreader.Read())
                {
                    myId = (string)myreader["Owner"];

                }
                foreach (var item in myId.Split(';'))
                {
                    if (item != userId)
                        tempOwners += ";" + item;
                }




            }
            catch (Exception e)
            {
                throw e;
            }

            finally
            {
                myConnection.Close();

            }
            EditOwnerInjection(tempOwners, gameId);
        }

        public List<Team> GetTeamsByPlayerId(string playerId)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Adding Games to GamesList
            try
            {
                List<int> teamIds = new List<int>();
                List<Team> theTeams = new List<Team>();
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.Parameters.Add("@playerId", SqlDbType.NVarChar);
                cmd.Parameters["@playerId"].Value = playerId;
                cmd.CommandText = "SELECT TeamId FROM dbo.MidTeamPlayer Where PlayerId=@playerId";
                SqlDataReader myreader = cmd.ExecuteReader();

                while (myreader.Read())
                {
                    teamIds.Add((int)myreader["TeamId"]);

                }
                foreach (var item in teamIds)
                {
                    theTeams.Add(GetTeamById(item));
                }

                return theTeams;
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

        public Tournament GetTournamentById(int tournamentId)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Adding Games to GamesList
            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.Parameters.Add("@tournamentId", SqlDbType.Int);
                cmd.Parameters["@tournamentId"].Value = tournamentId;
                cmd.CommandText = "SELECT * FROM dbo.Tournaments Where Id=@tournamentId";
                SqlDataReader myreader = cmd.ExecuteReader();

                Tournament theTournament = new Tournament();

                while (myreader.Read())
                {
                    theTournament.Name = (string)myreader["Name"];
                    theTournament.Id = (int)myreader["Id"];
                    theTournament.hostPlayerID = (string)myreader["hostPlayerId"];
                    theTournament.Description = (string)myreader["Description"];
                }
                return theTournament;
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

        public List<Team> getTournamentTeamssByTournamentId(int tournamentId)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Adding Games to GamesList
            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.Parameters.Add("@tournamentId", SqlDbType.Int);
                cmd.Parameters["@tournamentId"].Value = tournamentId;
                cmd.CommandText = "SELECT * FROM dbo.MidTournamentTeam Where TournamentId=@tournamentId";
                SqlDataReader myreader = cmd.ExecuteReader();

                Tournament theTournament = new Tournament();
                List<int> TeamIds = new List<int>();

                while (myreader.Read())
                {
                    TeamIds.Add((int)myreader["TeamId"]);
                }

                foreach (var teamId in TeamIds)
                {
                    theTournament.Teams.Add(GetTeamById(teamId));
                }
                return theTournament.Teams;
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

        public List<Team> getAllTeams()
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Adding Games to GamesList
            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandText = "SELECT * from dbo.Teams";
                SqlDataReader myreader = cmd.ExecuteReader();

                while (myreader.Read())
                {
                    Team tempTeam = new Team();
                    tempTeam.Name= (string)myreader["Name"];
                    tempTeam.Id = (int)myreader["Id"];

                    teamList.Add(tempTeam);
                }
                return teamList;
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

        public void createTournamentGame(TournamentMatch gameModel)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Create_TournamentGame";
                cmd.Parameters.Add(new SqlParameter("@TeamOneId", gameModel.teamOne.Id));
                cmd.Parameters.Add(new SqlParameter("@TeamTwoId", gameModel.teamTwo.Id));
                cmd.Parameters.Add(new SqlParameter("@IsPlayed", false));
                cmd.Parameters.Add(new SqlParameter("@TournamentId", gameModel.tournamentId));
                cmd.Parameters.Add(new SqlParameter("@TournamentRound", gameModel.TournamentRound));

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

        public List<TournamentMatch> getTournamentGamesByTourId(int tournamentId)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Adding Games to GamesList
            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.Parameters.Add("@tournamentId", SqlDbType.Int);
                cmd.Parameters["@tournamentId"].Value = tournamentId;
                cmd.CommandText = "SELECT * FROM dbo.TournamentGames Where TournamentId=@tournamentId";
                SqlDataReader myreader = cmd.ExecuteReader();

                List<TournamentMatch> tournanmentMatches = new List<TournamentMatch>();

                while (myreader.Read())
                {
                    TournamentMatch tmpTournamentMatch = new TournamentMatch();
                    tmpTournamentMatch.teamOne = GetTeamById((int)myreader["TeamIdOne"]);
                    tmpTournamentMatch.teamTwo = GetTeamById((int)myreader["TeamIdTwo"]);
                    tmpTournamentMatch.TournamentRound = (int)myreader["TournamentRound"];
                    tmpTournamentMatch.isPlayed = (bool)myreader["isPlayed"];
                    tmpTournamentMatch.Id = (int)myreader["Id"];
                    if (tmpTournamentMatch.isPlayed)
                    {
                    tmpTournamentMatch.Result = (string)myreader["Result"];
                        tmpTournamentMatch.isTeamOneWinner = (bool)myreader["IsTeamOneWinner"];
                    }
                    tmpTournamentMatch.tournamentId = (int)myreader["TournamentId"];
                    tournanmentMatches.Add(tmpTournamentMatch);
                }

               
                return tournanmentMatches;
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

        public TournamentMatch getTournamentMatchByid(int id)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            //Adding Games to GamesList
            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.Parameters.Add("@matchId", SqlDbType.Int);
                cmd.Parameters["@matchId"].Value = id;
                cmd.CommandText = "SELECT * FROM dbo.TournamentGames Where Id=@matchId";
                SqlDataReader myreader = cmd.ExecuteReader();

                TournamentMatch theMatch = new TournamentMatch();

                while (myreader.Read())
                {
                    theMatch.teamOne = GetTeamById((int)myreader["TeamIdOne"]);
                    theMatch.teamTwo = GetTeamById((int)myreader["TeamIdTwo"]);
                    theMatch.TournamentRound = (int)myreader["TournamentRound"];
                    theMatch.isPlayed = (bool)myreader["isPlayed"];
                    //tmpTournamentMatch.isTeamOneWinner = (bool)myreader["IsTeamOneWinner"];
                    theMatch.tournamentId = (int)myreader["TournamentId"];
                    theMatch.Id = (int)myreader["Id"];
                }
                return theMatch;
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

        public void updateTourMatch(bool isGamePlayed, bool isTeamOneWinner, int tournamentGameId, string result)
        {

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            try
            {
                myConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = myConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Update_TournamentGame";
                cmd.Parameters.Add(new SqlParameter("@IsPlayed", isGamePlayed));
                cmd.Parameters.Add(new SqlParameter("@IsTeamOneWinner", isTeamOneWinner));
                cmd.Parameters.Add(new SqlParameter("@gameId", tournamentGameId));
                cmd.Parameters.Add(new SqlParameter("@result", result));

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
