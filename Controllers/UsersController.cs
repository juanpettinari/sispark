using SisPark.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SisPark.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {


        private SisparkModel db = new SisparkModel();


        //
        // GET: /Users/
        [Authorize(Roles="Administrador")]
        public ActionResult Index()
        {
            var user = db.User.Include(u => u.Usertype);
            return View(user.ToList());
        }

        
        // GET: /Users/Login/
        [AllowAnonymous]
        [HttpGet]
        public ActionResult LogIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Auditor"))
                    return RedirectToAction("Index","TicketLogs");
                else
                    return RedirectToAction("Index","Tickets");
            }
            else
                return View();
        }


        // POST: /Users/Login
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogIn(User user)
        {
            if (IsValid(user.Usuario, user.Clave))
            {
                
                FormsAuthentication.SetAuthCookie(user.Usuario, false);
                return RedirectToAction("Index", "Tickets");
            }
            else
            {
                ModelState.AddModelError("", "Error en datos de inicio de sesión");
            }

            return View();
        }

        public ActionResult LogOut()
        {
            string LoggedUser = User.Identity.Name;
            UserLogsController.GuardarLogOutLog(LoggedUser);
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Users");
        }

        // GET: /Users/Registration

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public ActionResult Registration()
        {
            ViewBag.UserTypeId = new SelectList(db.Usertype, "usertypeid", "description");
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User user)
        {
           
            if (ModelState.IsValid)
            {

                var useridentico = db.User.FirstOrDefault(u => u.Usuario == user.Usuario);
                if (useridentico == null)
                {

                    var crypto = new SimpleCrypto.PBKDF2();

                    var encrpPass = crypto.Compute(user.Clave);

                    user.Clave = encrpPass;
                    user.ClaveSalt = crypto.Salt;

                    db.User.Add(user);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    ModelState.AddModelError("",String.Concat("Ya hay un usuario '",useridentico.Usuario,"'"));
                }
                
            }
            else
            {
                ModelState.AddModelError("", "Datos de login erroneos");
            }
            ViewBag.UserTypeId = new SelectList(db.Usertype, "usertypeid", "description", user.UserTypeId);
            return View(user);
        }

        //
        // GET: /Users/Edit/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int id = 0)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserTypeId = new SelectList(db.Usertype, "usertypeid", "description", user.UserTypeId);
            return View(user);
        }

        //
        // POST: /Users/Edit/5
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var crypto = new SimpleCrypto.PBKDF2();

                var encrpPass = crypto.Compute(user.Clave);

                user.Clave = encrpPass;
                user.ClaveSalt = crypto.Salt;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.UserTypeId = new SelectList(db.Usertype, "usertypeid", "description", user.UserTypeId);
            return View(user);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private bool IsValid(string usuario, string clave)
        {
            var crypto = new SimpleCrypto.PBKDF2();

            bool IsValid = false;

            var user = db.User.FirstOrDefault(u => u.Usuario == usuario);

            if(user !=null)
            {
                var userType = db.Usertype.FirstOrDefault(u => user.UserTypeId == u.usertypeid);

                if (userType.description != "Deshabilitado")
                {
                    var cry = crypto.Compute(clave, user.ClaveSalt);
                    if (user.Clave == cry)
                    {
                        IsValid = true;
                        UserLogsController.GuardarLogInLog(user);
                    }
                }
                else
                    ModelState.AddModelError("", "Usuario deshabilitado");
            }
            return IsValid;
        }
    }
}