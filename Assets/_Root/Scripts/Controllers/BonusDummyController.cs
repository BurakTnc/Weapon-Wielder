using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class BonusDummyController : MonoBehaviour
    {
        [SerializeField] private int hitCount;
        [SerializeField] private GameObject hitParticle;
        
        private bool _hasExploded;
        private readonly List<GameObject> _throwedPieces = new List<GameObject>();

        public void GetHit(float damage,Vector3 impactPoint,float hitAnimationLength)
        {
            ThrowPieces(hitAnimationLength);

            hitCount--;
            if (hitCount <= 0)
            {
                Explode(impactPoint);
            }
            if(!hitParticle)
                return;
            var particle = Instantiate(hitParticle);
            particle.transform.position = impactPoint;
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
                _throwedPieces.Add(transform.GetChild(i).gameObject);
            }

            foreach (var selectedPart in _throwedPieces)
            {
                selectedPart.transform.SetParent(null);
                if (selectedPart.TryGetComponent(out Rigidbody rb))
                {
                    rb.constraints = RigidbodyConstraints.None;
                    rb.isKinematic = false;
                    rb.AddExplosionForce(5, impactPoint, 30, .1f,ForceMode.VelocityChange);
                }

                if (selectedPart.TryGetComponent(out BoxCollider coll))
                {
                    coll.enabled = true;

                }
                Destroy(selectedPart.gameObject, 3);
            }
            SpawnTheBonus();
            Destroy(gameObject);
        }

        private void SpawnTheBonus()
        {
            
        }

        private void ThrowPieces(float hitAnimationLength)
        {

            transform.DOComplete();
            transform.DOShakeRotation(hitAnimationLength, Vector3.back * 35, 5, 100);
            
            var partsCount = transform.childCount;

            if (partsCount <= 0 )
                return;

            var part = Random.Range(0, partsCount);
            var selectedPart = transform.GetChild(part);
                
            selectedPart.SetParent(null);
            if (selectedPart.TryGetComponent(out Rigidbody rb))
            {
                rb.constraints = RigidbodyConstraints.None;
                rb.isKinematic = false;
                rb.AddExplosionForce(5, transform.position, 30, 0f,ForceMode.VelocityChange);
            }

            if (selectedPart.TryGetComponent(out BoxCollider coll))
            {
                coll.enabled = true;

            }

            Destroy(selectedPart.gameObject, 3);
        }
    }
}