using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zain_Meta.Meta_Scripts.DataRelated
{
    [CreateAssetMenu(fileName = "StudentsData_", menuName = "Data/StudentsData", order = 0)]
    public class StudentsData : SaveClass
    {
        public List<DataClass> classesData = new();

        public void AddEachPersonData(int[] totalRides, int curClassIndex)
        {
            var data = new DataClass(totalRides, curClassIndex);
            classesData.Add(data);
        }

        public override void ClearData()
        {
            ClearAllStateData();
        }

        public void ClearAllStateData()
        {
            classesData.Clear();
        }
    }

    [Serializable]
    public class DataClass
    {
        public DataClass(int[] rides, int curIndex)
        {
            totalRides = rides.ToList();
            curClassIndex = curIndex;
        }

        public List<int> totalRides;
        public int curClassIndex;
    }
}