using Levi9_competition.Data;
using Levi9_competition.Interfaces;
using Levi9_competition.Models;
using Microsoft.EntityFrameworkCore;

namespace Levi9_competition.Repos
{
    public class MatchRepo : IMatchRepo
    {
        private readonly AppDbContext _context;
        public MatchRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Match> CreateAsync(Match matchModel)
        {
            await _context.Matches.AddAsync(matchModel);
            await _context.SaveChangesAsync();
            return matchModel;
        }
    }
}
