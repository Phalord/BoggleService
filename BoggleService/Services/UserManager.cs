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

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace BoggleService.Services
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single, 
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class BoggleServices : IUserManagerContract
    {
        private static readonly Random random = new Random();
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private readonly Dictionary<string, string>
            usersToValidate = new Dictionary<string, string>();
        private readonly Dictionary<string, IUserManagerCallback>
            playersConnected = new Dictionary<string, IUserManagerCallback>();

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

        /// <summary>
        /// If the user name and password matches the ones
        /// of a registered player it gives access to the player.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void LogIn(string userName, string password)
        {
            log.Info(string.Format("Attempting log in as {0}", userName));

            UserAccount userAccount = Authentication.GetUserAccount(userName);
            AccountDTO accountDTO = null;

            string accessStatus;
            if (!(userAccount is null))
            {
                accountDTO = ManualMapper
                    .CreateAccountDTO(userAccount);
                if (userAccount.IsVerified)
                {
                    if (!UserAlreadyLoggedIn(userAccount))
                    {
                        if (playersInLobby.ContainsKey(userName))
                        {
                            string lobbyCode = GetLobbyCodeByPlayerInLobby(userName);
                            ExitLobby(userName, lobbyCode);
                        }
                        if (BCrypt.Net.BCrypt.Verify(password, userAccount.Password))
                        {
                            accessStatus = accessGranted;
                            playersConnected.Add(userAccount.UserName, UserManagerCallback);
                        }
                        else
                        {
                            accessStatus = wrongPassword;
                        }
                    }
                    else
                    {
                        accessStatus = accessGranted;
                        if (playersInLobby.ContainsKey(userName))
                        {
                            ExitLobby(userName, GetLobbyCodeByPlayerInLobby(userName));
                        }
                        try
                        {
                            playersConnected[userAccount.UserName].CloseSession();
                        }
                        catch (CommunicationObjectAbortedException communicationAborted)
                        {
                            log.Error(communicationAborted.Message, communicationAborted);
                        }
                        finally
                        {
                            playersConnected[userAccount.UserName] = UserManagerCallback;
                        }
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

            try
            {
                UserManagerCallback.GrantAccess(accessStatus, accountDTO);
            }
            catch (CommunicationObjectAbortedException communicationAborted)
            {
                LogOut(accountDTO.UserName);
                log.Error(communicationAborted.Message, communicationAborted);
            }
        }

        /// <summary>
        /// Logs out the player from the service.
        /// If the player's in a Lobby it gets removed.
        /// </summary>
        /// <param name="userName">Player's user name</param>
        public void LogOut(string userName)
        {
            if (userName.Length > 0 && playersConnected.ContainsKey(userName))
            {
                log.Info(string.Format("{0} Logging out.", userName));

                try
                {
                    playersConnected[userName].CloseSession();
                }
                catch (CommunicationObjectAbortedException communicationAborted)
                {
                    log.Error(communicationAborted.Message, communicationAborted);
                }
                finally
                {
                    if (playersInLobby.ContainsKey(userName))
                    {
                        string lobbyCode = GetLobbyCodeByPlayerInLobby(userName);
                        ExitLobby(userName, lobbyCode);
                    }
                    playersConnected.Remove(userName);
                }
            }
        }

        /// <summary>
        /// Creates a new account with the info provided
        /// if neither the username nor email are already registered.
        /// </summary>
        /// <param name="accountDTO">Data Transfer Object with needed information to create an account</param>
        public void CreateAccount(AccountDTO accountDTO)
        {
            UserAccount newUser = new UserAccount();
            string accountCreationStatus;

            if (IsUserNameRegistered(accountDTO.UserName))
            {
                accountCreationStatus = usernameRegistered;
            }
            else if (IsEmailRegistered(accountDTO.Email))
            {
                accountCreationStatus = emailRegistered;
            } else
            {
                newUser = ManualMapper
                    .CreateAccount(accountDTO, GenerateFriendCode);

                Authentication.CreateAccount(newUser);

                accountCreationStatus = accountCreated;
                GenerateVerificationCode(newUser);
                log.Info(string.Format("User {0} created", newUser.UserName));
            }

            try
            {
                UserManagerCallback.AskForEmailValidation(accountCreationStatus, newUser.Email);
            }
            catch (CommunicationObjectAbortedException communicationAborted)
            {
                LogOut(accountDTO.UserName);
                log.Error(communicationAborted.Message, communicationAborted);
            }
        }

        /// <summary>
        /// Receives validation code to validate that
        /// the email provided by the player is a working email.
        /// </summary>
        /// <param name="validationCode">Validation code sent to player's email</param>
        /// <param name="email">Player's email to validate</param>
        public void ValidateEmail(string validationCode, string email)
        {
            AccountDTO accountDTO = null;
            string validationStatus;
            
            if (usersToValidate.ContainsKey(email))
            {
                if (usersToValidate[email].Equals(validationCode))
                {
                    Authentication.ValidateAccountEmail(email);
                    UserAccount userAccount = Authentication.GetUserAccountByEmail(email);
                    playersConnected.Add(userAccount.UserName, UserManagerCallback);
                    accountDTO = ManualMapper.CreateAccountDTO(userAccount);
                    validationStatus = emailValidated;
                    log.Info(string.Format("Email {0} verified.", email));
                } else
                {
                    validationStatus = wrongValidationCode;
                }
            } else
            {
                validationStatus = emailNotFound;
            }

            try
            {
                UserManagerCallback.GrantValidation(validationStatus, accountDTO);
            }
            catch (CommunicationObjectAbortedException communicationAborted)
            {
                LogOut(accountDTO.UserName);
                log.Error(communicationAborted.Message, communicationAborted);
            }
        }


        #region Local Methods

        private bool UserAlreadyLoggedIn(UserAccount userAccount)
        {
            return playersConnected.ContainsKey(userAccount.UserName);
        }

        private bool IsUserNameRegistered(string userName)
        {
            UserAccount userAccount = Authentication.GetUserAccount(userName);
            
            if (userAccount == null)
            {
                return false;
            }

            return true;
        }

        private bool IsEmailRegistered(string email)
        {
            UserAccount userAccount = Authentication.GetUserAccountByEmail(email);

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

        private string GetLobbyCodeByPlayerInLobby(string userName)
        {
            return lobbies.Where(lobby => lobby.Value.Players.Contains(
                lobby.Value.Players.FirstOrDefault(player =>
                player.UserName.Equals(userName)))).FirstOrDefault().Key;
        }

        #endregion


        IUserManagerCallback UserManagerCallback
        {
            get
            {
                return OperationContext.Current
                    .GetCallbackChannel<IUserManagerCallback>();
            }
        }
    }
}
