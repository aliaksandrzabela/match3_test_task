using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame_match3.Match3;

namespace monogame_match3.Controls
{
    public class TextComponent : IComponent
    {        
        public string Text { get; set; }
        public Color PenColor { get; set; }
        public Vector2 Position { get; set; }

        public TextComponent(SpriteFont font)
        {            
            this.font = font;
        }

        private SpriteFont font;        

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, Text, Position, PenColor);
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
