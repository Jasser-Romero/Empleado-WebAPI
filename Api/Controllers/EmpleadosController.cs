using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ViewModels;

namespace Api.Controllers
{
    public class EmpleadosController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<EmpleadoViewModel> list;
            using(EmpresaEntities db = new EmpresaEntities())
            {
                list = db.Empleados.Select(e => new EmpleadoViewModel()
                {
                    IdEmpleado = e.IdEmpleado,
                    Apellidos = e.Apellidos,
                    Cargo = e.Cargo,
                    Ciudad = e.Ciudad,
                    FechaContratacion = e.FechaContratacion,
                    Nombre = e.Nombre,
                    Telefono = e.Telefono
                }).ToList();

            }
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            EmpleadoViewModel model = null;

            using (EmpresaEntities db = new EmpresaEntities())
            {
                model = db.Empleados.Where(e => e.IdEmpleado == id).Select(e => new EmpleadoViewModel()
                {
                    IdEmpleado = e.IdEmpleado,
                    Apellidos = e.Apellidos,
                    Cargo = e.Cargo,
                    Ciudad = e.Ciudad,
                    FechaContratacion = e.FechaContratacion,
                    Nombre = e.Nombre,
                    Telefono = e.Telefono
                }).FirstOrDefault();
            }

            if (model == null)
                return NotFound();

            return Ok(model);
        }
        [HttpPost]
        public IHttpActionResult Add(EmpleadoViewModel empleado)
        {
            if (!ModelState.IsValid)
                return BadRequest("No es un modelo valido");
            using (EmpresaEntities db = new EmpresaEntities())
            {
                var oEmpleado = new Empleados()
                {
                    Apellidos = empleado.Apellidos,
                    Cargo = empleado.Cargo,
                    Ciudad = empleado.Ciudad,
                    FechaContratacion = empleado.FechaContratacion,
                    Nombre = empleado.Nombre,
                    Telefono = empleado.Telefono
                };
                db.Empleados.Add(oEmpleado);
                db.SaveChanges();
            }
            return Ok("Registro agregado correctamente");
        }

        [HttpPut]
        public IHttpActionResult Update(EmpleadoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("No es un modelo valido");

            using (EmpresaEntities db = new EmpresaEntities())
            {
                var oEmpleado = db.Empleados.Where(l => l.IdEmpleado == model.IdEmpleado)
                    .FirstOrDefault<Empleados>();
                if (oEmpleado != null)
                {
                    oEmpleado.Apellidos = model.Apellidos;
                    oEmpleado.Cargo = model.Cargo;
                    oEmpleado.Ciudad = model.Ciudad;
                    oEmpleado.FechaContratacion = model.FechaContratacion;
                    oEmpleado.Nombre = model.Nombre;
                    oEmpleado.Telefono = model.Telefono;
                    db.SaveChanges();
                }
                else
                    return NotFound();
            }
            return Ok("Se ha actualizado el empleado");
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("No es un id de empleado válido");

            using (EmpresaEntities db = new EmpresaEntities())
            {
                var empleado = db.Empleados.Where(x => x.IdEmpleado == id)
                    .FirstOrDefault();
                db.Entry(empleado).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            return Ok();
        }
    }
}
