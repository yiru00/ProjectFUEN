using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.DTOs;
using ProjectFUEN.Models.EFModels;

using X.PagedList;

namespace ProjectFUEN.Controllers
{

    /// <summary>
    /// OrderDetail 是 orderitem, 是要查詢與顯示出貨狀態的
    /// </summary>
    public class OrderDetailsController : Controller
    {
        private readonly ProjectFUENContext _context;

        public OrderDetailsController(ProjectFUENContext context)
        {
            _context = context;
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index(int? state, int? page = 1 )
        {
            var projectFUENContext = _context.OrderDetails.Include(o => o.Member);
            const int pageSize = 1;

            ViewBag.OrderDetail = GetPagedProcess(page, pageSize);
            ViewBag.State = GetState(state);
            return View(await projectFUENContext.Skip<OrderDetail>(pageSize * ((page ?? 1) - 1)).Take(pageSize).ToListAsync());

        }

        protected IPagedList<OrderDetail> GetPagedProcess(int? page, int pageSize)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetStuffFromDatabase();
            IPagedList<OrderDetail> pagelist = listUnpaged.ToPagedList(page ?? 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }

        protected IPagedList<OrderDetailsDTO> GetPagedProcess(int? page, int pageSize, IEnumerable<OrderDetailsDTO> listUnpaged)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
 
            IPagedList<OrderDetailsDTO> pagelist = listUnpaged.ToPagedList(page ?? 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }


        protected IQueryable<OrderDetail> GetStuffFromDatabase()
        {
            return _context.OrderDetails;
        }

        //private string stateToString(int state)
        //{

        //}

        //無限回傳 要new新物件 err500 非同步不會做跳500
        //[HttpGet]
        //public async Task<IEnumerable<object>> Searcht(string account)
        //{
        //    //var emaccount = _context.OrderDetails.Include(o => o.Member).Where(x => x.Member.EmailAccount.Contains(account));

        //    var emaccount = await _context.OrderDetails.Include(o => o.Member).Select(x => new
        //    {
        //        id = x.Id,
        //        adress = x.Address,
        //        state = x.State,
        //        Member = x.Member,
        //    }).ToListAsync();//.Where(x => x.Member.EmailAccount.Contains(account));


        //    return emaccount;
        //}


        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailsDTO>>> Search(int state1,int? state,string account, int? page = 1)
        {  
            const int pageSize = 2;
            ViewBag.State = GetState(state);

            var data = _context.OrderDetails.Include(o => o.Member).Select(x => new OrderDetailsDTO
            {
                Id = x.Id,
                Member = x.Member,
                State = x.State,
                Address = x.Address,
                OrderDate = x.OrderDate,
                EmailAccount = x.Member.EmailAccount,
            });

            if (state.HasValue) data = data.Where(x => x.State == state.Value);
           

            if (string.IsNullOrEmpty(account)==false)
            {
                data = data.Where(s => s.EmailAccount.Contains(account));
            }
     
            ViewBag.OrderDetailDto = GetPagedProcess(page, pageSize, data);



            return View(await data.Skip(pageSize * ((page ?? 1) - 1)).Take(pageSize).ToListAsync());


        }

        //public IEnumerable<SelectListItem> GetState(int? State)
        //{
        //    var defaultdata = _context.OrderDetails.Select(x => x.State);
        //    var selectList =_context.OrderDetails.AsEnumerable().Select(C=> new List<SelectListItem>
        //    {
        //        new SelectListItem{Value=C.State.ToString()},
        //        new SelectListItem{Text="text-0",Value="value-0" },
        //        new SelectListItem{Text="text-1",Value="value-1" },
        //        new SelectListItem{Text="text-2",Value="value-2" },
        //        new SelectListItem{Text="text-3",Value="value-3" }
        //    } );



        //    selectList.Where(q => q.Value == "value-2").First().Selected = true;
        //    ViewBag.SelectList = selectList;

        //}

        public IEnumerable<SelectListItem> GetState(int? State)
        {
            var items = _context.OrderDetails
            .AsEnumerable()
            .Select(c => new SelectListItem
            {
                Value = c.State.ToString(),
                Text = GetStateName(c.State),
                Selected = (State.HasValue && c.State == State.Value)
            }).Prepend(new SelectListItem { Value = string.Empty, Text = "請選擇..." });

            return items;
        }

        //[HttpGet]
        //public async Task<IActionResult> Search(string account)
        //{
        //    var member = from m in _context.Members select m;
        //    if (account == null || _context.Members == null)
        //    {
        //        return NotFound();
        //    }
        //    if (!String.IsNullOrEmpty(account))
        //    {
        //        member = member.Where(s => s.EmailAccount!.Contains(account));
        //    }
        //    return View(await member.ToListAsync());
        //}

        ////阿郭search
        //public ActionResult search(string accountname)
        //{

        //    ViewBag.AccountName = accountname;
        //    var data = _context.OrderDetails.Include(x => x.Member);


        //    if (string.IsNullOrEmpty(accountname) == false) data = data.Where(p =>p.Member ));

        //    return View(data);
        //}



        //怪怪的
        //public ActionResult SelectState(int? stateNum)
        //{
        //    ViewBag.state = GetOrderState(stateNum);
        //    var data = _context.OrderDetails.Include(x => x.State);

        //    if (stateNum.HasValue) data = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<OrderDetail, int>)data.Where(p => p.State == stateNum.Value);


        //    return View(data);
        //}


        //private IEnumerable<SelectListItem>
        //    GetOrderState(int? stateNum)
        //{
        //    var state = _context.OrderDetails.Select(c => new SelectListItem { Value = c.State.ToString(), Selected = (stateNum.HasValue && c.State == stateNum.Value) }).ToList().Prepend(new SelectListItem { Value = string.Empty, Text = "  " });

        //    return state;
        //}


        //GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderItems == null)
            {
                return NotFound();
            }

            var SelectedorderItems = await _context.OrderItems
                .Include(o => o.OrderId)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (SelectedorderItems == null)
            {
                return NotFound();
            }

            return View(SelectedorderItems);
        }

        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.OrderDetails == null)
        //    {
        //        return NotFound();
        //    }

        //    var orderDetail = await _context.OrderDetails
        //        .Include(o => o.Member)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (orderDetail == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(orderDetail);
        //}


        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "EmailAccount");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.



        [HttpGet]
        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails.Include(x => x.Member).FirstAsync(x => x.Id == id);
            if (orderDetail == null)
            {
                return NotFound();
            }


            //ViewData["MemberId"] = new SelectList(_context.Members, "Id", "EmailAccount", orderDetail.MemberId);
            ViewBag.MemberId = orderDetail.Member.EmailAccount;
            ViewData["State"] = new SelectList(_context.OrderDetails, "State", "State", orderDetail.State);

            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("Id,MemberId,OrderDate,Address,State")] OrderDetail orderDetail)
        {
            string msg = "";
            if (id != orderDetail.Id)
            {
                msg = "新增失敗(更新資料列不正確)!!";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                    msg = "新增成功 !!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.Id))
                    {
                        msg = "新增失敗(找不到商品)!!";
                    }
                    else
                    {
                        msg = "新增失敗(其他錯誤)!!";
                    }
                }
            }

            //ViewData["MemberId"] = new SelectList(_context.Members, "Id", "EmailAccount", orderDetail.MemberId);
            // 直接重寫頁面並關閉fancybox
            await Response.WriteAsync($"<script>parent.$.fancybox.close();alert('{msg}');</script>");
            return Ok();
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderDetails == null)
            {
                return Problem("Entity set 'ProjectFUENContext.OrderDetails'  is null.");
            }
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
          return _context.OrderDetails.Any(e => e.Id == id);
        }

        private string GetStateName(int state)
        {
            string StateName = "";
            switch (state)
            {
                case 0:
                    StateName = "未出貨";
                    break;
                case 1:
                    StateName = "已出貨";
                    break;
                case 2:
                    StateName = "運送中";
                    break;
                case 3:
                    StateName = "已簽收";
                    break;
                case 4:
                    StateName = "已完成";
                    break;
                default:
                    StateName = "訂單異常";
                    break;

            }
            return StateName;
        }
    }
}
