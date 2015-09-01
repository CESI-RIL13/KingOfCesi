using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingLibrary
{
    public delegate void EffectList(int amount, Board board);
    public delegate List<Player> ImpactedPlayers(Board board);
    public class CardAction
    {
        public ImpactedPlayers ImpactedPlayers { get; set; }
        public EventEnum TypeEvent { get; set; }
        public Card ParentCard { get; set; }
        public LifeTimeEnum LifeTime { get; set; }
        public EffectList EffectList { get; set; }
        public int Amount { get; set; }

        public CardAction()
        {

        }

        public CardAction(EventEnum typeEvent, LifeTimeEnum lifeTime, EffectList effectList, int amount)
        {
            this.TypeEvent = typeEvent;
            this.LifeTime = lifeTime;
            this.EffectList = effectList;
            this.Amount = amount;
        }

        public void Execute(Board board)
        {
            EffectList(Amount, board);
        }

        public void ImpactHp(int amount, Board board)
        {
            foreach (Player p in ImpactedPlayers(board))
            {
                p.ImpactHp(amount);
            }
        }

        public void ImpactVP(int amount, Board board)
        {
            foreach (Player p in ImpactedPlayers(board))
            {
                p.ImpactVp(amount);
            }
        }

        public void ImpactEnergy(int amount, Board board)
        {
            foreach (Player p in ImpactedPlayers(board))
            {
                p.ImpactHp(amount);
            }
        }

        public void ImpactMaxHp(int amount, Board board)
        {
            foreach (Player p in ImpactedPlayers(board))
            {
                p.ImpactHp(amount);
            }
        }

        public void ImpactMaxRoll(int amount, Board board)
        {
            foreach (Player p in ImpactedPlayers(board))
            {
                p.ImpactHp(amount);
            }
        }

        public void ImpactMaxDice(int amount, Board board)
        {
            foreach (Player p in ImpactedPlayers(board))
            {
                p.ImpactHp(amount);
            }
        }

        public List<Player> SelectCurrentPlayer(Board board)
        {
            return new List<Player> { board.CurrentPlayer };
        }

        public List<Player> SelectPlayerInTokyo(Board board)
        {
            return SelectAllPlayers(board).Where(x => x.Location == LocationEnum.CESI_CITY || x.Location == LocationEnum.CESI_BAY).ToList();
        }

        public List<Player> SelectPlayerOutside(Board board)
        {
            return SelectAllPlayers(board).Where(x => x.Location != LocationEnum.CESI_CITY && x.Location != LocationEnum.CESI_BAY).ToList();
        }

        public List<Player> SelectPlayerInOtherSide(Board board)
        {
            if (SelectPlayerInTokyo(board).Contains(ParentCard.Owner))
            {
                return SelectPlayerOutside(board);
            }
            else
            {
                return SelectPlayerInTokyo (board);
            }
        }

        public List<Player> SelectLeftAndRightPlayers(Board board)
        {
            List<Player> allPlayers = SelectAllPlayers(board);
            int pos = allPlayers.IndexOf(ParentCard.Owner);
            int posRight;
            int posLeft;

            if(pos == 0)
            {
                posLeft = allPlayers.Count()-1;
            } else
            {
                posLeft = pos - 1;
            }

            if(pos == allPlayers.Count())
            {
                posRight = 0;
            } else
            {
                posRight = pos + 1;
            }

            if (posLeft == posRight)
            {
                return new List<Player> { allPlayers[posLeft] };
            }
            return new List<Player> { allPlayers[posLeft], allPlayers[posRight] };
        }

        public List<Player> SelectAllPlayersExceptMe(Board board)
        {
            List<Player> allPlayers = SelectAllPlayers(board);
            allPlayers.Remove(ParentCard.Owner);
            return allPlayers;
        }

        public List<Player> SelectAllPlayers(Board board)
        {
            return board.PlayersAlives();
        }

    }
}
