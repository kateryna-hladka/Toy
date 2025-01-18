﻿namespace Toy.Utilit
{
    public static class Rating
    {
        public const byte MaxStars = 5;
        public static string TextFormReview(int commentCount)
        {
            if ((commentCount % 100) >= 11 &&  (commentCount%100)<= 14)
                return "відгуків";
            return (commentCount % 10) switch
            {
                1 => "відгук",
                2 or 3 or 4 => "відгуки",
                _ => "відгуків",
            };
        }
        public static string RoundCommentCount(int commentCount)
        {
            if (commentCount < 1000)
                return commentCount.ToString();
            if (commentCount < 1e6)
            {
                double divide = commentCount / 1000d;
                double res = Math.Truncate(divide * 10) / 10;
                return res+"к";
            }
            return "більше 1м";
        }
    }
}
