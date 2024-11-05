using UnityEngine;
using Zain_Meta.Meta_Scripts.AI;
using Zain_Meta.Meta_Scripts.Helpers;
using Zain_Meta.Meta_Scripts.Managers;
using Zain_Meta.Meta_Scripts.Triggers;

namespace Zain_Meta.Meta_Scripts.Components
{
    [SelectionBase]
    public class ClassroomProfile : MonoBehaviour
    {
        [SerializeField] private ClassroomType classSubject;
        [SerializeField] private SeatProfile[] classroomSeats;
        [SerializeField] private TeachingArea teachingTriggerArea;
        [SerializeField] private TextAppear boardWork;
        public Transform podiumTransforms, teacherChair;

       [SerializeField] private bool _isOpen, _isFull, _canBeTaught;

        private ClassroomProfilesManager _classroomProfilesManager;

        private void Start()
        {
            teachingTriggerArea.HideTeachingArea();
        }

        private void OnEnable()
        {
            EventsManager.OnStudentSatInClass += CheckForClassStrength;
            EventsManager.OnTeacherStartTeaching += TeachAllStudentsOfThisClass;
            EventsManager.OnStudentLeftTheClassroom += ResetTheClass;
            EventsManager.OnShowBoardText += ShowTextOnBoard;
        }

        private void OnDisable()
        {
            EventsManager.OnStudentSatInClass -= CheckForClassStrength;
            EventsManager.OnTeacherStartTeaching -= TeachAllStudentsOfThisClass;
            EventsManager.OnStudentLeftTheClassroom -= ResetTheClass;
            EventsManager.OnShowBoardText -= ShowTextOnBoard;
        }

        private void ShowTextOnBoard(bool toTeach, ClassroomProfile classroomProfile)
        {
            if (classroomProfile != this) return;

            if (toTeach)
                boardWork.ShowMesh();
        }


        private void ResetTheClass(StudentStateManager student)
        {
            boardWork.HideMesh();
            _canBeTaught = false;
           // _areaShown = false;
            _isFull = false;
            /*if(!_isOpen) return;
            _isFull = IsClassFull();
            if (_isFull)
                teachingTriggerArea.ShowTeachingArea();
            else
                teachingTriggerArea.HideTeachingArea();*/
        }

        private void TeachAllStudentsOfThisClass(ClassroomProfile classroom)
        {
            if (classroom != this) return;

            for (var i = 0; i < classroomSeats.Length; i++)
            {
                classroomSeats[i].GiveHomeworkToThisKid();
            }
        }

        private void CheckForClassStrength(ClassroomProfile classroomProfile)
        {
            if(classroomProfile!=this) return;
            
            if (!_isOpen) return;
            
            _isFull = IsClassFull();
            _canBeTaught = _isFull;
            if (_isFull)
            {
                teachingTriggerArea.ShowTeachingArea();
            }
            else
            {
                teachingTriggerArea.HideTeachingArea();
            }
        }

        public void MakeSureToTeachMe()
        {
            _isFull = IsClassFull();
            _canBeTaught = _isFull;
        }

        public Transform GetAvailableSeat(StudentStateManager student)
        {
            if (!_isOpen) return null;

            if (_isFull) return null;
            for (var i = 0; i < classroomSeats.Length; i++)
            {
                if (!classroomSeats[i].IsThisChairMarked())
                {
                    classroomSeats[i].MarkForSitting(student);
                    student.GetRequirements().mySeat = classroomSeats[i];
                    return classroomSeats[i].GetSeatingPoint();
                }
            }

            return null;
        }

        public bool AnySeatsAvailable()
        {
            if (!_isOpen) return false;

            for (var i = 0; i < classroomSeats.Length; i++)
            {
                if (!classroomSeats[i].IsThisChairMarked())
                {
                    return true;
                }
            }

            return false;
        }

        private bool AnySeatsAvailableTest()
        {
            if (!_isOpen) return false;

            for (var i = 0; i < classroomSeats.Length; i++)
            {
                if (!classroomSeats[i].IsSeatOccupied())
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsClassFull()
        {
            return !AnySeatsAvailableTest();
        }

        public bool ClassCanBeTaught()
        {
            return _canBeTaught && !teachingTriggerArea.isPlayerTriggering;
        }

        public void OpenTheClass()
        {
            _isOpen = true;
            _classroomProfilesManager = ClassroomProfilesManager.Instance;
            _classroomProfilesManager.AddClasses(this);
        }

        public ClassroomType GetClassroomType() => classSubject;
        public TeachingArea GetTeachingArea() => teachingTriggerArea;
    }
}