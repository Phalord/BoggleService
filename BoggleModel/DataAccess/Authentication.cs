using BoggleModel.DataAccess.Entities;
using System.Linq;

namespace BoggleModel.DataAccess
{
    public abstract class Authentication
    {
        public static UserAccount GetUserAccount(
            string userName, UserAccount userAccount)
        {
            using (var database = new BoggleContext())
            {
                var query = database.UserAccounts
                    .Where(account => account.UserName.Equals(userName))
                    .FirstOrDefault();

                if (query != null)
                {
                    userAccount = new UserAccount(
                        query.UserName, query.Email,
                        query.Password, query.Player.FriendCode)
                    {
                        IsVerified = query.IsVerified
                    };

                    return userAccount;
                }
            }

            return userAccount;
        }

        public static void CreateAccount(UserAccount newUser)
        {
            UserAccountEntity accountEntity = new UserAccountEntity(
                newUser.UserName, newUser.Email,
                newUser.Password, newUser.PlayerAccount.FriendCode);

            using (var database = new BoggleContext())
            {
                database.UserAccounts.Add(accountEntity);
                database.SaveChanges();
            }
        }

        public static UserAccount GetUserAccountByEmail(
            string email, UserAccount userAccount)
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

            return userAccount;
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

        public static UserAccount GetUserAccountByFriendCode(string friendCode, UserAccount userAccount)
        {
            using (var database = new BoggleContext())
            {
                var query = database.UserAccounts.FirstOrDefault(
                    account => account.Player.FriendCode == friendCode);

                if (query != null)
                {
                    userAccount = new UserAccount(
                        query.UserName, query.Email,
                        query.Password, query.Player.FriendCode);
                }
            }

            return userAccount;
        }
    }
}
