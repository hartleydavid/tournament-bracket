namespace TournamentBracket.Models
{
    public class Tournament
    {
        //Primary Key
        public int Id { get; set; }

        //Tournament Name
        public required string Name { get; set; }
        //Description of Tournament
        public required string Description { get; set; }
        
        //The date that the tournament will be
        public DateTime TournamentDate { get; set; }

        public bool IncludeLosersBracket { get; set; }

        //Navigation Property to represent the collection of participants in the tournament
        public ICollection<Participant> Participants { get; set; }

    }
}
