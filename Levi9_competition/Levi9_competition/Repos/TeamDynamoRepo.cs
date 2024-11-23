using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;
using Levi9_competition.Interfaces;
using Levi9_competition.Models;

namespace Levi9_competition.Repos
{
    public class TeamDynamoRepo : ITeamRepo
    {
        private readonly DynamoDBContext _context;
        private const string TeamTableName = "Teams";
        private const string PlayerTableName = "Players";

        public TeamDynamoRepo(IAmazonDynamoDB dynamoDbClient)
        {
            _context = new DynamoDBContext(dynamoDbClient);
        }

        public async Task<Team> CreateAsync(Team teamModel)
        {
            await _context.SaveAsync(teamModel);
            return teamModel;
        }

        public async Task<Team?> GetByIdAsync(string id)
        {
            return await _context.LoadAsync<Team>(id);
        }

        public async Task<List<Player>> GetPlayersByGuidsAsync(List<string> playerIds)
        {
            var playerBatchGet = _context.CreateBatchGet<Player>();
            foreach (var playerId in playerIds)
            {
                playerBatchGet.AddKey(playerId);
            }

            await playerBatchGet.ExecuteAsync();
            return playerBatchGet.Results;
        }

        public async Task<Team> UpdateAsync(Team teamModel)
        {
            await _context.SaveAsync(teamModel);
            return teamModel;
        }

        public async Task<bool> TeamExisist(string teamName)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("TeamName", ScanOperator.Equal, teamName)
            };

            var search = _context.ScanAsync<Team>(conditions);
            var results = await search.GetNextSetAsync();
            return results.Count > 0;
        }

        public async Task DeleteAllAsync()
        {
            var allTeams = await GetAllTeamsAsync();
            foreach (var team in allTeams)
            {
                await _context.DeleteAsync(team);
            }
        }

        private async Task<List<Team>> GetAllTeamsAsync()
        {
            var scanConditions = new List<ScanCondition>();
            var search = _context.ScanAsync<Team>(scanConditions);
            return await search.GetRemainingAsync();
        }

        public Task<bool> RemoveAtId(string id)
        {
            throw new NotImplementedException();
        }
    }
}
