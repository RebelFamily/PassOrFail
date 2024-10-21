using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LibraryDiscipline : MonoBehaviour
{
    [SerializeField] private Librarian librarian;
    [SerializeField] private List<LibraryStudent> libraryStudents;
    private void Start()
    {
        librarian.RegisterEndEvent(EndActivity);
        StartCoroutine(StartGossiping());
    }
    private IEnumerator StartGossiping()
    {
        var delay = Random.Range(2f, 5f);
        var index = GetRandomIndex();
        yield return new WaitForSeconds(delay);
        if (libraryStudents[index].IsGossiping())
        {
            StartCoroutine(StartGossiping());
            yield break;
        }
        libraryStudents[index].StartGossiping();
        yield return null;
        StartCoroutine(StartGossiping());
    }
    private int GetRandomIndex()
    {
        var r = Random.Range(0, libraryStudents.Count);
        while (libraryStudents[r].IsGossiping())
        {
            r = Random.Range(0, libraryStudents.Count);
        }
        return r;
    }
    public void StartActivity()
    {
        librarian.SetLibrarianFlag(true);
        SharedUI.Instance.gamePlayUIManager.controls.EnableLibraryActivityControls();
    }
    private void EndActivity()
    {
        StopAllCoroutines();
        foreach (var t in libraryStudents)
        {
            t.DisableParticles();
        }
        GamePlayManager.Instance.LevelComplete(1f);
    }
}