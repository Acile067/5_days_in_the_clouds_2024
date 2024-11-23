namespace Levi9_competition.Models
{
    using Amazon.DynamoDBv2.DataModel;
    [DynamoDBTable("tablePlayers")]
    public class Player
    {
        [DynamoDBHashKey]
        public string Id { get; set; } = string.Empty;
        [DynamoDBProperty]
        public string Nickname { get; set; } = string.Empty;
        [DynamoDBProperty]
        public int Wins { get; set; }
        [DynamoDBProperty]
        public int Losses { get; set; }
        [DynamoDBProperty]
        public int Elo { get; set; }
        [DynamoDBProperty]
        public int HoursPlayed { get; set; }
        [DynamoDBProperty]
        public string? Team { get; set; }
        [DynamoDBProperty]
        public int? RatingAdjustment { get; set; }

    }
}
