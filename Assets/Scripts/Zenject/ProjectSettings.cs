using UnityEngine;

namespace Zenject
{
    [CreateAssetMenu(fileName = "ProjectSettings", menuName = "Configs/ProjectSettings", order = 0)]
    public class ProjectSettings : ScriptableObject
    {
        [SerializeField] private int targetFPS = 60;
        [SerializeField] private bool isMultiTouch = false;

        public int TargetFPS => targetFPS;

        public bool IsMultiTouch => isMultiTouch;
    }
}