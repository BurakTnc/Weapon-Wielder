using System;
using _Root.Scripts.Signals;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class CrossHairController : MonoBehaviour
    {
        [SerializeField] private float speedSideways, speedForward;
        [SerializeField] private float xPosClamp, minZPosClamp, maxZPosClamp;
        
        private bool _onFight;
        private bool _holding;
        private Vector3 _pos1, _pos2;

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void Update()
        {
            Aim();
        }

        private void Subscribe()
        {
            LevelSignals.Instance.OnFight += StartFight;
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnFight -= StartFight;
        }
        private void StartFight()
        {
            _onFight = true;
        }

        private void Aim()
        {
            if (!_onFight) 
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                _pos1 = GetMousePosition();
                _holding = true;
            }

            if (Input.GetMouseButton(0) && _holding)
            {
                _pos2 = GetMousePosition();
                var delta = _pos1 - _pos2;
                _pos1 = _pos2;
                transform.Translate(new Vector3(-delta.x * speedSideways * Time.deltaTime, 0,
                    -delta.y *speedForward * Time.deltaTime));
            }
            var position = transform.position;

            position = new Vector3(Mathf.Clamp(position.x, -xPosClamp, xPosClamp),
                position.y, Mathf.Clamp(position.z, minZPosClamp, maxZPosClamp));
            transform.position = position;
        }

        private Vector2 GetMousePosition()
        {
            var pos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);

            return pos;
        }
    }
}