namespace StreamingApp.Models
{
    // ViewModel principal que recibe la vista Index.cshtml
    public class HomeViewModel
    {
        public FeaturedContent? FeaturedContent { get; set; }

        // Filas por categoría
        public List<ContentRow> Rows { get; set; } = new();

        // Lista de películas favoritas del usuario
        public List<ContentItem> MyList { get; set; } = new();

        // Nombre del usuario logueado
        public string UserName { get; set; } = string.Empty;
    }

    // Contenido destacado del Home
    public class FeaturedContent
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string BackdropUrl { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Year { get; set; }

        public int DurationMinutes { get; set; }

        public decimal? Rating { get; set; }

        public string Genre { get; set; } = string.Empty;

        public bool IsInMyList { get; set; }

        // Se mantienen temporalmente porque Index.cshtml todavía las utiliza
        public string ContentType { get; set; } = "PELÍCULA";

        public bool IsComingSoon { get; set; } = false;

        public string ComingSoonLabel { get; set; } = string.Empty;
    }

    // Fila de películas por categoría
    public class ContentRow
    {
        public int CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;

        public List<ContentItem> Items { get; set; } = new();
    }

    // Tarjeta individual
    public class ContentItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ThumbnailUrl { get; set; } = string.Empty;

        public int Year { get; set; }

        public int DurationMinutes { get; set; }

        public decimal? Rating { get; set; }

        public bool IsInMyList { get; set; }
    }
}