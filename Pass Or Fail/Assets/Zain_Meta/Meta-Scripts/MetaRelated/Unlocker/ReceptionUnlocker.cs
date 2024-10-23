using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Triggers;

namespace Zain_Meta.Meta_Scripts.MetaRelated.Unlocker
{
    public class ReceptionUnlocker : MonoBehaviour, IUnlocker
    {
        [SerializeField] private Transform receptionistChairPivot;
        [SerializeField] private AdmissionReception reception;

        public void UnlockWithAnimation()
        {
            receptionistChairPivot.gameObject.SetActive(true);
            receptionistChairPivot.localScale = Vector3.one;
            var localScale = receptionistChairPivot.localScale;
            localScale.y = 0.1f;
            receptionistChairPivot.localScale = localScale;
            receptionistChairPivot.DOScaleY(1, .15f).SetEase(Ease.InBack).OnComplete(() =>
            {
                reception.SetReceptionist(true);
            });
        }

        public void UnlockWithoutAnimation()
        {
            receptionistChairPivot.gameObject.SetActive(true);
            receptionistChairPivot.localScale = Vector3.one;
            reception.SetReceptionist(true);
        }

        public void KeepItLocked()
        {
            receptionistChairPivot.gameObject.SetActive(false);
            receptionistChairPivot.localScale = Vector3.zero;
            reception.SetReceptionist(false);
        }
    }
}