using Amazon.DynamoDBv2.DataModel;

namespace Levi9_competition.Models
{
    [DynamoDBTable("Teams")]
    public class Team
    {
        [DynamoDBHashKey]
        public string Id { get; set; } = string.Empty;
        [DynamoDBProperty]
        public string TeamName { get; set; } = string.Empty;
        [DynamoDBProperty]
        public List<Player> Players { get; set; } = new List<Player>();
        [DynamoDBProperty]
        public bool TempTeam { get; set; } = false;
    }
}
