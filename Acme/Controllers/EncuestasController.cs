﻿using Acme.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace Acme.Controllers
{
    public class EncuestasController : Controller
    {
        private AcmeContext db = new AcmeContext();

        // GET: Encuestas
        public ActionResult Index()
        {
            var campos = db.Campos.ToList();
            var encuestas = db.Encuestas.ToList();
            return View(encuestas);
        }

        // GET: Encuestas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encuesta encuesta = await db.Encuestas.FindAsync(id);
            var campos = db.Campos.Where(x => x.EncuestaId == id);

            foreach (Campo item in campos)
            {
                encuesta.Campos.Add(item);
            }

            if (encuesta == null)
            {
                return HttpNotFound();
            }
            return View(encuesta);
        }

        public async Task<ActionResult> MostrarEncuesta(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Encuesta encuesta = await db.Encuestas.FindAsync(id);
            var campos = db.Campos.Where(x => x.EncuestaId == id);
            foreach (Campo campo in campos)
            {
                encuesta.Campos.Add(campo);
            }

            if (encuesta == null)
            {
                return HttpNotFound();
            }

            return View(encuesta);
        }

        // GET: Encuestas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Encuestas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre,Descripcion")] Encuesta encuesta)
        {
            if (ModelState.IsValid)
            {
                db.Encuestas.Add(encuesta);
                await db.SaveChangesAsync();
                return RedirectToAction($"../Encuestas/Index");
            }

            return View(encuesta);
        }

        // GET: Encuestas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encuesta encuesta = await db.Encuestas.FindAsync(id);
            if (encuesta == null)
            {
                return HttpNotFound();
            }
            return View(encuesta);
        }

        // POST: Encuestas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Descripcion")] Encuesta encuesta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(encuesta).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(encuesta);
        }

        // GET: Encuestas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encuesta encuesta = await db.Encuestas.FindAsync(id);
            if (encuesta == null)
            {
                return HttpNotFound();
            }
            return View(encuesta);
        }

        // POST: Encuestas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Encuesta encuesta = await db.Encuestas.FindAsync(id);
            db.Encuestas.Remove(encuesta);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CreateUrl(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var campos = db.Campos.Where(x => x.EncuestaId == id);
            Encuesta encuesta = await db.Encuestas.FindAsync(id);

            foreach (var item in campos)
            {
                encuesta.Campos.Add(item);
            }

            if (String.IsNullOrEmpty(encuesta.Url))
            {
                encuesta.Url = $"/Encuestas/MostrarEncuesta/{id}";
                db.Entry(encuesta).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            string tabla = $"create table {encuesta.Nombre.Trim().ToLower().Replace(" ", "")}" +
                $"(id int primary key IDENTITY(1, 1),";
            string aux = "";
            foreach (var item in campos)
            {
                switch (item.TipoCampo)
                {
                    case TipoCampo.Texto:
                        if (item.EsRequerido)
                        {
                            aux += aux.Equals(string.Empty)
                                ? $"{item.Nombre.Trim().ToLower().Replace(" ", "")} varchar(200) NOT NULL"
                                : $",{item.Nombre.Trim().ToLower().Replace(" ", "")} varchar(200) NOT NULL";
                        }
                        else
                        {
                            aux += aux.Equals(string.Empty)
                                ? $"{item.Nombre.Trim().ToLower().Replace(" ", "")} varchar(200)"
                                : $",{item.Nombre.Trim().ToLower().Replace(" ", "")} varchar(200)";
                        }
                        break;
                    case TipoCampo.Numero:
                        if (item.EsRequerido)
                        {
                            aux += aux.Equals(string.Empty)
                                ? $"{item.Nombre.Trim().ToLower().Replace(" ", "")} int NOT NULL"
                                : $",{item.Nombre.Trim().ToLower().Replace(" ", "")} int NOT NULL";
                        }
                        else
                        {
                            aux += aux.Equals(string.Empty)
                                ? $"{item.Nombre.Trim().ToLower().Replace(" ", "")} int"
                                : $",{item.Nombre.Trim().ToLower().Replace(" ", "")} int";
                        }
                        break;
                    case TipoCampo.Fecha:
                        if (item.EsRequerido)
                        {
                            aux += aux.Equals(string.Empty)
                                ? $"{item.Nombre.Trim().ToLower().Replace(" ", "")} date NOT NULL"
                                : $",{item.Nombre.Trim().ToLower().Replace(" ", "")} date NOT NULL";
                        }
                        else
                        {
                            aux += aux.Equals(string.Empty)
                                ? $"{item.Nombre.Trim().ToLower().Replace(" ", "")} date"
                                : $",{item.Nombre.Trim().ToLower().Replace(" ", "")} date";
                        }
                        break;
                }

            }
            tabla += aux + $")";

            var sqlcommand = String.Format(tabla);
            db.Database.ExecuteSqlCommand(sqlcommand);
            return RedirectToAction("Index");
        }

        public ActionResult AgregarCampo(int id)
        {
            Campo campo = new Campo();
            campo.EncuestaId = id;
            ViewBag.EncuestaId = new SelectList(db.Encuestas, "Id", "Nombre", campo.EncuestaId);
            return View(campo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarCampo([Bind(Include = "Id,Nombre,Titulo,EsRequerido,TipoCampo,EncuestaId")] Campo campo)
        {
            if (ModelState.IsValid)
            {
                db.Campos.Add(campo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EncuestaId = new SelectList(db.Encuestas, "Id", "Nombre", campo.EncuestaId);
            return View(campo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MostrarEncuesta(Encuesta encuesta)
        {

            var sqlcommand = String.Format("");
            db.Database.ExecuteSqlCommand(sqlcommand);
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
