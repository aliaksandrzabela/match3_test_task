using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace monogame_match3.Match3
{
    public class MouseClickComponent : Component
    {
        public event EventHandler Click;

        private MouseState mouseState;
        private MouseState previousMouseState;
        protected bool isMouseOver;

        public MouseClickComponent(Texture2D texture) : base(texture)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {            
        }

        public override void Update(GameTime gameTime)
        {
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();

            Rectangle mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);

            isMouseOver = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                isMouseOver = true;

                if (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
