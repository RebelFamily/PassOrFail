using System.Collections;
using UnityEngine;
namespace PassOrFail.MiniGames
{
    public class MouseEvents : MonoBehaviour
    {
        private bool _isDragging = false;
        private IDragAble _data;
        private readonly float _timeToMoveBack = .3f;
        private Transform _drgaingObject;
        private bool _isMovingBack;
        private bool _isToBlockInput;
        private Vector3 _startingPosition;
        private Quaternion _startingRotation;
        [SerializeField] private Camera camera;
        private float _previousX;
        private void LateUpdate()
        {
            if (_isToBlockInput) return;
            if (_isMovingBack) return;
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.GetRayIntersection(camera.ScreenPointToRay(Input.mousePosition));

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent(out _data))
                    {
                        _startingPosition = _data.StartingPosition;
                        _startingRotation = _data.StartingRotation;
                        _isDragging = true;
                        _drgaingObject = hit.transform;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (_isDragging)
                {
                    _isDragging = false;
                    SetObjectToMoveBack();
                }
            }

            if (_isDragging && _drgaingObject != null)
            {
                Vector3 mousePosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, Mathf.Abs(camera.transform.position.z)));
                _drgaingObject.position = new Vector3(mousePosition.x, mousePosition.y, 0);
                /*if (mousePosition.x > _previousX)
                    _drgaingObject.rotation = Quaternion.Euler(0, 0, _drgaingObject.eulerAngles.z - .3f);
                else if(mousePosition.x < _previousX)
                    _drgaingObject.rotation = Quaternion.Euler(0, 0, _drgaingObject.eulerAngles.z + .3f);

                _previousX = mousePosition.x;*/
            }
        }

        private void SetObjectToMoveBack()
        {
            _isMovingBack = true;
            _isDragging = false;
            StartCoroutine(MoveObjectBackWard(backTime: _timeToMoveBack, objectToMove: _drgaingObject));
            _drgaingObject = null;
        }

        private IEnumerator MoveObjectBackWard(float backTime, Transform objectToMove)
        {
            var elapsedTime = 0f;
            var startPosition = objectToMove.localPosition;
            var startRotation = objectToMove.localRotation;
            while (elapsedTime < backTime)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / backTime);
                objectToMove.localPosition = Vector3.Lerp(startPosition, _startingPosition, t);
                //objectToMove.localRotation = Quaternion.Lerp(startRotation, _startingRotation, t);
                yield return null;
            }

            //objectToMove.eulerAngles = _startingPosition;
            _isMovingBack = false;
        }
    }
}