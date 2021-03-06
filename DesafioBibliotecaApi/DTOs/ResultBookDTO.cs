using DesafioBibliotecaApi.Entidades;
using System;

namespace DesafioBibliotecaApi.DTOs
{
    public class ResultBookDTO : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public int QuantityInventory { get; set; }
        public Guid AuthorId { get; set; }

        
    }
}
