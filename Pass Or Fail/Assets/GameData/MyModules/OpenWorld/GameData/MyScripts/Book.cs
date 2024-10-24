using UnityEngine;

public class Book : MonoBehaviour
{
    public static Book SelectedBook { get; private set; } // Track the currently selected book
    private Color _originalColor; // Store the original color of the book
    private Renderer _bookRenderer; // Reference to the book's Renderer
    private Stack _parentStack; // book belongs this stack

    private void Start()
    {
        _bookRenderer = GetComponent<Renderer>(); // Get the book's Renderer
        _originalColor = _bookRenderer.material.color; // Store the original color
        _parentStack = GetComponentInParent<Stack>(); // store the current stack
    }

    private void OnMouseDown()
    {
        if(_parentStack.IsStackSorted()) return;
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
        _bookRenderer.material.color = Color.yellow; // Highlight color

        // Enable colliders for all stacks right away
        BookSorting.Instance.EnableAllStackColliders(true);
    }

    public void DeselectBook()
    {
        if (SelectedBook == this)
        {
            SelectedBook = null;
            _bookRenderer.material.color = _originalColor; // Reset to original color

            // Disable colliders for all stacks
            BookSorting.Instance.EnableAllStackColliders(false);
        }
    }

    public void MoveToStack(Transform newStack)
    {
        _parentStack = newStack.GetComponent<Stack>();
        transform.SetParent(newStack);
        transform.localPosition = new Vector3(0, transform.GetSiblingIndex() * 0.05f, 0);
        transform.localEulerAngles = new Vector3(0, -90f, -90f);
        DeselectBook();
    }
    public Color GetBookColor()
    {
        return _originalColor;
    }
}