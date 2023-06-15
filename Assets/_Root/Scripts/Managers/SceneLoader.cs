using System;
using _Root.Scripts.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Root.Scripts.Managers
{
    public class SceneLoader : MonoBehaviour
    {
        private void OnEnable()
        {
            CoreGameSignals.Instance.OnLevelLoad += LoadScene;
        }

        private void OnDisable()
        {
            CoreGameSignals.Instance.OnLevelLoad -= LoadScene;
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(0);
        }
    }
}
