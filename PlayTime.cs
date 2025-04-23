using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MG4G
{
    public class PlayTime
    {
        private float quarterTimeM;
        private float quarterTimeS;
        private float fullTimeM;
        private float fullTimeS;
        private int currQuar;
        private float currTimeM;
        private float currTimeS;
        private float timeTillNQS;
        private float timeTillNQM;

        public PlayTime(){
            quarterTimeM = 5;
            quarterTimeS = 0;
            fullTimeM = 20;
            fullTimeS = 0;
            currTimeM = 0;
            currTimeS = 0;
            currQuar = 1;
            timeTillNQM = quarterTimeM;
            timeTillNQS = quarterTimeS;
        }

        public void Update(GameTime gameTime){
            currTimeM = gameTime.TotalGameTime.Minutes - 5*(currQuar-1);
            currTimeS = gameTime.TotalGameTime.Seconds;
            timeTillNQM = quarterTimeM - currTimeM;
            timeTillNQS = quarterTimeS - currTimeS;
            if (timeTillNQS < 0){
                timeTillNQM--;
                timeTillNQS += 60;
            }
            if (timeTillNQM < 0){
                NextQ();
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font){
            spriteBatch.DrawString(font, "Q" + currQuar.ToString(), new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(font, timeTillNQM.ToString() + ":" + timeTillNQS.ToString(), new Vector2(0, 50), Color.Black);
        }

        public void NextQ(){
            quarterTimeM = 5;
            quarterTimeS = 0;
            fullTimeM = 20;
            fullTimeS = 0;
            currTimeM = 0;
            currTimeS = 0;
            timeTillNQM = quarterTimeM;
            timeTillNQS = quarterTimeS;
            currQuar++;
        }

    }
}