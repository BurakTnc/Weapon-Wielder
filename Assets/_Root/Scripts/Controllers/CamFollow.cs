using System;
using _Root.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class CamFollow : MonoBehaviour
    {
        private Transform _target;
        public Vector3 offset;
       [SerializeField] private float speed = 0.5f;
       [SerializeField] private Vector3 onMergeRotation, onFightRotation;
       [SerializeField] private GameObject confetti;

       private bool _isNeutral;


       private void OnEnable()
       {
           Subscribe();
       }

       private void OnDisable()
       {
           UnSubscribe();
       }

       private void Subscribe()
       {
           CameraSignals.Instance.OnMergeLook += GoToMergePosition;
           CameraSignals.Instance.OnFightLook += GoToFightPosition;
           CoreGameSignals.Instance.OnLevelComplete += ExplodeConfetti;
       }

       private void UnSubscribe()
       {
           CameraSignals.Instance.OnMergeLook -= GoToMergePosition;
           CameraSignals.Instance.OnFightLook -= GoToFightPosition;
           CoreGameSignals.Instance.OnLevelComplete -= ExplodeConfetti;
       }
       private void Start()
        {
            _target = GameObject.Find("Player").transform;
        }

       private void ExplodeConfetti()
       {
           confetti.SetActive(true);
       }
       private void LateUpdate()
        {
            if(_isNeutral)
                return;
            
            var targetPos = new Vector3(transform.position.x, _target.position.y + offset.y,
                _target.transform.position.z + offset.z);

            transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);

        }

       private void GoToMergePosition(Vector3 desiredPosition)
       {
           desiredPosition.x = 0;
           _isNeutral = true;
           transform.DOMove(desiredPosition, 2).SetEase(Ease.InSine);
           transform.DORotate(onMergeRotation, 2).SetEase(Ease.InSine);
       }

       private void GoToFightPosition(Vector3 desiredPosition)
       {
           _isNeutral = true;
           desiredPosition.x = 0;
           transform.DOMove(desiredPosition, 1).SetEase(Ease.InSine);
           transform.DORotate(onFightRotation, 1).SetEase(Ease.InSine);
       }
    }
}
