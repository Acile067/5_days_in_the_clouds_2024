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
    }
}
