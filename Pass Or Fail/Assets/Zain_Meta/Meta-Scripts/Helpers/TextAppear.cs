using DG.Tweening;
using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Helpers
{
    public class TextAppear : MonoBehaviour
    {
        [SerializeField] private MeshRenderer mesh;
        [SerializeField] private Color zeroAlpha, oneAlpha;

        private void Awake()
        {
            mesh = GetComponent<MeshRenderer>();
            HideMesh();
        }

        public void ShowMesh()
        {
            DOVirtual.DelayedCall(.5f, () =>
            {
                mesh.material.DOColor(oneAlpha, 1.25f);
            });
        }

        public void HideMesh()
        {
            mesh.material.DOColor(zeroAlpha, .75f);
        }
    }
}