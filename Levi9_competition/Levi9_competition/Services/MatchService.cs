using Levi9_competition.Dtos.Match;
using Levi9_competition.Interfaces;
using Levi9_competition.Mappers;
using Levi9_competition.Models;
using Microsoft.AspNetCore.Mvc;

namespace Levi9_competition.Services
{
    public class MatchService
    {
        private readonly ITeamRepo _teamRepo;
        private readonly IMatchRepo _matchRepo;

        public MatchService(ITeamRepo teamRepo,  IMatchRepo matchRepo)
        {
            _teamRepo = teamRepo;
            _matchRepo = matchRepo;
        }
        public async Task<bool> ProcessMatch(CreateMatchRequestDto matchModel)
        {
            if (matchModel.Duration < 1)
                throw new ArgumentException("Duration must be at least 1 hour.");

            if (matchModel.Team1Id == matchModel.Team2Id)
                throw new ArgumentException("Team1 and Team2 have same IDs.");

            if (string.IsNullOrEmpty(matchModel.Team1Id) || string.IsNullOrEmpty(matchModel.Team2Id))
                throw new ArgumentException("Team1Id and Team2Id must be valid.");

            if (matchModel.WinningTeamId != null &&
                matchModel.Team1Id != matchModel.WinningTeamId &&
                matchModel.Team2Id != matchModel.WinningTeamId)
                throw new ArgumentException("Invalid WinningTeamId.");

            var team1 = await _teamRepo.GetByIdAsync(matchModel.Team1Id);
            var team2 = await _teamRepo.GetByIdAsync(matchModel.Team2Id);

            if (team1 == null || team2 == null)
                throw new ArgumentException("One or both teams do not exist.");

            double team1AvgElo = CalculateTeamELO(team1.Players);
            double team2AvgElo = CalculateTeamELO(team2.Players);

            bool isDraw = matchModel.WinningTeamId == null;

            if (isDraw)
            {
                UpdateTeamPlayers(team1.Players, 0.5, matchModel.Duration, team2AvgElo);
                UpdateTeamPlayers(team2.Players, 0.5, matchModel.Duration, team1AvgElo);
            }
            else
            {
                double team1Score = matchModel.WinningTeamId == matchModel.Team1Id ? 1 : 0;
                double team2Score = 1 - team1Score;

                UpdateTeamPlayers(team1.Players, team1Score, matchModel.Duration, team2AvgElo);
                UpdateTeamPlayers(team2.Players, team2Score, matchModel.Duration, team1AvgElo);
            }

            var match = matchModel.ToMatchFromCreateDTO();
            await _matchRepo.CreateAsync(match);

            await _teamRepo.UpdateAsync(team1);
            await _teamRepo.UpdateAsync(team2);

            return true;
        }

        private double CalculateTeamELO(IEnumerable<Player> players)
        {
            return players.Average(player => player.Elo);
        }

        private void UpdateTeamPlayers(IEnumerable<Player> players, double score, int duration, double opponentElo)
        {
            foreach (var player in players)
            {
                double expectedScore = CalculateExpectedScore(player.Elo, opponentElo);
                int k = CalculateK(player.HoursPlayed);

                player.Elo = (int)Math.Round(player.Elo + k * (score - expectedScore));

                player.HoursPlayed += duration;

                int k2 = CalculateK(player.HoursPlayed);
                player.RatingAdjustment = k2;

                if (score == 1)
                {
                    player.Wins++;
                }
                else if (score == 0)
                {
                    player.Losses++;
                }
            }
        }
        private int CalculateK(int hoursPlayed)
        {
            if (hoursPlayed < 500) return 50;
            if (hoursPlayed < 1000) return 40;
            if (hoursPlayed < 3000) return 30;
            if (hoursPlayed < 5000) return 20;
            return 10;
        }
        private double CalculateExpectedScore(double r1, double r2)
        {
            return 1 / (1 + Math.Pow(10, (r2 - r1) / 400));
        }
    }
}
