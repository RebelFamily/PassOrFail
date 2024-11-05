using System;
using UnityEngine;

namespace Zain_Meta.Meta_Scripts.DataRelated
{
    [CreateAssetMenu(fileName = "Render", menuName = "Data/RenderData", order = 0)]
    public class RenderData : SaveClass
    {
        public Render[] renders;
    }
[Serializable]
    public struct Render
    {
        public Sprite renderToShowA,renderToShowB,renderToShowC;
    }
}