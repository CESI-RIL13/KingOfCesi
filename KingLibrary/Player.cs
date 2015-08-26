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
        public LocationEnum Location { get {
                if (Hp == 0)
                    return Location = LocationEnum.CIMETARY_CESI;
                else
                    return _location;
            } set {
                if (Hp > 0)
                    _location = value;
            }
        }
        private LocationEnum _location;
        public DateTime LastResponse { get; set; }
        public string IdConnection;
        public int NbLancer { get; set; }
        public List<Dice> Dices { get; set; }
        public List<Dice> SelectedDices = new List<Dice>();
        public bool HasResolveDice;
        public bool KingOfCesi;
        public bool Disconnected = false;

        public Player(string pseudo, string idConnection)
        {
            Pseudo = pseudo;
            IdConnection = idConnection;
            Energy = 0;
            VictoryPoint = 0;
            Hp = 2;
            Location = LocationEnum.OUT_CESI;
        }
        public void ThrowDices()
        {
            Dices = new List<Dice>();
            for (int i = 0; i<(6-SelectedDices.Count); i++)
            {
                Dices.Add(new Dice());
            }
            NbLancer--;
            if (NbLancer == 0)
            {
                foreach (Dice dice in Dices)
                {
                    SelectedDices.Add(dice);
                }
                Dices.Clear();
            }
        }

        public void SelectDice(int positionDe)
        {
            SelectedDices.Add(Dices[positionDe]);
            Dices.RemoveAt(positionDe);
        }

        public void UnselectDice(int positionDe)
        {
            Dices.Add(SelectedDices[positionDe]);
            SelectedDices.RemoveAt(positionDe);
        }

        public void GainVPWithDices(int dice, int value)
        {
            this.VictoryPoint += (dice + value-3);
        }

        public void Soingner(int soin)
        {
            this.Hp = (this.Hp + soin) >= 10 ? 10 : this.Hp + soin;
        }

        public void PrendreDegats(int point)
        {
            this.Hp = (this.Hp - point) <= 0 ? 0 : this.Hp - point;
        }
    }
}
