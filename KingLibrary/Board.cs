﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingLibrary
{
    public class Board
    {
        public int NbRound { get; set; }
        public List<Player> Players { get; set; }
        public Player CurrentPlayer { get; set; }
        public Dictionary<EventEnum, List<Card>> Observers { get; set; }

        public Board()
        {
            NbRound = 0;
            Players = new List<Player>();
            Observers = new Dictionary<EventEnum, List<Card>>();
        }

        /// <summary>
        /// Ajoute un joueur.
        /// </summary>
        /// <param name="name">Le nom du joueur.</param>
        //public void AddPlayer(string name) {
        //    Players.Add(new Player(name),);
        //}

        /// <summary>
        /// Supprime un joueur.
        /// </summary>
        /// <param name="player">Le joueur.</param>
        //public void RemovePlayer(Player player)
        //{
        //    Players.Remove(player);
        //}

        public void SubscribeEvent(EventEnum eventParam, Card card)
        {
            if (!Observers.ContainsKey(eventParam))
            {
                Observers[eventParam] = new List<Card>();
            }
            Observers[eventParam].Add(card);
        }

        public void UnsubscribeEvent(Card card)
        {
            foreach (KeyValuePair<EventEnum, List<Card>> keyValuePair in Observers)
            {
                keyValuePair.Value.Remove(card);
            }
        }

        public void NextPlayer()
        {
           for(int i = 0; i < Players.Count; i++)
            {
                if(Players[i].Pseudo == CurrentPlayer.Pseudo)
                {
                    CurrentPlayer.listededes = new List<Dice>();
                    CurrentPlayer = (i+1 == Players.Count ? Players[0] : Players[i + 1]);
                    CurrentPlayer.NbLancer = 3;
                    break;
                }
            }
        }
        
        public void DiceResolve()
        {
            int coeur = CurrentPlayer.selecaodedes.Count(x => x.ActiveFace == FaceEnum.LIFE);
            int griffe = CurrentPlayer.selecaodedes.Count(x => x.ActiveFace == FaceEnum.ATTACK);
            int energie = CurrentPlayer.selecaodedes.Count(x => x.ActiveFace == FaceEnum.ENERGY);
            int un = CurrentPlayer.selecaodedes.Count(x => x.ActiveFace == FaceEnum.ONE);
            int deux = CurrentPlayer.selecaodedes.Count(x => x.ActiveFace == FaceEnum.TWO);
            int trois = CurrentPlayer.selecaodedes.Count(x => x.ActiveFace == FaceEnum.THREE);

            CurrentPlayer.Energy += energie;
            if (un >= 3)
            {
                CurrentPlayer.GainVPWithDices(1, un);
            }
            if (deux >= 3)
            {
                CurrentPlayer.GainVPWithDices(2, deux);
            }
            if (trois >= 3)
            {
                CurrentPlayer.GainVPWithDices(3, trois);
            }

        }        
    }
}
