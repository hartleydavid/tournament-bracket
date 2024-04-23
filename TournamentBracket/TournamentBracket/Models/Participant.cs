namespace TournamentBracket.Models
{
    public class Participant
    {
        //Primary Key
        public int Id { get; set; }

        //Foreign Key properties to represent the tournament that the participant is in
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        //Participant Properties
        public string Name { get; set; }
       
        public string ImageURL { get; set; }

    }
}
