﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TournamentBracket.BlobStorage;
using TournamentBracket.Data;
using TournamentBracket.Models;
using static System.Net.WebRequestMethods;

namespace TournamentBracket.Controllers
{
    public class TournamentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TournamentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method will format the users email to the username format
        /// (Removes everything from the @ to the length from the string
        /// </summary>
        /// <param name="username">The email of the user we are going to format</param>
        /// <returns>String of the username in the correct formatting</returns>
        /// <exception cref="ArgumentException">Exception if a non email is entered</exception>
        private static string FormatUserName(string username)
        {
            //Find the inex of the @ in the email
            int atIndex = username.IndexOf("@");

            //If the index was found, return the substring up until that index
            if (atIndex > 0)
            {
                return username[..atIndex].Replace('.','-');
            }
            else
            {
                throw new ArgumentException("Invalid Username provided");
            }
        }

        /// <summary>
        /// Method that returns the view to the Index page of tournamets.
        /// Page is filter by the user logged in so only their brackets are visible
        /// </summary>
        /// <returns>The filtered index view based on the user that is logged in</returns>
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //Filter the brackets to only include the brackets that the logged in user created
            var filteredBrackets = _context.TournamentBrackets.Where(x => x.UserCreatedID 
                                                                        == User.Identity.Name).ToListAsync();
            return View(await filteredBrackets);
            //return View(await _context.TournamentBrackets.ToListAsync());
        }

        /// <summary>
        /// Method will redirect the page to the details view that is selected
        /// CURRENTLY WILL ONLY GO TO DOUBLE ELIMINATION PAGE
        /// </summary>
        /// <param name="id">The id of the bracket</param>
        /// <returns>The view of the bracket</returns>
        // GET: Tournaments/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Inlcude the participants in the tournaments
            var tournament = await _context.TournamentBrackets
                .Include(t => t.Participants)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tournament == null)
            {
                return NotFound();
            }

            return View("DblBracket", tournament);
        }

        /// <summary>
        /// Redirects page to the create view
        /// </summary>
        /// <returns>The create view for tournament brackets</returns>
        // GET: Tournaments/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Method will create the tournament bracket into the database and also
        /// create all necessary components of the bracket
        /// </summary>
        /// <param name="tournament">The name of the tournament</param>
        /// <param name="Names">The list of names of participants entered</param>
        /// <param name="Images">The list of Image files entered</param>
        /// <returns>The view of the tournament created</returns>
        // POST: Tournaments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BracketOptions")] Tournament tournament,
            List<string> Names, List<IFormFile> Images)
        {

            //Check if the number of images and names do not match and meet the minimun requirement
            if( !(Names.Count == Images.Count && Names.Count >= 3))
            {
                return View(tournament);
                // return RedirectToAction("Create", tournament);//BadRequest("Please fill out all fields and require 3 participants");
            }
            //Get the username of the current user
            string username = User.Identity.Name;
            //Add the number of participants
            tournament.NumberOfParticipants = Names.Count;
            //Add the user that created the bracket
            tournament.UserCreatedID = username;

            //Remove the added Fields of tournament that are added outside the form. (Validity problems)
            ModelState.Remove("UserCreatedID");
            ModelState.Remove("NumberOfParticipants");

            //Check if the bracket is a valid state (Checks userCreatedID for null reference)
            if (ModelState.IsValid)
            //if(TryValidateModel(tournament))
            {
                //Add the tournament to the db
                _context.Add(tournament);
                await _context.SaveChangesAsync();

                //Create a Blob Connection
                BlobFileServie blobFileServie;

                try
                {
                    //create the blob 
                    blobFileServie = new(FormatUserName(username), tournament.Name, true);
                }catch (Exception ex)
                {
                    return BadRequest("There has been an error with your account. Please refresh the page.");
                }

                //Add each of the participants to the participant table
                for (int i = 0; i < Names.Count; i++)
                {
                    //Upload the image file for this Participant to the Blob Container
                    await blobFileServie.UploadAsync(Images[i]);


                    //Create a new participant object
                    var newParticipant = new Participant
                    {
                        TournamentId = tournament.Id,
                        Name = Names[i],
                        //Add the Image handling

                        ImageURL = $"https://{blobFileServie.GetStorageAccountName()}.blob.core.windows.net/{blobFileServie.GetBlobName()}/{Images[i].FileName}"
                    };

                    //Add to the table
                    _context.Participants.Add(newParticipant);

                }

                //Remove previous save and just have this one?
                await _context.SaveChangesAsync();

                //Redirect to the index page
                return RedirectToAction(nameof(Index));
            }

            //Model state is invald, return the create view
            return View(tournament);
        }

        /// <summary>
        /// Method that will search if the tournament that is attempted to 
        /// be deleted and delete it if possible
        /// </summary>
        /// <param name="id">The ID of the bracket</param>
        /// <returns>The confirm delete view</returns>
        // GET: Tournaments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.TournamentBrackets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        /// <summary>
        /// On confirmed delete, go about deleting the bracket form the backend
        /// Remove all parts from the DB and delete the blob container
        /// </summary>
        /// <param name="id">The ID of the bracket</param>
        /// <returns>The index view</returns>
        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tournament = await _context.TournamentBrackets.FindAsync(id);
            if (tournament != null)
            {
                //Connect to blob storage, format the username to cut off the @ tag
                BlobFileServie blobFileServie = new BlobFileServie(FormatUserName(User.Identity.Name), tournament.Name, false);
                //Delete the container for this tournament
                await blobFileServie.DeleteAsync();
                //Delete the tournament from the db
                _context.TournamentBrackets.Remove(tournament);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
