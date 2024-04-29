using TournamentBracket.Enums;

namespace TournamentBracket.Models
{
    public class Tournament
    {
        //Primary Key
        public int Id { get; set; }

        //Foreign Key to who made the Bracket
        public string UserCreatedID { get; set; }

        //Tournament Name
        public required string Name { get; set; }
        
        public BracketOptions BracketOptions {  get; set; } 

        //Navigation Property to represent the collection of participants in the tournament
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();

        //public ICollection<Match> Matches{ get; set; } = new List<Match>();
        
        //Description of Tournament
        //public required string Description { get; set; }
        //The date that the tournament will be
        //public DateTime TournamentDate { get; set; }

    }
}
