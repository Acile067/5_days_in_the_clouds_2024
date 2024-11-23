using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Levi9_competition.Interfaces;
using Levi9_competition.Models;

namespace Levi9_competition.Repos
{
    public class PlayerDynamoRepo : IPlayerRepo
    {
        private readonly DynamoDBContext _context;
        private const string TableName = "Players";

        public PlayerDynamoRepo(IAmazonDynamoDB dynamoDbClient)
        {
            _context = new DynamoDBContext(dynamoDbClient);
        }

        public async Task<Player> CreateAsync(Player playerModel)
        {
            await _context.SaveAsync(playerModel);
            return playerModel;
        }

        public async Task<Player?> GetByIdAsync(string id)
        {
            return await _context.LoadAsync<Player>(id);
        }

        public async Task<bool> PlayerExisist(string nickname)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("Nickname", ScanOperator.Equal, nickname)
            };

            var search = _context.ScanAsync<Player>(conditions);
            var results = await search.GetNextSetAsync();
            return results.Count > 0;
        }

        public async Task<List<Player>> GetAllAsync()
        {
            var scanConditions = new List<ScanCondition>();
            var search = _context.ScanAsync<Player>(scanConditions);
            return await search.GetRemainingAsync();
        }

        public async Task<Player> UpdateAsync(Player playerModel)
        {
            await _context.SaveAsync(playerModel);
            return playerModel;
        }

        public async Task DeleteAllAsync()
        {
            var allPlayers = await GetAllAsync();
            foreach (var player in allPlayers)
            {
                await _context.DeleteAsync(player);
            }
        }
    }
}
