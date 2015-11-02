using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisPark.Models;
using System.Globalization;

namespace SisPark.Controllers
{
    [Authorize(Roles = "Administrador,App User")]
    public class TicketsController : Controller
    {
        private SisparkModel db = new SisparkModel();


        [HttpGet]
        public ActionResult CancelarTicket()
        {
            var eticket = from ti in db.Ticket
                          select ti;

            return View(eticket.Where(et => et.TicketStateId == 1));
        }


        [HttpGet]
        public ActionResult RetirarCoche()
        {
            var eticket = from ti in db.Ticket
                          select ti;

            return View(eticket.Where(et => et.TicketStateId == 1));
        }

        //Tickets entrada get
        public ActionResult TicketEntrada(int? carId)
        {
            ViewBag.CarId = new SelectList(db.Car, "carId", "Patente", carId);
            return View();
        }


        //tickets entrada post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TicketEntrada(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                List<string> ListaPatente = (from p in db.Ticket
                                      where p.TicketStateId == 1
                                      && p.Car.CarId == ticket.CarId
                                      select p.Car.Patente).ToList();

                if (ListaPatente.Count == 0)

                {
                    // ticketstateid = 1 ingresado, = 2 retirado, = 3 cancelado
                    ticket.TicketStateId = 1;
                    ticket.StartDateTime = DateTime.Now;
                    db.Ticket.Add(ticket);
                    db.SaveChanges();
                    Controllers.TicketLogsController.GuardarEntrada(User.Identity.Name, ticket);
                    ViewBag.Mensaje = "";
                    return RedirectToAction("Index");
                    
                }
                else
                {
                    ViewBag.Mensaje = "Patente ya ingresada";
                    ViewBag.CarId = new SelectList(db.Car, "CarId", "Patente", ticket.CarId);
                    return View();
                    
                }



            }
            ViewBag.CarId = new SelectList(db.Car, "CarId", "Patente", ticket.CarId);
            return View();
        }


        public ActionResult TicketSalida(int id = 0)
        {
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        [HttpPost, ActionName("TicketSalida")]
        [ValidateAntiForgeryToken]
        public ActionResult SalidaConfirmada(int id)
        {
            Ticket ticket = db.Ticket.Find(id);
            if (ModelState.IsValid)
            {
                ticket.FinishDateTime = DateTime.Now;
                TimeSpan timetotal = ticket.FinishDateTime.Value.Subtract(ticket.StartDateTime);
                double Horas = timetotal.TotalHours;
                ticket.TotalTime = (float)Horas;
                ticket.TotalPrice = ticket.TotalTime * ticket.Car.CarType.tarifa;
                ticket.TicketStateId = 2;

                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                Controllers.TicketLogsController.GuardarSalida(User.Identity.Name, ticket);
                return RedirectToAction("Details", new { id = ticket.TicketId });

            }
            
            return View();
        }

        public ActionResult TicketCancelado(int id = 0)
        {
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        [HttpPost, ActionName("TicketCancelado")]
        [ValidateAntiForgeryToken]
        public ActionResult CanceladoConfirmada(int id)
        {
            Ticket ticket = db.Ticket.Find(id);
            if (ModelState.IsValid)
            {
                ticket.TicketStateId = 3;

                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                Controllers.TicketLogsController.GuardarCancelado(User.Identity.Name, ticket);
                return RedirectToAction("Index");

            }

            return View();
        }
        
        //
        // GET: /Tickets/
        


        public ActionResult Index(string estadoTicket)
        {
            var ListaTicket = new List<string>();

            var ConsultaTicket = from t in db.Ticket
                                 orderby t.TicketState.description
                                 select t.TicketState.description;
            ListaTicket.AddRange(ConsultaTicket.Distinct());
            ViewBag.estadoTicket = new SelectList(ListaTicket);

            var eticket = from ti in db.Ticket
                          select ti;

            if (!String.IsNullOrEmpty(estadoTicket))
            {
                return View(eticket = eticket.Where(s => s.TicketState.description == estadoTicket));
            }
            else
            {
                return View(eticket);
            }
        }

        public ActionResult TotalDiario(string fecha)
        {
            ViewBag.Total = 0;
            if (!String.IsNullOrEmpty(fecha))
            {
                DateTime fecha1 = Convert.ToDateTime(fecha);
                DateTime fecha2 = fecha1.AddSeconds(86399);
                string useractivo = User.Identity.Name;
                var td = (from t in db.TicketLog where t.DateTimeLog > fecha1 & t.DateTimeLog < fecha2 & t.TransactionType == "Salida" & t.User.Usuario == useractivo select t.Ticket.TotalPrice).Sum();
                if (td.HasValue)
                    ViewBag.total = td;
                else
                    ViewBag.total = 0;
            }
            
            return View();
        }

        //
        // GET: /Tickets/Details/5

        public ActionResult Details(int id = 0)
        {
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}