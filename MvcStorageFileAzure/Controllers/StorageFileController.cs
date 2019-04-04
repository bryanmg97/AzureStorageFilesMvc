using MvcStorageFileAzure.Models;
using MvcStorageFileAzure.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcStorageFileAzure.Controllers
{
    public class StorageFileController : Controller
    {
        RepositoryStorageFile repo;
        public StorageFileController()
        {
            this.repo = new RepositoryStorageFile();
        }
        // GET: Index
        public ActionResult Index()
        {
            List<String> files = this.repo.GetStorageFiles();
            return View(files);
        }
        // GET: UploadFile
        public ActionResult UploadFile()
        {
            return View();
        }
        // POST: UploadFile
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            Stream stream = file.InputStream;
            repo.UploadFile(file.FileName, stream);
            ViewBag.Mensaje = "El fichero seleccionado se ha subido a Azure correctamente.";
            return View();
        }
        //GET: DeleteFile
        public ActionResult DeleteFile(String filename)
        {
            this.repo.DeleteFile(filename);
            return RedirectToAction("Index");
        }
        //GET: ReadTxtFile
        public ActionResult ReadTxtFile(String filename)
        {
            String filecontent = this.repo.ReadTxt(filename);
            ViewBag.Contenido = filecontent;
            return View();
        }
        //GET: ReadXmlFile
        public ActionResult ReadXmlFile(String filename)
        {
            List<Motorista> motoristas = this.repo.ReadXmlFile(filename);
            return View(motoristas);
        }
    }
}