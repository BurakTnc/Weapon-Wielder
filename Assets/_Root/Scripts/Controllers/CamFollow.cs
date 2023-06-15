using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class CamFollow : MonoBehaviour
    {
        private Transform _target;
        public Vector3 offset;
       [SerializeField] private float speed = 0.5f;



       private void Start()
        {
            _target = GameObject.Find("Player").transform;
        }
       private void LateUpdate()
        {
            var targetPos = new Vector3(transform.position.x, _target.position.y + offset.y,
                _target.transform.position.z + offset.z);

            transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);

        }
    }
}
