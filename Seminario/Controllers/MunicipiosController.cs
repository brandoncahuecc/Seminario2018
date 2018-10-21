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
using System.Web.Security;
using Seminario.Models.Seguridad;

namespace Seminario.Controllers
{
    [Authorize]
    public class MunicipiosController : Controller
    {
        private SeminarioContext db = new SeminarioContext();

        // GET: Municipios
        public async Task<ActionResult> Index()
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario == TipoUsuario.ControlMapa)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(await db.Municipios.ToListAsync());
        }

        // GET: Municipios/Create
        public ActionResult Create()
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario != TipoUsuario.Administrador)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Municipios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Municipio municipio)
        {
            if (ModelState.IsValid)
            {
                db.Municipios.Add(municipio);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(municipio);
        }

        // GET: Municipios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario != TipoUsuario.Administrador)
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Municipio municipio = await db.Municipios.FindAsync(id);
            if (municipio == null)
            {
                return HttpNotFound();
            }
            return View(municipio);
        }

        // POST: Municipios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Municipio municipio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(municipio).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(municipio);
        }

        // GET: Municipios/Delete/5
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
            Municipio municipio = await db.Municipios.FindAsync(id);
            db.Municipios.Remove(municipio);
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
