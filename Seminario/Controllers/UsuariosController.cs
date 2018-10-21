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
    public class UsuariosController : Controller
    {
        private SeminarioContext db = new SeminarioContext();

        // GET: Usuarios
        public async Task<ActionResult> Index()
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario != TipoUsuario.Administrador)
            {
                return RedirectToAction("Index", "Home");
            }
            var usuarios = db.Usuarios.Include(u => u.Municipio);
            return View(await usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario != TipoUsuario.Administrador)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var usuario = await db.Usuarios.Include(c => c.Municipio).Where(c => c.Id == id).FirstOrDefaultAsync();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario != TipoUsuario.Administrador)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.MunicipioId = new SelectList(db.Municipios, "Id", "Nombre");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (db.Usuarios.Any(c => c.User == usuario.User))
                {
                    ViewBag.Mensaje = "Usuario ya existente.";
                    ViewBag.MunicipioId = new SelectList(db.Municipios, "Id", "Nombre", usuario.MunicipioId);
                    return View(usuario);
                }
                db.Usuarios.Add(usuario);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MunicipioId = new SelectList(db.Municipios, "Id", "Nombre", usuario.MunicipioId);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario != TipoUsuario.Administrador)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            TempData["IdUsuario"] = ((UsuarioMembership)Membership.GetUser()).Id;
            ViewBag.MunicipioId = new SelectList(db.Municipios, "Id", "Nombre", usuario.MunicipioId);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                if (Convert.ToInt32(TempData["IdUsuario"].ToString()) == usuario.Id)
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            ViewBag.MunicipioId = new SelectList(db.Municipios, "Id", "Nombre", usuario.MunicipioId);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario != TipoUsuario.Administrador)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = await db.Usuarios.FindAsync(id);
            if (((UsuarioMembership)Membership.GetUser()).Id == usuario.Id)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index");
            } 
            db.Usuarios.Remove(usuario);
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
