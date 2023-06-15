using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class TargetDummyController : MonoBehaviour
    {
        [SerializeField] private float health;

        private bool _hasExploded;
        

        public void GetHit(int damage,Vector3 impactPoint,float hitAnimationLength)
        {
            ThrowPieces(hitAnimationLength);

            health -= damage;
            if (health <= 0)
            {
                Explode(impactPoint);
            }
        }

        private void ThrowPieces(float hitAnimationLength)
        {
            transform.DOComplete();
            transform.DOShakeRotation(hitAnimationLength, Vector3.back * 35, 5, 100);
            
            var partsCount = transform.childCount;
            var r = Random.Range(0, 2);
            var canWreck = r < 1;
            if (partsCount <= 0 || !canWreck)
                return;

            var part = Random.Range(0, partsCount);
            var selectedPart = transform.GetChild(part);
                
            selectedPart.SetParent(null);
            if (selectedPart.TryGetComponent(out Rigidbody rb))
            {
                rb.constraints = RigidbodyConstraints.None;
            }

            Destroy(selectedPart.gameObject, 3);
        }

        private void Explode(Vector3 impactPoint)
        {
            if(_hasExploded)
                return;

            _hasExploded = true;
            var partsCount = transform.childCount;
            
            if(partsCount<1)
                return;
            
            for (var i = 0; i < partsCount; i++)
            {
                if (transform.childCount < 1) 
                    break;
                
                var index = i;
                if (i<1)
                {
                    index = 0;
                }
                else
                {
                    index = i-1;
                }
                var selectedPart = transform.GetChild(transform.childCount-1);
                
                selectedPart.SetParent(null);
                if (selectedPart.TryGetComponent(out Rigidbody rb))
                {
                    rb.constraints = RigidbodyConstraints.None;
                    rb.AddExplosionForce(5, impactPoint, 1, .2f, ForceMode.VelocityChange);
                }

                SpawnThePotion();
                Destroy(selectedPart.gameObject, 3);
                Destroy(gameObject);
            }
        }

        private void SpawnThePotion()
        {
            var potion = Instantiate(Resources.Load<GameObject>("Spawnables/Upgrade Potion")).transform;
            potion.position = transform.position+new Vector3(0,.4f,-1);
        }
    }
}
