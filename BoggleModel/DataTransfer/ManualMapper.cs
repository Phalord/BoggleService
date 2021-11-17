using BoggleModel.DataTransfer.Dtos;
using System;

namespace BoggleModel.DataTransfer
{
    public abstract class ManualMapper
    {
        public static PlayerInfoDTO CreatePlayerInfoDTO(Player player)
        {
            PlayerInfoDTO result = new PlayerInfoDTO
            {
                UserName = player.Account.UserName,
                Nickname = player.Nickname,
                Nationality = player.Nationality,
                Status = player.Status,
                Email = player.Account.Email,
                IsVerified = player.Account.IsVerified
            };

            return result;
        }

        public static UserAccount CreateAccount(
            AccountDTO accountDTO, Func<string> generateFriendcode)
        {
            UserAccount userAccount = new UserAccount(
                accountDTO.UserName, accountDTO.Email,
                accountDTO.Password, generateFriendcode());

            return userAccount;
        }
    }
}
