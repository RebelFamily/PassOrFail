using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.MetaRelated.Unlocker
{
    public class TeacherUnlocker : MonoBehaviour, IUnlocker
    {
        [SerializeField] private Transform teacherObj;

        public void KeepItLocked()
        {
            teacherObj.gameObject.SetActive(false);
        }

        public void UnlockWithAnimation()
        {
            CameraManager.Instance.SetCameraTarget(teacherObj, 1f);
            teacherObj.gameObject.SetActive(true);
            teacherObj.localScale = Vector3.one;
            var localScale = teacherObj.localScale;
            localScale.y = 0.1f;
            teacherObj.localScale = localScale;
            teacherObj.DOScaleY(1, .5f).OnComplete(() =>
            {
                ParticlesManager.Instance.PlayTeacherUnlockVfxFor(teacherObj);
            });
        }

        public void UnlockWithoutAnimation()
        {
            teacherObj.gameObject.SetActive(true);
            teacherObj.localScale = Vector3.one;
        }
    }
}