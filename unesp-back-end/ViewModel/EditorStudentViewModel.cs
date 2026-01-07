using PlataformaGestaoIA.Models;
using System.ComponentModel.DataAnnotations;

namespace PlataformaGestaoIA.ViewModel;

public class EditorStudentViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Name { get; set; }
    [Required]
    //Criar validador de RA
    public string RA { get; set; }
    [Required]
    public string CPF { get; set; }
    [Required]
    public string RG { get; set; }
    [Required]
    public string CellPhone { get; set; }
    [Required]
    public CurrentCourse CurrentCourse { get; set; }

}
