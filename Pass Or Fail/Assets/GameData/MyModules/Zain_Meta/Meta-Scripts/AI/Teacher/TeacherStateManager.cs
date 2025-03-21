﻿using UnityEngine;
using Zain_Meta.Meta_Scripts.AI.Teacher.States;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.AI.Teacher
{
    public class TeacherStateManager : MonoBehaviour
    {
        [SerializeField] private TeacherRequirement requirement;

        public GoingToTeach GoingToTeach = new GoingToTeach();
        public TeachingState TeachingState = new TeachingState();
        public WaitingInClass WaitingInClass = new WaitingInClass();
        public StandingIdleInClass StandingIdleInClass = new StandingIdleInClass();
        public GoingToStaffRoom GoingToStaffRoom = new GoingToStaffRoom();
        public SleepyState SleepyState = new SleepyState();
        public DrinkCoffeeState DrinkCoffeeState = new DrinkCoffeeState();
        public MoveToRestingState MoveToRestingState = new MoveToRestingState();
        public RestingInClassState RestingInClassState = new RestingInClassState();

        private ITeacherState _curState;
        public string stateName;

        private void OnEnable()
        {
            ChangeState(GoingToTeach);
        }

        private void Start()
        {
            TeachersManager.Instance.AddNewTeacher(this);
        }

        private void LateUpdate()
        {
            _curState?.UpdateState(this);
            stateName = _curState?.ToString();
        }


        public void ChangeState(ITeacherState newState)
        {
            if (_curState != null)
            {
                if (_curState == newState) return;
                _curState.ExitState(this);
            }

            _curState = newState;
            _curState.EnterState(this);
        }

        public TeacherRequirement GetRequirement() => requirement;

        public bool IsSleepy()
        {
            return _curState == SleepyState;
        }
    }
}