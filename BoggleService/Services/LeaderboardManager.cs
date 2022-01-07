using BoggleModel;
using BoggleModel.DataAccess;
using BoggleModel.DataTransfer;
using BoggleModel.DataTransfer.Dtos;
using BoggleService.Contracts;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace BoggleService.Services
{
    public partial class BoggleServices : ILeaderboardManagerContract
    {
        public void RetrieveTopPlayers(string userName)
        {
            List<PerformanceRecord> topPlayers = GameService.GetTopPlayers();
            List<TopPlayerDTO> topPlayerDTOs = new List<TopPlayerDTO>();

            foreach (PerformanceRecord topPlayer in topPlayers)
            {
                TopPlayerDTO topPlayerDTO = ManualMapper.CreateTopPlayerDTO(topPlayer);
                topPlayerDTOs.Add(topPlayerDTO);
            }

            LeaderboardManagerCallback.DisplayTopPlayers(topPlayerDTOs.ToArray());
        }

        ILeaderboardManagerCallback LeaderboardManagerCallback
        {
            get
            {
                return OperationContext.Current
                    .GetCallbackChannel<ILeaderboardManagerCallback>();
            }
        }
    }
}
