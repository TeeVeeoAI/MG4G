using System.DirectoryServices.ActiveDirectory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG4G
{
    public class Score
    {
        private int leftScore;
        private int rightScore;
        private Vector2 position;
        private SpriteFont font;

        public int LeftScore{ get => leftScore; }
        public int RightScore{ get => rightScore; }

        public Score(Vector2 position, SpriteFont font){
            leftScore = 0;
            rightScore = 0;
            this.position = position;
            this.font = font;
        }

        public void UpdateScore(int leftScoreAdd, int rightScoreAdd, GameTime gameTime){
            leftScore += leftScoreAdd;
            rightScore += rightScoreAdd;
        }

        public void DrawScore(SpriteBatch spriteBatch){
            spriteBatch.DrawString(font, leftScore + " : " + rightScore, position, Color.Black);
        }
    }
}