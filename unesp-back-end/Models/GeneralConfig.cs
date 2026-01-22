using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace PlataformaGestaoIA.Models;

public class GeneralConfig
{
    public int Id { get; set; }
    public string ConfigHeader { get; set; }
    public string ConfigBody { get; set; }
    public string ConfigEmailDomainAvaliable { get; set; }
    public string ConfigConsent { get; set; }
    public string? Stage { get; set; }
    public DateTime? ConfirmationDeadline { get; set; }
}
