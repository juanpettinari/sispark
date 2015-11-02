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
    [Authorize(Roles="Administrador")]
    public class CarTypesController : Controller
    {

        private SisparkModel db = new SisparkModel();

        //
        // GET: /CarTypes/

        public ActionResult Index()
        {
            return View(db.CarType.ToList());
        }

        //
        // GET: /CarTypes/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CarTypes/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarType cartype)
        {
            if (ModelState.IsValid)
            {
                db.CarType.Add(cartype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cartype);
        }

        //
        // GET: /CarTypes/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CarType cartype = db.CarType.Find(id);
            if (cartype == null)
            {
                return HttpNotFound();
            }
            return View(cartype);
        }

        //
        // POST: /CarTypes/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CarType cartype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cartype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cartype);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}