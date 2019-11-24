namespace JwtExample
{
    public class JwtConfig
    {
        public string Audience { get; set; }

        public string Issuer { get; set; }

        public string Secret { get; set; }
    }
}