using System.Net.Mail;

namespace CathayDomain;

public static class EmailChecker
{
    public static bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}