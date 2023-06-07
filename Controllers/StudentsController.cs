using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Data;

namespace MVC.Models
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly int _recordsPerPage = 10;
        private GenericPaginator<Student> _paginatorStudents;
        private IEnumerable<Student> _students;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            ViewData["CurrentFilter"] = searchString;
            int _totalRecords = 0;
            _totalRecords = await _context.Students.CountAsync();

            _students = _context.Students.OrderBy(x => x.Id)
                                                 .Skip((page - 1) * _recordsPerPage)
                                                 .Take(_recordsPerPage)
                                                 .ToList();
            if (!String.IsNullOrEmpty(searchString))
            { 
                searchString = searchString.ToLower();
                _students = _context.Students.Where(s => s.Name.ToLower().Contains(searchString)
                                       || s.Career.ToLower().Contains(searchString)).ToList();
            }
            var _totalPages = (int)Math.Ceiling((double)_totalRecords / _recordsPerPage);
            _paginatorStudents = new GenericPaginator<Student>()
            {
                RecordsPerPage = _recordsPerPage,
                TotalRecords = _totalRecords,
                TotalPages = _totalPages,
                PageNow = page,
                Result = _students
            };

            return View (_paginatorStudents);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Create([Bind("Id,Name,Career,Age,Email,Year,Housed,Registered")] Student student)
        {
            student.Registered = DateTime.Now;   
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
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            student.Registered = DateTime.Now;
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Career,Age,Email,Year,Housed,Registered")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    student.Registered = DateTime.Now;
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.Students == null)
            {
                return Problem("Entity set 'AppDbContext.Students'  is null.");
            }
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
          return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
