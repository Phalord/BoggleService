using BoggleModel.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BoggleModel.DataAccess
{
    public abstract class GameService
    {
        public static Player GetPlayer(string userName)
        {
            Player player = null;
            using (BoggleContext database = new BoggleContext())
            {
                var query = database.Players
                    .Where(player_aux => player_aux.UserName.Equals(userName))
                    .FirstOrDefault();

                if (!(query is null))
                {
                    player = new Player()
                    {
                        UserName = userName,
                        Nickname = query.Nickname,
                        Nationality = query.Nationality
                    };
                }
            }
            return player;
        }

        public static List<PerformanceRecord> GetTopPlayers()
        {
            List<PerformanceRecord> topPlayers = new List<PerformanceRecord>();
            using (var database = new BoggleContext())
            {
                var query = database.PerformanceRecords
                    .OrderByDescending(performanceRecord => performanceRecord.TotalScore)
                    .Take(5);
                if (!(query is null))
                {
                    foreach (PerformanceRecordEntity performanceRecord in query.ToList())
                    {
                        PerformanceRecord topPlayer = new PerformanceRecord()
                        {
                            WordsFound = performanceRecord.WordsFound,
                            DroppedMatches = performanceRecord.DroppedMatches,
                            WonMatches = performanceRecord.WonMatches,
                            LostMatches = performanceRecord.LostMatches,
                            PlayedMatches = performanceRecord.PlayedMatches,
                            HighestScore = performanceRecord.HighestScore,
                            TotalScore = performanceRecord.TotalScore,
                            Nickname = database.Players.Where(
                                player => player.UserName
                                .Equals(performanceRecord.Username))
                            .FirstOrDefault().Nickname
                        };
                        topPlayers.Add(topPlayer);
                    }
                }
            }
            return topPlayers;
        }

        public static PerformanceRecord GetPerformanceRecord(string username)
        {
            PerformanceRecord performanceRecord = null;
            using (BoggleContext database = new BoggleContext())
            {
                var query = database.PerformanceRecords
                    .Where(record => record.Player.UserName.Equals(username))
                    .FirstOrDefault();

                if (!(query is null))
                {
                    performanceRecord = new PerformanceRecord()
                    {
                        WordsFound = query.WordsFound,
                        DroppedMatches = query.DroppedMatches,
                        LostMatches = query.LostMatches,
                        PlayedMatches = query.PlayedMatches,
                        HighestScore = query.HighestScore,
                        TotalScore = query.TotalScore,
                        Nickname = query.Player.Nickname
                    };
                }
            }
            return performanceRecord;
        }
    }
}
