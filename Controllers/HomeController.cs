using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Text.Json;
using NoteSait.Data;
using Microsoft.AspNetCore.Authorization;
using NoteSait.Services.Interfaces;
using NoteSait.Models;
using NoteSait.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace NoteSait.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IUserRepository userRepository;
        private IFileManagerService fileManager;
        private IFileRepository fileRepository;
       // private AlbumRepository albumRepository;
        public HomeController(IUserRepository userRepository, IFileManagerService fileManager, IFileRepository fileRepository)
        {
            this.userRepository = userRepository;
            this.fileManager = fileManager;
            this.fileRepository = fileRepository;
           // this.albumRepository = albumRepository;
            
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Albums = fileManager.GetAlbumsFromUser();
            return View();
        }
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AddFile() //IFormFile file, 
        {
            var albums = fileManager.GetAlbumsFromUser();
            ViewBag.AlbumsSelectList = new SelectList(albums,"Id", "Name");
            return View(new FileViewModel());
        }

        [HttpPost]
        public IActionResult AddFile(FileViewModel model) //IFormFile file, FileViewModel model
        {
            if (ModelState.IsValid)
            {
                bool feedback = fileManager.AddFile(model);
                if (!feedback)
                {
                    ModelState.AddModelError("Analysis", "Analysis Failed");
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }

        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult DowloadFile(string id)
        {
            DownloadedFile file = fileManager.GetFileBytesById(id);
            if (file != null) { return File(file.BytesFile, "application/octet-stream", file.FileName); }
            else { return RedirectToAction("FileNotFound"); }
        }

        [AllowAnonymous]
        public IActionResult FileNotFound()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteFile(string id)
        {
            fileManager.DeleteFile(id);
            FileModel file = fileRepository.Find(id);
            fileRepository.Remove(file);
            fileRepository.Save();
            return RedirectToAction("Index");
        }

        public IActionResult UploadPart()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadPart(FileViewModel fileViewModel)
        {
            fileManager.AddFile(fileViewModel);
            return Content("");
        }
    }
}
