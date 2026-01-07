namespace PlataformaGestaoIA
{
    public static class Configuration
    {
        public static string Key;
        public static string ApiKeyName;
        public static string ApiKeyValue;
        public static SmtpConfiguration Smtp = new();

        public class SmtpConfiguration
        {
            public string Host { get; set; }
            public int Port { get; set; } = 25;
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}
