using System;
using DG.Tweening;
using UnityEngine;

namespace _Root.Scripts.Objects
{
    public class Band : MonoBehaviour
    {
        [SerializeField] private Material material;
        [SerializeField] private float bandSpeed;

        private float _offset;
        private void Awake()
        {
          
        }

        private void Update()
        {
            _offset += Time.deltaTime * bandSpeed;
            material.SetTextureOffset("_MainTex",new Vector2(0,_offset));
        }
    }
}
