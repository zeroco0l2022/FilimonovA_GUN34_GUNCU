using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    [RequireComponent(typeof(PositionSaver))]
    public class EditorMover : MonoBehaviour
    {
        private float _currentDelay;

        //todo comment: Что произойдёт, если _delay > _duration?
        // Сработает проверка в Start(), duration увеличится delay*5f 

        [FormerlySerializedAs("_delay")]
        [Range(0.2f, 1.0f)] 
        [SerializeField]
        private float delay = 0.5f;

        [FormerlySerializedAs("_duration")]
        [Min(0.2f)] 
        [SerializeField]
        private float duration = 5f;

        private PositionSaver _save;

        private void Start()
        {
            //todo comment: Почему этот поиск производится здесь, а не в начале метода Update?
            // Потому что нам нудно получить ссылку на компонент один раз при забуске, а не каждый кадр
            _save = GetComponent<PositionSaver>();
            _save.Records.Clear();

            if (duration <= delay) duration = delay * 5f;
        }

        private void Update()
        {
            duration -= Time.deltaTime;
            if (duration <= 0f)
            {
                enabled = false;
                Debug.Log($"<b>{name}</b> finished", this);
                return;
            }

            //todo comment: Почему не написать (_delay -= Time.deltaTime;) по аналогии с полем _duration?
            // Потому что _delay - это постоянный интервал между записями, а duration - счетчик
            _currentDelay -= Time.deltaTime;
            if (_currentDelay <= 0f)
            {
                _currentDelay = delay;
                _save.Records.Add(new PositionSaver.Data
                {
                    Position = transform.position,
                    //todo comment: Для чего сохраняется значение игрового времени?
                    // Чтобы ReplayMover воспроизводил движения с то же скоростью
                    Time = Time.time
                });
            }
        }
    }
}