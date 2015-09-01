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
        public int HpMax { get; set; }
        public LocationEnum Location { get {
                if (Hp == 0)
                {
                    return Location = LocationEnum.CIMETARY_CESI;
                }
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
        public int NbLancerMax { get; set; }
        public int DicesMax { get; set; }
        
        public List<Dice> Dices { get; set; }
        public List<Dice> SelectedDices = new List<Dice>();
        public bool HasResolveDice;
        public bool KingOfCesi;
        public bool Disconnected = false;
        public List<Card> MyCards = new List<Card>();

        public Player(string pseudo, string idConnection)
        {
            Pseudo = pseudo;
            IdConnection = idConnection;
            Energy = 0;
            VictoryPoint = 0;
            Hp = 10;
            HpMax = 10;
            DicesMax = 6;
            NbLancerMax = 3;
            Location = LocationEnum.OUT_CESI;
        }
        public void ThrowDices()
        {
            Dices = new List<Dice>();
            for (int i = 0; i<(DicesMax-SelectedDices.Count); i++)
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

        public void Soigner(int soin)
        {
            this.Hp = (this.Hp + soin) >= HpMax ? HpMax : this.Hp + soin;
        }

        public void PrendreDegats(int point)
        {
            this.Hp = (this.Hp - point) <= 0 ? 0 : this.Hp - point;
        }

        public void ImpactHp(int point)
        {
            this.Hp = (this.Hp + point) <= 0 ? 0 : this.Hp + point >= HpMax ? HpMax : this.Hp + point;
        }

        public void ImpactEnergy(int point)
        {
            this.Energy = (this.Energy + point) <= 0 ? 0 : this.Energy + point;
        }

        public void ImpactVp(int point)
        {
            this.VictoryPoint = (this.VictoryPoint + point) <= 0 ? 0 : this.VictoryPoint + point >= 20 ? VictoryPoint : this.VictoryPoint + point;
        }

        public void ImpactMaxHp(int point)
        {
            this.HpMax = (this.HpMax + point) <= 10 ? 10 : this.HpMax + point;
        }

        public void ImpactMaxRoll(int point)
        {
            this.NbLancerMax = (this.NbLancerMax + point) <= 3 ? 3 : this.NbLancerMax + point;
        }

        public void ImpactMaxDice(int point)
        {
            this.DicesMax = (this.DicesMax + point) <= 0 ? 0 : this.NbLancerMax + point >= 8 ? 8 : this.NbLancerMax + point;
        }
    }
}
