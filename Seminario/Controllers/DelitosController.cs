using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Seminario.Models;
using Seminario.Models.Seguridad;
using System.Web.Security;

namespace Seminario.Controllers
{
    [Authorize]
    public class DelitosController : Controller
    {
        private SeminarioContext db = new SeminarioContext();

        // GET: Delitos
        public async Task<ActionResult> Index()
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario == TipoUsuario.ControlMapa)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(await db.Delitos.ToListAsync());
        }

        // GET: Delitos/Create
        public ActionResult Create()
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario == TipoUsuario.ControlMapa)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Delitos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Delito delito)
        {
            if (ModelState.IsValid)
            {
                db.Delitos.Add(delito);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(delito);
        }

        // GET: Delitos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario == TipoUsuario.ControlMapa)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delito delito = await db.Delitos.FindAsync(id);
            if (delito == null)
            {
                return HttpNotFound();
            }
            return View(delito);
        }

        // POST: Delitos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Delito delito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(delito).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(delito);
        }

        // GET: Delitos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario != TipoUsuario.Administrador)
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delito delito = await db.Delitos.FindAsync(id);
            db.Delitos.Remove(delito);
            await db.SaveChangesAsync();
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
