namespace TournamentBracket.Models
{
    public class Match
    {
        //Primary Key
        public int Id { get; set; }

        //Foreign Key to Tournament 
        public int TournamentId { get; set; }

        //Players in the match
        //Participant Instead of string
        public string PlayerOne { get; set; }
        public string PlayerTwo { get; set; }

        //What round number the match is
        public int RoundNumber { get; set; }

        //The winner of this round
        //0 - No Result, 1 - Player One, 2 - Player Two
        public int Winner { get; set; }
    }
}
