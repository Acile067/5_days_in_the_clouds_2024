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
                .Include(t => t.Players)  
                .FirstOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }
        public async Task<List<Player>> GetPlayersByGuidsAsync(List<string> playerIds)
        {
            return await _context.Players
                .Where(p => playerIds.Contains(p.Id)) 
                .ToListAsync();
        }

        public Task<bool> TeamExisist(string teamName)
        {
            return _context.Teams.AnyAsync(x => x.TeamName == teamName);
        }

        public async Task<Team> UpdateAsync(Team teamModel)
        {

            var existingTeam = await _context.Teams
                .Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.Id.ToLower() == teamModel.Id.ToLower());

            if (existingTeam == null)
            {
                throw new KeyNotFoundException("Team not found.");
            }

            existingTeam.TeamName = teamModel.TeamName;

            var existingPlayerIds = existingTeam.Players.Select(p => p.Id).ToList();
            var newPlayerIds = teamModel.Players.Select(p => p.Id).ToList();

            var playersToRemove = existingTeam.Players
                .Where(p => !newPlayerIds.Contains(p.Id))
                .ToList();
            foreach (var player in playersToRemove)
            {
                existingTeam.Players.Remove(player);
            }

            var playersToAdd = teamModel.Players
                .Where(p => !existingPlayerIds.Contains(p.Id))
                .ToList();
            foreach (var player in playersToAdd)
            {
                existingTeam.Players.Add(player);
            }

            _context.Teams.Update(existingTeam);
            await _context.SaveChangesAsync();

            return existingTeam;
        }
    }
}
