using Levi9_competition.Data;
using Levi9_competition.Interfaces;
using Levi9_competition.Models;
using Microsoft.EntityFrameworkCore;

namespace Levi9_competition.Repos
{
    public class TeamRepo : ITeamRepo
    {
        private readonly AppDbContext _context;
        public TeamRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Team> CreateAsync(Team teamModel)
        {
            await _context.Teams.AddAsync(teamModel);
            await _context.SaveChangesAsync();
            return teamModel;
        }

        public async Task<Team?> GetByIdAsync(string id)
        {
            return await _context.Teams
                .Include(t => t.Players)  // Inkludirajte igrače
                .FirstOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }
        public async Task<List<Player>> GetPlayersByGuidsAsync(List<string> playerIds)
        {
            return await _context.Players
                .Where(p => playerIds.Contains(p.Id)) 
                .ToListAsync();
        }
    }
}
