using BoggleModel.DataTransfer.Dtos;
using System.ServiceModel;

namespace BoggleService.Contracts
{
    [ServiceContract(
       CallbackContract = typeof(ILeaderboardManagerCallback),
       SessionMode = SessionMode.Required)]
    public interface ILeaderboardManagerContract
    {
        [OperationContract(IsOneWay = true)]
        void RetrieveTopPlayers(string userName);
    }

    [ServiceContract]
    public interface ILeaderboardManagerCallback
    {
        [OperationContract]
        void DisplayTopPlayers(TopPlayerDTO[] topPlayersDTOs);
    }
}
