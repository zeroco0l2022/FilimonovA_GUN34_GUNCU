using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(BoardGenerator))]
    public class BoardGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var boardGenerator = (BoardGenerator)target;
            EditorGUILayout.Space();
            if (GUILayout.Button("Создать доску"))
            {
                boardGenerator.CreateBoard();
            }
        
            if (GUILayout.Button("Очистить доску"))
            {
                boardGenerator.ClearBoard();
            }
        }
    }
} 