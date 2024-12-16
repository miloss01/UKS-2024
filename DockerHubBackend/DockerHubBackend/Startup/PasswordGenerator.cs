using System.Text;

namespace DockerHubBackend.Startup
{
    public class PasswordGenerator
    {
        private static readonly string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string Digits = "0123456789";
        private static readonly string SpecialChars = "!@#$%^&*()-_=+,.?";

        public static string GenerateRandomPassword(int length)
        {
            if (length < 1)
                throw new ArgumentException("Password length must be greater than 0.");

            string allChars = UppercaseChars + LowercaseChars + Digits + SpecialChars;

            Random random = new Random();
            StringBuilder password = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(allChars.Length);
                password.Append(allChars[index]);
            }

            return password.ToString();
        }
    }
}
