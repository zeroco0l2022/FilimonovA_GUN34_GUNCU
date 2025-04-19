using UnityEngine;
using UnityEngine.Serialization;

namespace Homework.Scripts
{
    public class Gates : MonoBehaviour
    {
        private int _score = 0;
        
        [FormerlySerializedAs("_ballTag")] [SerializeField]
        private string ballTag = "Ball"; // Тег мяча, висит на префабе мяча
        
        // Start is called before the first frame update
        void Start()
        {
            // Нет коллайдера, выходим с логом
            var collider = GetComponent<Collider>();
            if (collider == null)
            {
                Debug.LogError("Gates must have Collider component!");
                return;
            }
            
            if (collider.isTrigger) return;
            // Если коллайдер не установлен как триггер, выводим предупреждение
            Debug.LogWarning("Gates collider is not set as trigger.");
            collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Проверяем, что столкнулись с мячем по тегу
            if (!other.CompareTag(ballTag)) return;
            
            // Увеличиваем счет, вывод в консоль, уничтожаем мяч
            _score++;
            Debug.Log($"Goal! Current score: {_score}");
            Destroy(other.gameObject);
        }
    }
}
