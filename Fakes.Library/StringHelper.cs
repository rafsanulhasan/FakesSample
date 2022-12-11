using System.Net.Mail;

namespace MsFakes.Library
{
    public static class StringHelper
    {
        public static bool IsNullOrWhiteSpace(this string email)
        {
            return string.IsNullOrWhiteSpace(email);
        }

        public static bool IsValidEmail(this string email)
        {
            return MailAddress.TryCreate(email, out var _);
        }
    }
}
