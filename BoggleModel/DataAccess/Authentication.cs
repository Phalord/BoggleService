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
            UserAccountEntity accountEntity = new UserAccountEntity(
                newUser.Email, newUser.Email,
                newUser.Password, newUser.PlayerAccount.FriendCode);

            using (var database = new BoggleContext())
            {
                database.UserAccounts.Add(accountEntity);
                database.SaveChanges();
            }
        }

        public static void GetUserAccountByEmail(
            string email, ref UserAccount userAccount)
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
                var query = database.Players
                    .Where(player => player.Account.Email.Equals(email))
                    .FirstOrDefault();
                
                if (query != null)
                {
                    query.Account.IsVerified = true;
                    database.SaveChanges();
                }
            }
        }
    }
}
