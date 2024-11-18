using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Components
{
    public class Receptionist : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [SerializeField] private GameObject sleepyVfx;
        private static readonly int Admission = Animator.StringToHash("admission");

        public void StartWorking()
        {
            anim.SetBool(Admission, true);
        }

        public void BackToIdle()
        {
            anim.SetBool(Admission, false);
        }

        public void ShowSleepyVfx()
        {
            sleepyVfx.SetActive(true);
        }

        public void HideSleepyVfx()
        {
            sleepyVfx.SetActive(false);
        }
    }
}