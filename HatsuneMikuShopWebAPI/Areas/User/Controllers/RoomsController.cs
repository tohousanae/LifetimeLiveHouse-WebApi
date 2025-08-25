﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelSystem.Access.Data;
using HotelSystem.Models;

namespace HotelSystem.Areas.User.Controllers
{
    [Area("User")]
    public class RoomsController : Controller
    {
        private readonly HotelSysDBContext2 _context;

        public RoomsController(HotelSysDBContext2 context)
        {
            _context = context;
        }

        // GET: User/Rooms
        public async Task<IActionResult> Index()
        {
            var rooms = _context.Room.Where(r => r.StatusCode == "1");
            ViewData["RoomPhotos"] = await _context.RoomPhoto.ToListAsync();

            ViewData["PeopleNum"] = await _context.Room.Where(r => r.StatusCode == "1").Select(r => r.PeopleNum).Distinct().ToListAsync();

            return View(await rooms.ToListAsync());
        }

        // GET: User/Rooms/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.StatusCodeNavigation)
                .FirstOrDefaultAsync(m => m.RoomID == id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["RoomPhotos"] = await _context.RoomPhoto.Where(r=>r.RoomID==id).ToListAsync();

            //string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "RoomPhotos", id)); //  /wwwroot/RoomPhotos/A2001

            //foreach (string item in files)
            //{
            //    string fileName = Path.GetFileName(item);

            //    ViewData["RoomPhotos"] += $"<img src='/RoomPhotos/{id}/{fileName}' height='600'>";
            //}

            return View(room);
        }


        //public IActionResult showPhotos(string roomID)
        //{

        //    string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "RoomPhotos", roomID)); //  /wwwroot/RoomPhotos/A2001

        //    foreach (string item in files)
        //    {
        //        string fileName = Path.GetFileName(item);
                
        //        ViewData["RoomPhotos"] += $"<img src='~/RoomPhotos/{fileName}'>";
        //    }


        //    return View();
        //}


        // GET: User/Rooms/Create
        public IActionResult Create()
        {
            ViewData["StatusCode"] = new SelectList(_context.RoomStatus, "StatusCode", "StatusCode");
            return View();
        }

        // POST: User/Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomID,RoomName,PeopleNum,Price,Area,Floor,Introduction,Note,CreatedDate,StatusCode")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StatusCode"] = new SelectList(_context.RoomStatus, "StatusCode", "StatusCode", room.StatusCode);
            return View(room);
        }

        // GET: User/Rooms/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["StatusCode"] = new SelectList(_context.RoomStatus, "StatusCode", "StatusCode", room.StatusCode);
            return View(room);
        }

        // POST: User/Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RoomID,RoomName,PeopleNum,Price,Area,Floor,Introduction,Note,CreatedDate,StatusCode")] Room room)
        {
            if (id != room.RoomID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomID))
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
            ViewData["StatusCode"] = new SelectList(_context.RoomStatus, "StatusCode", "StatusCode", room.StatusCode);
            return View(room);
        }

        // GET: User/Rooms/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.StatusCodeNavigation)
                .FirstOrDefaultAsync(m => m.RoomID == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: User/Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var room = await _context.Room.FindAsync(id);
            if (room != null)
            {
                _context.Room.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(string id)
        {
            return _context.Room.Any(e => e.RoomID == id);
        }
    }
}
