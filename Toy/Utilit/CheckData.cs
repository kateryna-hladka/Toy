using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Toy.Utilit
{
    public static class CheckData
    {
        public static bool CheckRegex(string str, string regex)
        {
            return Regex.IsMatch(str, regex);
        }
        public static bool CheckPhone(string phone)
        {
            return CheckData.CheckRegex(phone, StaticVariables.regPhone)
                && phone.Length >= StaticVariables.minPhoneEmainLength;
        }
        public static bool CheckEmail(string email)
        {
            return CheckData.CheckRegex(email, StaticVariables.regEmail) &&
                    email.Length >= StaticVariables.minPhoneEmainLength;

        }
        public static bool CheckPassword(string password)
        {
            return (password.Length >= StaticVariables.minPasswordLength &&
                    password.Length <= StaticVariables.maxPasswordLength);
        }
    }
}
