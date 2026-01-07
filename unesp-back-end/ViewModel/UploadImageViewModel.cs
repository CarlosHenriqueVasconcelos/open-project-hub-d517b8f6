using System.ComponentModel.DataAnnotations;

namespace PlataformaGestaoIA.ViewModel;

public class UploadImageViewModel
{
    [Required(ErrorMessage = "Imagem inválida")]
    public string Base64Image { get; set; }
}