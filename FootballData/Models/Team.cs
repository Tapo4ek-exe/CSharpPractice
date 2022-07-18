using System.ComponentModel.DataAnnotations;


namespace FootballData.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Player> Players { get; set; }
        public List<PlayersInTeams> PlayersInTeams { get; set; }
    }
}
