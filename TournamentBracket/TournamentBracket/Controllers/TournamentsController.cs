using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // GET: Tournaments
        public async Task<IActionResult> Index()
        {
            return View(await _context.TournamentBrackets.ToListAsync());
        }

        // GET: Tournaments/Details/5
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

            return View(tournament);
        }

        // GET: Tournaments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tournaments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,TournamentDate,BracketOptions")] Tournament tournament,
            List<string> Names, List<IFormFile> Images)
        {

            //Check if any files were uploaded
            if(Images == null || Images.Count == 0)
            {
                //Change the Response
                //Alert about invalid input and redirect back to creation
                return BadRequest("No File uploaded");
            }

            //Check if the number of images and names do not match
            if( !(Names.Count == Images.Count && Names.Count >= 3))
            {
                return BadRequest("Please fill out all fields and require 3 participants");
            }


            if (ModelState.IsValid)
            {

                //Add the tournament to the db
                _context.Add(tournament);
                await _context.SaveChangesAsync();

                //Create a Blob Connection
                BlobFileServie blobFileServie = new BlobFileServie("TestIdentity", tournament.Name, true);

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


                return RedirectToAction(nameof(Index));
            }
            return View(tournament);
        }

        // GET: Tournaments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.TournamentBrackets.FindAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }
            return View(tournament);
        }

        // POST: Tournaments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,TournamentDate,BracketOptions")] Tournament tournament)
        {
            if (id != tournament.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tournament);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TournamentExists(tournament.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tournament);
        }

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

        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tournament = await _context.TournamentBrackets.FindAsync(id);
            if (tournament != null)
            {
                //Connect to blob storage
                BlobFileServie blobFileServie = new BlobFileServie("testidentity", tournament.Name, false);
                //Delete the container for this tournament
                await blobFileServie.DeleteAsync();
                //Delete the tournament from the db
                _context.TournamentBrackets.Remove(tournament);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TournamentExists(int id)
        {
            return _context.TournamentBrackets.Any(e => e.Id == id);
        }
    }
}
