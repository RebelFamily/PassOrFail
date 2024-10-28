using DG.Tweening;
using PassOrFail.MiniGames;
using UnityEngine;

public class LabChecker : MonoBehaviour
{
    [SerializeField] private MiniGameStudentHandler miniGameStudentHandler;
    [SerializeField] private StudentLabData[] studentLabData;
    [SerializeField] private Transform landPlacePosition, landRestPosition, studentReadyPosition;
    [SerializeField] private GameObject liquid;
    [SerializeReference] private Material cyanMaterial, magentaMaterial;
    [SerializeField] private SpriteRenderer goalToAchieve;
    [SerializeField] private GameObject goodEffect, badEffect, smokeEffect;
    [SerializeField] private GameObject cyanLiquid, magentaLiquid;
    [SerializeField] private GameObject canvasObject;
    [SerializeField] private Transform liquidSparks;
    private Variables.ColorsName _lastSpawnColor = Variables.ColorsName.None;
    private BoxCollider _boxCollider;
    private GameObject _spawnedModel;
    private float _accuracy = 100;
    private bool _isButtonPressed, _isWaitingForPressedTime;
    private Variables.ColorsName _targetSlotColor = Variables.ColorsName.None;
    private bool _isJobFinished;
    private Vector3 _previousPartPosition;
    private bool _isFillingWrongly;

    private float _buttonClickTime;
    private int _currentStudentIndex;
    private Vector3 _initialPos;

    private void OnEnable()
    {
        EventManager.OnStudentReachedDestination += PlaceLandModel;
        EventManager.OnBoundaryEnter += SetRequirements;
    }

    private void OnDisable()
    {
        EventManager.OnStudentReachedDestination -= PlaceLandModel;
        EventManager.OnBoundaryEnter -= SetRequirements;
    }

    private void PlaceLandModel(Transform student)
    {
        if (studentLabData[_currentStudentIndex].transform != student) return;
        _lastSpawnColor = Variables.ColorsName.None;
        _isJobFinished = false;
        _isFillingWrongly = false;
        _accuracy = 100;
        var model = studentLabData[_currentStudentIndex].labData.landModel.transform;
        Debug.Log("Placing Model on: " + model);
        model.parent = transform;
        model.DOMove(landPlacePosition.position, .5f).OnComplete(() =>
        {
            studentLabData[_currentStudentIndex].PlaceFlaskInHands();
            Invoke(nameof(OpenLandModel), .2f);
        });
        model.DOLocalRotate(Vector3.zero, .2f);
    }

    private void OpenLandModel()
    {
        Debug.Log("Placing OpenLandModel: " + studentLabData[_currentStudentIndex].transform);
        _previousPartPosition = studentLabData[_currentStudentIndex].labData.landDisablePart.transform.position;
        studentLabData[_currentStudentIndex].transform.DOMove(studentReadyPosition.position, .5f);
        studentLabData[_currentStudentIndex].transform.DORotate(studentReadyPosition.eulerAngles, .5f);
        studentLabData[_currentStudentIndex].labData.landDisablePart.transform.DOMove(landRestPosition.position, .5f).OnComplete((
            () =>
            {
                canvasObject.SetActive(true);
            }));
        goalToAchieve.sprite = studentLabData[_currentStudentIndex].labData.targetToAchieve;
    }

    private void CloseLandModel()
    {
        //close model
        //play confetti based on results
        Debug.Log("Closing Model: " + studentLabData[_currentStudentIndex].transform);
        canvasObject.SetActive(false);
        StopLiquidFlow();
        studentLabData[_currentStudentIndex].StopPouring();
        studentLabData[_currentStudentIndex].HideFlaskInHand();
        studentLabData[_currentStudentIndex].labData.landDisablePart.transform.DOMove(_previousPartPosition, 1f)
            .OnComplete((
                () =>
                {
                    //var model = studentLabData[_currentStudentIndex].labData.landModel.transform;
                    studentLabData[_currentStudentIndex].labData.jerkAnimator.enabled = true;
                    Invoke(nameof(ProceedModel),1.7f);
                   
                }));
       
    }

    private void ProceedModel()
    {
        studentLabData[_currentStudentIndex].labData.jerkAnimator.enabled = false;
        /*model.DOShakePosition(2f, .07f).OnComplete(((() =>
        {
            ShowResultEffect();
            Invoke(nameof(MergeModelWithStudent), 1.8f);
        })));*/
        ShowResultEffect();
        Invoke(nameof(MergeModelWithStudent), 1.8f);
    }
    private void ShowResultEffect()
    {
        switch (_accuracy)
        {
            case > 80:
                goodEffect.SetActive(true);
                //miniGameStudentHandler.ShowStudentExpression(Expressions.ExpressionType.Happy);
                break;
            case > 65 and <= 80:
                badEffect.SetActive(true);
                //miniGameStudentHandler.ShowStudentExpression(Expressions.ExpressionType.Normal);
                break;
            case <= 65:
                smokeEffect.SetActive(true);
                //miniGameStudentHandler.ShowStudentExpression(Expressions.ExpressionType.Angry0);
                break;
        }
    }

    private void MergeModelWithStudent()
    {
        goodEffect.SetActive(false);
        badEffect.SetActive(false);
        smokeEffect.SetActive(false);
        var model = studentLabData[_currentStudentIndex].labData.landModel.transform;
        model.parent = studentLabData[_currentStudentIndex].labData.landModelParent;
        model.DOMove(studentLabData[_currentStudentIndex].labData.landModelFinalPosition.position, .5f);
        model.DORotate(studentLabData[_currentStudentIndex].labData.landModelFinalPosition.eulerAngles, .5f).OnComplete(
            (
                () =>
                {
                    miniGameStudentHandler.ExitStudent(emotion: Expressions.ExpressionType.Normal, true);
                    goalToAchieve.sprite = null;
                }));
        _currentStudentIndex += 1;
    }

    private void SetRequirements(Variables.ColorsName colorsName, bool isJobFinished)
    {
        _targetSlotColor = colorsName;
        _isJobFinished = isJobFinished;
        if (_isJobFinished)
        {
            _isButtonPressed = false;
            CloseLandModel();
        }
        else
        {
            if (_lastSpawnColor != colorsName)
                _isFillingWrongly = true;
            else
                _isFillingWrongly = false;
            //_accuracy -= 5;
        }
    }

    public void CyanButtonClick()
    {
        if (_isJobFinished) return;
        _buttonClickTime = Time.time;
        _isWaitingForPressedTime = true;
        CancelInvoke(nameof(PerformMagentaButton));
        Invoke(nameof(PerformCyanButton),.2f);
    }
    
    public void MagentaButtonClick()
    {
        if (_isJobFinished) return;
        _buttonClickTime = Time.time;
        _isWaitingForPressedTime = true;
        CancelInvoke(nameof(PerformCyanButton));
        Invoke(nameof(PerformMagentaButton),.2f);
    }

    private void PerformCyanButton()
    {
        if(!_isWaitingForPressedTime) return;
        studentLabData[_currentStudentIndex].PourLiquid(Variables.ColorsName.Cyan);
        if (_lastSpawnColor == Variables.ColorsName.None)
        {
            _lastSpawnColor = Variables.ColorsName.Cyan;
            SpawnLiquid(true, Variables.ColorsName.Cyan);
        }
        else if (_lastSpawnColor == Variables.ColorsName.Cyan)
        {
            _isButtonPressed = true;
        }
        else if (_lastSpawnColor == Variables.ColorsName.Magenta)
        {
            _lastSpawnColor = Variables.ColorsName.Cyan;
            SpawnLiquid(false, Variables.ColorsName.Cyan);
        }

        cyanLiquid.SetActive(true);
        liquidSparks.gameObject.SetActive(true);

        if (_targetSlotColor != Variables.ColorsName.None && _targetSlotColor != Variables.ColorsName.Cyan)
        {
            //_accuracy -= 5;
            _isFillingWrongly = true;
        }

        else
        {
            _isFillingWrongly = false;
        }
    }
    private void PerformMagentaButton()
    {
        if(!_isWaitingForPressedTime) return;
        studentLabData[_currentStudentIndex].PourLiquid(Variables.ColorsName.Magenta);
        if (_lastSpawnColor == Variables.ColorsName.None)
        {
            _lastSpawnColor = Variables.ColorsName.Magenta;
            SpawnLiquid(true, Variables.ColorsName.Magenta);
        }
        else if (_lastSpawnColor == Variables.ColorsName.Magenta)
        {
            _isButtonPressed = true;
        }
        else if (_lastSpawnColor == Variables.ColorsName.Cyan)
        {
            _lastSpawnColor = Variables.ColorsName.Magenta;
            SpawnLiquid(false, Variables.ColorsName.Magenta);
        }

        if (_targetSlotColor != Variables.ColorsName.None && _targetSlotColor != Variables.ColorsName.Magenta)
        {
            //_accuracy -= 5;
            _isFillingWrongly = true;
        }

        else
        {
            _isFillingWrongly = false;
        }

        liquidSparks.gameObject.SetActive(true);
        magentaLiquid.SetActive(true);
    }

    public void ButtonClickStopped()
    {
        if (_isJobFinished) return;
        _isWaitingForPressedTime = false;
        Debug.Log("buttonclick stopped");
        StopLiquidFlow();
        _isButtonPressed = false;
        studentLabData[_currentStudentIndex].StopPouring();
    }

    private void StopLiquidFlow()
    {
        cyanLiquid.SetActive(false);
        magentaLiquid.SetActive(false);
        liquidSparks.gameObject.SetActive(false);
    }

    private void SpawnLiquid(bool isNew, Variables.ColorsName colorsName)
    {
        if (isNew)
        {
            var t = studentLabData[_currentStudentIndex].labData;
            _spawnedModel = Instantiate(liquid, t.initialLiquidPosition.position, t.initialLiquidPosition.rotation,
                t.liquidParent);
            _boxCollider = _spawnedModel.GetComponent<BoxCollider>();
        }
        else
        {
           var t = studentLabData[_currentStudentIndex].labData.initialLiquidPosition.position;
            var spawnPos = new Vector3(t.x, _boxCollider.bounds.max.y, t.z);
            _spawnedModel = Instantiate(liquid, spawnPos, _boxCollider.transform.rotation,
                studentLabData[_currentStudentIndex].labData.liquidParent);
            _boxCollider = _spawnedModel.GetComponent<BoxCollider>();
        }

        _initialPos = _spawnedModel.transform.position;
        switch (colorsName)
        {
            case Variables.ColorsName.Cyan:
                _spawnedModel.GetComponent<MeshRenderer>().material = cyanMaterial;
                break;
            case Variables.ColorsName.Magenta:
                _spawnedModel.GetComponent<MeshRenderer>().material = magentaMaterial;
                break;
        }

        _isButtonPressed = true;
    }

    private readonly Vector3 _increaseScale = new Vector3(0, .1f, 0);

    private void Update()
    {
        if (!_isButtonPressed) return;
        _buttonClickTime -= Time.deltaTime;
        if (_buttonClickTime < .35f) return;
        _spawnedModel.transform.localScale += _increaseScale;
        liquidSparks.position = new Vector3(_initialPos.x, _boxCollider.bounds.max.y, _initialPos.z);
        if (_isFillingWrongly)
        {
            _accuracy -= .1f;
        }
    }
}