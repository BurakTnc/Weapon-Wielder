using System;
using System.Collections.Generic;
using _Root.Scripts.Enums;
using UnityEngine;

namespace _Root.Scripts.Managers
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance;
        
        [SerializeField] private Pool[] _pool;

        [Serializable]
        private struct Pool
        {
            public Queue<GameObject> ObjectPool;
            public GameObject pooledObject;
            public int poolSize;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            {
                Destroy(this);
                return;
            }

            Instance = this;
            
            SpawnPooledObjects();
        }

        private void SpawnPooledObjects()
        {
            for (var i = 0; i < _pool.Length; i++)
            {
                _pool[i].ObjectPool = new Queue<GameObject>();

                for (var j = 0; j < _pool[i].poolSize; j++)
                {
                    var obj = Instantiate(_pool[i].pooledObject, transform);

                    obj.SetActive(false);
                    _pool[i].ObjectPool.Enqueue(obj);
                }
            }
        }

        public GameObject GetPooledObject(PooledObjectType objectType)
        {
            var poolIndex = (int)objectType;
            
            if (poolIndex >= _pool.Length)
            {
                return null;
            }
            
            var obj = _pool[poolIndex].ObjectPool.Dequeue();

            obj.SetActive(true);
            _pool[(int)objectType].ObjectPool.Enqueue(obj);

            return obj;
        }
    }
}