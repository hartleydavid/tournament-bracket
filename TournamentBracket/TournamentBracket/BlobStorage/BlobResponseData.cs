namespace TournamentBracket.BlobStorage
{
    /** Class representing the response from the blob storage when handling 
     * file transfers to blob storage
     *
     * Class Stucture derived from Codewrinkles from YouTube
     */
    public class BlobResponseData
    {
        //String repsentation of what happened in the file transfer
        public string? Status { get; set; }

        //Int value of what error occured (200,400,500 series error codes)
        public int StatusID { get; set; }


        //Boolean Value displayling if there was an error or not
        public bool Error { get; set; }


    }
}
