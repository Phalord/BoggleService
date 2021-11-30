using System.Linq;

namespace BoggleModel.DataAccess
{
    public abstract class GameService
    {
        public static Player GetPlayer(
            string userName, Player player)
        {
            using (var database = new BoggleContext())
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

                    return player;
                }
            }

            return player;
        }
    }
}
