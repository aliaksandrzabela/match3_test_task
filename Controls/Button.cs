using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monogame_match3.Match3;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace monogame_match3.Controls
{
    public class Button : MouseClickComponent
    {
        private readonly TextComponent text;
        private readonly SpriteFont font;
        public string Text { get; set; }
        public Color PenColor { get; set; }

        public Button(Texture2D texture, SpriteFont font) : base(texture)
        {
            this.font = font;
            text = new TextComponent(font);
        }
  

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rectangle, isMouseOver ? Color.Gray : Color.White);

            if(!string.IsNullOrEmpty(Text))
            {
                float x = (Rectangle.X + (Rectangle.Width / 2)) - (font.MeasureString(Text).X / 2);
                float y = (Rectangle.Y + (Rectangle.Height / 2)) - (font.MeasureString(Text).Y / 2);
                text.Position = new Vector2(x, y);
                text.PenColor = PenColor;
                text.Text = Text;
                text.Draw(gameTime, spriteBatch);
            }
        }
    }
}
