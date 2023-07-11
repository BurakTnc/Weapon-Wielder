using System;
using System.Collections;
using _Root.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class DragNDropController : MonoBehaviour
    {
        private Camera _camera;
        private Vector3 _offset;
        private Vector3 _previousPos;
        private bool _isMoving;
        private ShooterController _shooterController;
        private GangController _gangController;
        private int _gridIndex;
        private void Awake()
        {
            _camera = Camera.main;
            _shooterController = GetComponent<ShooterController>();
        }

        private void OnMouseDown()
        {
            transform.DOKill();
            _offset = Input.mousePosition - _camera.WorldToScreenPoint(transform.position);
            if (_isMoving)
                return;
            _previousPos = transform.position;
        }

        private void OnMouseUp()
        {
            _shooterController.canMerge = true;
            StartCoroutine(MergeAbilityLength());
            transform.DOMove(_previousPos, .5f).SetEase(Ease.OutBack).OnComplete(() => _isMoving = false);
        }

        private void OnMouseDrag()
        {
            
            var calculatedPos = _camera.ScreenToWorldPoint(Input.mousePosition - _offset);
            var desiredPos = new Vector3(calculatedPos.x, -.5f, calculatedPos.z);
            transform.position = desiredPos;
        }

        private IEnumerator MergeAbilityLength()
        {
            yield return new WaitForSeconds(.02f);
            _shooterController.canMerge = false;
        }

        public void LeaveGridByMerge()
        {
            LevelSignals.Instance.OnGridLeave?.Invoke(_gridIndex);
        }

        public void InitFirstGrid(int gridIndex)
        {
            _gridIndex = gridIndex;
        }

        public void PlaceSoldier(Vector3 placedPosition, Transform grid)
        {
            var index = grid.GetSiblingIndex();

            grid.GetComponent<BoxCollider>().enabled = false;
            
            LevelSignals.Instance.OnNewGrid?.Invoke(_gridIndex,index);
            _gridIndex = index;
            _previousPos = new Vector3(placedPosition.x, -.5f, placedPosition.z);
            transform.DOMove(_previousPos, .5f).SetEase(Ease.OutBack).OnComplete(() => _isMoving = false);
        
        }
    }
}