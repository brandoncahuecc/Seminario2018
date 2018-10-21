using Seminario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Seminario.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            using (var context = new SeminarioContext())
            {
                if (context.Municipios.ToList().Count == 0)
                {
                    Municipio municipio = new Municipio()
                    {
                        Nombre = "Salama",
                        Comisaria = Comisaria.C52_1
                    };
                    context.Municipios.Add(municipio);
                    Usuario usuario = new Usuario()
                    {
                        Nombre = "ADMIN",
                        Apellido = "ISTRADOR",
                        Telefono = 00000000,
                        Correo = "admin@admin.com",
                        User = "admin",
                        Password = "salama2018",
                        MunicipioId = municipio.Id,
                        TipoUsuario = TipoUsuario.Administrador,
                        Estado = EstadoUsuario.Activo,
                    };
                    context.Usuarios.Add(usuario);
                    context.SaveChanges();
                }
                if (context.Usuarios.Where(c => c.TipoUsuario == TipoUsuario.Administrador).ToList().Count == 0)
                {
                    Municipio municipio = context.Municipios.FirstOrDefault();
                    Usuario usuario = new Usuario()
                    {
                        Nombre = "ADMIN",
                        Apellido = "ISTRADOR",
                        Telefono = 00000000,
                        Correo = "admin@admin.com",
                        User = "admin",
                        Password = "salama2018",
                        MunicipioId = municipio.Id,
                        TipoUsuario = TipoUsuario.Administrador,
                        Estado = EstadoUsuario.Activo,
                    };
                    context.Usuarios.Add(usuario);
                    context.SaveChanges();
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(UsuarioLogin usuario)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(usuario.User, usuario.Password))
                {
                    FormsAuthentication.RedirectFromLoginPage(usuario.User, false);
                    return null;
                }
            }
            ViewBag.Mensaje = "Usuario o Contraseña Incorrecta";
            return View(usuario);
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}