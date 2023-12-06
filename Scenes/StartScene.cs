using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame_match3.Controls;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace monogame_match3.Scenes
{
    public partial class StartScene : SceneBase
    {
        private readonly TextComponent text;
        private readonly Button playButton;
        public StartScene(ScenesModel scenesModel) : base(scenesModel)
        {
            SpriteFont font = scenesModel.Content.Load<SpriteFont>("UI/Fonts/Font");

            text = new TextComponent(font);
            text.Text = "Match 3";
            text.Position = new Vector2(320, 100);
            text.PenColor = Color.Black;

            playButton = new Button(
                scenesModel.Content.Load<Texture2D>("UI/Buttons/Button"),
                font
            );
            playButton.Position = new Vector2(260, 300);
            playButton.Text = "Start";
            playButton.PenColor = Color.Black;
            playButton.Click += OnPlayButtonClick;

            AddComponent(playButton);
            AddComponent(text);
        }

        private void OnPlayButtonClick(object sender, EventArgs eventArgs)
        {
            scenesController.SwitchScene(SceneType.GameScene);
        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

    }
}
