using UnityEngine;
public class Book : MonoBehaviour
{
    public static Book SelectedBook { get; private set; } // Track the currently selected book
    private Color _originalColor = Color.white; // Store the original color of the book
    [SerializeField] private Renderer bookRenderer; // Reference to the book's Renderer
    [SerializeField] private Stack parentStack; // book belongs this stack
    [SerializeField] private BookSorting.BookColor bookColor;
    private void OnMouseDown()
    {
        if(parentStack.IsStackSorted()) return;
        // Only allow selection if this book is the last child in the stack
        if (transform.parent != null && transform.GetSiblingIndex() == transform.parent.childCount - 1)
        {
            if (SelectedBook == this)
            {
                DeselectBook();
            }
            else
            {
                SelectBook();
            }
        }
    }
    private void SelectBook()
    {
        if (SelectedBook != null)
        {
            SelectedBook.DeselectBook(); // Deselect previously selected book
        }

        SelectedBook = this;
        bookRenderer.material.color = Color.red; // Highlight color

        // Enable colliders for all stacks right away
        BookSorting.Instance.EnableAllStackColliders(true);
    }
    public void DeselectBook()
    {
        if (SelectedBook == this)
        {
            SelectedBook = null;
            bookRenderer.material.color = _originalColor; // Reset to original color

            // Disable colliders for all stacks
            BookSorting.Instance.EnableAllStackColliders(false);
        }
    }
    public void MoveToStack(Transform newStack)
    {
        parentStack = newStack.GetComponent<Stack>();
        transform.SetParent(newStack);
        transform.localPosition = new Vector3(0, transform.GetSiblingIndex() * 0.05f, 0);
        transform.localEulerAngles = new Vector3(0, -90f, -90f);
        DeselectBook();
    }
    public BookSorting.BookColor GetBookColor()
    {
        return bookColor;
    }
}