using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingLibrary
{
    public class CardFactory
    {
        public static List<Card> GenerateDeck()
        {
            List<Card> cards = new List<Card>();

            Card tramway = new Card("Tramway", 4, new List<CardAction>(), null);

            CardAction cardAction = new CardAction();
            cardAction.TypeEvent = EventEnum.CARD_BOUGHT;
            cardAction.LifeTime = LifeTimeEnum.ONE_SHOT;
            cardAction.EffectList += cardAction.ImpactVP;
            cardAction.ImpactedPlayers += cardAction.SelectCurrentPlayer;
            cardAction.Amount = 2;
            tramway.CardActions.Add(cardAction);

            cards.Add(tramway);

            Card tank = new Card("Tank", 4, new List<CardAction>(), null);

            cardAction = new CardAction();
            cardAction.TypeEvent = EventEnum.CARD_BOUGHT;
            cardAction.LifeTime = LifeTimeEnum.ONE_SHOT;
            cardAction.EffectList += cardAction.ImpactVP;
            cardAction.Amount = 4;
            cardAction.ImpactedPlayers += cardAction.SelectCurrentPlayer;
            tank.CardActions.Add(cardAction);
            cardAction = new CardAction();
            cardAction.TypeEvent = EventEnum.CARD_BOUGHT;
            cardAction.LifeTime = LifeTimeEnum.ONE_SHOT;
            cardAction.EffectList += cardAction.ImpactHp;
            cardAction.Amount = -3;
            cardAction.ImpactedPlayers += cardAction.SelectCurrentPlayer;
            tank.CardActions.Add(cardAction);

            cards.Add(tank);

            Card soin = new Card("Soin", 3, new List<CardAction>(), null);

            cardAction = new CardAction();
            cardAction.TypeEvent = EventEnum.CARD_BOUGHT;
            cardAction.LifeTime = LifeTimeEnum.ONE_SHOT;
            cardAction.EffectList += cardAction.ImpactHp;
            cardAction.Amount = 2;
            cardAction.ImpactedPlayers = cardAction.SelectCurrentPlayer;
            soin.CardActions.Add(cardAction);

            cards.Add(soin);

            Card cafeDuCoin = new Card("Cafe Du Coin", 3, new List<CardAction>(), null);

            cardAction = new CardAction();
            cardAction.TypeEvent = EventEnum.CARD_BOUGHT;
            cardAction.LifeTime = LifeTimeEnum.ONE_SHOT;
            cardAction.EffectList += cardAction.ImpactVP;
            cardAction.Amount = 1;
            cardAction.ImpactedPlayers += cardAction.SelectCurrentPlayer;
            cafeDuCoin.CardActions.Add(cardAction);

            cards.Add(cafeDuCoin);

            Card raffinerieDeGaz = new Card("Raffinerie de gaz", 6, new List<CardAction>(), null);

            cardAction = new CardAction();
            cardAction.TypeEvent = EventEnum.CARD_BOUGHT;
            cardAction.LifeTime = LifeTimeEnum.ONE_SHOT;
            cardAction.EffectList += cardAction.ImpactHp;
            cardAction.Amount = -3;
            raffinerieDeGaz.CardActions.Add(cardAction);
            cardAction = new CardAction();
            cardAction.TypeEvent = EventEnum.CARD_BOUGHT;
            cardAction.LifeTime = LifeTimeEnum.ONE_SHOT;
            cardAction.EffectList += cardAction.ImpactVP;
            cardAction.Amount = 2;
            raffinerieDeGaz.CardActions.Add(cardAction);

            cards.Add(raffinerieDeGaz);

             return CardShuffle(cards);
        }

        public static List<Card> CardShuffle(List<Card> Cards)
        {
            List<Card> Deck = new List<Card>();
            for(int i = 0; i< Cards.Count; i++)
            {
                Deck.Add(null);
            }
            Random random = new Random();
            foreach(Card c in Cards)
            {
                int randomValue = random.Next(0, Cards.Count);
                while(Deck.ElementAtOrDefault(randomValue) != null) {
                    randomValue = random.Next(0, Cards.Count);
                }
                Deck[randomValue] = c;
            }
            return Deck;
        }
    }
}
