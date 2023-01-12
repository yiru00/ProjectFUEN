using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.DTOs;
using ProjectFUEN.Models.EFModels;


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
        public async Task<IActionResult> Index()
        {
            var projectFUENContext = _context.OrderDetails.Include(o => o.Member);
            //state int to string
            //var state = _context.OrderDetails.Select(x => stateToString(x.State));
            //if ( ) { };
            return View(await projectFUENContext.ToListAsync());
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




        //[HttpGet]
        //public async Task<ActionResult<OrderDetailsDTO>> Search(string account)
        //{
        //    var emaccount = _context.OrderDetails.Include(o => o.Member).Select(x => new OrderDetailsDTO
        //    {
        //        Member = x.Member,
        //        State = x.State,
        //        Address = x.Address,
        //        OrderDate = x.OrderDate,
        //        EmailAccount = x.Member.EmailAccount,           
        //    });

        //    if (!string.IsNullOrEmpty(account))
        //    {
        //        emaccount = emaccount.Where(s => s.EmailAccount.Contains(account));
        //    }

        //    return View(await emaccount.ToListAsync());
        //}

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MemberId,OrderDate,Address,State")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "EmailAccount", orderDetail.MemberId);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "EmailAccount", orderDetail.MemberId);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MemberId,OrderDate,Address,State")] OrderDetail orderDetail)
        {
            if (id != orderDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.Id))
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
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "EmailAccount", orderDetail.MemberId);
            return View(orderDetail);
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
    }
}
