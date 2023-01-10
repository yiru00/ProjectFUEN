using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
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

namespace ProjectFUEN.Controllers
{
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
        public string MailToHtml(string couponCode)
        {
            //service做發信
            //I/Repository GetAllEmail 找全部會員的EmailAccount 直接用IEnumerable傳
            //細節-> 改html模板、Index Modal
            //GET 改 POST


            MailMessage mail = new MailMessage();
            //前面是發信email後面是顯示的名稱
            mail.From = new MailAddress("projectfuen@gmail.com", "攝影交流平台");

            StringBuilder sb = new StringBuilder();


            //repository 做找全部會員的email Email要 直接用IEnumerable傳
            var emilAddresses = _context.Members.Select(m => m.EmailAccount).ToList();

            //要刪掉
            IEnumerable<string> emilAddress = new string[] { "jim2345678@gmail.com", "chenhuanhuan1127@gmail.com" };

            foreach (string ss in emilAddress)
            {
                sb.Append($"{ss},");
            }

            string emailAddresses = sb.ToString().Substring(0, sb.Length - 1);

            //收信者email
            //mail.To.Add(email);
            mail.Bcc.Add(emailAddresses);

            //設定優先權
            mail.Priority = MailPriority.Normal;

            //標題
            mail.Subject = "恭喜您獲得優惠券！趕快來使用吧";

            string FilePath = Directory.GetCurrentDirectory() + "\\Views\\Templates\\MailContent.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            MailText = MailText.Replace("[CouponCode]", couponCode);


            str.Close();
            //內容
            mail.Body = MailText;

            //內容使用html
            mail.IsBodyHtml = true;

            //設定gmail的smtp (這是google的)
            SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);

            //您在gmail的帳號密碼
            MySmtp.Credentials = new System.Net.NetworkCredential("projectfuen@gmail.com", "qpzilvexdqnfzozc");

            //開啟ssl
            MySmtp.EnableSsl = true;

            //發送郵件
            MySmtp.Send(mail);

            //放掉宣告出來的MySmtp
            MySmtp = null;



            //放掉宣告出來的mail
            mail.Dispose();

            return "優惠碼已發送成功";
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
        public async Task<IActionResult> Create(CouponVM coupon)
        {
            // [V]VM不能填Code 在create頁面就先顯示code的guid
            // [ ]寫Service Create(coupon.ToDto) 判斷Discount >=1要是整數 大於0.01 err.msg 欄位下顯示 & return View(coupon)
            // [ ]I/Repository void Create(couponDto) => 轉成ToEntity 資料庫add(新增資料)、saveChangesAsync() 完成後 controller return Index

            ViewData["Guid"] = coupon.Code;

            (bool IsSuccess, string ErrorMessage) response = couponService.Create(coupon.VMToCouponDto());

            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Discount", response.ErrorMessage);
                return View(coupon);
            }

            //if (ModelState.IsValid)
            //{
            //    _context.Add(coupon);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
           
        }

        // GET: Coupons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // [] VM不能編輯code (readOnly)
            // [] I/Repository bool CouponIsExist(id)
            //      1.判斷ID是否存在 存取資料庫.FindAsync(id) ??? SingleOrDefault(c=>c.id==id) => 
            //      2.資料庫是null??
            //      3.id會是null??? 用網址列打?
            //    回傳 true/false到controller false=> return 404 or 小視窗找不到
            //    I/Repository Dto GetByCouponId 資料庫.FindAsync(id) ??
            //    到controller轉成VM return

            if (id == null || _context.Coupons == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon);
        }

        // POST: Coupons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CouponVM coupon)
        {
            //[] 把VM轉成dto
            //[] 要再驗證一次 id存在嗎? GET驗證過了??
            //[] I/Repository EditCoupon(dto) 轉成entity => db.Update(entity) Save

            // 有可能不一樣嗎
            if (id != coupon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coupon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CouponExists(coupon.Id))
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
            return View(coupon);
        }

        // GET: Coupons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // 同Edit

            if (id == null || _context.Coupons == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon.ToCouponDto().ToCouponVM());
        }

        // POST: Coupons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // db會是null??
            // 判斷id是否存在 不存在根本就不會點到?!
            // I/Repository Delete(id) db.FindAsync(id) 資料庫remove(id的那筆資料) save
            if (_context.Coupons == null)
            {
                return Problem("Entity set 'ProjectFUENContext.Coupons'  is null.");
            }
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon != null)
            {
                _context.Coupons.Remove(coupon);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CouponExists(int id)
        {
          return _context.Coupons.Any(e => e.Id == id);
        }
    }
}
