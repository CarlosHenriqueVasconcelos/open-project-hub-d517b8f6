using PlataformaGestaoIA.Models;
using System.ComponentModel.DataAnnotations;

namespace PlataformaGestaoIA.ViewModel
{
    public class EditorAllocationResultViewModel
    {
        [Required]
        public User Student { get; set; }

        [Required]
        public Project Project { get; set; }

        [Required]
        [StringLength(7)]
        public string Semester { get; set; }
    }
}