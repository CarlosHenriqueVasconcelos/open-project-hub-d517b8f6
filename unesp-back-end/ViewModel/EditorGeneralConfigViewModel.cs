using System.ComponentModel.DataAnnotations;

namespace PlataformaGestaoIA.ViewModel;

public class EditorGeneralConfigViewModel
{
    [Required(ErrorMessage = "O título é obrigatório")]
    public string ConfigHeader { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória")]
    public string ConfigBody { get; set; }
    [Required(ErrorMessage = "Obrigatório informar ao menos 1 domínio")]
    public string ConfigEmailDomainAvaliable { get; set; }

    [Required(ErrorMessage = "Texto de consentimento é obrigatório")]
    public string ConfigConsent { get; set; }

    public string? Stage { get; set; }
    public DateTime? ConfirmationDeadline { get; set; }
    public DateTime? ConfirmationDeadlinePhase2 { get; set; }
}
