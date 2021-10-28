using System.Globalization;
using System.Resources;

namespace BoggleModel
{
    public partial class Board
    {
        public Board(string matchLanguage)
        {
            Dices = GenerateDices(matchLanguage);
        }

        public virtual Dice[] Dices { get; private set; }

        private Dice[] GenerateDices(string matchLanguage)
        {
            ResourceManager resourceManager =
                new ResourceManager("BoardDices", typeof(Board).Assembly);
            CultureInfo cultureInfo = new CultureInfo(matchLanguage);

            Dice[] dices = new Dice[16];

            for (int index = 0; index < dices.Length; index++)
            {
                string resource = "dice" + index;
                dices[index] = new Dice(resourceManager.GetString(resource, cultureInfo));
            }

            return dices;
        }
    }

    public partial class Dice
    {

        public Dice(string faces)
        {
            Faces = BuildFacesArray(faces);
        }

        public string[] Faces { get; set; }

        private string[] BuildFacesArray(string faces)
		{
			string[] facesArray = new string[6];
			int arrayIndex = 0;

			foreach (char face in faces)
			{
				if (!face.Equals('-'))
				{
					facesArray[arrayIndex] += face;
				}
				else
				{
					arrayIndex++;
				}
			}

			return facesArray;
        }
    }
}
