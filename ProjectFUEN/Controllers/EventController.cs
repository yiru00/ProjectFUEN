using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fileUpload.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using NuGet.Protocol;
using ProjectFUEN.Models.DTOs;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.ViewModels;
using X.PagedList;

namespace ProjectFUEN.Controllers
{
    [Authorize]   
    
    public class EventController : Controller
    {
        FilePhoto fileManager;
        private readonly ProjectFUENContext _context;

        public EventController(ProjectFUENContext context)
        {
            fileManager = new FilePhoto();
            _context = context;
        }

        // GET: Event
        //public async Task<IActionResult> Index()
        //{
        //      return View(await _context.Events.ToListAsync());
        //}

        // 已勾選的Data也要傳遞到後端
        //public IEnumerable<ProductIndexVM> Search(string productName, string strcheckBoxes)
        //{
        //    //IEnumerable<int> checkBoxes = strcheckBoxes.Split(",").Select(x => int.Parse(x));

        //    IEnumerable<Product> products = _context.Products.Include(x => x.Brand).Include(x => x.Category);

        //    if (!string.IsNullOrEmpty(productName)) products = products.Where(x => x.Name.Contains(productName));

        //    return products.ToList().Select(x => new ProductIndexVM()
        //    {
        //        Id = x.Id,
        //        Name = x.Name,
        //        BrandId = x.BrandId,
        //        BrandName = x.Brand.Name,
        //        CategoryId = x.CategoryId,
        //        CategoryName = x.Category.Name,
        //        Price = x.Price,
        //    });
        //}


        public async Task<IActionResult> Index(int? page = 1)
        {
            //每頁幾筆
            const int pageSize = 3;
            //處理頁數
            ViewBag.Event = GetPagedProcess(page, pageSize);//填入頁面資料
            //填入頁面資料
            return View(await _context.Events.Skip<Event>(pageSize * ((page ?? 1) - 1)).Take(pageSize).ToListAsync());
        }

        protected IPagedList<Event> GetPagedProcess(int? page, int pageSize)
        {// 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1) return null;
            // 從資料庫取得資料
            var listUnpaged = GetStuffFromDatabase();
            IPagedList<Event> pagelist = listUnpaged.ToPagedList(page ?? 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;

            return pagelist;
        }

        protected IQueryable<Event> GetStuffFromDatabase()
        {
            return _context.Events;
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }
            //ViewBag.Products = await _context.Products.Include(x => x.Brand).Include(x => x.Category).FirstOrDefaultAsync(m => m.Id == id);

            //return View();

            var @event = await _context.Events.Include(x => x.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Event/Details/5
        //public async Task<IActionResult> Details(in? id)
        //{
        //    if (id == null || _context.Events == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewBag.Products = _context.Products.Include(x => x.Brand).Include(x => x.Category).ToList();

        //    return View();
        //    var @event = await _context.Events.Include(x => x.Products)
        //        .FirstAsync(m => m.Id == id);
        //    if (@event == null)
        //    {
        //        return NotFound();
        //    }
        //}

        // GET: Event/Create
        public IActionResult Create()
        {
            ViewBag.Products = _context.Products.Include(x => x.Brand).Include(x => x.Category).ToList();

            return View();
        }

        // POST: Event/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateVM vm, int[] checkBoxes)
        {
            if (!ModelState.IsValid) return (View(vm));

            Event eventEntity = new Event();

            // 照儲存到資料夾
            (bool isCopied, string message, string File) uploadSuccess = fileManager.UploadFile(vm.File);
            if (uploadSuccess.isCopied) eventEntity.Photo = uploadSuccess.File;
            else
            {
                ViewBag.photo = uploadSuccess.message;
                return View(vm);
            }

            // Insert to DB
            eventEntity.Id = vm.Id;
            eventEntity.EventName = vm.EventName;
            eventEntity.StartDate = vm.StartDate;
            eventEntity.EndDate = vm.EndDate;

            // 找出以勾選的Proudcts
            List<Product> products = new List<Product>();

            foreach (int id in vm.CheckBoxes)
            {
                products.Add(await _context.Products.FirstAsync(x => x.Id == id));
            }

            eventEntity.Products.AddRange(products);

            _context.Add(eventEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            //EventVM vm = new EventVM();
            //vm.Products = _context.Products.Include(x => x.Brand).Include(x => x.Category).ToList();
            //return View(vm);

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event.ToVM());
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile file, int id, [Bind("Id,EventName,Photo,StartDate,EndDate")] EventVM @event)
        {
            var dataInDb = await _context.Events.Where(x => x.Id != @event.Id).FirstOrDefaultAsync(e => e.EventName == @event.EventName);
            if (dataInDb != null)
            {
                ModelState.AddModelError("EventName", "這個 活動名稱 已經取過了!");
                return View(@event);
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
                        _context.Update(@event.ToEntity());
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));

                    }
                }
                else if (uploadSuccess.Item2 == "檔案必須是圖片檔案")//上傳成圖檔以外的
                {
                    ViewBag.photoError = uploadSuccess.Item2; //錯誤訊息
                    return View(@event);
                }
                return View(@event);
            }
            else //有上傳檔案
            {
                if (uploadSuccess.Item2 == "") //上傳圖檔，錯誤訊息=""
                {
                    @event.Photo = uploadSuccess.Item3; //傳入新檔名
                    _context.Update(@event.ToEntity());
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else //上傳圖檔以外的(ppt.pdf...)
                {
                    ViewBag.photoError = uploadSuccess.Item2;
                    return View(@event);
                }

            }

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(@event);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!EventExists(@event.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(@event);
        }

        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'ProjectFUENContext.Events'  is null.");
            }
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
