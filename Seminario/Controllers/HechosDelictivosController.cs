using Seminario.Models;
using Seminario.Models.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net;

namespace Seminario.Controllers
{
    [Authorize]
    public class HechosDelictivosController : Controller
    {
        private SeminarioContext db = new SeminarioContext();

        // GET: HechosDelictivos
        public ActionResult Index()
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario == TipoUsuario.ControlMapa)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult ContraLaVida()
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario == TipoUsuario.ControlMapa)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = (UsuarioMembership)Membership.GetUser();
            var logueado = db.Usuarios.Find(user.Id);
            ViewBag.LugarId = new SelectList(db.Lugares.ToList()
                .Where(c => c.MunicipioId == logueado.MunicipioId), "Id", "Nombre");
            ViewBag.DelitoId = new SelectList(db.Delitos.ToList()
                .Where(c => c.TipoDelito == TipoDelito.ContraLaVida), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContraLaVida(Vida vida)
        {
            var user = (UsuarioMembership)Membership.GetUser();
            var logueado = db.Usuarios.Find(user.Id);
            vida.UsuarioId = logueado.Id;

            if (ModelState.IsValid)
            {
                HechoDelictivo hechoDelictivo = TransformarVida(vida);
                db.HechosDelictivos.Add(hechoDelictivo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.LugarId = new SelectList(db.Lugares.ToList()
                .Where(c => c.MunicipioId == logueado.MunicipioId),
                "Id", "Nombre", vida.LugarId);
            ViewBag.DelitoId = new SelectList(db.Delitos.ToList()
                .Where(c => c.TipoDelito == TipoDelito.ContraLaVida),
                "Id", "Nombre", vida.DelitoId);
            return View(vida);

        }

        private HechoDelictivo TransformarVida(Vida vida)
        {
            HechoDelictivo hechoDelictivo = new HechoDelictivo()
            {
                UsuarioId = vida.UsuarioId,
                Fecha = vida.Fecha.AddHours(vida.Hora.Hour).AddMinutes(vida.Hora.Minute),
                LugarId = vida.LugarId,
                Direccion = vida.Direccion,
                NombreVictima = vida.NombreVictima,
                Genero = vida.Genero,
                Edad = vida.Edad,
                DelitoId = vida.DelitoId,
                Causa = vida.Causa,
                Observaciones = vida.Observaciones,
                FechaIngreso = DateTime.Now.ToLocalTime()
            };
            return hechoDelictivo;
        }

        public ActionResult ContraElPatrimonio()
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario == TipoUsuario.ControlMapa)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = (UsuarioMembership)Membership.GetUser();
            var logueado = db.Usuarios.Find(user.Id);
            ViewBag.LugarId = new SelectList(db.Lugares.ToList()
                .Where(c => c.MunicipioId == logueado.MunicipioId), "Id", "Nombre");
            ViewBag.DelitoId = new SelectList(db.Delitos.ToList()
                .Where(c => c.TipoDelito == TipoDelito.ContraElPatrimonio), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContraElPatrimonio(Patrimonio patrimonio)
        {
            var user = (UsuarioMembership)Membership.GetUser();
            var logueado = db.Usuarios.Find(user.Id);
            patrimonio.UsuarioId = logueado.Id;

            if (ModelState.IsValid)
            {
                HechoDelictivo hechoDelictivo = TransformarPatrimonio(patrimonio);
                db.HechosDelictivos.Add(hechoDelictivo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.LugarId = new SelectList(db.Lugares.ToList()
                .Where(c => c.MunicipioId == logueado.MunicipioId),
                "Id", "Nombre", patrimonio.LugarId);
            ViewBag.DelitoId = new SelectList(db.Delitos.ToList()
                .Where(c => c.TipoDelito == TipoDelito.ContraElPatrimonio),
                "Id", "Nombre", patrimonio.DelitoId);
            return View(patrimonio);

        }

        private HechoDelictivo TransformarPatrimonio(Patrimonio patrimonio)
        {
            HechoDelictivo hechoDelictivo = new HechoDelictivo()
            {
                UsuarioId = patrimonio.UsuarioId,
                Fecha = patrimonio.Fecha.AddHours(patrimonio.Hora.Hour).AddMinutes(patrimonio.Hora.Minute),
                LugarId = patrimonio.LugarId,
                Direccion = patrimonio.Direccion,
                NombreVictima = patrimonio.NombreVictima,
                Genero = patrimonio.Genero,
                Edad = patrimonio.Edad,
                Tipo = patrimonio.Tipo,
                Placas = patrimonio.Placas,
                Marca = patrimonio.Marca,
                Color = patrimonio.Color,
                Modelo = patrimonio.Modelo,
                Motor = patrimonio.Motor,
                Chasis = patrimonio.Chasis,
                Movil = patrimonio.Movil.ToString(),
                Registro = patrimonio.Registro,
                Calibre = patrimonio.Calibre,
                DelitoId = patrimonio.DelitoId,
                Oficio = patrimonio.Oficio.ToString(),
                Denunciante = patrimonio.Denunciante,
                Observaciones = patrimonio.Observaciones,
                FechaIngreso = DateTime.Now.ToLocalTime()
            };
            return hechoDelictivo;
        }


        public ActionResult LContraLaVida()
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario == TipoUsuario.ControlMapa)
            {
                return RedirectToAction("Index", "Home");
            }
            List<HechoDelictivo> delitos;
            List<Vida> vidas;
            var user = (UsuarioMembership)Membership.GetUser();
            var logueado = db.Usuarios.Include(c => c.Municipio).FirstOrDefault(c => c.Id == user.Id);
            if (logueado.TipoUsuario == TipoUsuario.Administrador)
            {
                delitos = db.HechosDelictivos.Include(c => c.Usuario)
                .Include(c => c.Lugar).Include(c => c.Delito).Include(c => c.Lugar.Municipio)
                .Where(c => c.Delito.TipoDelito == TipoDelito.ContraLaVida
                && c.Lugar.Municipio.Comisaria == logueado.Municipio.Comisaria)
                .OrderByDescending(c => c.Fecha).OrderByDescending(c => c.FechaIngreso).ToList();
                vidas = DelitosAVida(delitos);
                return View(vidas);
            }
            delitos = db.HechosDelictivos.Include(c => c.Usuario)
                .Include(c => c.Lugar).Include(c => c.Delito)
                .Where(c => c.Delito.TipoDelito == TipoDelito.ContraLaVida
                && c.Lugar.MunicipioId == logueado.MunicipioId)
                .OrderByDescending(c => c.Fecha).OrderByDescending(c => c.FechaIngreso).ToList();
            vidas = DelitosAVida(delitos);
            return View(vidas);
        }

        private List<Vida> DelitosAVida(List<HechoDelictivo> delitos)
        {
            List<Vida> vidas = new List<Vida>();
            foreach (var item in delitos)
            {

                vidas.Add(
                new Vida()
                {
                    Id = item.Id,
                    UsuarioId = item.UsuarioId,
                    Fecha = item.Fecha,
                    Hora = item.Fecha,
                    LugarId = item.LugarId,
                    Direccion = item.Direccion,
                    NombreVictima = item.NombreVictima,
                    Genero = item.Genero,
                    Edad = item.Edad,
                    DelitoId = item.DelitoId,
                    Causa = item.Causa,
                    Observaciones = item.Observaciones,
                    Usuario = item.Usuario,
                    Delito = item.Delito,
                    Lugar = item.Lugar
                });
            }
            return vidas;
        }

        public ActionResult LContraElPatrimonio()
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario == TipoUsuario.ControlMapa)
            {
                return RedirectToAction("Index", "Home");
            }
            List<HechoDelictivo> delitos;
            List<Patrimonio> patrimonios;

            var user = (UsuarioMembership)Membership.GetUser();
            var logueado = db.Usuarios.Include(c => c.Municipio).FirstOrDefault(c => c.Id == user.Id);
            if (logueado.TipoUsuario == TipoUsuario.Administrador)
            {
                delitos = db.HechosDelictivos.Include(c => c.Usuario)
                .Include(c => c.Lugar).Include(c => c.Delito)
                .Where(c => c.Delito.TipoDelito == TipoDelito.ContraElPatrimonio
                && c.Lugar.Municipio.Comisaria == logueado.Municipio.Comisaria)
                .OrderByDescending(c => c.Fecha).OrderByDescending(c => c.FechaIngreso).ToList();
                patrimonios = DelitosAPatrimonio(delitos);
                return View(patrimonios);
            }
            delitos = db.HechosDelictivos.Include(c => c.Usuario)
                .Include(c => c.Lugar).Include(c => c.Delito)
                .Where(c => c.Delito.TipoDelito == TipoDelito.ContraElPatrimonio
                && c.Lugar.MunicipioId == logueado.MunicipioId)
                .OrderByDescending(c => c.Fecha).OrderByDescending(c => c.FechaIngreso).ToList();
            patrimonios = DelitosAPatrimonio(delitos);
            return View(patrimonios);
        }

        private List<Patrimonio> DelitosAPatrimonio(List<HechoDelictivo> delitos)
        {
            List<Patrimonio> patrimonios = new List<Patrimonio>();
            foreach (var item in delitos)
            {

                patrimonios.Add(
                new Patrimonio()
                {
                    Id = item.Id,
                    UsuarioId = item.UsuarioId,
                    Fecha = item.Fecha,
                    Hora = item.Fecha,
                    LugarId = item.LugarId,
                    Direccion = item.Direccion,
                    NombreVictima = item.NombreVictima,
                    Genero = item.Genero,
                    Edad = item.Edad,
                    Tipo = item.Tipo,
                    Placas = item.Placas,
                    Marca = item.Marca,
                    Color = item.Color,
                    Modelo = item.Modelo,
                    Movil = Convert.ToInt32(item.Movil),
                    Registro = item.Registro,
                    Calibre = item.Calibre,
                    DelitoId = item.DelitoId,
                    Oficio = Convert.ToInt32(item.Oficio),
                    Denunciante = item.Denunciante,
                    Observaciones = item.Observaciones,
                    Usuario = item.Usuario,
                    Delito = item.Delito,
                    Lugar = item.Lugar
                });
            }
            return patrimonios;
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (((UsuarioMembership)Membership.GetUser()).TipoUsuario == TipoUsuario.ControlMapa)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return null;
            }
            HechoDelictivo hechoDelictivo = await db.HechosDelictivos.FindAsync(id);

            if ((DateTime.Now - hechoDelictivo.FechaIngreso).TotalMinutes < 30)
            {
                db.HechosDelictivos.Remove(hechoDelictivo);
                await db.SaveChangesAsync();
            }
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