using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisPark.Models;
using System.Web.Security;

namespace SisPark.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UserTypesController : Controller
    {
        private SisparkModel db = new SisparkModel();
        //
        // GET: /UserTypes/
        public ActionResult Index()
        {
            return View(db.Usertype.ToList());
        }

        //
        // GET: /UserTypes/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserTypes/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usertype usertype)
        {
            if (ModelState.IsValid)
            {
                var ut = db.Usertype.FirstOrDefault(u => u.description == usertype.description);
                if (ut == null)
                {
                    db.Usertype.Add(usertype);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Tipo de usuario Repetido");
                }
                
                
            }

            return View(usertype);
        }

        //
        // GET: /UserTypes/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Usertype usertype = db.Usertype.Find(id);
            if (usertype == null)
            {
                return HttpNotFound();
            }
            return View(usertype);
        }

        //
        // POST: /UserTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usertype usertype)
        {
            if (ModelState.IsValid)
            {
                // Comprueba que no haya un tipo de usuario igual
                var ut = db.Usertype.FirstOrDefault(u => u.description == usertype.description);
                if (ut == null)
                {
                    db.Entry(usertype).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Ya existe un tipo de usuario igual");
                }
            }
            return View(usertype);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}