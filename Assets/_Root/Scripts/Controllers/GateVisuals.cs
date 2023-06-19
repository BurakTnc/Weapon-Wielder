using DG.Tweening;
using UnityEngine;

namespace _Root.Scripts.Controllers
{
    public class GateVisuals : MonoBehaviour
    {
        [SerializeField] private Sprite redSprite, greenSprite;
        [SerializeField] private Material greenMaterial, redMaterial;
        [SerializeField] private MeshRenderer[] cylinders;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void SetGateColor(bool isGreen)
        {
            if (isGreen)
            {
                foreach (var obj in cylinders)
                {
                    obj.material.DOColor(greenMaterial.color, .3f).SetEase(Ease.InSine);
                }

                spriteRenderer.sprite = greenSprite;
            }
            else
            {
                foreach (var obj in cylinders)
                {
                    obj.material.DOColor(redMaterial.color, .3f).SetEase(Ease.InSine);
                }

                spriteRenderer.sprite = redSprite;
            }
        }
    }
}