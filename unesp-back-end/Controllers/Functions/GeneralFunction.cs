using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.Controllers.Functions
{
    public class GeneralFunction
    {
        public static string GenerateTag(string value)
        {
            return value.Replace(" ", "-").ToLower().Replace("@", "-").Replace(".", "-");
        }

        public static string GetPdfDirectory(string path = "")
        {
            var executableDirectory = AppDomain.CurrentDomain.BaseDirectory; // Obtém o diretório do executável
            var saveDirectory = Path.Combine(executableDirectory, path); // Combina com a pasta 'PDF'

            // Cria a pasta 'PDF' caso não exista
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            return saveDirectory;
        }
    }
}
