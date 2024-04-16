namespace TournamentBracket.Models
{
    public class Tournament
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public bool IncludeLosersBracket { get; set; }

        public List<string> Participants { get; set; }

    }
}
