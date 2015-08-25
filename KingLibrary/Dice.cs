using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingLibrary
{
    public class Dice
    {
        public FaceEnum ActiveFace { get; set; }
        public static Random rand = new Random();
        public Dice()
        {
            ActiveFace = (FaceEnum) rand.Next(0,5);
        }
    }
}
