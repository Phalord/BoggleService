using BoggleModel.DataTransfer.Dtos;
using System;

namespace BoggleModel.DataTransfer
{
    public abstract class ManualMapper
    {
        public static PlayerInfoDTO CreatePlayerInfoDTO(Player player)
        {
            PlayerInfoDTO playerDTO = new PlayerInfoDTO
            {
                UserName = player.Account.UserName,
                Nickname = player.Nickname,
                Nationality = player.Nationality,
                Status = player.Status,
                Email = player.Account.Email,
                IsVerified = player.Account.IsVerified
            };

            return playerDTO;
        }

        public static AccountDTO CreateAccountDTO(UserAccount userAccount)
        {
            AccountDTO accountDTO = new AccountDTO
            {
                UserName = userAccount.UserName,
                Password = userAccount.Password,
                Email = userAccount.Email
            };

            return accountDTO;
        }

        public static PlayerAnalyticsDTO CreatePlayerAnalyticDTO(PerformanceRecord performanceRecord)
        {
            PlayerAnalyticsDTO playerAnalyticsDTO = new PlayerAnalyticsDTO()
            {
                Nickname = performanceRecord.Nickname,
                WordsFound = performanceRecord.WordsFound,
                DroppedMatches = performanceRecord.DroppedMatches,
                WonMatches = performanceRecord.WonMatches,
                LostMatches = performanceRecord.LostMatches,
                PlayedMatches = performanceRecord.PlayedMatches,
                HighestScore = performanceRecord.HighestScore,
                TotalScore = performanceRecord.TotalScore
            };

            return playerAnalyticsDTO;
        }

        public static PlayerOverviewDTO CreatePlayerOverviewDTO(PerformanceRecord performanceRecord)
        {
            PlayerOverviewDTO playerOverviewDTO = new PlayerOverviewDTO()
            {
                UserName = performanceRecord.PlayerPerformance.UserName,
                Nickname = performanceRecord.Nickname,
                Victories = performanceRecord.WonMatches,
                GamesPlayed = performanceRecord.PlayedMatches,
                HighestScore = performanceRecord.HighestScore,
                TotalScore = performanceRecord.TotalScore
            };
            return playerOverviewDTO;
        }

        public static UserAccount CreateAccount(
            AccountDTO accountDTO, Func<string> generateFriendcode)
        {
            UserAccount userAccount = new UserAccount(
                accountDTO.UserName, accountDTO.Email,
                accountDTO.Password, generateFriendcode());

            return userAccount;
        }

        public static PublicLobbyPreviewDTO CreatePublicLobbyPreviewDTO(Lobby lobby)
        {
            PublicLobbyPreviewDTO publicLobbyPreviewDTO = new PublicLobbyPreviewDTO
            {
                LobbySize = lobby.Size,
                PlayersInside = lobby.Players.Count,
                GameMode = lobby.GameMatch.GameMode,
                LobbyCode = lobby.Code
            };

            return publicLobbyPreviewDTO;
        }

        public static TopPlayerDTO CreateTopPlayerDTO(PerformanceRecord performanceRecord)
        {
            TopPlayerDTO topPlayerDTO = new TopPlayerDTO()
            {
                PlayerNickname = performanceRecord.Nickname,
                PlayedMatches = performanceRecord.PlayedMatches,
                WonMatches = performanceRecord.WonMatches,
                TotalScore = performanceRecord.TotalScore
            };

            return topPlayerDTO;
        }
    }
}
