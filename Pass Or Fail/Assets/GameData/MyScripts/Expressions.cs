using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
public class Expressions : MonoBehaviour
{
    [SerializeField] private ExpressionFace[] expressionFaces;
    private Animator _characterAnimator;
    [SerializeField] private MeshRenderer meshRenderer;
    private int _offsetIndex = 0;
    private readonly Vector2[] _offsetValues0 =
    {
        new Vector2(0.36f,0f), new Vector2(0f,1.325f), new Vector2(0.36f,0.7f),
        new Vector2(0.7f,0f), new Vector2(0f,0.66f), new Vector2(0.012f,1f),
        new Vector2(0.36f,0f), new Vector2(0.7f,0.7f)
    };
    private static readonly int Idle = Animator.StringToHash("Idle");
    public enum ExpressionType
    {
        Normal = 0,
        Happy = 1,
        Excited = 2,
        Angry0 = 3,
        Angry1 = 4,
        Sad = 5,
        Frustrated = 6,
        Surprised = 7
    }
    public void ShowExpression(ExpressionType expressionType)
    {
        meshRenderer.material.mainTextureOffset = _offsetValues0[(int)expressionType];
    }
    public Sprite GetExpressionRender(ExpressionType type)
    {
        //Debug.Log("ExpressionType:" + type);
        return (from face in expressionFaces where type == face.expressionType select face.expressionRender).FirstOrDefault();
    }
    public void RandomIdleAnimation() // being used in animator by message behavior
    {
        if (!_characterAnimator) _characterAnimator = GetComponent<Animator>();
        _characterAnimator.SetInteger(Idle, Random.Range(0, 3));
    }
    public void ShowRandomExpression()
    {
        var r = Random.Range(0, 8);
        ShowExpression((ExpressionType)r);
    }
    public void ShowExpression()
    {
        meshRenderer.material.mainTextureOffset = _offsetValues0[_offsetIndex];
        _offsetIndex++;
        if (_offsetIndex >= _offsetValues0.Length)
            _offsetIndex = 0;
        Invoke(nameof(ShowExpression), 3f);
    }
    [Serializable]
    public class ExpressionFace
    {
        public ExpressionType expressionType;
        public Sprite expressionRender;
    }
}