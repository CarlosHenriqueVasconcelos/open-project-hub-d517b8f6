using System.ComponentModel.DataAnnotations;

namespace PlataformaGestaoIA.ViewModel
{
    public class EditorCompanyViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        //[StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve conter entre 3 e 40 caracteres")]
        public string Name { get; set; }

        //verificar se campo é obrigatório [Required(ErrorMessage = "A razão social é obrigatória")]
        public string LegalName { get; set; }

        //verificar se é campo obrigatório [Required(ErrorMessage = "O cnpj é obrigatório")]
        //Criar validador para cnpj
        public string CNPJ { get; set; }
    }
}