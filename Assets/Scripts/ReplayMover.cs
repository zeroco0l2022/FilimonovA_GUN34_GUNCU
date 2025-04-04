using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(PositionSaver))]
    public class ReplayMover : MonoBehaviour
    {
        private float _duration;

        private int _index;
        private PositionSaver.Data _prev;
        private PositionSaver _save;

        private void Start()
        {
            ////todo comment: зачем нужны эти проверки?
            // Чтобы убедиться, что компонент PositionSaver есть у объекта и у него есть записанные данные
            if (!TryGetComponent(out _save) || _save.Records.Count == 0)
            {
                Debug.LogError("Records incorrect value", this);
                //todo comment: Для чего выключается этот компонент?
                // Потому что без записанных данных мы не можем воспроизводить движение
                enabled = false;
            }
        }

        private void Update()
        {
            var curr = _save.Records[_index];
            //todo comment: Что проверяет это условие (с какой целью)? 
            // Настало ли время для перехода к следующей точке
            if (Time.time > curr.Time)
            {
                _prev = curr;
                _index++;
                //todo comment: Для чего нужна эта проверка?
                // Проверяем достигли ли последней записанной точки
                if (_index >= _save.Records.Count)
                {
                    enabled = false;
                    Debug.Log($"<b>{name}</b> finished", this);
                }
            }

            //todo comment: Для чего производятся эти вычисления (как в дальнейшем они применяются)?
            // Насколько продвинулись между предыдущей и текущей точками. Чтобы воспроизвести движение с то же скоростью
            var delta = (Time.time - _prev.Time) / (curr.Time - _prev.Time);
            //todo comment: Зачем нужна эта проверка?
            // Чтобы избежать исключения при попытке делить на ноль, если curr.Time - _prev.Time == 0
            if (float.IsNaN(delta)) delta = 0f;
            //todo comment: Опишите, что происходит в этой строчке так подробно, насколько это возможно
            // Выполняется линейная интерполяция между предыдущей и текущей точками
            // с коэффициэнтом delta. Delta определет, насколько близко к начальное и конечной точке находимся(0 - начало, 1 - конец)
            transform.position = Vector3.Lerp(_prev.Position, curr.Position, delta);
        }
    }
}