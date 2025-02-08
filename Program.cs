using NoteSait.Data;
using NoteSait.Models;
using NoteSait.Services;
using NoteSait.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Use Notify on Sait
//FIX 
//builder.Services.AddNotyf(config=> { config.DurationInSeconds = 10;config.IsDismissable = true;config.Position = NotyfPosition.BottomRight; });

//Service Porstgre Database
builder.Services.AddDbContext<Context>(options =>
 options.UseNpgsql(builder.Configuration.GetConnectionString("MyDatabase")));
builder.Services.AddIdentity<User, IdentityRole>().
   AddEntityFrameworkStores<Context>().
   AddDefaultTokenProviders();
// позволяет добраться до сессий
builder.Services.AddHttpContextAccessor();
//Repository Db 
builder.Services.AddScoped<IUserRepository, UserRepository>(); //Repository User
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<AlbumRepository>();
//Service File Manager
builder.Services.AddScoped<IFileManagerService, FileManagerService>();

//Service Note Analysis
builder.Services.AddScoped<IAnalysisNotes, AnalysisNotes>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{//   
    FileProvider = new PhysicalFileProvider(@"/projects/NoteSait/UploadFiles/AllAlbums/"), ///Users/artemkolerov/Desktop/VsProj/NoteSait/UploadFiles/AllAlbums
    RequestPath = "/AllAlbums"
});


app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
