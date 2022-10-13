using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using EventManagement.Models;
using Microsoft.Win32;

namespace EventManagement.Controllers
{
    public class eventController : Controller
    {
        private eventManagementEntities db = new eventManagementEntities();
        // GET: event
        public ActionResult Index()
        {
            var events = db.events.ToList();
            return View(events.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.did = new SelectList(db.decorationPlans, "did", "dname");
            ViewBag.etype = new SelectList(db.eventType, "eventType1", "eventType1");
            ViewBag.eventDate = db.events.Select(x => x.edate).ToList();
            eventMVCModel e = new eventMVCModel();
            e.foodItems = db.foodItems.ToList();
            return View(e);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(eventMVCModel e)
        {
            if (e.eid != 0)
            {
                var dt = DateTime.Parse(e.edate);
                e.edate = dt.ToString("dd-MM-yyyy");
                int pph = db.decorationPlans.Where(x => x.did == e.did).Select(x => x.pricePerH).SingleOrDefault();
                e.d_price = e.gathering * pph / 100;
                e.f_price = 0;
                var foodOrders = new List<foodOrders>();
                foreach (var foodItem in e.foodItems)
                {
                    if (foodItem.Checked)
                    {
                        System.Diagnostics.Debug.WriteLine("### SELECTED ####" + foodItem.fname + foodItem.fid);
                        e.f_price += foodItem.price;
                        foodOrders.Add(new foodOrders()
                        {
                            fid = foodItem.fid,
                            eid = e.eid,
                            qty = Convert.ToInt32(e.gathering),
                        });
                    }
                }

                e.f_price *= e.gathering;
                e.total = e.d_price + e.f_price;
                if (ModelState.IsValid)
                {
                    events ev = new events
                    {
                        eid = e.eid,
                        edate = e.edate,
                        etype = e.etype,
                        gathering = e.gathering,
                        did = e.did,
                        d_price = e.d_price,
                        f_price = e.f_price,
                        total = e.total
                    };
                    try
                    {
                        db.events.Add(ev);
                        db.SaveChanges();
                        saveOrdersToDB(foodOrders);
                        ViewData["Success"] = "EVENT SCHEDULED. Event ID : " + e.eid;
                    }
                    catch (Exception ex)
                    {
                        ViewData["Success"] = "DATABASE ERROR";
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
                else
                {
                    ViewData["Success"] = "INVALID DATA";
                }
            }
            else
            {
                ViewData["Success"] = "SELECT APPROPRIATE DATE ";
            }
            ViewBag.did = new SelectList(db.decorationPlans, "did", "dname", e.did);
            ViewBag.etype = new SelectList(db.eventType, "eventType1", "eventType1", e.etype);
            return View(e);
        }

        public void saveOrdersToDB(List<foodOrders> orders)
        {
            using(eventManagementEntities dbOrders = new eventManagementEntities())
            {
                foreach (var order in orders)
                {
                    var lastOrder = db.foodOrders.Count();
                    order.fo_id = lastOrder + 1;
                    dbOrders.foodOrders.Add(order);
                    dbOrders.SaveChanges();
                }
            }
        }
    }
}