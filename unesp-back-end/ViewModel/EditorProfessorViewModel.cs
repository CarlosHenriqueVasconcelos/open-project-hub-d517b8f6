using System.ComponentModel.DataAnnotations;

namespace PlataformaGestaoIA.ViewModel
{
    public class EditorProfessorViewModel
	{
		[Required(ErrorMessage = "O nome é obrigatório")]
		public string Name { get; set; }
    }
}