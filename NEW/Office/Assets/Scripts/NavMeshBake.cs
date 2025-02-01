using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;

public class NavMeshBake : MonoBehaviour
{
    public static NavMeshBake Instance;
    private NavMeshSurface navMeshSurface;

    private void Awake()
    {
        Instance = this;
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    public IEnumerator BakeAndCheckNavMesh()
    {
        
        Debug.Log("Starting NavMesh Bake...");
        navMeshSurface.BuildNavMesh();

        yield return null;

        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
        if (navMeshData.vertices.Length > 0)
        {
            Debug.Log("✅ NavMesh successfully baked!");
        }
        else
        {
            Debug.LogError("❌ NavMesh did NOT bake! Check NavMeshSurface settings.");
        }
    }
}