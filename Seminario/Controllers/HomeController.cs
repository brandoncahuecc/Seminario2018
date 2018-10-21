using Seminario.Models;
using Seminario.Models.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Seminario.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        SeminarioContext db = new SeminarioContext();
        public ActionResult Index()
        {
            var user = (UsuarioMembership)Membership.GetUser();
            var logueado = db.Usuarios.Find(user.Id);
            //ViewBag.Mensaje = $"Bienvenido {logueado.Nombre} {logueado.Apellido} - {logueado.TipoUsuario}";
            ViewBag.Mensaje = $"{logueado.Nombre} {logueado.Apellido} ";

            return View();
        }
    }
}