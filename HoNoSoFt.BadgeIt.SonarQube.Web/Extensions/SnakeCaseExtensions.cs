using System.Linq;

namespace HoNoSoFt.BadgeIt.SonarQube.Web.Models
{
    public static class SnakeCaseExtensions
    {
        public static string ToSnakeCase(this string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }

        public static string ToSnakeCase<T>(this T str) where T : System.Enum
        {
            return str.ToString().ToSnakeCase();
            // return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}