namespace StreamingApp.Models
{
    // ViewModel principal que recibe la vista Index.cshtml
    public class HomeViewModel
    {
        public FeaturedContent? FeaturedContent { get; set; }
        public List<ContentRow> Rows { get; set; } = new();
    }

    // Contenido destacado (hero banner)
    public class FeaturedContent
    {
        public int    Id            { get; set; }
        public string Title         { get; set; } = string.Empty;
        public string BackdropUrl   { get; set; } = string.Empty;   // imagen de fondo del hero
        public string ContentType   { get; set; } = "SERIES";       // "SERIES" | "PELÍCULA" | "DOCUMENTAL"
        public string Genre         { get; set; } = string.Empty;
        public string Year          { get; set; } = string.Empty;
        public string Rating        { get; set; } = string.Empty;   // ej. "TV-14"
        public string Description   { get; set; } = string.Empty;
        public bool   IsComingSoon  { get; set; }
        public string ComingSoonLabel { get; set; } = string.Empty; // ej. "Season 2 Coming Soon"
    }

    // Fila de tarjetas (ej. "Your Next Watch", "Trending Now", …)
    public class ContentRow
    {
        public string Title { get; set; } = string.Empty;
        public List<ContentItem> Items { get; set; } = new();
    }

    // Tarjeta individual dentro de una fila
    public class ContentItem
    {
        public int    Id           { get; set; }
        public string Title        { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;    // miniatura 16:9
    }
}
