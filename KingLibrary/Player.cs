using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingLibrary
{
    public class Player
    {
        public string Pseudo { get; set; }
        public MonsterEnum Monster { get; set; }
        public int Energy { get; set; }
        public int VictoryPoint { get; set; }
        public int Hp { get; set; }
        public LocationEnum Location { get; set; }
        public DateTime LastResponse { get; set; }
        public string IdConnection;
        public int NbLancer { get; set; }
        public List<Dice> listededes { get; set; }
        public List<Dice> selecaodedes = new List<Dice>();
        public bool IsCurrentPlayer { get {

                return true;
        } }

        public Player(string pseudo, string idConnection)
        {
            Pseudo = pseudo;
            IdConnection = idConnection;
            Energy = 0;
            VictoryPoint = 0;
            Hp = 10;
            Location = LocationEnum.OUT_CESI;
        }
        public void ThrowDices()
        {
            listededes = new List<Dice>();
            for (int i = 0; i<(6-selecaodedes.Count); i++)
            {
                listededes.Add(new Dice());
            }
            NbLancer--;
            if (NbLancer == 0)
            {
                foreach (Dice dice in listededes)
                {
                    selecaodedes.Add(dice);
                }
                listededes.Clear();
            }
        }

        public void SelectDice(int positionDe)
        {
            selecaodedes.Add(listededes[positionDe]);
            listededes.RemoveAt(positionDe);
        }

        public void UnselectDice(int positionDe)
        {
            listededes.Add(selecaodedes[positionDe]);
            selecaodedes.RemoveAt(positionDe);
        }

        public void GainVPWithDices(int dice, int value)
        {
            this.VictoryPoint += (dice + value-3);
        }
    }
}
