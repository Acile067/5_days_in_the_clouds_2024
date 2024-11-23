using Levi9_competition.Data;
using Levi9_competition.Interfaces;
using Levi9_competition.Models;
using Microsoft.EntityFrameworkCore;

namespace Levi9_competition.Repos
{
    public class PlayerRepo : IPlayerRepo
    {
        private readonly AppDbContext _context;
        public PlayerRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Player> CreateAsync(Player playerModel)
        {
            await _context.Players.AddAsync(playerModel);
            await _context.SaveChangesAsync();
            return playerModel;
        }

        public async Task<Player?> GetByIdAsync(string id)
        {
            return await _context.Players.FirstOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }

        public Task<bool> PlayerExisist(string nickname)
        {
            return _context.Players.AnyAsync(x => x.Nickname == nickname);
        }
        public async Task<List<Player>> GetAllAsync()
        {
            return await _context.Players.ToListAsync();
        }
        public async Task<Player> UpdateAsync(Player playerModel)
        {
            var player = await _context.Players.FirstOrDefaultAsync(x => x.Id == playerModel.Id);

            if (player == null)
            {
                throw new ArgumentException("Player not found.");
            }

            player.Nickname = playerModel.Nickname;
            player.Wins = playerModel.Wins;
            player.Losses = playerModel.Losses;
            player.Elo = playerModel.Elo;
            player.HoursPlayed = playerModel.HoursPlayed;
            player.Team = playerModel.Team;
            player.RatingAdjustment = playerModel.RatingAdjustment;

            _context.Players.Update(player);
            await _context.SaveChangesAsync();

            return player;
        }
        public async Task UpdateRangeAsync(IEnumerable<Player> players)
        {
            // Proverava da li lista nije prazna
            if (players == null || !players.Any())
                throw new ArgumentException("The player list cannot be null or empty.");

            // Ažurava entitete u DbContext
            _context.Players.UpdateRange(players);

            // Čuva promene u bazi
            await _context.SaveChangesAsync();
        }
    }
}
