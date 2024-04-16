namespace TournamentBracket.Models
{
    public class Tournament
    {

        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public bool IncludeLosersBracket { get; set; }

        //public required List<Participant> Participants { get; set; }

    }
}
