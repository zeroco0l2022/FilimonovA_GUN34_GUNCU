using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public int cubeCount = 10;
    public Vector3 spawnArea = new Vector3(10f, 0f, 10f);
    
    private List<GameObject> generatedCubes = new List<GameObject>();
    
    // Метод для генерации кубиков
    public void GenerateCubes()
    {
        ClearCubes(); // Сначала очищаем предыдущие кубики
        
        for (int i = 0; i < cubeCount; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnArea.x/2, spawnArea.x/2),
                Random.Range(0, spawnArea.y),
                Random.Range(-spawnArea.z/2, spawnArea.z/2)
            );
            
            GameObject cube = PrefabUtility.InstantiatePrefab(cubePrefab) as GameObject;
            if (cube != null)
            {
                cube.transform.position = transform.position + randomPosition;
                cube.transform.SetParent(transform);
                generatedCubes.Add(cube);
                
                // Отмечаем объект как созданный в редакторе
                Undo.RegisterCreatedObjectUndo(cube, "Generate Cube");
            }
        }
    }
    
    // Метод для очистки кубиков
    public void ClearCubes()
    {
        foreach (GameObject cube in generatedCubes)
        {
            if (cube != null)
            {
                Undo.DestroyObjectImmediate(cube);
            }
        }
        generatedCubes.Clear();
    }
} 