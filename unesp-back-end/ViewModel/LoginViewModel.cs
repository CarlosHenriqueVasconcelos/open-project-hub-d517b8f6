using System.ComponentModel.DataAnnotations;

namespace PlataformaGestaoIA.ViewModel;

public class LoginViewModel
{
	[Required(ErrorMessage = "Informe o E-mail")]
	[EmailAddress(ErrorMessage = "E-mail inválido")]
	public string Email { get; set; }

	[Required(ErrorMessage = "Informe a senha")]
	public string Password { get; set; }
}

public class ResponseLoginViewModel
{
	public string Token { get; set; }
}