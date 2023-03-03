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
using NuGet.Packaging;
using Microsoft.AspNetCore.Authorization;

namespace ProjectFUEN.Controllers
{
    [Authorize]
    public class ProductPhotoesController : Controller
    {
        FileManager2 fileManager;
        private readonly ProjectFUENContext _context;
        private IProductPhotoService productPhotoService;

        public ProductPhotoesController(ProjectFUENContext context)
        {
            fileManager = new FileManager2();
            _context = context;
            IProductPhotoRepository repo = new ProductPhotoRepository(_context);
            this.productPhotoService = new IProductPhotoService(repo);
        }


        //GET: ProductPhotoes1
        public async Task<IActionResult> Index(int id)
        //public ActionResult Index()
        {
            var projectFUENContext = _context.ProductPhotos.Where(x => x.ProductId==id);
            return View(projectFUENContext);

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
        public IActionResult Create(int id)
        {

            //ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.Id == id), "Id", "Name");
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            ViewData["ProductName"] = product.Name;
            ViewData["Productid"] = id;
            return View();
        }

        // POST: ProductPhotoes1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductPhotoVM vm)

        {
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
            ProductPhotoVM productPhoto = new ProductPhotoVM()
            {
                Id = vm.Id,
                ProductId = vm.ProductId,
                Source = vm.Source,
                Sources= vm.Sources,
               
            };

            if (ModelState.IsValid)
            {
                var product = _context.Products.Include(x => x.ProductPhotos).First(x => x.Id == vm.ProductId);

                List<ProductPhoto> photos = new List<ProductPhoto>();


                foreach (var item in vm.Sources)
                {
                    photos.Add(new ProductPhoto()
                    {
                        Source = item.FileName
                    });
                }

                product.ProductPhotos.AddRange(photos);

                _context.SaveChanges();
                return Redirect("Index/" + vm.ProductId);
            
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
        public async Task<IActionResult> Edit(IFormFile file, int id, [Bind("Id,ProductId,Source")] ProductPhotoVM productPhoto)
        {
            if (id != productPhoto.Id)
            {
                return NotFound();
            }
            //判斷是否有上傳圖檔，若檔案類型/未上傳 回傳錯誤訊息，上傳成功回傳新檔名，錯誤訊息=""
            (bool, string, string) uploadSuccess = fileManager.UploadFile(file);

            //上傳檔案失敗(沒上傳東西/上傳圖檔以外的)=>
            //有上傳檔案=>判斷有沒有跳檔案錯誤的訊息，沒跳就將新的檔案(uploadSuccess.Item3)更新到instructor.ResumePhoto


            if (!uploadSuccess.Item1)//上傳失敗 item1=false
            {

                if (uploadSuccess.Item2 == "記得選取檔案") //未上傳任何檔案，用原有的
                {
                    ModelState.Remove("file");
                    if (ModelState.IsValid)
                    {
                        _context.Update(productPhoto.ToEntity());
                        await _context.SaveChangesAsync();
                    }
                }
                else if (uploadSuccess.Item2 == "檔案必須是圖片檔案")//上傳成圖檔以外的
                {
                    ViewBag.photoError = uploadSuccess.Item2; //錯誤訊息
                    return View(productPhoto);
                }
                return View(productPhoto);
            }
            else //有上傳檔案
            {
                if (uploadSuccess.Item2 == "") //上傳圖檔，錯誤訊息=""
                {
                    productPhoto.Source = uploadSuccess.Item3; //傳入新檔名
                    _context.Update(productPhoto.ToEntity());
                    await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index)+"%2F"+productPhoto.ProductId);
                    //7027/ProductPhotoes/Index/1018
                    return RedirectToAction("Index", "ProductPhotoes", new { id = productPhoto.ProductId });
                 ///////////////////////////////   ////////////////////////////////////////////////////////////////////////////

                }
                else //上傳圖檔以外的(ppt.pdf...)
                {
                    ViewBag.photoError = uploadSuccess.Item2;
                    return View(productPhoto);
                }
            }
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
                return Problem("Entity set 'ProjectFUENContext.ProductPhotos' is null.");
            }
            var productPhoto = await _context.ProductPhotos.FindAsync(id);
            if (productPhoto != null)
            {
                _context.ProductPhotos.Remove(productPhoto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index) +"/"+ productPhoto.ProductId);
            //return Redirect("Index/" + productPhoto.ProductId);
        }

        private bool ProductPhotoExists(int id)
        {
          return _context.ProductPhotos.Any(e => e.Id == id);
        }
        public async Task<IActionResult> DeleteoOneSelf(int id)
        {
            var photo = await _context.ProductPhotos.FindAsync(id);
            _context.ProductPhotos.Remove(photo);
            await _context.SaveChangesAsync();
            //return RedirectToAction("Index", photo.ProductId);
            return RedirectToAction("Index", "ProductPhotoes", new { id = photo.ProductId });

        }
    }
}
