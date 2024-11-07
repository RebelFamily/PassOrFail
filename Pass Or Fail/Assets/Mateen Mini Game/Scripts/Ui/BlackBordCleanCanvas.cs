using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace PassOrFail.MiniGames
{
    public class BlackBordCleanCanvas : MonoBehaviour, IMiniGame
    {
        [SerializeField] private Button tapItBtn;
        private bool _isAlreadyGameStart;
        [SerializeField] private UnityEvent onScreenClick;
        private void OnEnable()
        {
            tapItBtn.onClick.AddListener(ClickOnScreen);
        }
        private void OnDisable()
        {
            tapItBtn.onClick.RemoveListener(ClickOnScreen);
        }
        private void ClickOnScreen()
        {
            if (_isAlreadyGameStart) return;
            _isAlreadyGameStart = true;
            onScreenClick?.Invoke();
            tapItBtn.gameObject.SetActive(false);
        }
        private void InvokeEndMiniGame()
        {
            GamePlayManager.Instance.LevelComplete(0f);   
        }
        public void StartMiniGame()
        {
            
        }
        public void EndMiniGame()
        {
            Invoke(nameof(InvokeEndMiniGame), 1.2f);
        }
    }
}