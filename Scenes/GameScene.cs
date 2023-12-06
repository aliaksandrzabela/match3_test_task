using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame_match3.Controls;
using monogame_match3.Match3;
using System;

namespace monogame_match3.Scenes
{
    public partial class GameScene : SceneBase
    {
        private Match3GameField gameField;
        private bool roundEnd = false;
        public GameScene(ScenesModel scenesModel) : base(scenesModel)
        {

        }

        public override void OnEnter()
        {
            roundEnd = false;
            gameField = new Match3GameField(content);
            gameField.OnRoundEnd += OnRoundEnd;
            AddComponent(gameField);
        }

        public override void OnExit()
        {
        }

        private void OnRoundEnd()
        {
            scenesController.SwitchScene(SceneType.GameOverScene);
            roundEnd = true;
        }

        protected override void OnAferUpdate()
        {
            if(roundEnd)
            {
                RemoveComponent(gameField);
                gameField.OnRoundEnd -= OnRoundEnd;
                gameField = null;
            }
        }
    }
}
