using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationAnnuaire.Data;
using WebApplicationAnnuaire.Models;

namespace WebApplicationAnnuaire.Controllers
{
    public class EmployesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employes
        public IActionResult Index(string search, int? siteSearch, int? serviceSearch, int page = 1, int pageSize = 10)
        /*public IActionResult Index(string search, int? siteSearch, int? serviceSearch, int page = 1, int pageSize = 10)*/
        {
            var query = _context.Employes.Include(e => e.Service).Include(e => e.Site).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                //query = query.Where(e => e.Nom.Contains(search) || e.Prenom.Contains(search) || e.Email.Contains(search));
                query = query.Where(e => e.Nom.Contains(search));
            }

            if (siteSearch.HasValue)
            {
                query = query.Where(e => e.SiteId == siteSearch.Value);
            }

            if (serviceSearch.HasValue)
            {
                query = query.Where(e => e.ServiceId == serviceSearch.Value);
            }

            /*    var employes = query.ToList();
                ViewBag.Sites = _context.Site.ToList();
                ViewBag.Services = _context.Service.ToList();
                return View(employes);*/

            var totalCount = query.Count();
            var employes = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.Search = search;
            ViewBag.SiteSearch = siteSearch;
            ViewBag.ServiceSearch = serviceSearch;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.CurrentPage = page;

            ViewBag.Sites = _context.Site.ToList();
            ViewBag.Services = _context.Service.ToList();

            return View(employes);

            /*    var totalCount = query.Count();
                var employes = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.Sites = _context.Site.ToList();
                ViewBag.Services = _context.Service.ToList();
                ViewBag.TotalCount = totalCount;
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                return View(employes);*/
        }


        // GET: Employes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employes == null)
            {
                return NotFound();
            }

            var employe = await _context.Employes
                .Include(e => e.Service)
                .Include(e => e.Site)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employe == null)
            {
                return NotFound();
            }

            return View(employe);
        }

        // GET: Employes/Create
        [Authorize(Roles = "Administration")]
        public IActionResult Create()
        {
            ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Nom");
            ViewData["SiteId"] = new SelectList(_context.Site, "Id", "Ville");
            return View();
        }

        // POST: Employes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administration")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Prenom,TelephoneFixe,TelephonePortable,Email,ServiceId,SiteId")] Employe employe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Nom", employe.ServiceId);
            ViewData["SiteId"] = new SelectList(_context.Site, "Id", "Ville", employe.SiteId);
            return View(employe);
        }

        // GET: Employes/Edit/5
        [Authorize(Roles = "Administration")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employes == null)
            {
                return NotFound();
            }

            var employe = await _context.Employes.FindAsync(id);
            if (employe == null)
            {
                return NotFound();
            }
            ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Nom", employe.ServiceId);
            ViewData["SiteId"] = new SelectList(_context.Site, "Id", "Ville", employe.SiteId);
            return View(employe);
        }

        // POST: Employes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administration")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Nom,Prenom,TelephoneFixe,TelephonePortable,Email,ServiceId,SiteId")] Employe employe)
        {
            if (id != employe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeExists(employe.Id))
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
            ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Nom", employe.ServiceId);
            ViewData["SiteId"] = new SelectList(_context.Site, "Id", "Ville", employe.SiteId);
            return View(employe);
        }

        // GET: Employes/Delete/5
        [Authorize(Roles = "Administration")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employes == null)
            {
                return NotFound();
            }

            var employe = await _context.Employes
                .Include(e => e.Service)
                .Include(e => e.Site)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employe == null)
            {
                return NotFound();
            }

            return View(employe);
        }

        // POST: Employes/Delete/5
        [Authorize(Roles = "Administration")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Employes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employes'  is null.");
            }
            var employe = await _context.Employes.FindAsync(id);
            if (employe != null)
            {
                _context.Employes.Remove(employe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeExists(int? id)
        {
          return (_context.Employes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
