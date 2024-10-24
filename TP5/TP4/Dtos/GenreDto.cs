using System.ComponentModel.DataAnnotations;

namespace TP4.Dtos
{
    public class GenreDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
