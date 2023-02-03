using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fileUpload.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using ProjectFUEN.Models;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Infrastructures.Repositories;
using ProjectFUEN.Models.Services;
using ProjectFUEN.Models.Services.interfaces;
using ProjectFUEN.Models.VM;
using X.PagedList;

namespace ProjectFUEN.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        FileManager2 fileManager;
        private readonly ProjectFUENContext _context;
        private IProductPhotoService productPhotoService;

        public ProductsController(ProjectFUENContext context)
        {
            fileManager = new FileManager2();
            _context = context;
            IProductPhotoRepository repo = new ProductPhotoRepository(_context);
            this.productPhotoService = new IProductPhotoService(repo);
        }

        // GET: Products
        public IActionResult Index(string search, int? page = 1)
        {
            //每頁幾筆
            const int pageSize = 3;
            //處理頁數
            ViewBag.ProductIndexVm = GetPagedProcess(page, pageSize);

            var data = _context.Products
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.ReleaseDate,
                    p.ManufactorDate,
                    p.ProductSpec,
                    p.Inventory,

                    CategoryName = p.Category.Name,
                    BrandName = p.Brand.Name

                }).ToList()
                .Select(p => new ProductIndexVm
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryName = p.CategoryName,
                    BrandName = p.BrandName,
                    Price = p.Price,
                    ReleaseDate = p.ReleaseDate,
                    ManufactorDate = p.ManufactorDate,
                    ProductSpec = p.ProductSpec,
                    Inventory=p.Inventory
                });




            if (!String.IsNullOrEmpty(search)) data = data.Where(s => s.Name.Contains(search)).ToList();
            //if (!String.IsNullOrEmpty(search)) data = data.Where(s => s.CategoryName.Contains(search)).ToList();
            //if (!String.IsNullOrEmpty(search)) data = data.Where(s => s.BrandName.Contains(search)).ToList();

            return View(data.Skip<ProductIndexVm>(pageSize * ((page ?? 1) - 1)).Take(pageSize).ToList());
            //return View(data);

        }
        protected IPagedList<ProductIndexVm> GetPagedProcess(int? page, int pageSize)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetStuffFromDatabase();
            IPagedList<ProductIndexVm> pagelist = listUnpaged.ToPagedList(page ?? 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        protected IQueryable<ProductIndexVm> GetStuffFromDatabase()
        {
            var product = _context.Products.Select(x => new ProductIndexVm()
            {
                Id= x.Id,
                Name= x.Name,
                Price= x.Price,
                Inventory= x.Inventory,
                ManufactorDate= x.ManufactorDate,
                ReleaseDate= x.ReleaseDate,
                Brand= x.Brand,
                Category= x.Category,
                ProductSpec= x.ProductSpec,
            });
            return product;
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

            ProductEditVm vm = new ProductEditVm()
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

            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(vm);
        }

        //POST: Products/Edit/5
         //To protect from overposting attacks, enable the specific properties you want to bind to.
         //For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditVm vm)
        {

            if (id != vm.Id)
            {
                return NotFound();
            }
           
            if (ModelState.IsValid)
            {
                try
                {
                    var product = _context.Products.First(x => x.Id == id);

                    product.CategoryId = vm.CategoryId;
                    product.BrandId = vm.BrandId;
                    product.Id = vm.Id;
                    product.Name = vm.Name;
                    product.Price = vm.Price;
                    product.ManufactorDate = vm.ManufactorDate;
                    product.Inventory = vm.Inventory;
                    product.ProductSpec = vm.ProductSpec;

                    _context.Update(product);

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
        public async Task<IActionResult> DeleteoOneSelf(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}