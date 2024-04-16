namespace TournamentBracket.Models
{
    public class Participant
    {

        public int Id { get; set; }
        public int TournamentId { get; set; }
        public string Name { get; set; }
           
        public string ImageFileName { get; set; }
        //Image uploaded by user
        //Team strucuture?
    }
}
