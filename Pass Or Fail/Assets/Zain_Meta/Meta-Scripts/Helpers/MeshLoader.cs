using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Helpers
{
    public class MeshLoader : MonoBehaviour
    {
        [SerializeField] private ClassroomUpgrade[] itemUpgrades;
        [SerializeField] private string meshPath = "Meshes/Classroom";
        [SerializeField] private MeshFilter filter;
        

        public void LoadTheMesh(int level,int index)
        {
            /*// Load the mesh from resources
            var newMesh = Resources.Load<Mesh>(meshPath);

            if (newMesh != null)

                filter.mesh = newMesh;
            else
                Debug.LogError("Failed to load mesh from Resources.");*/


            filter.mesh=itemUpgrades[level].meshLevels[index];
        }
    }
}