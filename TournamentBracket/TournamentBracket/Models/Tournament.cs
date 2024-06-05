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
        
        //The option of the bracket to display
        public BracketOptions BracketOptions {  get; set; } 

        //Navigation Property to represent the collection of participants in the tournament
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();

        //Int value of the number of participants in the tournament
        public int NumberOfParticipants {  get; set; }


    }
}
