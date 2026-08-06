using CineStreamCR.BLL;
using CineStreamCR.BLL.Services.Actor;
using CineStreamCR.BLL.Services.Director;
using CineStreamCR.DAL.Data;
using CineStreamCR.DAL.Repositories.Actors;
using CineStreamCR.DAL.Repositories.Categories;
using CineStreamCR.DAL.Repositories.Directors;
using CineStreamCR.DAL.Repositories.Movies;
using CineStreamCR.DAL.Repositories.Reviews;
using CineStreamCR.DAL.Repositories.Users;
using CineStreamCR.DAL.Repositories.WatchLists;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ProyectoDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IMovieCategoryRepository, MovieCategoryRepository>();
builder.Services.AddScoped<IMovieDirectorsRepository, MovieDirectorsRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IWatchListRepository, WatchListRepository>();
builder.Services.AddScoped<IWatchListMoviesRepository, WatchListMoviesRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IMovieActorsRepository, MovieActorsRepository>();
builder.Services.AddScoped<IDirectorRepository, DirectorRepository>();

// Servicios
builder.Services.AddScoped<IActorService, ActorService>();
builder.Services.AddScoped<IDirectorService, DirectorService>();
builder.Services.AddAutoMapper(cfg => { }, typeof(ClassMapping));

// Add services to the container.
builder.Services.AddControllersWithViews();


// Register EF Core DbContext (SQLServer). Update the connection string in appsettings.json
var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider
//        .GetRequiredService<ProyectoDBContext>();

//    db.Database.Migrate();
//}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
