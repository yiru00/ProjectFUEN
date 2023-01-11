using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fileUpload.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Infrastructures.Repositories;
using ProjectFUEN.Models.Services.interfaces;
using ProjectFUEN.Models.Services;
using ProjectFUEN.Models.VM;

namespace ProjectFUEN.Controllers
{
    public class ProductPhotoesController : Controller
    {
        FileManager fileManager;
        private readonly ProjectFUENContext _context;
        private IProductPhotoService productPhotoService;

        public ProductPhotoesController(ProjectFUENContext context)
        {
            fileManager = new FileManager();
            _context = context;
            IProductPhotoRepository repo = new ProductPhotoRepository(_context);
            this.productPhotoService = new IProductPhotoService(repo);
        }


        // GET: ProductPhotoes1
        //public async Task<IActionResult> Index()
        public ActionResult Index()
        {
            //var projectFUENContext = _context.ProductPhotos.Include(p => p.Product);
            //return View(await projectFUENContext.ToListAsync());

            var data = _context.ProductPhotos
               .Select(p => new
               {
                   p.Id,
                   p.ProductId,
                   p.Source,


               }).ToList()
               .Select(p => new ProductPhotoVM
               {
                   Id= p.Id,
                   ProductId= p.ProductId,
                   Source= p.Source,
                
               });
            return View(data);
        }

        // GET: ProductPhotoes1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductPhotos == null)
            {
                return NotFound();
            }

            var productPhoto = await _context.ProductPhotos
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productPhoto == null)
            {
                return NotFound();
            }

            return View(productPhoto);
        }

        // GET: ProductPhotoes1/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: ProductPhotoes1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file, [Bind("Id,ProductId,Source")] ProductPhotoVM productPhoto)
        {
            //上傳照片
            (bool, string, string) uploadSuccess = fileManager.UploadFile(file);
            if (!uploadSuccess.Item1)
            {
                ViewBag.photo = uploadSuccess.Item2;
                return View(productPhoto);
            };
            if (ModelState.IsValid)
            {
                _context.Add(productPhoto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productPhoto.ProductId);
            return View(productPhoto);
        }

        // GET: ProductPhotoes1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductPhotos == null)
            {
                return NotFound();
            }

            var productPhoto = await _context.ProductPhotos.FindAsync(id);
            if (productPhoto == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productPhoto.ProductId);
            return View(productPhoto);
        }

        // POST: ProductPhotoes1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Source")] ProductPhotoVM productPhoto)
        {
            if (id != productPhoto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productPhoto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductPhotoExists(productPhoto.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productPhoto.ProductId);
            return View(productPhoto);
        }

        // GET: ProductPhotoes1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductPhotos == null)
            {
                return NotFound();
            }

            var productPhoto = await _context.ProductPhotos
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productPhoto == null)
            {
                return NotFound();
            }

            return View(productPhoto);
        }

        // POST: ProductPhotoes1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductPhotos == null)
            {
                return Problem("Entity set 'ProjectFUENContext.ProductPhotos'  is null.");
            }
            var productPhoto = await _context.ProductPhotos.FindAsync(id);
            if (productPhoto != null)
            {
                _context.ProductPhotos.Remove(productPhoto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductPhotoExists(int id)
        {
          return _context.ProductPhotos.Any(e => e.Id == id);
        }
    }
}
