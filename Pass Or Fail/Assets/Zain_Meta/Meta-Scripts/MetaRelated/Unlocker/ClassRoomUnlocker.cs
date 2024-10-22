using DG.Tweening;
using UnityEngine;

namespace Zain_Meta.Meta_Scripts.MetaRelated.Unlocker
{
    public class ClassRoomUnlocker : MonoBehaviour,IUnlocker
    {
        [SerializeField] private Transform roofPivot;
        [SerializeField] private Transform interiorPropsPivot;
        [SerializeField] private GameObject doorObj;
        
        
        public void KeepItLocked()
        {
            roofPivot.gameObject.SetActive(true);
            interiorPropsPivot.localScale = Vector3.zero;
            roofPivot.localScale = Vector3.one;
            doorObj.SetActive(true);
        }
        public void UnlockWithAnimation()
        {
            roofPivot.localScale = Vector3.one;
            interiorPropsPivot.localScale = Vector3.one;
            var localScale = interiorPropsPivot.localScale;
            localScale.y = 0.1f;
            interiorPropsPivot.localScale = localScale;
            roofPivot.DOScaleX(0, .25f).SetEase(Ease.Linear).OnComplete(() =>
            {
                interiorPropsPivot.DOScaleY(1, .25f);
                doorObj.SetActive(false);
                roofPivot.gameObject.SetActive(false);
            });
        }

        public void UnlockWithoutAnimation()
        {
            roofPivot.gameObject.SetActive(false);
            interiorPropsPivot.localScale = Vector3.one;
            doorObj.SetActive(false);
        }

        
    }
}