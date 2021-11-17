﻿using BoggleModel;
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
            Authentication.GetUserAccount(userName, ref userAccount);
            PlayerInfoDTO playerInfoDTO = null;

            string accessStatus;
            if (userAccount != null)
            {
                playerInfoDTO = ManualMapper
                    .CreatePlayerInfoDTO(userAccount.PlayerAccount);

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

            Callback.GrantAccess(accessStatus, playerInfoDTO);
        }

        public void ValidateEmail(string validationCode, string email)
        {
            PlayerInfoDTO playerInfoDTO = null;
            string validationStatus;
            
            if (usersToValidate.ContainsKey(email))
            {
                if (usersToValidate[email].Equals(validationCode))
                {
                    Authentication.ValidateAccountEmail(email);
                    UserAccount userAccount = null;
                    Authentication.GetUserAccountByEmail(email, ref userAccount);
                    ManualMapper.CreatePlayerInfoDTO(userAccount.PlayerAccount);
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

            Callback.GrantValidation(validationStatus, playerInfoDTO);
        }


        #region Local Methods

        private bool IsUserNameRegistered(string userName)
        {
            UserAccount userAccount = null;
            Authentication.GetUserAccount(userName, ref userAccount);

            if (userAccount == null)
            {
                return false;
            }

            return true;
        }

        private bool IsEmailRegistered(string email)
        {
            UserAccount userAccount = null;
            Authentication.GetUserAccountByEmail(email, ref userAccount);

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
            GetUserAccountByFriendCode(friendCode, ref userAccount);

            if (userAccount == null)
            {
                return false;
            }

            return true;
        }

        private void GetUserAccountByFriendCode(
            string friendCode, ref UserAccount userAccount)
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
