using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class BoardGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public Material darkMaterial;
    public int boardSize = 8;

    public void CreateBoard()
    {
        ClearBoard();
        
        var offset = (boardSize - 1) / 2f;
        for (var i = 0; i < boardSize; i++)
        {
            for (var j = 0; j < boardSize; j++)
            {
                var cellObject = Instantiate(cellPrefab, 
                    new Vector3(i - offset, 0, j - offset), 
                    Quaternion.identity, 
                    transform);

                if ((i + j) % 2 != 0)
                {
                    cellObject.GetComponent<MeshRenderer>().material = darkMaterial;
                }
                
                cellObject.name = "Cell";
            }
        }
    }
    
    public void ClearBoard()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
