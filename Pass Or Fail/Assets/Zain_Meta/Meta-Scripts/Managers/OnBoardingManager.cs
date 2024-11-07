using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.Helpers;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class OnBoardingManager : MonoBehaviour
    {
        public static OnBoardingManager Instance;
        public static bool TutorialComplete;

        private void Awake()
        {
            Instance = this;
            backToHomeBtn.SetActive(false);
        }

        [SerializeField] private bool forceComplete;
        [SerializeField] private WaypointMarker waypointMarker;
        [SerializeField] private TutorialState[] tutorialStates;
        [SerializeField] private GameObject backToHomeBtn;
        [SerializeField] private TutorialState curTutState;
        [SerializeField] private CameraSwitcher cameraSwitcher;
        [SerializeField] private UnlockNavigationManager navigationManager;
        [SerializeField] private int curStateIndex;

        [SerializeField] private Transform
            receptionTarget,
            receptionCashTarget,
            receptionistUnlockerTarget,
            firstClassroomUnlockerTarget,
            teachingClassTrigger,
            secondClassroomUnlockerTarget;

        [SerializeField] private GameObject startingHandTut,
            receptionistUnlocker,
            classroomUnlocker,
            secondClassroomUnlocker;

        private void Start()
        {
            RecoverTutorialState();
        }

        private void RecoverTutorialState()
        {
            curStateIndex = PlayerPrefs.GetInt("CurTutIndex", 0);
            receptionistUnlocker.SetActive(false);
            classroomUnlocker.SetActive(false);
            secondClassroomUnlocker.SetActive(false);

            curTutState = tutorialStates[curStateIndex];
            for (var i = 0; i < curStateIndex; i++)
            {
                SetItemsBasedOnState(tutorialStates[i]);
            }

            if (curTutState == TutorialState.Complete)
                TutorialComplete = true;
            if (forceComplete)
            {
                TutorialComplete = forceComplete;
                receptionistUnlocker.SetActive(true);
                classroomUnlocker.SetActive(true);
                secondClassroomUnlocker.SetActive(true);
            }

            navigationManager.HideEverything();

            if (TutorialComplete)
            {
                EventsManager.TutCompleteEvent();
                //navigationManager.ReloadThePurchasesData();
                navigationManager.LookAtNextUnlock();
                startingHandTut.SetActive(false);
                waypointMarker.gameObject.SetActive(false);
                waypointMarker.arrowPivot.gameObject.SetActive(false);
                backToHomeBtn.SetActive(true);
                return;
            }

            DOVirtual.DelayedCall(1f, TakeActionOnState);
        }

        private void TakeActionOnState()
        {
            if (TutorialComplete) return;
            waypointMarker.gameObject.SetActive(true);
            waypointMarker.arrowPivot.gameObject.SetActive(true);
            switch (curTutState)
            {
                case TutorialState.IdleState:
                    waypointMarker.gameObject.SetActive(false);
                    waypointMarker.arrowPivot.gameObject.SetActive(false);
                    break;
                case TutorialState.GotoReception:
                    waypointMarker.target = receptionTarget;
                    cameraSwitcher.ZoomToTarget(receptionTarget,true);
                    break;
                case TutorialState.AdmitKids:
                    waypointMarker.target = receptionTarget;
                    break;
                case TutorialState.PickReceptionCash:
                    waypointMarker.target = receptionCashTarget;
                    cameraSwitcher.ZoomToTarget(receptionCashTarget,true);
                    break;
                case TutorialState.UnlockReceptionist:
                    receptionistUnlocker.SetActive(true);
                    receptionistUnlockerTarget.localScale = Vector3.zero;
                    waypointMarker.target = receptionistUnlockerTarget;
                    cameraSwitcher.ZoomToTarget(receptionistUnlockerTarget,true);
                    receptionistUnlockerTarget.DOScale(1, .5f);
                    StudentsDataManager.Instance.SpawnExtra();
                    break;
                case TutorialState.UnlockClassroom:
                    classroomUnlocker.SetActive(true);
                    firstClassroomUnlockerTarget.localScale = Vector3.zero;
                    waypointMarker.target = firstClassroomUnlockerTarget;
                    cameraSwitcher.ZoomToTarget(firstClassroomUnlockerTarget,true);
                    firstClassroomUnlockerTarget.DOScale(1, .5f);
                    break;
                case TutorialState.TeachTheClass:
                    waypointMarker.target = teachingClassTrigger;
                    cameraSwitcher.ZoomToTarget(teachingClassTrigger,true);
                    break;
                case TutorialState.UnlockNextClassroom:
                    secondClassroomUnlocker.SetActive(true);
                    waypointMarker.target = secondClassroomUnlockerTarget;
                    cameraSwitcher.ZoomToTarget(secondClassroomUnlockerTarget,true);
                    break;
                case TutorialState.Complete:
                    waypointMarker.gameObject.SetActive(false);
                    waypointMarker.arrowPivot.gameObject.SetActive(false);
                    TutorialComplete = true;
                    navigationManager.LookAtNextUnlock();
                    backToHomeBtn.SetActive(true);
                    EventsManager.TutCompleteEvent();
                    break;
                case TutorialState.IdleForSometime:
                    waypointMarker.gameObject.SetActive(false);
                    waypointMarker.arrowPivot.gameObject.SetActive(false);
                    break;
            }
        }

        public void SetStateBasedOn(TutorialState curState, TutorialState newState)
        {
            if (curTutState != curState) return;
            if (curTutState == newState) return;
            curStateIndex++;

            PlayerPrefs.SetInt("CurTutIndex", curStateIndex);
            if (curStateIndex >= tutorialStates.Length)
            {
                TutorialComplete = true;
                waypointMarker.gameObject.SetActive(false);
                waypointMarker.arrowPivot.gameObject.SetActive(false);
                return;
            }

            curTutState = newState;
            PlayerPrefs.SetInt("CurTutIndex", curStateIndex);
            DOVirtual.DelayedCall(1f, TakeActionOnState);
        }

        public void GoToNextState()
        {
            startingHandTut.SetActive(false);
            SetStateBasedOn(TutorialState.IdleState, TutorialState.GotoReception);
        }

        public bool CheckForCurrentState(TutorialState stateToCheck)
        {
            return stateToCheck == curTutState;
        }

        private void SetItemsBasedOnState(TutorialState state)
        {
            startingHandTut.SetActive(false);
            switch (state)
            {
                case TutorialState.IdleState:
                    startingHandTut.SetActive(true);
                    break;
                case TutorialState.GotoReception:
                    break;
                case TutorialState.PickReceptionCash:
                    break;
                case TutorialState.AdmitKids:
                    waypointMarker.gameObject.SetActive(true);
                    waypointMarker.arrowPivot.gameObject.SetActive(true);
                    break;
                case TutorialState.UnlockReceptionist:
                    receptionistUnlocker.SetActive(true);
                    break;
                case TutorialState.UnlockClassroom:
                    classroomUnlocker.SetActive(true);
                    break;
                case TutorialState.TeachTheClass:
                    break;
                case TutorialState.UnlockNextClassroom:
                    secondClassroomUnlocker.SetActive(true);
                    break;
                case TutorialState.Complete:
                    waypointMarker.gameObject.SetActive(false);
                    waypointMarker.arrowPivot.gameObject.SetActive(false);
                    TutorialComplete = true;
                    break;
                case TutorialState.IdleForSometime:
                    break;
            }
        }

        public void HideWaypoints()
        {
            waypointMarker.gameObject.SetActive(false);
            waypointMarker.arrowPivot.gameObject.SetActive(false);
        }

        public bool CanSpawnStudents()
        {
            return curStateIndex > 3;
        }
    }
}