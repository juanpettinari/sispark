using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisPark.Models;

namespace SisPark.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TicketStatesController : Controller
    {


        private SisparkModel db = new SisparkModel();

        //
        // GET: /TicketStates/

        public ActionResult Index()
        {
            return View(db.TicketState.ToList());
        }

        //
        // GET: /TicketStates/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TicketStates/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TicketState ticketstate)
        {
            if (ModelState.IsValid)
            {
                db.TicketState.Add(ticketstate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ticketstate);
        }

        //
        // GET: /TicketStates/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TicketState ticketstate = db.TicketState.Find(id);
            if (ticketstate == null)
            {
                return HttpNotFound();
            }
            return View(ticketstate);
        }

        //
        // POST: /TicketStates/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TicketState ticketstate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketstate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticketstate);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}