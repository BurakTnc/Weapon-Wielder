using System;
using _Root.Scripts.Signals;
using Unity.VisualScripting;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class PlayerCollisionController : MonoBehaviour
    {

        private GangController _gangController;

        private void Awake()
        {
            _gangController = GetComponent<GangController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out BonusDummyController dummy))
            {
                CoreGameSignals.Instance.OnLevelComplete?.Invoke();
            }

            if (other.gameObject.CompareTag("Finish"))
            {
                LevelSignals.Instance.OnGrid?.Invoke(_gangController.GetGangList());
                LevelSignals.Instance.OnStop?.Invoke();
                
            }
        }
    }
}