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
    [Authorize(Roles = "Administrador,App User,Auditor")]
    public class CarsController : Controller
    {
        private SisparkModel db = new SisparkModel();

        //
        // GET: /Cars/

        public ActionResult Index()
        {
            var car = db.Car.Include(c => c.CarType);
            return View(car.ToList());
        }


        //
        // GET: /Cars/Create

        public ActionResult Create()
        {
            ViewBag.CarTypeId = new SelectList(db.CarType, "CarTypeId", "description");
            return View();
        }

        //
        // POST: /Cars/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Car car)
        {
            if (ModelState.IsValid)
            {
                var caridentico = db.Car.FirstOrDefault(c => c.Patente == car.Patente);
                if (caridentico == null)
                {
                    db.Car.Add(car);
                    db.SaveChanges();
                    return RedirectToAction("TicketEntrada", "Tickets", new { carId = car.CarId });
                }
                else
                {
                    ModelState.AddModelError("", String.Concat("Ya hay un auto con la patente: ", caridentico.Patente));
                }
            }
            ViewBag.CarTypeId = new SelectList(db.CarType, "CarTypeId", "description", car.CarTypeId);
            return View(car);
        }

        //
        // GET: /Cars/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Car car = db.Car.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarTypeId = new SelectList(db.CarType, "CarTypeId", "description", car.CarTypeId);
            return View(car);
        }

        //
        // POST: /Cars/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Car car)
        {
            if (ModelState.IsValid)
            {
                    db.Entry(car).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }
            ViewBag.CarTypeId = new SelectList(db.CarType, "CarTypeId", "description", car.CarTypeId);
            return View(car);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}