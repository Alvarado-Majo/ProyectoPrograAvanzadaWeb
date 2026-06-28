using Microsoft.AspNetCore.Mvc;
using StreamingApp.Models;

namespace StreamingApp.Controllers
{
    public class HomeController : Controller
    {
        // Inyecta tu DbContext o servicio aquí cuando conectes datos reales
        // private readonly AppDbContext _db;
        // public HomeController(AppDbContext db) { _db = db; }

        public IActionResult Index()
        {
            // ─── Datos de ejemplo ───────────────────────────────────────────────
            // Reemplaza esto con una consulta real a tu base de datos
            var vm = new HomeViewModel
            {
                FeaturedContent = new FeaturedContent
                {
                    Id = 1,
                    Title = "Wednesday",
                    BackdropUrl = "/images/wednesday-backdrop.jpg",   // pon la ruta real
                    ContentType = "SERIES",
                    Genre = "Show · Fantasy",
                    Year = "2022",
                    Rating = "TV-14",
                    Description = "Smart, sarcastic and a little dead inside, Wednesday Addams " +
                                    "investigates a murder spree while making new friends — and foes " +
                                    "— at Nevermore Academy.",
                    IsComingSoon = true,
                    ComingSoonLabel = "Season 2 Coming Soon"
                },
                Rows = new List<ContentRow>
                {
                    new ContentRow
                    {
                        Title = "Your Next Watch",
                        Items = new List<ContentItem>
                        {
                            new ContentItem { Id = 2, Title = "Stranger Things",   ThumbnailUrl = "/images/thumb-st.jpg" },
                            new ContentItem { Id = 3, Title = "Squid Game",        ThumbnailUrl = "/images/thumb-sg.jpg" },
                            new ContentItem { Id = 4, Title = "Bridgerton",        ThumbnailUrl = "/images/thumb-br.jpg" },
                            new ContentItem { Id = 5, Title = "The Crown",         ThumbnailUrl = "/images/thumb-tc.jpg" },
                            new ContentItem { Id = 6, Title = "Ozark",             ThumbnailUrl = "/images/thumb-oz.jpg" },
                            new ContentItem { Id = 7, Title = "Money Heist",       ThumbnailUrl = "/images/thumb-mh.jpg" },
                        }
                    },
                    new ContentRow
                    {
                        Title = "Trending Now",
                        Items = new List<ContentItem>
                        {
                            new ContentItem { Id = 8,  Title = "Dark",             ThumbnailUrl = "/images/thumb-dk.jpg" },
                            new ContentItem { Id = 9,  Title = "Narcos",           ThumbnailUrl = "/images/thumb-na.jpg" },
                            new ContentItem { Id = 10, Title = "The Witcher",      ThumbnailUrl = "/images/thumb-tw.jpg" },
                            new ContentItem { Id = 11, Title = "Emily in Paris",   ThumbnailUrl = "/images/thumb-ep.jpg" },
                            new ContentItem { Id = 12, Title = "Cobra Kai",        ThumbnailUrl = "/images/thumb-ck.jpg" },
                        }
                    }
                }
            };
            // ────────────────────────────────────────────────────────────────────

            return View(vm);
        }

        public IActionResult Details(int id)
        {
            // TODO: cargar detalle del contenido por id
            return View();
        }

        public IActionResult Shows() => View();
        public IActionResult Movies() => View();
        public IActionResult Games() => View();
        public IActionResult MyList() => View();
    }
}
