using Amazon.DynamoDBv2.DataModel;

namespace Levi9_competition.Models
{
    [DynamoDBTable("tableMatches")]
    public class Match
    {
        [DynamoDBHashKey]
        public string Id { get; set; } = string.Empty;
        [DynamoDBProperty]
        public string Team1Id { get; set; } = string.Empty;
        [DynamoDBProperty]
        public string Team2Id { get; set; } = string.Empty;
        [DynamoDBProperty]
        public string? WinningTeamId { get; set; }
        [DynamoDBProperty]
        public int Duration { get; set; }
    }
}
