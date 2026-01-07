using System.ComponentModel.DataAnnotations;

namespace PlataformaGestaoIA.ViewModel;

public class EditorSkillViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Name { get; set; }

    public bool IsSoftSkill { get; set; }
}
