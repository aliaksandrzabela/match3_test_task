using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using monogame_match3.Controls;
using monogame_match3.Match3.EventsData;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace monogame_match3.Match3
{
    class Match3GameField : Component
    {
        public const int ELEMENTSIZE = 31;
        public const float ELEMENT_MOVE_TIME = 1.5f;
        public const int ROUND_TIME = 60;

        public event Action OnRoundEnd;

        private int width = 8;
        private int height = 8;        

        private Match3GameElement[,] field;
        private Dictionary<Match3GameElementType, string> gameElementTypeToTextureName;
        private ContentManager content;
        private List<Component> gameElements;
        private Match3GameFieldModel fieldModel;
        private bool isWait = false;

        private int score = 0;

        private Match3GameElementSelection selection;
        private Match3GameElement firstSelected;
        private Match3GameElement secondSelected;

        private readonly TextComponent scoreText;
        private readonly TextComponent timeText;

        private float roudnTime = 0f;

        public Match3GameField(ContentManager content) : base(null)
        {
            this.content = content;

            field = new Match3GameElement[width, height];

            gameElementTypeToTextureName = new Dictionary<Match3GameElementType, string>() {
                { Match3GameElementType.Red, "GameElement/block_fire_jelly" },
                { Match3GameElementType.Green, "GameElement/block_forest_jelly" },
                { Match3GameElementType.Blue, "GameElement/block_water_jelly" },
                { Match3GameElementType.Gray, "GameElement/block_earth_jelly" },
                { Match3GameElementType.White, "GameElement/block_air_jelly" },
                { Match3GameElementType.Super, "GameElement/block_flash_jelly" }
            };

            gameElements = new List<Component>();

            fieldModel = new Match3GameFieldModel(width, height);
            fieldModel.OnGamefieldUpdated += OnGamefieldUpdated;
            fieldModel.Start();
            score = 0;

            selection = new Match3GameElementSelection(content.Load<Texture2D>("GameElement/selected"));

            scoreText = new TextComponent(content.Load<SpriteFont>("UI/Fonts/Font"));
            scoreText.Text = score.ToString();
            scoreText.PenColor = Color.Black;
            scoreText.Position = new Vector2(width * ELEMENTSIZE + 30, 10);

            roudnTime = 0f;

            timeText = new TextComponent(content.Load<SpriteFont>("UI/Fonts/Font"));
            timeText.PenColor = Color.Black;
            timeText.Position = new Vector2(width * ELEMENTSIZE + 30, 50);
            timeText.Text = ROUND_TIME.ToString();
        }

        private Match3GameElement CreateElement((int x, int y) position, Match3GameElementType type)
        {
            Match3GameElement element = new Match3GameElement(content.Load<Texture2D>(gameElementTypeToTextureName[type]), type);
            element.SetPostion(position.x, position.y);
            gameElements.Add(element);
            field[position.x, position.y] = element;
            element.Click += OnElementClick;
            return element;
        }

        private void DestroyElement(Match3GameElement element)
        {
            gameElements.Remove(element);
            element.Click -= OnElementClick;            
            score++;
            scoreText.Text = score.ToString();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < gameElements.Count; i++)
            {
                gameElements[i].Draw(gameTime, spriteBatch);
            }
            selection.Draw(gameTime, spriteBatch);
            scoreText.Draw(gameTime, spriteBatch);
            timeText.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            roudnTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(roudnTime >= ROUND_TIME)
            {
                OnRoundEnd?.Invoke();
                return;
            }

            timeText.Text = (ROUND_TIME - roudnTime).ToString();

            for (int i = 0; i < gameElements.Count; i++)
            {
                gameElements[i].Update(gameTime);
            }
            if (!isWait)
            {
                fieldModel.Run();
            }
        }

        private void OnGamefieldUpdated(Match3GameFieldEvent gameFieldEvent)
        {
            switch (gameFieldEvent.eventType)
            {
                case Match3GameFieldEventType.ElementCreated:
                    Match3EventDataElementCreated elementCreatedData = gameFieldEvent.eventData as Match3EventDataElementCreated;
                    CreateElement(elementCreatedData.position, elementCreatedData.type);
                    break;
                case Match3GameFieldEventType.ElementMoved:
                    Match3EventDataElementMoved elementMovedData = gameFieldEvent.eventData as Match3EventDataElementMoved;
                    MoveElement(elementMovedData);
                    break;
                case Match3GameFieldEventType.ElementsDeleted:
                    Match3EventDataElementsDeleted elementsDeletedData = gameFieldEvent.eventData as Match3EventDataElementsDeleted;
                    DestroyElements(elementsDeletedData.positions);
                    break;
            }
        }

        private void MoveElement(Match3EventDataElementMoved elementMovedData)
        {
            Match3GameElement movedElementFrom = field[elementMovedData.from.col, elementMovedData.from.row];
            if(elementMovedData.isSwap)
            {
                Match3GameElement movedElementTo = field[elementMovedData.to.col, elementMovedData.to.row];

                field[elementMovedData.from.col, elementMovedData.from.row] = movedElementTo;
                field[elementMovedData.to.col, elementMovedData.to.row] = movedElementFrom;
                movedElementFrom.Move(elementMovedData.to);
                movedElementTo.Move(elementMovedData.from);
            }
            else
            {
                field[elementMovedData.from.col, elementMovedData.from.row] = null;
                field[elementMovedData.to.col, elementMovedData.to.row] = movedElementFrom;
                movedElementFrom.Move(elementMovedData.to);
            }            
        }

        private void DestroyElements(List<(int col, int row)> positions)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                if (field[positions[i].col, positions[i].row] != null)
                {
                    DestroyElement(field[positions[i].col, positions[i].row]);
                }
            }
        }

        private void OnElementClick(object element, EventArgs eventArgs)
        {
            Match3GameElement m3element = (Match3GameElement)element;

            if (m3element != null)
            {
                if (firstSelected == null)
                {
                    firstSelected = m3element;

                    selection.IsVisible = true;
                    selection.SetPostion(m3element.FieldPosition.col, m3element.FieldPosition.row);
                }
                else
                {
                    secondSelected = m3element;
                }

                if (firstSelected != null && secondSelected != null)
                {
                    if (CheckIsNeighbourPostions(firstSelected.FieldPosition, secondSelected.FieldPosition))
                    {
                        bool result = fieldModel.SwapElements(firstSelected.FieldPosition, secondSelected.FieldPosition);
                        if(!result)
                        {
                            firstSelected.ShowWrongSwap(secondSelected.FieldPosition);
                            secondSelected.ShowWrongSwap(firstSelected.FieldPosition);                            
                        }
                        ResetSelection();
                    }
                    else
                    {
                        ResetSelection();
                    }
                }
            }
        }

        private void ResetSelection()
        {
            firstSelected = secondSelected = null;
            selection.IsVisible = false;
        }

        private bool CheckIsNeighbourPostions((int col, int row) from, (int col, int row) to)
        {
            if (Math.Abs(from.col - to.col) > 1)
            {
                return false;
            }
            if (Math.Abs(from.row - to.row) > 1)
            {
                return false;
            }
            return true;
        }
    }
}
