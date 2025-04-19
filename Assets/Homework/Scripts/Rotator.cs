using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Homework.Scripts
{
    public class Rotator : MonoBehaviour
    {
        [FormerlySerializedAs("_rotate")] [SerializeField]
        private Vector3 rotate = new Vector3(0, 90, 0);
        
        private Rigidbody _rigidbody;
        
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            if (_rigidbody == null)
            {
                Debug.LogError("Rigidbody component not found");
                return;
            }
            
            _rigidbody.isKinematic = true;
            
            StartCoroutine(RotateObject());
        }
        
        private IEnumerator RotateObject()
        {
            while (true)
            {
                var deltaRotation = Quaternion.Euler(rotate * Time.fixedDeltaTime);
                
                _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
                
                yield return new WaitForFixedUpdate();
            }
        }
        
        // Визуализация направления вращения в редакторе
        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
                return;
            
            if (rotate.x != 0)
            {
                Gizmos.color = Color.red;
                DrawRotationCircle(Vector3.right, Mathf.Sign(rotate.x));
            }
            
            if (rotate.y != 0)
            {
                Gizmos.color = Color.green;
                DrawRotationCircle(Vector3.up, Mathf.Sign(rotate.y));
            }

            if (rotate.z == 0) return;
            Gizmos.color = Color.blue;
            DrawRotationCircle(Vector3.forward, Mathf.Sign(rotate.z));
        }
        
        private void DrawRotationCircle(Vector3 axis, float direction)
        {
            const int segments = 32;
            const float radius = 1.0f;
            
            var center = transform.position;
            var up = axis.normalized;
            
            var forward = Mathf.Abs(Vector3.Dot(up, Vector3.forward)) < 0.9f ? Vector3.Cross(up, Vector3.forward).normalized : Vector3.Cross(up, Vector3.right).normalized;
            
            var right = Vector3.Cross(up, forward).normalized * direction;
            forward *= direction;
            
            var previousPoint = center + forward * radius;
            
            for (var i = 1; i <= segments; i++)
            {
                var angle = i * 2 * Mathf.PI / segments;
                var point = center + right * Mathf.Sin(angle) * radius + forward * Mathf.Cos(angle) * radius;
                Gizmos.DrawLine(previousPoint, point);
                previousPoint = point;
            }
        }
    }
}
