using UnityEngine;
public class Stack : MonoBehaviour
{
    private const int MaxChildren = 6;
    private bool _isStackRegisteredForCompletion = false;
    private void OnMouseDown()
    {
        // Check if a book is selected
        var book = Book.SelectedBook;
        if (book != null)
        {
            if (transform.childCount == 0)
            {
                // Move the selected book to this stack
                MoveBookToStack(book);
            }
            else if (MaxChildren == transform.childCount)
            {
                book.DeselectBook();
            }
            else
            {
                if (book.GetBookColor() == GetLastAddedBook().GetBookColor())
                {
                    // Move the selected book to this stack
                    MoveBookToStack(book);
                }
                else
                {
                    book.DeselectBook();
                }
            }
        }
    }
    private void MoveBookToStack(Book book)
    {
        book.MoveToStack(transform);
        BookSorting.Instance.CheckIfActivityFinished();
        BookSorting.Instance.EndTutorial();
    }
    // Method to get the last added book in the stack
    private Book GetLastAddedBook()
    {
        if (transform.childCount > 0)
        {
            // Get the last child and return its Book component
            return transform.GetChild(transform.childCount - 1).GetComponent<Book>();
        }
        return null; // Return null if there are no books in the stack
    }
    public bool IsStackSorted()
    {
        if (transform.childCount == 0)
            return false;
        var color = transform.GetChild(0).GetComponent<Book>().GetBookColor();
        for (var i = 1; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).GetComponent<Book>().GetBookColor().Equals(color))
                return false;
        }
        return transform.childCount == MaxChildren;
    }
    public void RegisterStackAsSorted()
    {
        _isStackRegisteredForCompletion = true;
    }
    public bool IsStackRegisteredForCompletion()
    {
        return _isStackRegisteredForCompletion;
    }
}