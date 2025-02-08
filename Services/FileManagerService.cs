using System;
using System.Security.Cryptography;
using System.Text;
using NoteSait.Models;
using NoteSait.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NoteSait.Models.DataModel;

namespace NoteSait.Services;

public class FileManagerService : IFileManagerService
{
    private IHttpContextAccessor httpContext;
    private IAnalysisNotes analysisNotes;
    private IFileRepository fileRepository;
    private UserManager<User> userManager;
    public FileManagerService(IHttpContextAccessor httpContext, IFileRepository fileRepository, UserManager<User> userManager, IAnalysisNotes analysisNotes)
    {
        this.httpContext = httpContext;
        this.fileRepository = fileRepository;
        this.userManager = userManager;
        this.analysisNotes = analysisNotes;
    }

    public bool AddFile(FileViewModel fileView)
    {
        List<string> pathPhoto = new List<string>();
        string pathMid = "";
        foreach (var item in fileView.formFiles)
        {
            var forTypeFile = item.FileName.Split('.');
            string typef = forTypeFile[forTypeFile.Length - 1];
            //fileView.Name = $"{fileView.Name}";
            var currentDirectory = Directory.GetCurrentDirectory();
            var uploadFolder = Path.Combine(currentDirectory, "UploadFiles");
            var uploadedDirectory = Path.Combine(uploadFolder, $"{httpContext.HttpContext?.User.Identity.Name}");
            if (!Directory.Exists(uploadedDirectory)) { Directory.CreateDirectory(uploadedDirectory); }
            var filepath = Path.Combine(uploadedDirectory, item.FileName);
            pathMid = Path.Combine(uploadedDirectory, $"{fileView.Name}.mid");
            pathPhoto.Add(filepath);
            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                item.CopyTo(stream);
            }
        }

        StateAnalysis analysis = analysisNotes.GetPhotosForAnalysis(pathPhoto.ToArray(), pathMid);
        if (analysis == StateAnalysis.OK)
        {
            FileModel file = new FileModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = fileView.Name,
                Message = fileView.Message,
                Path = pathMid,
                AlbumId = fileView.albums //Album Id
            };
            AddFileToDbAsync(file);
            foreach (var img in pathPhoto)
            {
                DeleteFileByPath(img);
            }
            return true;
        }
        return false;
    }
    public async Task AddFileToDbAsync(FileModel fileModel)
    {
        fileRepository.Add(fileModel);
    }
    public List<AlbumModel> GetAlbumsFromUser()
    {
        string UserId = userManager.GetUserId(httpContext.HttpContext.User);
        List<AlbumModel> albums = fileRepository.GetAlbumsFromUser(UserId);
        return albums;
    }
    public bool DeleteFile(string id)
    {
        var file = GetFileById(id);
        DeleteFileByPath(file.Path);
        return true;
    }
    public bool DeleteFileByPath(string path)
    {
        if (!System.IO.File.Exists(path))
        {
            return false;
        }
        System.IO.File.Delete(path);
        return true;
    }
    public FileModel GetFileById(string id)
    {
        return fileRepository.FirstOrDefault(x => x.Id == id);
    }
    public DownloadedFile GetFileBytesById(string id)
    {
        DownloadedFile file = new DownloadedFile();
        FileModel fileModel = GetFileById(id);
        if (fileModel == null) { return null; }
        file.FileName = $"{fileModel.Name}.mid";
        string filePath = fileModel.Path;
        if (!System.IO.File.Exists(filePath)) { return null; }
        file.BytesFile = System.IO.File.ReadAllBytes(filePath);
        return file;
    }
}
