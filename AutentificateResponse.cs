namespace TestTaskGame
{
    public class AutentificateResponse
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Coins { get; set; }
        public string Token { get; set; }

        public AutentificateResponse(Player player, string token)
        {
            Name = player.Name;
            Level = player.Level;
            Coins = player.Coins;
            Token = token;
        }
    }
}