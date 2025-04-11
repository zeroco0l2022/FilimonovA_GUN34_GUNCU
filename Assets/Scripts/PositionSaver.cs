using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class PositionSaver : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        [Tooltip(
            "Для заполнения этого поля нужно воспользоваться контекстным меню в инспекторе и командой “Create File”")]
        private TextAsset _json;

        [SerializeField]
        [HideInInspector]
        private List<Data> _records = new();
        public List<Data> Records { get => _records; private set => _records = value; }

        private void Awake()
        {
            //todo comment: Что будет, если в теле этого условия не сделать выход из метода?
            // Продолжится выпполнение и выбросится исключение при попытке обратиться десериализовать объект
            if (_json == null)
            {
                gameObject.SetActive(false);
                Debug.LogError("Please, create TextAsset and add in field _json");
                return;
            }

            JsonUtility.FromJsonOverwrite(_json.text, this);
            //todo comment: Для чего нужна эта проверка (что она позволяет избежать)?
            // Чтобы избежать исключения если в json нет поля Records
            if (Records == null)
                Records = new List<Data>(10);
        }

        private void OnDrawGizmos()
        {
            //todo comment: Зачем нужны эти проверки (что они позволляют избежать)?
            // Исключение исключения при попытке обратиться к нулевой ссылке или пустому списку
            if (Records == null || Records.Count == 0) return;
            var data = Records;
            var prev = data[0].Position;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(prev, 0.3f);
            //todo comment: Почему итерация начинается не с нулевого элемента?
            // Потому что первая точка уже отрисована выше
            for (var i = 1; i < data.Count; i++)
            {
                var curr = data[i].Position;
                Gizmos.DrawWireSphere(curr, 0.3f);
                Gizmos.DrawLine(prev, curr);
                prev = curr;
            }
        }

        [Serializable]
        public struct Data
        {
            public Vector3 Position;
            public float Time;
        }

#if UNITY_EDITOR
        [ContextMenu("Create File")]
        private void CreateFile()
        {
            //todo comment: Что происходит в этой строке?
            // Это создает пустой текстовый файл в корне проекта
            var stream = File.Create(Path.Combine(Application.dataPath, "Path.txt"));
            //todo comment: Подумайте для чего нужна эта строка? (а потом проверьте догадку, закомментировав) 
            // Это закрывает поток, чтобы он не занимал ресурсы
            stream.Dispose();
            AssetDatabase.Refresh();
            //В Unity можно искать объекты по их типу, для этого используется префикс "t:"
            //После нахождения, Юнити возвращает массив гуидов (которые в мета-файлах задаются, например)
            var guids = AssetDatabase.FindAssets("t:TextAsset");
            foreach (var guid in guids)
            {
                //Этой командой можно получить путь к ассету через его гуид
                var path = AssetDatabase.GUIDToAssetPath(guid);
                //Этой командой можно загрузить сам ассет
                var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                //todo comment: Для чего нужны эти проверки?
                // Чтобы убедиться, что ассет загружен и его имя совпадает
                if (asset != null && asset.name == "Path")
                {
                    _json = asset;
                    EditorUtility.SetDirty(this);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    //todo comment: Почему мы здесь выходим, а не продолжаем итерироваться?
                    // Потому что мы нашли нужный ассет
                    return;
                }
            }
        }

        private void OnDestroy()
        {
            if (_json == null) return;

            var jsonString = JsonUtility.ToJson(this, true);

            var assetPath = AssetDatabase.GetAssetPath(_json);
            if (string.IsNullOrEmpty(assetPath)) return;

            File.WriteAllText(assetPath, jsonString);

            AssetDatabase.Refresh();

            Debug.Log($"Данные сохранены в {assetPath}");
        }
#endif
    }
}