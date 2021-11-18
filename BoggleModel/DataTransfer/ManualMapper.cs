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
