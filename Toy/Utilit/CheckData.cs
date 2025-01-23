using System.Text.RegularExpressions;

namespace Toy.Utilit
{
    public static class CheckData
    {
        public static bool CheckRegex(string str, string regex)
        {
            return Regex.IsMatch(str, regex);
        }
    }
}
