using BoggleModel;
using BoggleModel.DataAccess;
using BoggleModel.DataTransfer;
using BoggleModel.DataTransfer.Dtos;
using BoggleService.Contracts;
using System.ServiceModel;

namespace BoggleService.Services
{
    public partial class BoggleServices : IProfileManagerContract
    {
        /// <summary>
        /// Retreives a Player's Performance Record by its username
        /// and sends it back to the client in a PlayerAnalyticsDTO.
        /// </summary>
        /// <param name="userName"></param>
        public void RetreivePlayerAnalytics(string userName)
        {
            Player player = GameService.GetPlayer(userName);
            PlayerAnalyticsDTO playerAnalyticsDTO = null;
            if (!(player is null))
            {
                PerformanceRecord performanceRecord =
                    GameService.GetPerformanceRecord(userName);

                playerAnalyticsDTO = ManualMapper.CreatePlayerAnalyticDTO(performanceRecord);
            }

            try
            {
                ProfileManagerCallback.DisplayPlayerAnalytics(playerAnalyticsDTO);
            }
            catch (CommunicationObjectAbortedException communicationAborted)
            {
                LogOut(userName);
                log.Error(communicationAborted.Message, communicationAborted);
            }
        }

        /// <summary>
        /// Retrieves the PerformanceRecord of a given Player
        /// and sends it back summarized in a PlayerOverviewDTO.
        /// </summary>
        /// <param name="userName"></param>
        public void RetreivePlayerOverview(string userName)
        {
            Player player = GameService.GetPlayer(userName);
            PlayerOverviewDTO playerOverviewDTO = null;
            if (!(player is null))
            {
                PerformanceRecord performanceRecord =
                    GameService.GetPerformanceRecord(userName);
                performanceRecord.PlayerPerformance = player;

                playerOverviewDTO = ManualMapper.CreatePlayerOverviewDTO(performanceRecord);
            }

            try
            {
                ProfileManagerCallback.DisplayPlayerOverview(playerOverviewDTO);
            }
            catch (CommunicationObjectAbortedException communicationAborted)
            {
                LogOut(userName);
                log.Error(communicationAborted.Message, communicationAborted);
            }
        }

        IProfileManagerCallback ProfileManagerCallback
        {
            get
            {
                return OperationContext.Current
                    .GetCallbackChannel<IProfileManagerCallback>();
            }
        }
    }
}
