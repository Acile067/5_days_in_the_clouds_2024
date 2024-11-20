using Levi9_competition.Dtos.Match;
using Levi9_competition.Interfaces;
using Levi9_competition.Mappers;
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
            {
                throw new ArgumentException("Team1Id and Team2Id must be valid.");
            }

            if (matchModel.WinningTeamId != null &&
                matchModel.Team1Id != matchModel.WinningTeamId &&
                matchModel.Team2Id != matchModel.WinningTeamId)
            {
                throw new ArgumentException("Invalid WinningTeamId.");
            }


            var team1 = await _teamRepo.GetByIdAsync(matchModel.Team1Id);
            var team2 = await _teamRepo.GetByIdAsync(matchModel.Team2Id);

            if (team1 == null || team2 == null)
                throw new ArgumentException("One or both teams do not exist.");

            double team1AvgElo = team1.Players.Average(p => p.Elo);
            double team2AvgElo = team2.Players.Average(p => p.Elo);

            double team1Score = matchModel.WinningTeamId == matchModel.Team1Id ? 1 :
                                matchModel.WinningTeamId == matchModel.Team2Id ? 0 : 0.5;
            double team2Score = 1 - team1Score;

            foreach (var player in team1.Players)
            {
                int newHoursPlayed = player.HoursPlayed + matchModel.Duration;
                int k = CalculateK(newHoursPlayed);
                double expectedScore = CalculateExpectedScore(player.Elo, team2AvgElo);
                
                player.HoursPlayed = newHoursPlayed;

                if (team1Score == 1) 
                { 
                    player.Wins++;
                    player.Elo = (int)Math.Round(CalculateNewElo(player.Elo, expectedScore, team1Score, k));
                }
                else if (team1Score == 0) 
                { 
                    player.Losses++;
                    player.Elo = -(int)Math.Round(CalculateNewElo(player.Elo, expectedScore, team1Score, k));
                }
            }

            foreach (var player in team2.Players)
            {
                int newHoursPlayed = player.HoursPlayed + matchModel.Duration;
                int k = CalculateK(newHoursPlayed);
                double expectedScore = CalculateExpectedScore(player.Elo, team1AvgElo);
                
                player.HoursPlayed = newHoursPlayed;

                if (team2Score == 1) 
                { 
                    player.Wins++;
                    player.Elo = (int)Math.Round(CalculateNewElo(player.Elo, expectedScore, team1Score, k));
                }
                else if (team2Score == 0) 
                { 
                    player.Losses++;
                    player.Elo = -(int)Math.Round(CalculateNewElo(player.Elo, expectedScore, team1Score, k));
                }
            }

            var match = matchModel.ToMatchFromCreateDTO();
            await _matchRepo.CreateAsync(match);

            await _teamRepo.UpdateAsync(team1);
            await _teamRepo.UpdateAsync(team2);

            return true;
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

        private double CalculateNewElo(double r1, double expected, double actual, int k)
        {
            return r1 + k * (actual - expected);
        }
    }
}
