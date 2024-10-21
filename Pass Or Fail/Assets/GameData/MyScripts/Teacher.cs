using UnityEngine;
public class Teacher : MonoBehaviour
{
    [SerializeField] private GameObject[] teachers;
    private void Start()
    {
        teachers[PlayerPrefsHandler.currentTeacher].SetActive(true);
    }
}