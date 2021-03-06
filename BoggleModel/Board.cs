using System;
using System.Runtime.Serialization;
using System.Globalization;
using BoggleModel.Properties;

namespace BoggleModel
{
    [DataContract]
    public partial class Board
    {
        public Board(string matchLanguage)
        {
            Dices = GenerateDices(matchLanguage);
        }

        [DataMember]
        public Dice[] Dices { get; private set; }

        private Dice[] GenerateDices(string matchLanguage)
        {
            CultureInfo cultureInfo = new CultureInfo(matchLanguage);

            Dice[] dices = new Dice[16];

            for (int index = 0; index < dices.Length; index++)
            {
                string resource = string.Format("dice{0}", index + 1);
                dices[index] = new Dice(Localization.ResourceManager
                    .GetString(resource, cultureInfo));
            }

            return dices;
        }

        public void ShakeDice()
        {
            // TODO Implementar el acomodo de los dados
            throw new NotImplementedException();
        }

        public bool ValidateWord(string word)
        {
            // TODO Implementar validación de la palabra
            throw new NotImplementedException();
        }

    }

    [DataContract]
    public partial class Dice
    {

        public Dice(string faces)
        {
            Faces = BuildFacesArray(faces);
        }

        [DataMember]
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
