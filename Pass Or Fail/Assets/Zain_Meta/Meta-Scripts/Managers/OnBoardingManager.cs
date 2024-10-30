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
        }

        [SerializeField] private bool forceComplete;
        [SerializeField] private WaypointMarker waypointMarker;
        [SerializeField] private TutorialState[] tutorialStates;
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

            if (forceComplete)
            {
                TutorialComplete = forceComplete;
                receptionistUnlocker.SetActive(true);
                classroomUnlocker.SetActive(true);
                secondClassroomUnlocker.SetActive(true);
            }

            if (TutorialComplete)
            {
                navigationManager.ReloadThePurchasesData();
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
                    cameraSwitcher.ZoomToTarget(receptionTarget);
                    break;
                case TutorialState.AdmitKids:
                    waypointMarker.target = receptionTarget;
                    break;
                case TutorialState.PickReceptionCash:
                    waypointMarker.target = receptionCashTarget;
                    cameraSwitcher.ZoomToTarget(receptionCashTarget);
                    break;
                case TutorialState.UnlockReceptionist:
                    receptionistUnlocker.SetActive(true);
                    waypointMarker.target = receptionistUnlockerTarget;
                    cameraSwitcher.ZoomToTarget(receptionistUnlockerTarget);
                    break;
                case TutorialState.UnlockClassroom:
                    classroomUnlocker.SetActive(true);
                    waypointMarker.target = firstClassroomUnlockerTarget;
                    cameraSwitcher.ZoomToTarget(firstClassroomUnlockerTarget);
                    break;
                case TutorialState.TeachTheClass:
                    waypointMarker.target = teachingClassTrigger;
                    cameraSwitcher.ZoomToTarget(teachingClassTrigger);
                    break;
                case TutorialState.UnlockNextClassroom:
                    secondClassroomUnlocker.SetActive(true);
                    waypointMarker.target = secondClassroomUnlockerTarget;
                    cameraSwitcher.ZoomToTarget(secondClassroomUnlockerTarget);
                    break;
                case TutorialState.Complete:
                    waypointMarker.gameObject.SetActive(false);
                    waypointMarker.arrowPivot.gameObject.SetActive(false);
                    TutorialComplete = true;
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
            DOVirtual.DelayedCall(2f, TakeActionOnState);
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
                    cameraSwitcher.ZoomToTarget(receptionTarget);
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
    }
}