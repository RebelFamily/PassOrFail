using UnityEngine;

namespace PassOrFail.MiniGames
{
    [System.Serializable]
    public class LabData
    {
        public GameObject landModel;
        public GameObject landDisablePart;
        public Transform landModelFinalPosition;
        public Transform landModelParent;
        public Transform initialLiquidPosition;
        public Transform liquidParent;
        public GameObject leftFlask,rightFlask;
        public Sprite targetToAchieve;
    }

    [System.Serializable]
    public class AcidData
    {
        public BoxCollider rangeCollider;
        public Variables.ColorsName colorName;
    }
    [RequireComponent(typeof(Student))]
    public class StudentLabData : MonoBehaviour
    {
        public LabData labData;
        private Student _student;
        private static readonly int LeftPour = Animator.StringToHash("LeftPour");
        private static readonly int RightPour = Animator.StringToHash("RightPour");
        private static readonly int Stop = Animator.StringToHash("Stop");

        private void Awake()
        {
            _student = GetComponent<Student>();
        }

        public void PlaceFlaskInHands()
        {
            labData.leftFlask.SetActive(true);
            labData.rightFlask.SetActive(true);
        }

        public void HideFlaskInHand()
        {
            labData.leftFlask.SetActive(false);
            labData.rightFlask.SetActive(false);
        }

        public void PourLiquid(Variables.ColorsName colorsName)
        {
            _student.GetAnimator().ResetTrigger(Stop);
            Debug.Log("Pouring Liquid on : "+gameObject.name);
            _student.GetAnimator().SetLayerWeight(1, 0f);
            switch (colorsName)
            {
                case Variables.ColorsName.Cyan:
                    _student.GetAnimator().SetTrigger(RightPour);
                    break;
                case Variables.ColorsName.Magenta:
                    _student.GetAnimator().SetTrigger(LeftPour);
                    break;
            }
        }

        public void StopPouring()
        {
            Debug.Log("buttonclick StopPouring");
            _student.GetAnimator().SetTrigger(Stop);
            _student.GetAnimator().SetLayerWeight(1, 1f);
        }

    }
    
    
}

