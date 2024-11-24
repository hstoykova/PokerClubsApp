namespace PokerClubsApp.Common
{
    public static class EntityValidationConstants
    {
        public static class Club
        {
            public const int ClubNameMinLength = 2;
            public const int ClubNameMaxLength = 60;
        }

        public static class Player
        {
            public const int PlayerNicknameMinLength = 2;
            public const int PlayerNicknameMaxLength = 15;
        }
        public static class Union
        {
            public const int UnionNameMinLength = 2;
            public const int UnionNameMaxLength = 50;
        }
        public static class GameType
        {
            public const int GameTypeNameMinLength = 2;
            public const int GameTypeNameMaxLength = 50;
        }
        
        public static class GameResult
        {
            public const string FromDateFormat = "dd-MM-yyyy";
            public const string ToDateFormat = "dd-MM-yyyy";
            public const double FeeMinValue = 0.01d;
            public const double FeeMaxValue = 20000;
            public const double ResultMinValue = 0.01d;
            public const double ResultsMaxValue = 200000;
        }
    }
}
