using UnityEngine;

namespace _Root.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Soldier Data", menuName = "Soldiers", order = 0)]
    public class SoldierData : ScriptableObject
    {
        public float fireRate;
        public float range;
        public float damage;
    }
}