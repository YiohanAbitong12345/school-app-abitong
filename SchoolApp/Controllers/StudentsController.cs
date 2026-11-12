
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolAppContext _context;

        public StudentsController(SchoolAppContext context)
        {
            _context = context;
        }

        // GET: Students?searchString=maria
        // Search by student name, student number, or email
        public async Task<IActionResult> Index(string? searchString)
        {
            // Start with all students
            var students = from s in _context.Student select s;

            // Check if the user entered something
            if (!string.IsNullOrEmpty(searchString))
            {
                // Convert search text to lowercase
                var text = searchString.ToLower();

                // Search name, student number, or email
                students = students.Where(s =>
                    s.Name!.ToLower().Contains(text) ||
                    s.StudentNo!.ToLower().Contains(text) ||
                    s.Email!.ToLower().Contains(text));
            }

            // Keep the search text in the search box
            ViewData["searchString"] = searchString;

            // Display the search results
            return View(await students.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,StudentNo,Name,Email,ConfirmEmail,Phone,Age,BirthDate")] Student student)
        {
            Console.WriteLine("Name length: " + (student.Name?.Length ?? 0));
            Console.WriteLine("ModelState valid: " + ModelState.IsValid);

            // Check if the email is already registered
            if (await _context.Student.AnyAsync(s => s.Email == student.Email))
            {
                ModelState.AddModelError("Email", "This email is already registered.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,StudentNo,Name,Email,ConfirmEmail,Phone,Age,BirthDate")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            // Check if another student already uses this email
            if (await _context.Student.AnyAsync(s =>
                s.Email == student.Email && s.StudentId != student.StudentId))
            {
                ModelState.AddModelError("Email", "This email is already registered.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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

            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);

            if (student != null)
            {
                _context.Student.Remove(student);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Check if a student exists
        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.StudentId == id);
        }
    }
}
