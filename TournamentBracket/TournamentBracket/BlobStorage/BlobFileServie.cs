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


        /// <summary>
        /// Constructor that takes the username, tournament name (blob container), and boolean value on if its creating a bracket
        /// The constructor will connect to the account via parameters and create a blob container
        /// if we are trying to create one, and then connect to the container
        /// </summary>
        /// <param name="UserName">The username of the user connecting to blob service</param>
        /// <param name="TournamentName">The name of the tournament</param>
        /// <param name="isCreating">Boolean on if the user is creating a bracket or just connecting to it</param>
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

        /// <summary>
        /// Getter that will retrieve and return the name of the blob container
        /// </summary>
        /// <returns>The name of the blob container that is connected to from the constructor</returns>
        public string GetBlobName()
        {
            return _filesContainter.Name;
        }

        /// <summary>
        /// Getter that returns the name of the storage account of the blob container
        /// Is the "username" of the profile (@... less email address)
        /// </summary>
        /// <returns>String that is the name of the storage account of the blob</returns>
        public string GetStorageAccountName()
        {
            return _storageAccount;
        }

        /// <summary>
        /// Function that uploads a file to the files container of the blob storage 
        /// </summary>
        /// <param name="file"> The IFormFile object of the file we are uploading</param>
        /// <returns>Response data on the file upload</returns>
        public async Task<BlobResponseData> UploadAsync(IFormFile file)
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

        /// <summary>
        /// Function will delete the blob container that we are connected to.
        /// </summary>
        /// <returns>Response data of the delete command</returns>
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
