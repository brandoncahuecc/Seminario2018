using Seminario.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using System.Web.Security;
using Seminario.Models.Seguridad;

namespace Seminario.Controllers
{
    [Authorize]
    public class FaesController : Controller
    {
        private SeminarioContext db = new SeminarioContext();
        public ActionResult Index()
        {
            var user = (UsuarioMembership)Membership.GetUser();
            var logueado = db.Usuarios.Include(c => c.Municipio).FirstOrDefault(c => c.Id == user.Id);
            if (logueado.TipoUsuario != TipoUsuario.Administrador)
            {
                return RedirectToAction("Index", "Home");
            }

            Filtro filtro = new Filtro()
            {
                Anio = (Anio)DateTime.Now.Year,
                Mes = (Mes)DateTime.Now.Month - 1,
                Dia = 0,
                Hora = 0
            };
            var municipios = db.Municipios.Where(c => c.Comisaria == logueado.Municipio.Comisaria).ToList();
            ViewBag.MunicipioId = new SelectList(municipios, "Id", "Nombre");
            return View(filtro);
        }

        [HttpPost]
        public ActionResult Index(Filtro filtro)
        {
            var user = (UsuarioMembership)Membership.GetUser();
            var logueado = db.Usuarios.Include(c => c.Municipio).FirstOrDefault(c => c.Id == user.Id);
            var municipios = db.Municipios.Where(c => c.Comisaria == logueado.Municipio.Comisaria).ToList();
            if (ModelState.IsValid)
            {
                int dias = DateTime.DaysInMonth((int)filtro.Anio, (int)filtro.Mes + 1);
                if (dias < filtro.Dia)
                {
                    ViewBag.Mensaje = $"El mes solo tiene {dias} dias";
                    ViewBag.MunicipioId = new SelectList(municipios, "Id", "Nombre", filtro.MunicipioId);
                    return View(filtro);
                }

                TempData["nombreCompleto"] = filtro;
                return RedirectToAction("Graficas");
            }

            ViewBag.MunicipioId = new SelectList(municipios, "Id", "Nombre", filtro.MunicipioId);
            return View(filtro);
        }

        public ActionResult Export()
        {
            var user = (UsuarioMembership)Membership.GetUser();
            var logueado = db.Usuarios.Include(c => c.Municipio).FirstOrDefault(c => c.Id == user.Id);
            if (logueado.TipoUsuario != TipoUsuario.Administrador)
            {
                return RedirectToAction("Index", "Home");
            }
            Excel excel = new Excel()
            {
                Anio = (Anio)DateTime.Now.Year,
                Mes = (Mes)DateTime.Now.Month - 1
            };
            var municipios = db.Municipios.Where(c => c.Comisaria == logueado.Municipio.Comisaria).ToList();
            ViewBag.MunicipioId = new SelectList(municipios, "Id", "Nombre");
            return View(excel);
        }

        [HttpPost]
        public ActionResult Export(Excel excel)
        {
            if (ModelState.IsValid)
            {
                List<HechoDelictivo> hechosDelictivos = ListaExport(excel);

                if (hechosDelictivos.Count() > 0)
                {
                    DataTable dtVida = new DataTable("Contra la Vida");
                    DataTable dtPatrimonio = new DataTable("Contra el Patrimonio");
                    dtVida.Columns.AddRange(
                        new DataColumn[10]{
                        new DataColumn("Oficinista"),
                        new DataColumn("Fecha"),
                        new DataColumn("Lugar"),
                        new DataColumn("Dirección"),
                        new DataColumn("Nombre de la Victima"),
                        new DataColumn("Género"),
                        new DataColumn("Edad"),
                        new DataColumn("Delito"),
                        new DataColumn("Causa"),
                        new DataColumn("Observaciones")
                    });

                    dtPatrimonio.Columns.AddRange(
                        new DataColumn[21]{
                        new DataColumn("Oficinista"),
                        new DataColumn("Fecha"),
                        new DataColumn("Lugar"),
                        new DataColumn("Dirección"),
                        new DataColumn("Nombre de la Victima"),
                        new DataColumn("Género"),
                        new DataColumn("Edad"),
                        new DataColumn("Delito"),
                        new DataColumn("Tipo"),
                        new DataColumn("Placas"),
                        new DataColumn("Marca"),
                        new DataColumn("Color"),
                        new DataColumn("Modelo"),
                        new DataColumn("Motor"),
                        new DataColumn("Chasis"),
                        new DataColumn("Móvil"),
                        new DataColumn("Registro"),
                        new DataColumn("Calibre"),
                        new DataColumn("Oficio"),
                        new DataColumn("Denunciante"),
                        new DataColumn("Observaciones")
                        });
                    if (hechosDelictivos.Where(c => c.Delito.TipoDelito == TipoDelito.ContraLaVida).Count() > 0)
                    {
                        foreach (var item in hechosDelictivos.Where(c => c.Delito.TipoDelito == TipoDelito.ContraLaVida))
                        {
                            dtVida.Rows.Add(
                               $"{item.Usuario.Nombre} {item.Usuario.Apellido}",
                               item.Fecha,
                               item.Lugar.Nombre,
                               item.Direccion,
                               item.NombreVictima,
                               item.Genero,
                               item.Edad,
                               item.Delito.Nombre,
                               item.Causa,
                               item.Observaciones
                                );
                        }
                    }
                    if (hechosDelictivos.Where(c => c.Delito.TipoDelito == TipoDelito.ContraElPatrimonio).Count() > 0)
                    {
                        foreach (var item in hechosDelictivos.Where(c => c.Delito.TipoDelito == TipoDelito.ContraElPatrimonio))
                        {
                            dtPatrimonio.Rows.Add(
                               $"{item.Usuario.Nombre} {item.Usuario.Apellido}",
                               item.Fecha,
                               item.Lugar.Nombre,
                               item.Direccion,
                               item.NombreVictima,
                               item.Genero,
                               item.Edad,
                               item.Delito.Nombre,
                               item.Tipo,
                               item.Placas,
                               item.Marca,
                               item.Color,
                               item.Modelo,
                               item.Motor,
                               item.Chasis,
                               item.Movil,
                               item.Registro,
                               item.Calibre,
                               item.Oficio,
                               item.Denunciante,
                               item.Observaciones
                                );
                        }
                    }

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        if (hechosDelictivos.Where(c => c.Delito.TipoDelito == TipoDelito.ContraLaVida).Count() > 0)
                        {
                            wb.Worksheets.Add(dtVida);
                        }
                        if (hechosDelictivos.Where(c => c.Delito.TipoDelito == TipoDelito.ContraElPatrimonio).Count() > 0)
                        {
                            wb.Worksheets.Add(dtPatrimonio);
                        }
                        using (MemoryStream stream = new MemoryStream())
                        {
                            ViewBag.Mensaje = "";
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"HechosDelictivos{hechosDelictivos[0].Lugar.Municipio.Nombre}{excel.Mes}{excel.Anio}.xlsx");
                        }
                    }

                }
            }
            ViewBag.Mensaje = "El mes no contiene datos que exportar.";
            var user = (UsuarioMembership)Membership.GetUser();
            var logueado = db.Usuarios.Include(c => c.Municipio).FirstOrDefault(c => c.Id == user.Id);
            var municipios = db.Municipios.Where(c => c.Comisaria == logueado.Municipio.Comisaria).ToList();
            ViewBag.MunicipioId = new SelectList(municipios, "Id", "Nombre", excel.MunicipioId);
            return View(excel);
        }

        public List<HechoDelictivo> ListaExport(Excel excel)
        {
            return db.HechosDelictivos
                    .Include(c => c.Usuario)
                    .Include(c => c.Lugar)
                    .Include(c => c.Lugar.Municipio)
                    .Include(c => c.Delito)
                    .Where(c => c.Lugar.Municipio.Id == excel.MunicipioId
                    && c.Fecha.Year == (int)excel.Anio
                    && c.Fecha.Month == (int)excel.Mes + 1)
                    .ToList();
        }

        public ActionResult Graficas()
        {
            Filtro filtro = TempData["nombreCompleto"] as Filtro;

            if (filtro == null)
            {
                return RedirectToAction("Index");
            }
            Municipio municipio = db.Municipios.Find(filtro.MunicipioId);
            ViewBag.Mes = filtro.Mes;
            ViewBag.Filtro = JsonConvert.SerializeObject(filtro);
            ViewBag.Municipio = municipio.Nombre;
            return View();
        }


        //Repositorio Delitos
        public JsonResult DelitosComunes(string filt)
        {
            List<IGrouping<int, HechoDelictivo>> datos = new List<IGrouping<int, HechoDelictivo>>();
            Filtro filtro = JsonConvert.DeserializeObject<Filtro>(filt);

            //1
            if (filtro.TipoDelito == 0
                && filtro.Semana == Semana.SeleccionOpcional
                && filtro.Dia == 0 && filtro.Hora == 0)
            {
                datos = Delito1(filtro);
            }

            //2
            if (filtro.TipoDelito != 0
                && filtro.Semana == Semana.SeleccionOpcional
                && filtro.Dia == 0 && filtro.Hora == 0)
            {
                datos = Delito2(filtro);
            }

            //3
            if (filtro.TipoDelito == 0
                && filtro.Semana != Semana.SeleccionOpcional
                && filtro.Dia == 0 && filtro.Hora == 0)
            {
                datos = Delito3(filtro);
            }

            //4
            if (filtro.TipoDelito != 0
                && filtro.Semana != Semana.SeleccionOpcional
                && filtro.Dia == 0 && filtro.Hora == 0)
            {
                datos = Delito4(filtro);
            }

            //5
            if (filtro.TipoDelito == 0
                && filtro.Dia > 0 && filtro.Hora == 0)
            {
                datos = Delito5(filtro);
            }

            //6
            if (filtro.TipoDelito != 0
                && filtro.Dia > 0 && filtro.Hora == 0)
            {
                datos = Delito6(filtro);
            }

            //7
            if (filtro.TipoDelito == 0
                && filtro.Dia > 0 && filtro.Hora > 0)
            {
                if (filtro.Hora == 24)
                {
                    filtro.Hora = 0;
                }
                datos = Delito7(filtro);
            }

            //8
            if (filtro.TipoDelito != 0
                && filtro.Dia > 0 && filtro.Hora > 0)
            {
                if (filtro.Hora == 24)
                {
                    filtro.Hora = 0;
                }
                datos = Delito8(filtro);
            }

            List<Ejemplo> ejemplos = new List<Ejemplo>();
            foreach (var item in datos)
            {
                var delito = db.Delitos.Find(item.Key);

                ejemplos.Add(
                    new Ejemplo()
                    {
                        Nombre = delito.Nombre,
                        Numero = item.Count()
                    });
            }

            return (Json(ejemplos, JsonRequestBehavior.AllowGet));
        }

        //1
        public List<IGrouping<int, HechoDelictivo>> Delito1(Filtro filtro)
        {
            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio)
                .GroupBy(d => d.DelitoId).ToList();
        }

        //2
        public List<IGrouping<int, HechoDelictivo>> Delito2(Filtro filtro)
        {
            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio
                && c.Delito.TipoDelito == filtro.TipoDelito)
                .GroupBy(d => d.DelitoId).ToList();
        }

        //3
        public List<IGrouping<int, HechoDelictivo>> Delito3(Filtro filtro)
        {
            int[] rango = new int[2];
            rango[1] = (int)filtro.Semana * 7;
            rango[0] = rango[1] - 6;

            int dias = DateTime.DaysInMonth((int)filtro.Anio, (int)filtro.Mes + 1);

            if (rango[1] > dias)
            {
                rango[1] = dias;
            }

            string fecha1 = $"{rango[0]}/{(int)filtro.Mes + 1}/2018";
            string fecha2 = $"{rango[1]}/{(int)filtro.Mes + 1}/2018";

            DateTime fechaMin, fechaMax;
            fechaMin = DateTime.Parse(fecha1);
            fechaMax = DateTime.Parse(fecha2);

            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio
                && c.Fecha >= fechaMin
                && c.Fecha <= fechaMax)
                .GroupBy(d => d.DelitoId).ToList();
        }

        //4
        public List<IGrouping<int, HechoDelictivo>> Delito4(Filtro filtro)
        {
            int[] rango = new int[2];
            rango[1] = (int)filtro.Semana * 7;
            rango[0] = rango[1] - 6;

            int dias = DateTime.DaysInMonth((int)filtro.Anio, (int)filtro.Mes + 1);

            if (rango[1] > dias)
            {
                rango[1] = dias;
            }

            string fecha1 = $"{rango[0]}/{(int)filtro.Mes + 1}/2018";
            string fecha2 = $"{rango[1]}/{(int)filtro.Mes + 1}/2018";

            DateTime fechaMin, fechaMax;
            fechaMin = DateTime.Parse(fecha1);
            fechaMax = DateTime.Parse(fecha2);

            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio
                && c.Delito.TipoDelito == filtro.TipoDelito
                && c.Fecha >= fechaMin
                && c.Fecha <= fechaMax)
                .GroupBy(d => d.DelitoId).ToList();
        }

        //5
        public List<IGrouping<int, HechoDelictivo>> Delito5(Filtro filtro)
        {
            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio
                && c.Fecha.Day == filtro.Dia)
                .GroupBy(d => d.DelitoId).ToList();
        }

        //6
        public List<IGrouping<int, HechoDelictivo>> Delito6(Filtro filtro)
        {
            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio
                && c.Delito.TipoDelito == filtro.TipoDelito
                && c.Fecha.Day == filtro.Dia)
                .GroupBy(d => d.DelitoId).ToList();
        }

        //7
        public List<IGrouping<int, HechoDelictivo>> Delito7(Filtro filtro)
        {
            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio
                && c.Fecha.Day == filtro.Dia
                && c.Fecha.Hour == filtro.Hora)
                .GroupBy(d => d.DelitoId).ToList();
        }

        //8
        public List<IGrouping<int, HechoDelictivo>> Delito8(Filtro filtro)
        {
            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio
                && c.Delito.TipoDelito == filtro.TipoDelito
                && c.Fecha.Day == filtro.Dia
                && c.Fecha.Hour == filtro.Hora)
                .GroupBy(d => d.DelitoId).ToList();
        }


        //Repositorio Lugares
        public JsonResult LugaresComunes(string filt)
        {
            List<IGrouping<int, HechoDelictivo>> datos = new List<IGrouping<int, HechoDelictivo>>();
            Filtro filtro = JsonConvert.DeserializeObject<Filtro>(filt);

            //1
            if (filtro.TipoDelito == 0
                && filtro.Semana == Semana.SeleccionOpcional
                && filtro.Dia == 0 && filtro.Hora == 0)
            {
                datos = Lugar1(filtro);
            }

            //2
            if (filtro.TipoDelito != 0
                && filtro.Semana == Semana.SeleccionOpcional
                && filtro.Dia == 0 && filtro.Hora == 0)
            {
                datos = Lugar2(filtro);
            }

            //3
            if (filtro.TipoDelito == 0
                && filtro.Semana != Semana.SeleccionOpcional
                && filtro.Dia == 0 && filtro.Hora == 0)
            {
                datos = Lugar3(filtro);
            }

            //4
            if (filtro.TipoDelito != 0
                && filtro.Semana != Semana.SeleccionOpcional
                && filtro.Dia == 0 && filtro.Hora == 0)
            {
                datos = Lugar4(filtro);
            }

            //5
            if (filtro.TipoDelito == 0
                && filtro.Dia > 0 && filtro.Hora == 0)
            {
                datos = Lugar5(filtro);
            }

            //6
            if (filtro.TipoDelito != 0
                && filtro.Dia > 0 && filtro.Hora == 0)
            {
                datos = Lugar6(filtro);
            }

            //7
            if (filtro.TipoDelito == 0
                && filtro.Dia > 0 && filtro.Hora > 0)
            {
                if (filtro.Hora == 24)
                {
                    filtro.Hora = 0;
                }
                datos = Lugar7(filtro);
            }

            //8
            if (filtro.TipoDelito != 0
                && filtro.Dia > 0 && filtro.Hora > 0)
            {
                if (filtro.Hora == 24)
                {
                    filtro.Hora = 0;
                }
                datos = Lugar8(filtro);
            }

            List<Ejemplo> ejemplos = new List<Ejemplo>();
            foreach (var item in datos)
            {
                var lugar = db.Lugares.Find(item.Key);

                ejemplos.Add(
                    new Ejemplo()
                    {
                        Nombre = lugar.Nombre,
                        Numero = item.Count()
                    });
            }

            return (Json(ejemplos, JsonRequestBehavior.AllowGet));
        }

        //1
        public List<IGrouping<int, HechoDelictivo>> Lugar1(Filtro filtro)
        {
            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio)
                .GroupBy(d => d.LugarId).ToList();
        }

        //2
        public List<IGrouping<int, HechoDelictivo>> Lugar2(Filtro filtro)
        {
            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio
                && c.Delito.TipoDelito == filtro.TipoDelito)
                .GroupBy(d => d.LugarId).ToList();
        }

        //3
        public List<IGrouping<int, HechoDelictivo>> Lugar3(Filtro filtro)
        {
            int[] rango = new int[2];
            rango[1] = (int)filtro.Semana * 7;
            rango[0] = rango[1] - 6;

            int dias = DateTime.DaysInMonth((int)filtro.Anio, (int)filtro.Mes + 1);

            if (rango[1] > dias)
            {
                rango[1] = dias;
            }

            string fecha1 = $"{rango[0]}/{(int)filtro.Mes + 1}/2018";
            string fecha2 = $"{rango[1]}/{(int)filtro.Mes + 1}/2018";

            DateTime fechaMin, fechaMax;
            fechaMin = DateTime.Parse(fecha1);
            fechaMax = DateTime.Parse(fecha2);

            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio
                && c.Fecha >= fechaMin
                && c.Fecha <= fechaMax)
                .GroupBy(d => d.LugarId).ToList();
        }

        //4
        public List<IGrouping<int, HechoDelictivo>> Lugar4(Filtro filtro)
        {
            int[] rango = new int[2];
            rango[1] = (int)filtro.Semana * 7;
            rango[0] = rango[1] - 6;

            int dias = DateTime.DaysInMonth((int)filtro.Anio, (int)filtro.Mes + 1);

            if (rango[1] > dias)
            {
                rango[1] = dias;
            }

            string fecha1 = $"{rango[0]}/{(int)filtro.Mes + 1}/2018";
            string fecha2 = $"{rango[1]}/{(int)filtro.Mes + 1}/2018";

            DateTime fechaMin, fechaMax;
            fechaMin = DateTime.Parse(fecha1);
            fechaMax = DateTime.Parse(fecha2);

            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio
                && c.Delito.TipoDelito == filtro.TipoDelito
                && c.Fecha >= fechaMin
                && c.Fecha <= fechaMax)
                .GroupBy(d => d.LugarId).ToList();
        }

        //5
        public List<IGrouping<int, HechoDelictivo>> Lugar5(Filtro filtro)
        {
            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio
                && c.Fecha.Day == filtro.Dia)
                .GroupBy(d => d.LugarId).ToList();
        }

        //6
        public List<IGrouping<int, HechoDelictivo>> Lugar6(Filtro filtro)
        {
            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio
                && c.Delito.TipoDelito == filtro.TipoDelito
                && c.Fecha.Day == filtro.Dia)
                .GroupBy(d => d.LugarId).ToList();
        }

        //7
        public List<IGrouping<int, HechoDelictivo>> Lugar7(Filtro filtro)
        {
            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio
                && c.Fecha.Day == filtro.Dia
                && c.Fecha.Hour == filtro.Hora)
                .GroupBy(d => d.LugarId).ToList();
        }

        //8
        public List<IGrouping<int, HechoDelictivo>> Lugar8(Filtro filtro)
        {
            return db.HechosDelictivos.Include(c => c.Delito)
                .Where(c => c.Lugar.MunicipioId == filtro.MunicipioId
                && c.Fecha.Month == (int)filtro.Mes + 1
                && c.Fecha.Year == (int)filtro.Anio
                && c.Delito.TipoDelito == filtro.TipoDelito
                && c.Fecha.Day == filtro.Dia
                && c.Fecha.Hour == filtro.Hora)
                .GroupBy(d => d.LugarId).ToList();
        }

    }
}