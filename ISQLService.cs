using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TestTaskGame
{
    public interface ISQLService
    {       
        bool Register(Player player);
        bool Pay(string playerName, string itemName);
        Player GetPlayer(Player player);
        List<Gun> GetGuns();
        List<Character> GetCharacters();        
    }

    public class SQLService : ISQLService
    {
        private readonly AppSettings _appSettings;
        private string _connString;

        public SQLService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _connString = _appSettings.DBConnectionString;
        }

        private DataTable GetData( string query)
        {
            SqlConnection connection = new SqlConnection(_connString);

            DataTable dataTable = new DataTable();

            try
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = query;

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                
                adapter.Fill(dataTable);

            }
            catch (System.Exception )
            {
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return dataTable;
        }

        private void AddData( string query)
        {
            SqlConnection connection = new SqlConnection(_connString);
            try
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = query;
                command.ExecuteNonQuery();                

            }
            catch (System.Exception)
            {

            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

        }

        public List<Character> GetCharacters()
        {
            throw new System.NotImplementedException();
        }

        public List<Gun> GetGuns()
        {
            throw new System.NotImplementedException();
        }

        public Player GetPlayer(Player player)
        {
            string query;

            if (player.Password is null)
            {
                 query = "select * from players where player_name ='" + player.Name +"'";
            }
            else
            {
                 query = "select * from players where player_name ='" + player.Name + "' and player_password = '" +player.Password+ "'";
            }
            
            DataTable playerTable = GetData(query);

            if (playerTable.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                Player dbPlayer = new Player()
                {
                    Name = playerTable.Rows[0].Field<string>("player_name"),
                    Coins = playerTable.Rows[0].Field<int>("player_coins"),
                    Level = playerTable.Rows[0].Field<int>("player_lvl"),
                    Password = playerTable.Rows[0].Field<string>("player_password"),
                };

                return dbPlayer;
            }

        }

        public bool Pay(string playerName, string itemName)
        {
            throw new System.NotImplementedException();
        }

        public bool Register(Player player)
        {
            Player pl1 = GetPlayer(player);
            if ( pl1!= null)
            {
                return false;
            }
            else
            {
                string query = "insert into players (player_name, player_password) values ('" + player.Name + "','" + player.Password + "')";
                AddData(query);
                return true;
            }
        }
    }
}