using UnityEngine;

namespace Zenject
{
    public class ProjectSetup : IInitializable
    {
        private readonly ProjectSettings _projectSettings;

        public ProjectSetup(ProjectSettings projectSettings)
        {
            _projectSettings = projectSettings;
        }

        public void Initialize()
        {
            Application.targetFrameRate = _projectSettings.TargetFps;
            Input.multiTouchEnabled = _projectSettings.IsMultiTouch;
        }
    }
}