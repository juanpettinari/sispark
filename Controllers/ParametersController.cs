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
    [Authorize(Roles = "Administrador,Auditor")]
    public class ParametersController : Controller
    {

        private SisparkModel db = new SisparkModel();

        //
        // GET: /Parameters/

        public ActionResult Index()
        {
            return View(db.Parameters.ToList());
        }

        //
        // GET: /Parameters/Details/5

        public ActionResult Details(int id = 0)
        {
            Parameters parameters = db.Parameters.Find(id);
            if (parameters == null)
            {
                return HttpNotFound();
            }
            return View(parameters);
        }

        //
        // GET: /Parameters/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Parameters/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Parameters parameters)
        {
            if (ModelState.IsValid)
            {
                db.Parameters.Add(parameters);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parameters);
        }

        //
        // GET: /Parameters/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Parameters parameters = db.Parameters.Find(id);
            if (parameters == null)
            {
                return HttpNotFound();
            }
            return View(parameters);
        }

        //
        // POST: /Parameters/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Parameters parameters)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parameters).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parameters);
        }

        //
        // GET: /Parameters/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Parameters parameters = db.Parameters.Find(id);
            if (parameters == null)
            {
                return HttpNotFound();
            }
            return View(parameters);
        }

        //
        // POST: /Parameters/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Parameters parameters = db.Parameters.Find(id);
            db.Parameters.Remove(parameters);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}