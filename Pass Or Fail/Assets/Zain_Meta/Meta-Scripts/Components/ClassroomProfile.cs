using DG.Tweening;
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
        public Transform podiumTransforms,teacherChair;

        private bool _isOpen, _isFull,_canBeTaught;

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
        }

        private void OnDisable()
        {
            EventsManager.OnStudentSatInClass -= CheckForClassStrength;
            EventsManager.OnTeacherStartTeaching -= TeachAllStudentsOfThisClass;
            EventsManager.OnStudentLeftTheClassroom -= ResetTheClass;
        }

        private void ResetTheClass(StudentStateManager student)
        {
            _canBeTaught = false;
            if(!_isOpen) return;
            DOVirtual.DelayedCall(2f, () =>
            {
                _isFull = IsClassFull();
                if (_isFull)
                    teachingTriggerArea.ShowTeachingArea();
                else
                    teachingTriggerArea.HideTeachingArea();
            });
        }

        private void TeachAllStudentsOfThisClass(ClassroomProfile classroom)
        {
            if (classroom != this) return;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < classroomSeats.Length; i++)
            {
                classroomSeats[i].GiveHomeworkToThisKid();
            }
        }

        private void CheckForClassStrength()
        {
            if (!_isOpen) return;

            _isFull = IsClassFull();
            _canBeTaught = _isFull;
            if (_isFull)
                teachingTriggerArea.ShowTeachingArea();
            else
                teachingTriggerArea.HideTeachingArea();
        }


        public Transform GetAvailableSeat(StudentStateManager student)
        {
            if (!_isOpen) return null;

            if (_isFull) return null;
            for (var i = 0; i < classroomSeats.Length; i++)
            {
                if (!classroomSeats[i].IsSeatOccupied())
                {
                    classroomSeats[i].MarkForSitting(student);
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
                if (!classroomSeats[i].IsSeatOccupied())
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsClassFull()
        {
            return !AnySeatsAvailable();
        }

        public bool ClassCanBeTaught()
        {
            return _canBeTaught && !teachingTriggerArea.isPlayerTriggering ;
        }
        public void OpenTheClass()
        {
            _isOpen = true;
            _classroomProfilesManager = ClassroomProfilesManager.Instance;
            _classroomProfilesManager.AddClasses(this);
        }

        public int GetClassroomType() => (int)classSubject;
        public TeachingArea GetTeachingArea() => teachingTriggerArea;
    }
}