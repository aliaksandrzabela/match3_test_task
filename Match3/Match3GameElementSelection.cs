using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monogame_match3.Match3
{
    public class Match3GameElementSelection : Component
    {
        public bool IsVisible = false;

        public Match3GameElementSelection(Texture2D texture) : base(texture)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.Draw(texture, Rectangle, Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {
        }

        public void SetPostion(int x, int y)
        {
            Position = new Vector2(x * Match3GameField.ELEMENTSIZE, y * Match3GameField.ELEMENTSIZE);
        }
    }
}
