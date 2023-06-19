using System;
using _Root.Scripts.Signals;
using Unity.VisualScripting;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class PlayerCollisionController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out GateController gate))
            {
                gate.Selection();
            }
            if (other.gameObject.TryGetComponent(out BonusDummyController dummy))
            {
                CoreGameSignals.Instance.OnLevelComplete?.Invoke();
            }

            if (other.gameObject.CompareTag("Finish"))
            {
                CoreGameSignals.Instance.OnLevelComplete?.Invoke();
            }
        }
    }
}