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
    [Authorize]
    public class UserLogsController : Controller
    {


        private SisparkModel db = new SisparkModel();
        public static void GuardarLogInLog(User user)
        {
            using (var db = new SisparkModel())
            {
            var sysUserLog = db.UserLog.Create();
            sysUserLog.UserId = user.UserId;
            sysUserLog.FechaYHora = DateTime.Now;
            sysUserLog.Accion = "Login";

            db.UserLog.Add(sysUserLog);
            db.SaveChanges();
            }
        }



        public static void GuardarLogOutLog(string LoggedUser)
        {

            using (var db = new SisparkModel())
            {
                var user = db.User.FirstOrDefault(u => u.Usuario == LoggedUser);
                var sysUserLog = db.UserLog.Create();
                sysUserLog.UserId = user.UserId;
                sysUserLog.FechaYHora = DateTime.Now;
                sysUserLog.Accion = "Logout";
               
                db.UserLog.Add(sysUserLog);
                db.SaveChanges();
            }
        }
        // Buscar por        USUARIO=x 
        //                   Fecha> y fecha< 
        //                   accion=login o logout
        [Authorize(Roles = "Administrador,Auditor")]
        public ActionResult Index(string LogAccion, string userseek, string fechaDesde, string fechaHasta)
        {
            var ListaAccion = new List<string>();

            var ConsultaAccion = from d in db.UserLog
                              orderby d.Accion
                              select d.Accion;
            ListaAccion.AddRange(ConsultaAccion.Distinct());
            ViewBag.LogAccion = new SelectList(ListaAccion);

            var ListaUser = new List<string>();

            var ConsultaUser = from u in db.UserLog
                               orderby u.User.Usuario
                               select u.User.Usuario;
            ListaUser.AddRange(ConsultaUser.Distinct());
            ViewBag.userseek = new SelectList(ListaUser);

            var ulogs = from l in db.UserLog
                        select l;

            if (!String.IsNullOrEmpty(fechaDesde))
            {
                DateTime fecha1 = Convert.ToDateTime(fechaDesde);
                ulogs = ulogs.Where(fd => fd.FechaYHora >= fecha1);
            }
            if (!String.IsNullOrEmpty(fechaHasta))
            {
                DateTime fecha2 = Convert.ToDateTime(fechaHasta);
                ulogs = ulogs.Where(fd => fd.FechaYHora <= fecha2);
            }
            if (!String.IsNullOrEmpty(userseek))
            {
                ulogs = ulogs.Where(c => c.User.Usuario == userseek);
            }

            if (string.IsNullOrEmpty(LogAccion))
                return View(ulogs);
            else
            {
                return View(ulogs.Where(x => x.Accion == LogAccion));
            }
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}