using UnityEngine;

namespace GameTerms.Bootstrap
{
    public sealed class MainSceneBootstrap : MonoBehaviour
    {
        private void Awake()
        {
            if (FindFirstObjectByType<UI.AppShellController>() != null)
            {
                return;
            }

            new GameObject("AppShell").AddComponent<UI.AppShellController>();
        }
    }
}
