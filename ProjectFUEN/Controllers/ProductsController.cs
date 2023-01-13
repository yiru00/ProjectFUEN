using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fileUpload.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Infrastructures.Repositories;
using ProjectFUEN.Models.Services;
using ProjectFUEN.Models.Services.interfaces;
using ProjectFUEN.Models.VM;

namespace ProjectFUEN.Controllers
{
    public class ProductsController : Controller
    {
        FileManager fileManager;
        private readonly ProjectFUENContext _context;
        private IProductPhotoService productPhotoService;

        public ProductsController(ProjectFUENContext context)
        {
            fileManager = new FileManager();
            _context = context;
            IProductPhotoRepository repo = new ProductPhotoRepository(_context);
            this.productPhotoService = new IProductPhotoService(repo);
        }

        // GET: Products
        public ActionResult Index()
        {

            //var projectFUENContext = _context.Products.Include(p => p.Brand).Include(p => p.Category);
            //return View(await projectFUENContext.ToListAsync());

            var data = _context.Products
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.ReleaseDate,
                    p.ManufactorDate,
                    p.ProductSpec,

                    CategoryName = p.Category.Name,
                    BrandName = p.Brand.Name

                }).ToList()
                .Select(p => new ProductIndexVm
                {
                    Id = p.Id,
                    CategoryName = p.CategoryName,
                    BrandName = p.BrandName,
                    Price = p.Price,
                    ReleaseDate = p.ReleaseDate,
                    ManufactorDate = p.ManufactorDate,
                    ProductSpec = p.ProductSpec,
                });
            return View(data);

        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVm vm)
        {
            // View驗證不成功
            if (!ModelState.IsValid) return View(vm);


            // 圖片Copy to project的資料夾
            foreach (var file in vm.Sources)
            {
                //上傳照片
                (bool isCopied, string message, string Source) uploadSuccess = fileManager.UploadFile(file);

                // 失敗呈現在View上面
                if (!uploadSuccess.isCopied)
                {
                    ViewBag.photo = uploadSuccess.message;
                    return View(vm);
                }
            }

            //存取資料庫
            Product product = new Product()
            {
                Id = vm.Id,
                Name= vm.Name,
                Price= vm.Price,
                ManufactorDate= vm.ManufactorDate,
                Inventory= vm.Inventory,
                ProductSpec= vm.ProductSpec,
                BrandId = vm.BrandId,
                CategoryId = vm.CategoryId
            };
            product.ProductPhotos.AddRange(vm.Sources.Select(x => new ProductPhoto()
            {
                Source = x.FileName
            }));
            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
            
        }

        //GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }


            var product = await _context.Products.Include(x => x.ProductPhotos).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ProductVm vm = new ProductVm()
            {
                Id= product.Id,
                BrandId = product.BrandId,
                CategoryId= product.CategoryId,
                Inventory= product.Inventory,
                Price= product.Price,
                ProductSpec= product.ProductSpec,
                Name= product.Name,
                ManufactorDate= product.ManufactorDate,
            };

            List<string> photoNames = new List<string>();
            foreach (var fileName in product.ProductPhotos)
            {
                photoNames.Add(fileName.Source);
            }
            vm.FileNames = photoNames;

            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(vm);
        }

        //POST: Products/Edit/5
         //To protect from overposting attacks, enable the specific properties you want to bind to.
         //For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductVm vm)
        {

            if (id != vm.Id)
            {
                return NotFound();
            }
            foreach (var file in vm.Sources)
            {
                //上傳照片
                (bool isCopied, string message, string Source) uploadSuccess = fileManager.UploadFile(file);

                // 失敗呈現在View上面
                if (!uploadSuccess.isCopied)
                {
                    ViewBag.photo = uploadSuccess.message;
                    return View(vm);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(vm.Id))
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
            return View(vm);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ProjectFUENContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}