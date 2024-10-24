using System;

namespace TP4.Dtos
{
    public class MovieDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        public string Storeline { get; set; }
        public byte[] Poster{get; set; }
        public byte Genreld { get; set; }
        public string GenreName { get; set; }
    }
}
