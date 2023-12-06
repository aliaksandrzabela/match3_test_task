using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace monogame_match3.Scenes
{
    public class ScenesModel
    {
        private readonly Dictionary<SceneType, SceneBase> scenes;
        private SceneBase currentScene;
        public readonly ContentManager Content;

        public ScenesModel(ContentManager content)
        {
            Content = content;
            scenes = new()
            {
                { SceneType.StartScene, new StartScene(this) },
                { SceneType.GameScene, new GameScene(this) },
                { SceneType.GameOverScene, new GameOverScene(this) },
            };
            SwitchScene(SceneType.StartScene);
        }

        public void SwitchScene(SceneType sceneType)
        {
            if (!scenes.TryGetValue(sceneType, out SceneBase scene))
            {
                throw new Exception($"SwitchScene error. Scene with type {sceneType} not exist");
            }
            if (currentScene != null)
            {
                currentScene.OnExit();
            }
            currentScene = scene;
            currentScene.OnEnter();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (currentScene != null)
            {
                currentScene.OnDraw(gameTime, spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (currentScene != null)
            {
                currentScene.OnUpdate(gameTime);
            }
        }

    }
}
