using BoggleModel.DataTransfer.Dtos;
using System.ServiceModel;

namespace BoggleService.Contracts
{
    /// <summary>
    /// Contracts related to the profile section of the client.
    /// </summary>
    [ServiceContract(
       CallbackContract = typeof(IProfileManagerCallback),
       SessionMode = SessionMode.Required)]
    public interface IProfileManagerContract
    {
        [OperationContract(IsOneWay = true)]
        void RetreivePlayerOverview(string userName);

        [OperationContract(IsOneWay = true)]
        void RetreivePlayerAnalytics(string userName);
    }

    /// <summary>
    /// Callbacks related to the Profile Manager Service
    /// </summary>
    [ServiceContract]
    public interface IProfileManagerCallback
    {
        [OperationContract]
        void DisplayPlayerOverview(PlayerOverviewDTO playerOverviewDTO);

        [OperationContract]
        void DisplayPlayerAnalytics(PlayerAnalyticsDTO playerAnalyticsDTO);
    }
}
