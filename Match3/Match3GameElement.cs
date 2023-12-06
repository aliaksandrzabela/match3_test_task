using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monogame_match3.Match3
{
    public class Match3GameElement : MouseClickComponent
    {
        public Match3GameElementType ElementType { get; private set; }
        public (int col, int row) FieldPosition { get; private set; }

        private float moveTime = 0f;
        private bool isMoving = false;
        private Vector2 moveFrom;
        private Vector2 moveTo;
        private bool isWrongSwap = false;
        private bool isFirsMove = false;

        public Match3GameElement(Texture2D texture, Match3GameElementType elementType) : base(texture)
        {
            ElementType = elementType;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rectangle, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (isMoving)
            {
                moveTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                Position = Lerp(moveFrom, moveTo, moveTime / Match3GameField.ELEMENT_MOVE_TIME);
                if (moveTime >= Match3GameField.ELEMENT_MOVE_TIME)
                {
                    isMoving = false;
                    Position = moveTo;
                }
            }

            if (isWrongSwap)
            {
                moveTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                if(isFirsMove)
                {
                    Position = Lerp(moveFrom, moveTo, moveTime / (Match3GameField.ELEMENT_MOVE_TIME / 2));
                }
                else
                {
                    Position = Lerp(moveTo, moveFrom, moveTime / (Match3GameField.ELEMENT_MOVE_TIME / 2));
                }

                if (moveTime > Match3GameField.ELEMENT_MOVE_TIME / 2 && isFirsMove)
                {
                    moveTime = 0;
                    isFirsMove = false;                    
                }

                if (moveTime >= Match3GameField.ELEMENT_MOVE_TIME / 2 && !isFirsMove)
                {
                    isWrongSwap = false;
                    Position = moveFrom;
                }
            }
        }

        public void SetPostion(int x, int y)
        {
            FieldPosition = (x, y);
            Position = new Vector2(x * Match3GameField.ELEMENTSIZE, y * Match3GameField.ELEMENTSIZE);
        }

        public void Move((int col, int row) position)
        {
            isMoving = true;
            moveTime = 0f;
            moveFrom = Position;
            moveTo = new Vector2(position.col * Match3GameField.ELEMENTSIZE, position.row * Match3GameField.ELEMENTSIZE);
            FieldPosition = position;
        }

        public void ShowWrongSwap((int col, int row) position)
        {
            isWrongSwap = true;
            isFirsMove = true;
            moveTime = 0f;
            moveFrom = Position;
            moveTo = new Vector2(position.col * Match3GameField.ELEMENTSIZE, position.row * Match3GameField.ELEMENTSIZE);
        }

        private Vector2 Lerp(Vector2 firstVector, Vector2 secondVector, float by)
        {
            float retX = Lerp(firstVector.X, secondVector.X, by);
            float retY = Lerp(firstVector.Y, secondVector.Y, by);
            return new Vector2(retX, retY);
        }

        private float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }
    }
}
