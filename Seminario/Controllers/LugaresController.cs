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
    public class LugaresController : Controller
    {
        private SeminarioContext db = new SeminarioContext();

        // GET: Lugares
        public async Task<ActionResult> Index()
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario == TipoUsuario.ControlMapa)
            {
                return RedirectToAction("Index", "Home");
            }
            var lugares = db.Lugares.Include(l => l.Municipio);
            return View(await lugares.ToListAsync());
        }
        

        // GET: Lugares/Create
        public ActionResult Create()
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario == TipoUsuario.ControlMapa)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.MunicipioId = new SelectList(db.Municipios, "Id", "Nombre");
            return View();
        }

        // POST: Lugares/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Lugar lugar)
        {
            if (ModelState.IsValid)
            {
                db.Lugares.Add(lugar);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MunicipioId = new SelectList(db.Municipios, "Id", "Nombre", lugar.MunicipioId);
            return View(lugar);
        }

        // GET: Lugares/Edit/5
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
            Lugar lugar = await db.Lugares.FindAsync(id);
            if (lugar == null)
            {
                return HttpNotFound();
            }
            ViewBag.MunicipioId = new SelectList(db.Municipios, "Id", "Nombre", lugar.MunicipioId);
            return View(lugar);
        }

        // POST: Lugares/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Lugar lugar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lugar).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MunicipioId = new SelectList(db.Municipios, "Id", "Nombre", lugar.MunicipioId);
            return View(lugar);
        }

        // GET: Lugares/Delete/5
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
            Lugar lugar = await db.Lugares.FindAsync(id);
            db.Lugares.Remove(lugar);
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
