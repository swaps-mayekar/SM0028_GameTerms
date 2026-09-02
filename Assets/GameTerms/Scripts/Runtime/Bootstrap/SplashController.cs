using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameTerms.Bootstrap
{
    public sealed class SplashController : MonoBehaviour
    {
        [SerializeField] private string mainSceneName = "1_MainScene";

        private void Start()
        {
            SceneManager.LoadScene(mainSceneName);
        }
    }
}
