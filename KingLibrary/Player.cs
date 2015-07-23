﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingLibrary
{
    public class Player
    {
        public string Pseudo { get; set; }
        public Monster Monster { get; set; }
        public int Energy { get; set; }
        public int VictoryPoint { get; set; }
        public int Hp { get; set; }
        public LocationEnum Location { get; set; }
        public DateTime LastResponse { get; set; }
        public string IdConnection;

        public Player(string pseudo, string idConnection)
        {
            Pseudo = pseudo;
            IdConnection = idConnection;
            Energy = 0;
            VictoryPoint = 0;
            Hp = 10;
            Location = LocationEnum.OUT_CESI;
        }
    }
}
