using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Homework.Scripts
{
    public class Mover : MonoBehaviour
    {
        [FormerlySerializedAs("_start")] [SerializeField] 
        private Vector3 start;
        
        [FormerlySerializedAs("_end")] [SerializeField] 
        private Vector3 end;
        
        [FormerlySerializedAs("_speed")] [SerializeField] 
        private float speed = 1.0f;
        
        [FormerlySerializedAs("_delay")] [SerializeField] 
        private float delay = 1.0f;

        private Rigidbody _rigidbody;
        
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null)
            {
                Debug.LogError("Rigidbody component not found on the object!");
                return;
            }
            
            StartCoroutine(MoveObject());
        }
        
        private IEnumerator MoveObject()
        {
            var startWorldPosition = transform.TransformPoint(start);
            var endWorldPosition = transform.TransformPoint(end);
            var currentTarget = endWorldPosition;
            var movingToEnd = true;
            
            while (true)
            {
                var direction = (currentTarget - transform.position).normalized;
                
                while (Vector3.Distance(transform.position, currentTarget) > 0.01f)
                {
                    _rigidbody.MovePosition(transform.position + direction * (speed * Time.fixedDeltaTime));
                    yield return new WaitForFixedUpdate();
                }
                
                transform.position = currentTarget;
                
                yield return new WaitForSeconds(delay);
                
                movingToEnd = !movingToEnd;
                currentTarget = movingToEnd ? endWorldPosition : startWorldPosition;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
                return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.TransformPoint(start), 0.2f);
            
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.TransformPoint(end), 0.2f);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.TransformPoint(start), transform.TransformPoint(end));
        }

        private void Reset()
        {
            start = Vector3.zero; 
            end = Vector3.forward * 5f;
        }
    }
}
