using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Helpers
{
    public class MeshLoader : MonoBehaviour
    {
        [SerializeField] private string meshPath = "Meshes/Classroom";
        [SerializeField] private MeshFilter filter;

        [ContextMenu("Load The Mesh")]
        private void LoadTheMesh()
        {
            // Load the mesh from resources
            var newMesh = Resources.Load<Mesh>(meshPath);

            if (newMesh != null)

                filter.mesh = newMesh;
            else
                Debug.LogError("Failed to load mesh from Resources.");
        }
    }
}