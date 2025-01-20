namespace Toy.Utilit
{
    public static class Transform
    {
        public static string TextFormReview(int commentCount)
        {
            if ((commentCount % 100) >= 11 && (commentCount % 100) <= 14)
                return " відгуків";
            return (commentCount % 10) switch
            {
                1 => " відгук",
                2 or 3 or 4 => " відгуки",
                _ => " відгуків",
            };
        }
        public static string TextFormAge(byte age)
        {
            return (age) switch
            {
                1 => " року",
                _ => " років",
            };
        }
        public static string TextFormSex(string sex)
        {
            return (sex) switch
            {
                "f" => "Для дівчат",
                _ => " Для хлопців",
            };
        }
    }
}
