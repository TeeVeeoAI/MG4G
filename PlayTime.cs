using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MG4G
{
    public class PlayTime
    {
        private float quarterTimeM, fullTimeM, currTimeM, timeTillNQM;
        private float quarterTimeS, fullTimeS,currTimeS, timeTillNQS, halfTimeS;
        private int currQuar;
        private bool halfTime, hasHT, gameOver;
        private Vector2 v2HT;

        public bool HalfTime{ get => halfTime; }
        public bool GameOver{ get => gameOver; }
        public Vector2 V2HT{ set => v2HT = value; }

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
            v2HT = new Vector2(1920, 1080);
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
            spriteBatch.DrawString(font, ((halfTimeS+15) - gameTime.TotalGameTime.Seconds).ToString(), v2HT, Color.Black);
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