using Levi9_competition.Models;

namespace Levi9_competition.Interfaces
{
    public interface IMatchRepo
    {
        Task<Match> CreateAsync(Match matchModel);
    }
}
