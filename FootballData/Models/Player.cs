using System.ComponentModel.DataAnnotations;


namespace FootballData.Models
{
    public class Player
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        public List<Team> Teams { get; set; }
        public List<PlayersInTeams> PlayersInTeams { get; set; }
    }
}
