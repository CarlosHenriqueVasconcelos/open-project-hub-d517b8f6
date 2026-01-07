using System.Text.Json.Serialization;

namespace PlataformaGestaoIA.Models;

public class Student
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? RA { get; set; }
    public string? CPF { get; set; }
    public string? RG { get; set; }
    public string? Cellphone { get; set; }
    public CurrentCourse? CurrentCourse { get; set; }
    public IList<StudentRegistration>? StudentRegistrations { get; set; }
    public User? User { get; set; }
}