using System;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.PlayerRelated
{
    public class PlayerCashSystem : MonoBehaviour
    {
        [SerializeField] private ArcadeMovement arcadeController;
        public Transform spawnPos;
        private CashManager _cashManager;
        private float _curSmoothTimer;
        
        #region Event Callbacks

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        #endregion


        private void Start()
        {
            _cashManager = CashManager.Instance;
        }

        
        public void AddCashToPlayerStack(int len)
        {
            StackingCoroutine(len);
        }


        private void StackingCoroutine(int len)
        {
            Vibration.VibratePop();
            _cashManager.AddCash(5 * len);
        }

        public ArcadeMovement GetController() => arcadeController;
    }
}