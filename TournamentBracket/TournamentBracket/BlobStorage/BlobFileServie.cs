using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace TournamentBracket.BlobStorage
{

    /** public class that will connect to the Azure blob storage and all
     * 
     * Class Stucture derived from Codewrinkles from YouTube
     */
    public class BlobFileServie
    {

        private readonly string _storageAccount = "tournamentbracketimages";
        private readonly string _key = new BlobKeyConfig().get_Key();
        private readonly string _connectionString = "private readonly string";
        private readonly BlobContainerClient _filesContainter;

        public BlobFileServie()
        {
            var credential = new StorageSharedKeyCredential(_storageAccount, _key);
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
            _filesContainter = blobServiceClient.GetBlobContainerClient("participant-images");
        }

        /**
         * 
         * 
         */
        public async Task<BlobResponseData> UploadAsync(IFormFile file)
        {
            BlobResponseData response = new BlobResponseData();

            BlobClient client = _filesContainter.GetBlobClient(file.Name);

            //Try-Catch for Exception thrown during file upload
            try
            {
                //Open a stream to acces the data of the IFormFile
                await using Stream data = file.OpenReadStream();
                //Upload the image to the blob
                await client.UploadAsync(data);

                //If no error, successful repsonse data will be filled out
                response.Status = $"File {file.FileName} Uploaded Successfully";
                response.StatusID = 200;
                response.Error = false;
            }
            //Catch exception thrown. {Should only be RequestFailedException?}
            catch (Exception ex)
            {
                response.Status = ex.Message;
                response.StatusID = 400; //Research what status error code series to have for this
                response.Error = true;
            }

            //Return the response
            return response;
        }

        /**
         * 
         * 
         */
        public async Task<BlobResponseData> DeleteAsync(string blobFilename)
        {
            BlobResponseData response = new BlobResponseData();

            BlobClient file = _filesContainter.GetBlobClient(blobFilename);

            //Try-Catch for deleting file
            try
            {
                //Attempt to delete
                await file.DeleteAsync();

                //If no exception, response data is successful
                response.Status = $"File {blobFilename} Deleted Successfully";
                response.StatusID = 200;
                response.Error = false;
            }
            //RequestFailedException is thrown
            catch (RequestFailedException ex)
            {
                //Reponse data reflects error
                response.Status = ex.Message;
                response.StatusID = 400; //Research what status error code series to have for this
                response.Error = true;
            }

            //Return response data
            return response;
        }
    }
}
