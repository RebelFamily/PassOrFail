using DG.Tweening;
using UnityEngine;

namespace PassOrFail.MiniGames
{
    public class SecurityMachine : MonoBehaviour
    {
        [SerializeField] private StudentBag[] bagsData;
        [SerializeField] private SpriteRenderer propSprite;
        [SerializeField] private GameObject fullBodySpriteObject;

        [SerializeField] private Transform maskTransform;
        [SerializeField] private Vector3 maskVisiblePosition, maskHidePosition;
        [SerializeField] private GameObject scanParticles;
        private int _currentBagIndex;
        private bool _isLimitReached;
        private void OnEnable()
        {
            EventManager.OnStudentReachedDestination += DisplayDataOnMachine;
            EventManager.OnStudentChecked += HideData;
        }

        private void OnDisable()
        {
            EventManager.OnStudentReachedDestination -= DisplayDataOnMachine;
            EventManager.OnStudentChecked -= HideData;
        }

        private void DisplayDataOnMachine(Transform student)
        {
            if(_isLimitReached) return;
            if(student != bagsData[_currentBagIndex].transform) return;
            Debug.Log("BagIndex : student: "+bagsData[_currentBagIndex].transform + " prop Sprite: "+bagsData[_currentBagIndex].bagData.propSprite);
            //Debug.Log("BagIndex : isLimitReached: "+_isLimitReached + " what the fuck ");
            //Debug.Log("Hiding what");
            scanParticles.SetActive(true);
            maskTransform.localPosition = maskVisiblePosition;
            var propTransform = propSprite.transform;
            propTransform.localScale = bagsData[_currentBagIndex].bagData.propSmallScale;
            propTransform.localPosition = bagsData[_currentBagIndex].bagData.smallScalePosition;
            propSprite.sprite = bagsData[_currentBagIndex].bagData.propSprite;
            fullBodySpriteObject.SetActive(true);
            //play Checking Effect which will complete in 1.5 seconds
            maskTransform.DOLocalMove(maskHidePosition, 1.5f).OnComplete((() =>
            {
                scanParticles.SetActive(false);
                Invoke(nameof(EnlargeMainProp), .1f);
            }));
        }

        private void EnlargeMainProp()
        {
            if(_isLimitReached) return;
            fullBodySpriteObject.SetActive(false);
            propSprite.transform.DOScale(bagsData[_currentBagIndex].bagData.propFullScale, .5f);
            propSprite.transform.DOLocalMove(bagsData[_currentBagIndex].bagData.largeScalePosition, .5f);
        }

        private void HideData()
        {
            //Debug.Log("Hiding data");
            maskTransform.localPosition = maskVisiblePosition;
            _currentBagIndex += 1;
            if (_currentBagIndex >= bagsData.Length)
            {
                _isLimitReached = true;
                Debug.Log("BagIndex : isLimitReached: "+_isLimitReached);
            }
        }
    }
}