using LoginEmpleado.CRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginEmpleado.CRUD.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmpleadoController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewAll()
        {
            return View(GetAllEmpleado());
        }

        IEnumerable<Empleado> GetAllEmpleado()
        {
            using (DBModel db = new DBModel())
            {
                //return db.Empleados.ToList();
                return db.Empleadoes.ToList<Empleado>();
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            Empleado emp = new Empleado();
            if (id != 0)
            {
                using (DBModel db = new DBModel())
                {
                    emp = db.Empleadoes.Where(x => x.EmpleadoID == id).FirstOrDefault<Empleado>();
                }
            }
            return View(emp);
        }

        [HttpPost]
        public ActionResult AddOrEdit(Empleado emp)
        {
            try
            {
                if (emp.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(emp.ImageUpload.FileName);
                    string extension = Path.GetExtension(emp.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    emp.ImagePath = "~/AppFiles/Images/" + fileName;
                    emp.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                }
                using (DBModel db = new DBModel())
                {
                    if (emp.EmpleadoID == 0)
                    {
                        db.Empleadoes.Add(emp);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(emp).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmpleado()), message = "Envío exitoso" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                using (DBModel db = new DBModel())
                {
                    Empleado emp = db.Empleadoes.Where(x => x.EmpleadoID == id).FirstOrDefault<Empleado>();
                    db.Empleadoes.Remove(emp);
                    db.SaveChanges();
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmpleado()), message = "Borrado exitoso" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}