using BoggleModel;
using BoggleService.Contracts;
using BoggleService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace BoggleService.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, 
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class UserManager : IUserManagerContract
    {
        private static readonly Random random = new Random();
        private readonly Dictionary<string, string> usersToValidate = new Dictionary<string, string>();

        #region Constants
        private const int friendCodeSize = 10;
        private const int emailValidationCodeSize = 5;
        private const string accessGranted = "AccessGranted";
        private const string wrongPassword = "WrongPassword";
        private const string unverifiedEmail = "UnverifiedEmail";
        private const string nonExistentUser = "NonExistentUser";
        private const string usernameRegistered = "UserNameRegistered";
        private const string emailRegistered = "EmailRegistered";
        private const string accountCreated = "AccountCreated";
        private const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const string emailNotFound = "EmailNotFound";
        private const string wrongValidationCode = "WrongValidationCode";
        private const string emailValidated = "EmailValidated";
        #endregion


        public void CreateAccount(
            string userName, string email, string password)
        {
            string accountCreationStatus;

            if (IsUserNameRegistered(userName))
            {
                accountCreationStatus = usernameRegistered;
                Callback.AskForEmailValidation(accountCreationStatus, string.Empty);
            }
            else if (IsEmailRegistered(email))
            {
                accountCreationStatus = emailRegistered;
                Callback.AskForEmailValidation(accountCreationStatus, string.Empty);
            } else
            {
                UserAccount newUser = new UserAccount(userName,
                email, password, GenerateFriendCode());

                using (var database = new BoggleContext())
                {
                    database.UserAccounts.Add(newUser);
                    database.SaveChanges();
                    accountCreationStatus = accountCreated;
                    GenerateVerificationCode(newUser);
                    Console.WriteLine("User {0} created", newUser.UserName);
                    Callback.AskForEmailValidation(accountCreationStatus, newUser.Email);
                }
            }

        }

        public void LogIn(string userName, string password)
        {
            Console.WriteLine("Attempting log in as {0}", userName);

            UserAccount userAccount = null;
            GetUserAccount(userName, ref userAccount);

            string accessStatus;
            if (userAccount != null)
            {
                if (userAccount.IsVerified)
                {
                    if (userAccount.Password.Equals(password))
                    {
                        accessStatus = accessGranted;
                    }
                    else
                    {
                        accessStatus = wrongPassword;
                    }
                }
                else
                {
                    accessStatus = unverifiedEmail;
                    GenerateVerificationCode(userAccount);
                }
            }
            else
            {
                accessStatus = nonExistentUser;
            }

            Callback.GrantAccess(accessStatus);
        }

        public void ValidateEmail(string validationCode, string email)
        {
            string validationStatus;

            if (usersToValidate.ContainsKey(email))
            {
                if (usersToValidate["email"].Equals(validationCode))
                {
                    using (var database = new BoggleContext())
                    {
                        var query = database.UserAccounts
                                .FirstOrDefault(account => account.Email == email);
                        if (query != null)
                        {
                            query.Verify();
                            database.SaveChanges();
                        }
                    }
                    validationStatus = emailValidated;
                    Console.WriteLine("Email {0} verified.", email);
                } else
                {
                    validationStatus = wrongValidationCode;
                }
            } else
            {
                validationStatus = emailNotFound;
            }

            Callback.GrantValidation(validationStatus);
        }


        #region Local Methods

        private bool IsUserNameRegistered(string userName)
        {
            UserAccount userAccount = null;
            GetUserAccount(userName, ref userAccount);

            if (userAccount == null)
            {
                return false;
            }

            return true;
        }

        private bool IsEmailRegistered(string email)
        {
            UserAccount userAccount = null;
            GetUserAccountByEmail(email, ref userAccount);

            if (userAccount == null)
            {
                return false;
            }

            return true;
        }

        private void GenerateVerificationCode(UserAccount newUser)
        {
            string emailValidationCode = new string(Enumerable
                .Repeat(characters, emailValidationCodeSize)
                .Select(characters => characters[random.Next(characters.Length)]).ToArray());

            usersToValidate.Add(newUser.Email, emailValidationCode);
            SendEmailValidationCode(newUser.Email, emailValidationCode);
        }

        private void SendEmailValidationCode(string userEmail, string emailValidationCode)
        {
            EmailSender emailSender = new EmailSender(userEmail);
            string body = "Your email verification code is:\n" + emailValidationCode;
            string subject = "Email validation";
            emailSender.SendEmail(subject, body);
        }

        private string GenerateFriendCode()
        {
            string friendCode;
            do
            {
                friendCode = new string(Enumerable
                    .Repeat(characters, friendCodeSize)
                    .Select(characters => characters[random.Next(characters.Length)]).ToArray());
            } while (IsFriendCodeInUse(friendCode));

            return friendCode;
        }

        private bool IsFriendCodeInUse(string friendCode)
        {
            UserAccount userAccount = null;
            GetUserAccountByFriendCode(friendCode, ref userAccount);

            if (userAccount == null)
            {
                return false;
            }

            return true;
        }

        private void GetUserAccount(string userName, ref UserAccount userAccount)
        {
            using (var database = new BoggleContext())
            {
                var query = database.UserAccounts
                    .FirstOrDefault(account => account.UserName == userName);
                if (query != null)
                {
                    userAccount = new UserAccount(
                        query.UserName, query.Email,
                        query.Password, query.PlayerAccount.FriendCode);
                }
            }
        }

        private void GetUserAccountByEmail(string email, ref UserAccount userAccount)
        {
            using (var database = new BoggleContext())
            {
                var query = database.UserAccounts
                    .FirstOrDefault(account => account.Email == email);
                if (query != null)
                {
                    userAccount = new UserAccount(
                        query.UserName, query.Email,
                        query.Password, query.PlayerAccount.FriendCode);
                }
            }
        }

        private void GetUserAccountByFriendCode(string friendCode, ref UserAccount userAccount)
        {
            using (var database = new BoggleContext())
            {
                var query = database.UserAccounts
                    .FirstOrDefault(account => account.PlayerAccount.FriendCode == friendCode);
                if (query != null)
                {
                    userAccount = new UserAccount(
                        query.UserName, query.Email,
                        query.Password, query.PlayerAccount.FriendCode);
                }
            }
        }

        #endregion


        IUserManagerCallback Callback
        {
            get
            {
                return OperationContext.Current
                    .GetCallbackChannel<IUserManagerCallback>();
            }
        }
    }
}
