using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using monogame_match3.Match3;
using Microsoft.Xna.Framework.Content;

namespace monogame_match3.Scenes
{
    public abstract class SceneBase
    {
        protected readonly ScenesController scenesController;
        protected readonly ContentManager content;
        private readonly List<IComponent> components;
        public SceneBase(ScenesModel scenesModel)
        {
            components = new();
            scenesController = new ScenesController(scenesModel);
            content = scenesModel.Content;
        }

        public void AddComponent(IComponent component)
        {
            components.Add(component);
        }
        public void RemoveComponent(IComponent component)
        {
            components.Remove(component);
        }

        public abstract void OnEnter();
        public abstract void OnExit();

        public virtual void OnUpdate(GameTime gameTime)
        {
            foreach(IComponent component in components)
            {
                component.Update(gameTime);
            }
            OnAferUpdate();
        }

        public virtual void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (IComponent component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
        }

        protected virtual void OnAferUpdate()
        {

        }

    }
}
