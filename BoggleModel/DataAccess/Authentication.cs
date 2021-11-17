using BoggleModel.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleModel.DataAccess
{
    public abstract class Authentication
    {
        public static void GetUserAccount(
            string userName, ref UserAccount userAccount)
        {
            using (var database = new BoggleContext())
            {
                var query = database.UserAccounts.FirstOrDefault(
                    account => account.UserName.Equals(userName));

                if (query != null)
                {
                    userAccount = new UserAccount(
                        query.UserName, query.Email,
                        string.Empty, query.Player.FriendCode);
                }
            }
        }

        public static void CreateAccount(UserAccount newUser)
        {
            UserAccountEntity accountEntity = new UserAccountEntity()
            {
                UserName = newUser.UserName,
                Email = newUser.Email,
                Password = newUser.Password,
                IsVerified = newUser.IsVerified
            };

            accountEntity.Player = new PlayerEntity()
            {
                Nickname = accountEntity.UserName,
                Nationality = string.Empty,
                FriendCode = accountEntity.Player.FriendCode,
                Status = newUser.PlayerAccount.Status,
                Account = accountEntity,
                PerformanceRecord = new PerformanceRecordEntity(),
                FriendRequests = new List<FriendRequestEntity>()
            };

            using (var database = new BoggleContext())
            {
                database.UserAccounts.Add(accountEntity);
                database.SaveChanges();
            }
        }

        public static void GetUserAccountByEmail(string email, ref UserAccount userAccount)
        {
            using (var database = new BoggleContext())
            {
                var query = database.UserAccounts
                    .FirstOrDefault(account => account.Email == email);
                if (query != null)
                {
                    userAccount = new UserAccount(
                        query.UserName, query.Email,
                        query.Password, query.Player.FriendCode);
                }
            }
        }

        public static void ValidateAccountEmail(string email)
        {
            using (var database = new BoggleContext())
            {
                var query = database.UserAccounts
                    .Where(account => account.Email == email)
                    .FirstOrDefault();
                
                if (query != null)
                {
                    query.IsVerified = true;
                    database.SaveChanges();
                }
            }


            throw new NotImplementedException();
        }
    }
}
