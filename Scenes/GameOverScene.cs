using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame_match3.Controls;
using System;

namespace monogame_match3.Scenes
{
    public partial class GameOverScene : SceneBase
    {
        private readonly TextComponent text;
        private readonly Button playButton;
        public GameOverScene(ScenesModel scenesModel) : base(scenesModel)
        {
            SpriteFont font = scenesModel.Content.Load<SpriteFont>("UI/Fonts/Font");

            text = new TextComponent(font);
            text.Text = "Game Over";
            text.Position = new Vector2(320, 100);
            text.PenColor = Color.Black;

            playButton = new Button(
                scenesModel.Content.Load<Texture2D>("UI/Buttons/Button"),
                font
            );
            playButton.Position = new Vector2(260, 300);
            playButton.Text = "OK";
            playButton.PenColor = Color.Black;
            playButton.Click += OnPlayButtonClick;

            AddComponent(playButton);
            AddComponent(text);
        }

        private void OnPlayButtonClick(object sender, EventArgs eventArgs)
        {
            scenesController.SwitchScene(SceneType.StartScene);
        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

    }
}
