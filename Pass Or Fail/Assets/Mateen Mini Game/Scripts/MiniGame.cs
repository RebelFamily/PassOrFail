using System;
using UnityEngine;

namespace PassOrFail.MiniGames
{
    public class MiniGame : MonoBehaviour
    {
        private void Awake()
        {
            Invoke(nameof(CloseLoading),1f);
        }

        private void CloseLoading()
        {
            SharedUI.Instance.HideAll();
        }

        public void GameComplete()
        {
            Debug.Log("Game Complete");
        }
    }
}

