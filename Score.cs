using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MG4G
{
    public class Score
    {
        private int leftScore;
        private int rightScore;

        public int LeftScore{
            get{ return leftScore; }
        }
        public int RightScore{
            get{ return rightScore; }
        }

        //fråga tim om
        /*
            public int RightScore{ get => rightScore; set => rightScore = value; }
        */

        public Score(int leftScore, int rightScore){
            this.leftScore = leftScore;
            this.rightScore = rightScore;
        }

        public void UpdateScore(int leftScore, int rightScore, GameTime gameTime){
            this.leftScore = leftScore;
            this.rightScore = rightScore;
        }
    }
}