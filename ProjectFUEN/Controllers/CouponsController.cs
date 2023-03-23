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
            couponService.SendCoupon(couponCode);
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
            (bool IsSuccess, string ErrorMsg) response = couponService.EditCoupon(coupon.EditVMToDto(), discountType);

            if (response.IsSuccess) return RedirectToAction(nameof(Index));
            else if (!response.IsSuccess && response.ErrorMsg == null) return NotFound();
            else ModelState.AddModelError("Discount", response.ErrorMsg);

            return View(coupon);
        }

        [HttpDelete, ActionName("DeleteCoupon")]
        public void DeleteCoupon(int id)
        {
            repo.Delete(id);
        }
    }
}
