using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.Helpers;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.MetaRelated.Unlocker
{
    public class ClassRoomUnlocker : MonoBehaviour, IUnlocker
    {
        [SerializeField] private ClassroomType type;
        [SerializeField] private ClassroomProfile classroomProfile;
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
            CameraManager.Instance.SetCameraTarget(classroomProfile.transform,1.5f);
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
                classroomProfile.OpenTheClass();
            });
            
            if (OnBoardingManager.TutorialComplete) return;

            if (type == ClassroomType.Maths)
                OnBoardingManager.Instance.SetStateBasedOn(TutorialState.UnlockClassroom,
                    TutorialState.IdleForSometime);
            if (type == ClassroomType.Science)
                OnBoardingManager.Instance.SetStateBasedOn(TutorialState.UnlockNextClassroom,
                    TutorialState.Complete);
        }

        public void UnlockWithoutAnimation()
        {
            roofPivot.gameObject.SetActive(false);
            interiorPropsPivot.localScale = Vector3.one;
            doorObj.SetActive(false);
            DOVirtual.DelayedCall(.1f, () => { classroomProfile.OpenTheClass(); });
        }
    }
}