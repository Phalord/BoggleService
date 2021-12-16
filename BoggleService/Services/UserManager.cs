using BoggleModel;
using BoggleModel.DataAccess;
using BoggleModel.DataTransfer;
using BoggleModel.DataTransfer.Dtos;
using BoggleService.Contracts;
using BoggleService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace BoggleService.Services
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single, 
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class BoggleServices : IBoggleServiceContracts
    {
        private static readonly Random random = new Random();
        private readonly Dictionary<string, string>
            usersToValidate = new Dictionary<string, string>();
        private readonly Dictionary<string, IBoggleServicesCallback>
            playersConnected = new Dictionary<string, IBoggleServicesCallback>();

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
        private const string playerLogged = "PlayerAlreadyLogged";
        #endregion


        public void CreateAccount(AccountDTO accountDTO)
        {
            string accountCreationStatus;

            if (IsUserNameRegistered(accountDTO.UserName))
            {
                accountCreationStatus = usernameRegistered;
                Callback.AskForEmailValidation(accountCreationStatus, string.Empty);
            }
            else if (IsEmailRegistered(accountDTO.Email))
            {
                accountCreationStatus = emailRegistered;
                Callback.AskForEmailValidation(accountCreationStatus, string.Empty);
            } else
            {
                UserAccount newUser = ManualMapper
                    .CreateAccount(accountDTO, GenerateFriendCode);

                Authentication.CreateAccount(newUser);

                accountCreationStatus = accountCreated;
                GenerateVerificationCode(newUser);
                Console.WriteLine("User {0} created", newUser.UserName);
                Callback.AskForEmailValidation(accountCreationStatus, newUser.Email);
            }

        }

        public void LogIn(string userName, string password)
        {
            Console.WriteLine("Attempting log in as {0}", userName);

            UserAccount userAccount = null;
            userAccount = Authentication.GetUserAccount(userName, userAccount);
            AccountDTO accountDTO = null;

            string accessStatus;
            if (!(userAccount is null))
            {
                if (userAccount.IsVerified)
                {
                    if(!UserAlreadyLoggedIn(userAccount))
                    {
                        if (BCrypt.Net.BCrypt.Verify(password, userAccount.Password))
                        {
                            accessStatus = accessGranted;
                            playersConnected.Add(userAccount.UserName, Callback);
                            accountDTO = ManualMapper
                                .CreateAccountDTO(userAccount);
                        }
                        else
                        {
                            accessStatus = wrongPassword;
                        }
                    }
                    else
                    {
                        accessStatus = playerLogged;
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

            Callback.GrantAccess(accessStatus, accountDTO);
        }

        public void ValidateEmail(string validationCode, string email)
        {
            AccountDTO accountDTO = null;
            string validationStatus;
            
            if (usersToValidate.ContainsKey(email))
            {
                if (usersToValidate[email].Equals(validationCode))
                {
                    Authentication.ValidateAccountEmail(email);
                    UserAccount userAccount = null;
                    userAccount = Authentication.GetUserAccountByEmail(email, userAccount);
                    playersConnected.Add(userAccount.UserName, Callback);
                    accountDTO = ManualMapper.CreateAccountDTO(userAccount);
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

            Callback.GrantValidation(validationStatus, accountDTO);
        }


        #region Local Methods

        private bool UserAlreadyLoggedIn(UserAccount userAccount)
        {
            return playersConnected.ContainsKey(userAccount.UserName);
        }

        private bool IsUserNameRegistered(string userName)
        {
            UserAccount userAccount = null;
            userAccount = Authentication.GetUserAccount(userName, userAccount);
            
            if (userAccount == null)
            {
                return false;
            }

            return true;
        }

        private bool IsEmailRegistered(string email)
        {
            UserAccount userAccount = null;
            userAccount = Authentication.GetUserAccountByEmail(email, userAccount);

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

            if (usersToValidate.ContainsKey(newUser.Email))
            {
                usersToValidate[newUser.Email] = emailValidationCode;
            } else
            {
                usersToValidate.Add(newUser.Email, emailValidationCode);
            }
            SendEmailValidationCode(newUser.Email, emailValidationCode);
        }

        private void SendEmailValidationCode(
            string userEmail, string emailValidationCode)
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
            userAccount = Authentication.GetUserAccountByFriendCode(friendCode, userAccount);

            if (userAccount == null)
            {
                return false;
            }

            return true;
        }

        #endregion


        IBoggleServicesCallback Callback
        {
            get
            {
                return OperationContext.Current
                    .GetCallbackChannel<IBoggleServicesCallback>();
            }
        }
    }
}
