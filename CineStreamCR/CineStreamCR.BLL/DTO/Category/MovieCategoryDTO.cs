using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.BLL.DTO.Category
{
    public class MovieCategoryDTO
    {
        public int MovieId { get; set; }
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }
        public string? MovieTitle { get; set; }
    }
    //par asignar una categoria a una pelicula
    public class AssignCategoryToMovieDTO
    {
        public int MovieId { get; set; }
        public int CategoryId { get; set; }
    }
}

