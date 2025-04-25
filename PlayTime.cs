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
        private bool halfTime;
        private float halfTimeS;
        private bool hasHT;
        private bool gameOver;

        public bool HalfTime{ get => halfTime; }
        public bool GameOver{ get => gameOver; }

        public PlayTime(){
            fullTimeM = 4;
            fullTimeS = 0;
            quarterTimeM = fullTimeM/4;
            quarterTimeS = 0;
            currTimeM = 0;
            currTimeS = 0;
            currQuar = 1;
            timeTillNQM = quarterTimeM;
            timeTillNQS = quarterTimeS;
            halfTime = false;
            hasHT = false;
            gameOver = false;
        }

        public void Update(GameTime gameTime){
            if (gameOver){
                return;
            }
            if (gameTime.TotalGameTime.Seconds <= halfTimeS + 15f && halfTime){
                return;
            } else if(gameTime.TotalGameTime.Seconds >= halfTimeS + 15f && halfTime){
                halfTime = false;
                hasHT = true;
            }
            currTimeM = gameTime.TotalGameTime.Minutes;
            currTimeS = gameTime.TotalGameTime.Seconds;
            timeTillNQM = quarterTimeM - currTimeM;
            timeTillNQS = quarterTimeS - currTimeS + (hasHT ? 15 : 0);
            if (timeTillNQS < 0){
                timeTillNQM--;
                timeTillNQS += 60;
            }
            if (timeTillNQM < 0){
                NextQ(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font){
            spriteBatch.DrawString(font, "Q" + currQuar.ToString(), new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(font, timeTillNQM.ToString() + ":" + timeTillNQS.ToString(), new Vector2(0, 50), Color.Black);
        }

        public void NextQ(GameTime gameTime){
            quarterTimeM += fullTimeM/4;
            quarterTimeS = 0;
            timeTillNQM = quarterTimeM;
            timeTillNQS = quarterTimeS;
            currQuar++;
            if (currQuar == 3){
                halfTime = true;
                halfTimeS = gameTime.TotalGameTime.Seconds;
                quarterTimeM += (hasHT ? 15/60 : 0);
                quarterTimeS += (hasHT ? 15 : 0);
            }
            if (currQuar > 4){
                gameOver = true;
            }
        }
    }
}