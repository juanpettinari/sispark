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
    public class TicketLogsController : Controller
    {


        private SisparkModel db = new SisparkModel();
        [Authorize(Roles = "Administrador,App User,Auditor")]
        public static void GuardarEntrada(string Logged,Ticket ticket)
        {
            using (var db = new SisparkModel())
            {
                var user = db.User.FirstOrDefault(u => u.Usuario == Logged);
                var TicLog = db.TicketLog.Create();
                TicLog.UserId = user.UserId;
                TicLog.DateTimeLog = DateTime.Now;
                TicLog.TransactionType = "Entrada";
                TicLog.TicketId = ticket.TicketId;

                db.TicketLog.Add(TicLog);
                db.SaveChanges();
            }
            
        }

        [Authorize(Roles = "Administrador,App User,Auditor")]
        public static void GuardarSalida(string Logged, Ticket ticket)
        {
            using (var db = new SisparkModel())
            {
                var user = db.User.FirstOrDefault(u => u.Usuario == Logged);
                var TicLog = db.TicketLog.Create();
                TicLog.UserId = user.UserId;
                TicLog.DateTimeLog = DateTime.Now;
                TicLog.TransactionType = "Salida";
                TicLog.TicketId = ticket.TicketId;

                db.TicketLog.Add(TicLog);
                db.SaveChanges();
            }

        }

        [Authorize(Roles = "Administrador,App User,Auditor")]
        public static void GuardarCancelado(string Logged, Ticket ticket)
        {
            using (var db = new SisparkModel())
            {
                var user = db.User.FirstOrDefault(u => u.Usuario == Logged);
                var TicLog = db.TicketLog.Create();
                TicLog.UserId = user.UserId;
                TicLog.DateTimeLog = DateTime.Now;
                TicLog.TransactionType = "Cancelado";
                TicLog.TicketId = ticket.TicketId;

                db.TicketLog.Add(TicLog);
                db.SaveChanges();
            }

        }

        //
        // GET: /TicketLogs/
        [Authorize(Roles = "Administrador,Auditor")]
        public ActionResult Index(string LogTransaccion, string userseek, string patenteseek, string fechaDesde, string fechaHasta)
        {
            var ListaTransaccion = new List<string>();

            var ConsultaTransaccion = from d in db.TicketLog
                                      orderby d.TransactionType
                                      select d.TransactionType;
            ListaTransaccion.AddRange(ConsultaTransaccion.Distinct());
            ViewBag.LogTransaccion = new SelectList(ListaTransaccion);

            var ListaUser = new List<string>();

            var ConsultaUser = from u in db.TicketLog
                               orderby u.User.Usuario
                               select u.User.Usuario;
            ListaUser.AddRange(ConsultaUser.Distinct());
            ViewBag.userseek = new SelectList(ListaUser);

            var ListaPatente = new List<string>();

            var ConsultaPatente = from p in db.TicketLog
                                  orderby p.Ticket.Car.Patente
                                  select p.Ticket.Car.Patente;
            ListaPatente.AddRange(ConsultaPatente.Distinct());
            ViewBag.patenteseek = new SelectList(ListaPatente);

            var tlogs = from l in db.TicketLog
                        select l;

            if (!String.IsNullOrEmpty(fechaDesde))
            {
                DateTime fecha1 = Convert.ToDateTime(fechaDesde);
                tlogs = tlogs.Where(fd => fd.DateTimeLog >= fecha1);
            }

            if (!String.IsNullOrEmpty(fechaHasta))
            {
                DateTime fecha2 = Convert.ToDateTime(fechaHasta);
                tlogs = tlogs.Where(fd => fd.DateTimeLog >= fecha2);
            }

            if (!String.IsNullOrEmpty(patenteseek))
                tlogs = tlogs.Where(p => p.Ticket.Car.Patente == patenteseek);

            if (!String.IsNullOrEmpty(userseek))
                tlogs = tlogs.Where(u => u.User.Usuario == userseek);

            if (string.IsNullOrEmpty(LogTransaccion))
                return View(tlogs);
            else
                return View(tlogs.Where(t => t.TransactionType == LogTransaccion));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}