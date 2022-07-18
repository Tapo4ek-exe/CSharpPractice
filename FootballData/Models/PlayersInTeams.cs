

namespace FootballData.Models
{
    public class PlayersInTeams
    {
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public double Salary { get; set; }
    }
}
