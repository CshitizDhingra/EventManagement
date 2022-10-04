using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventManagement.Models;

namespace EventManagement.Controllers
{
    public class eventsController : Controller
    {
        private eventManagementEntities db = new eventManagementEntities();

        // GET: events
        public ActionResult Index()
        {
            var events = db.events.Include(e => e.decorationPlans).Include(e => e.eventType);
            return View(events.ToList());
        }

        // GET: events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            events events = db.events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        // GET: events/Create
        public ActionResult Create()
        {
            ViewBag.did = new SelectList(db.decorationPlans, "did", "dname");
            ViewBag.etype = new SelectList(db.eventType, "eventType1", "eventType1");
            return View();
        }

        // POST: events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "eid,etype,edate,gathering,did,d_price,fo_id,f_price,total")] events events)
        {
            if (ModelState.IsValid)
            {
                db.events.Add(events);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.did = new SelectList(db.decorationPlans, "did", "dname", events.did);
            ViewBag.etype = new SelectList(db.eventType, "eventType1", "eventType1", events.etype);
            return View(events);
        }

        // GET: events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            events events = db.events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            ViewBag.did = new SelectList(db.decorationPlans, "did", "dname", events.did);
            ViewBag.etype = new SelectList(db.eventType, "eventType1", "eventType1", events.etype);
            return View(events);
        }

        // POST: events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "eid,etype,edate,gathering,did,d_price,fo_id,f_price,total")] events events)
        {
            if (ModelState.IsValid)
            {
                db.Entry(events).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.did = new SelectList(db.decorationPlans, "did", "dname", events.did);
            ViewBag.etype = new SelectList(db.eventType, "eventType1", "eventType1", events.etype);
            return View(events);
        }

        // GET: events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            events events = db.events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        // POST: events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            events events = db.events.Find(id);
            db.events.Remove(events);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
