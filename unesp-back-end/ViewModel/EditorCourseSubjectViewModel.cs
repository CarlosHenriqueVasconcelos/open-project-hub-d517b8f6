using System.ComponentModel.DataAnnotations;

namespace PlataformaGestaoIA.ViewModel
{
    public class EditorCourseSubjectViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O código é obrigatório")]
        public string Code { get; set; }
    }
}