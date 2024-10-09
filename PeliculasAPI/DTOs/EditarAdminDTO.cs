using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
