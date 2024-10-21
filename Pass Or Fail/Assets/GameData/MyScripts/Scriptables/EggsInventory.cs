using System;
using UnityEngine;
[CreateAssetMenu(fileName = "EggsInventory", menuName = "ScriptableObjects/EggsInventory", order = 4)]
public class EggsInventory : ScriptableObject
{
    public Egg[] eggs;
    [Serializable]
    public class Egg
    {
        public string eggName;
        public GameObject eggObject;
        public bool isEggAlright;
        public Vector3 positionForFemale;
        public Vector3 positionForMale;
    }
}