using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.DTOs;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Infrastructures.Repositories;
using ProjectFUEN.Models.Services;
using ProjectFUEN.Models.Services.Interfaces;
using ProjectFUEN.Models.ViewModels;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace ProjectFUEN.Controllers
{
    [Authorize]
    public class CouponsController : Controller
    {
        private readonly ProjectFUENContext _context;
        private CouponService couponService;
        private ICouponRepository repo;

        public CouponsController(ProjectFUENContext context)
        {
            _context = context;
            this.repo = new CouponRepository(_context);
            this.couponService = new CouponService(repo);
        }

        // GET: Coupons
        public  IActionResult Index()
        {
            IEnumerable<CouponVM> allCoupons = repo.GetAll().Select(c => c.ToCouponVM());
            return View(allCoupons);
        }
        public void MailToHtml(string couponCode)
        {
            //[V]service做發信
            //[V]I/Repository GetAllEmail 找全部會員的EmailAccount 直接用IEnumerable傳
            //[ ]細節-> 改html模板
            //[V]GET 改 POST

            //couponService.SendCoupon(couponCode);
        }

        // GET: Coupons/Create
        public IActionResult Create()
        {
            ViewData["Guid"] = Guid.NewGuid().ToString().Substring(0,8);

            return View();
        }

        // POST: Coupons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int discountType, CreateCouponVM coupon)
        {
            // [V]VM不能填Code 在create頁面就先顯示code的guid
            // [V]寫Service Create(coupon.ToDto) 判斷Discount >=1要是整數 大於0.01 err.msg 欄位下顯示 & return View(coupon)
            // [V]I/Repository void Create(couponDto) => 轉成ToEntity 資料庫.Coupons.Add(新增資料)、saveChangesAsync() 完成後 controller return Index

            // await??? 要寫在asp-for

            ViewData["Guid"] = coupon.Code;

            (bool IsSuccess, string ErrorMessage) response = couponService.Create(coupon.CreateVMToDto(), discountType);

            if (response.IsSuccess) return RedirectToAction(nameof(Index));
            else
            {
                ModelState.AddModelError("Discount", response.ErrorMessage);
                return View(coupon);
            }
        }

        // GET: Coupons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // [V] Service bool CouponIsExist(id)
            //      1.判斷ID是否存在 存取資料庫.FindAsync(id) ??? SingleOrDefault(c=>c.id==id) => 
            //      3.id會是null 用網址列打 => 判斷後 404
            //    回傳 true/false到controller false=> return 404 or 小視窗找不到
            //    I/Repository Dto GetByCouponId 資料庫.FindAsync(id) ??
            //    到controller轉成VM return

            // Q[] Find 只能用List且比較快 

            (bool IsSuccess, CouponDto data) response = couponService.CouponIsExist(id);

            if (response.IsSuccess) return View(response.data.ToEditCouponVM());
            else return NotFound();
        }

        // POST: Coupons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int discountType, EditCouponVM coupon)
        {
            //[V] VM不能編輯code (readOnly)
            //[V] 把VM轉成dto
            //[N] 要再驗證一次 id存在嗎 GET驗證過了 且 bind東西是一樣的吧?!
            //[ ] I/Repository EditCoupon(dto) 轉成entity => db.Update(entity) Save

            // 有可能不一樣嗎
            //if (id != coupon.Id)
            //{
            //    return NotFound();
            //}
          
            (bool IsSuccess, string ErrorMsg) response = couponService.EditCoupon(coupon.EditVMToDto(), discountType);

            if (response.IsSuccess) return RedirectToAction(nameof(Index));
            else if (!response.IsSuccess && response.ErrorMsg == null) return NotFound();
            else ModelState.AddModelError("Discount", response.ErrorMsg);

            return View(coupon);
        }

        // GET: Coupons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            (bool IsSuccess, CouponDto data) response = couponService.CouponIsExist(id);

            if (response.IsSuccess) return View(response.data.ToDeleteCouponVM());
            else return NotFound();
        }

        // POST: Coupons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // db會是null??
            // 判斷id是否存在 不存在根本就不會點到?!
            // I/Repository Delete(id) db.FindAsync(id) 資料庫remove(id的那筆資料) save

            repo.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete, ActionName("DeleteCoupon")]
        public void DeleteCoupon(int id)
        {
            // db會是null??
            // 判斷id是否存在 不存在根本就不會點到?!
            // I/Repository Delete(id) db.FindAsync(id) 資料庫remove(id的那筆資料) save

            repo.Delete(id);
        }
    }
}
