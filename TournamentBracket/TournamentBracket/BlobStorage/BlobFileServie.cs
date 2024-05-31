using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Identity.Client;

namespace TournamentBracket.BlobStorage
{

    /** public class that will connect to the Azure blob storage and all
     * 
     * Class Stucture derived from Codewrinkles from YouTube
     */
    public class BlobFileServie
    {

        private readonly string _storageAccount = "tournamentbracketimages";
        private readonly string _key = new BlobKeyConfig().Get_Key();
        
        //Reference to the Blob Container with the image files
        private readonly BlobContainerClient _filesContainter;



        public BlobFileServie(string UserName, string TournamentName, bool isCreating)
        {
            //Check if the username provided is valid (Not null)
            ArgumentNullException.ThrowIfNull(UserName);

            var credential = new StorageSharedKeyCredential(_storageAccount, _key);
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
            string containterName = UserName.ToLower() + "-" + TournamentName.ToLower().Replace(' ','-');
            //Create a blob
            if ( isCreating )
            {
                //Is this necessary having a temp container?
                BlobContainerClient _tempFilesContainter = blobServiceClient.CreateBlobContainer(containterName);
                //Set the Access type of the blob to be public so images can be displayed
                _tempFilesContainter.SetAccessPolicy(PublicAccessType.Blob, default, default, default);
            }

            //Connect to the blob container that the images will be uploaded to
            _filesContainter = blobServiceClient.GetBlobContainerClient(containterName);
           
        }


        public string GetBlobName()
        {
            return _filesContainter.Name;
        }

        public string GetStorageAccountName()
        {
            return _storageAccount;
        }


        public async Task<BlobResponseData> UploadAsync(IFormFile file /*, string TournamentName*/)
        {
            BlobResponseData response = new BlobResponseData();

            BlobClient client = _filesContainter.GetBlobClient(file.FileName);

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
        public async Task<BlobResponseData> DeleteAsync()
        {
            BlobResponseData response = new BlobResponseData();

            //Try-Catch for deleting blob
            try
            {
                //Attempt to delete the blob
                await _filesContainter.DeleteAsync();

                //If no exception, response data is successful
                response.Status = $"Blob Deleted Successfully";
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
