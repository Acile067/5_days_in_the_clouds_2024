using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Levi9_competition.Interfaces;
using Levi9_competition.Models;

namespace Levi9_competition.Repos
{
    public class MatchDynamoRepo: IMatchRepo
    {
        private readonly DynamoDBContext _context;

        public MatchDynamoRepo(IAmazonDynamoDB dynamoDbClient)
        {
            _context = new DynamoDBContext(dynamoDbClient);
        }

        public async Task<Match> CreateAsync(Match matchModel)
        {
            await _context.SaveAsync(matchModel);
            return matchModel;
        }

        public async Task DeleteAllAsync()
        {
            var allMatches = await GetAllMatchesAsync();

            foreach (var match in allMatches)
            {
                await _context.DeleteAsync(match);
            }
        }

        private async Task<List<Match>> GetAllMatchesAsync()
        {
            var scanConditions = new List<ScanCondition>();
            var search = _context.ScanAsync<Match>(scanConditions);
            return await search.GetRemainingAsync();
        }
    }
}
