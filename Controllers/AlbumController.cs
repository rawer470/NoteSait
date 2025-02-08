using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteSait.Models;
using NoteSait.Models.DataModel;
using NoteSait.Models.ViewModel;
using NoteSait.Services;
using NoteSait.Services.Interfaces;

namespace NoteSait.Controllers
{
    public class AlbumController : Controller
    {
        private AlbumRepository albumRepository;
        private IHttpContextAccessor httpContext;
        private UserManager<User> userManager;
        private IFileManagerService fileManager;
        public AlbumController(AlbumRepository albumRepository, IHttpContextAccessor httpContext, UserManager<User> userManager, IFileManagerService fileManager)
        {
            this.albumRepository = albumRepository;
            this.httpContext = httpContext;
            this.userManager = userManager;
            this.fileManager = fileManager;
        }

        public IActionResult Index()
        {
            ViewBag.Albums = fileManager.GetAlbumsFromUser();
            return View();
        }

        public IActionResult AddAlbum()
        {
            return View(new AlbumViewModel());
        }

        [HttpPost]
        public IActionResult AddAlbum(AlbumViewModel viewModel)
        {
            var forTypeFile = viewModel.formFiles.FileName.Split('.');
            string typef = forTypeFile[forTypeFile.Length - 1];
            //fileView.Name = $"{fileView.Name}";
            var currentDirectory = Directory.GetCurrentDirectory();
            var currentPath = Path.Combine(currentDirectory, "UploadFiles");
            var ImageFolder = Path.Combine("AllAlbums");
            var uploadedDirectory = Path.Combine(ImageFolder, $"{httpContext.HttpContext?.User.Identity.Name}");
            var allPath = Path.Combine(currentPath, uploadedDirectory);
            if (!Directory.Exists(allPath)) { Directory.CreateDirectory(allPath); }
            var imageAllPath = Path.Combine(allPath, $"{viewModel.Name}.{typef}");
            var imagePath = Path.Combine(uploadedDirectory, $"{viewModel.Name}.{typef}");
            using (var stream = new FileStream(imageAllPath, FileMode.Create))
            {
                viewModel.formFiles.CopyTo(stream);
            }

            AlbumModel album = new AlbumModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = viewModel.Name,
                ImagePath = imagePath,
                UserId = userManager.GetUserId(httpContext.HttpContext?.User)
            };
            AddFileToDbAsync(album);
            return RedirectToAction("Index", "Home");
        }
        public async Task AddFileToDbAsync(AlbumModel albumModel)
        {
            albumRepository.Add(albumModel);
        }

        public IActionResult DeleteFile(string id)
        {
            Delete(id);
            return RedirectToAction("Index");
        }



        public bool Delete(string id)
        {
            var album = GetAlbumById(id);
            
            DeleteImage(album);
            albumRepository.Remove(album);
            albumRepository.Save();
            return true;
        }
        public async Task DeleteImage(AlbumModel album)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var uploadFolder = Path.Combine(currentDirectory,"UploadFiles");
            var allPath = Path.Combine(uploadFolder, album.ImagePath);
            DeleteFileByPath(allPath);
        }
        public bool DeleteFileByPath(string path)
        {// /Users/artemkolerov/Desktop/VsProj/NoteSait/UploadFiles/AllAlbums/Admin/Addey Road.jpg
            if (!System.IO.File.Exists(path))
            {
                return false;
            }
            System.IO.File.Delete(path);
            return true;
        }
        public AlbumModel GetAlbumById(string id)
        {
            return albumRepository.FirstOrDefault(x => x.Id == id);

        }
    }
}
