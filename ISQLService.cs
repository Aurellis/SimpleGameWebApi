using Microsoft.Extensions.Options;

using System;
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
        List<Gun> GetGuns(string playerName);
        List<Character> GetCharacters(string playerName);        
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
            catch (System.Exception ex)
            {
                string mes = ex.Message;
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

        public List<Character> GetCharacters(string playerName)
        {
            string query = "select *, case when exists(select char_name from players_characters where player_name = '" + playerName + "' and char_name = characters.char_name) then 1 else 0 end as char_unlocked from characters ";

            DataTable charTable = GetData(query);

            List<Character> allCharacters = new List<Character>();

            for (int i = 0; i < charTable.Rows.Count; i++)
            {
                allCharacters.Add(new Character()
                {   Name = charTable.Rows[i].Field<string>("char_name"),
                     Price = charTable.Rows[i].Field<int>("char_price"),
                     Unlocked = Convert.ToBoolean(charTable.Rows[i].Field<int>("char_unlocked"))
                });
            }

            foreach (var character in allCharacters)
            {
                query = " exec GetGunsCharacter '" + playerName+ "' ,'"+character.Name+"' ";

                DataTable gunTable = GetData(query);

                character.AvailableGuns = new List<Gun>();

                for (int i = 0; i < gunTable.Rows.Count; i++)
                {                    
                    character.AvailableGuns.Add(new Gun()
                    {
                        Name = gunTable.Rows[i].Field<string>("gun_name"),
                        Damage = gunTable.Rows[i].Field<int>("gun_damage"),
                        MaxLevel = gunTable.Rows[i].Field<int>("gun_lvl"),
                        Price = gunTable.Rows[i].Field<int>("gun_price"),
                        NumBullets = gunTable.Rows[i].Field<int>("gun_num_bullets"),
                        RateOfFire = gunTable.Rows[i].Field<int>("gun_rate_of_fire"),
                        RechargeRate = gunTable.Rows[i].Field<int>("gun_recharge_rate"),
                        Unlocked = Convert.ToBoolean(gunTable.Rows[i].Field<int>("gun_unlocked"))
                    });
                }
                
            }

            return allCharacters;

        }

        public List<Gun> GetGuns(string playerName)
        {
            string query = "exec GetGuns '"+ playerName + "'";

            DataTable gunTable = GetData(query);

            List<Gun> allGuns = new List<Gun>();

            for (int i = 0; i < gunTable.Rows.Count; i++)
            {
                allGuns.Add(new Gun() {Name = gunTable.Rows[i].Field<string>("gun_name"),
                                                   Damage = gunTable.Rows[i].Field<int>("gun_damage"),
                                                   MaxLevel = gunTable.Rows[i].Field<int>("gun_lvl"),
                                                   Price = gunTable.Rows[i].Field<int>("gun_price"),
                                                   NumBullets = gunTable.Rows[i].Field<int>("gun_num_bullets"),
                                                   RateOfFire = gunTable.Rows[i].Field<int>("gun_rate_of_fire"),
                                                   RechargeRate = gunTable.Rows[i].Field<int>("gun_recharge_rate"),
                                                   Unlocked = Convert.ToBoolean(gunTable.Rows[i].Field<int>("gun_unlocked"))
                                                  } );
            }

            return allGuns;
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