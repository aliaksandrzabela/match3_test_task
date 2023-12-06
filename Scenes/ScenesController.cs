namespace monogame_match3.Scenes
{
    public partial class ScenesController
    {
        private readonly ScenesModel scenesModel;

        public ScenesController(ScenesModel scenesModel)
        {
            this.scenesModel = scenesModel;
        }

        public void SwitchScene(SceneType sceneType)
        {
            scenesModel.SwitchScene(sceneType);
        }
    }
}
