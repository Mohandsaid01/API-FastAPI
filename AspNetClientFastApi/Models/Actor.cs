using System;

namespace AspNetClientFastApi.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? Picture { get; set; }
    }
}
