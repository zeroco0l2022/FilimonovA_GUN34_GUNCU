using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Homework.Scripts
{
	public class Player : MonoBehaviour
	{
		private bool _ready;
		private Rigidbody _ball;
		
		[FormerlySerializedAs("_ballPrefab")] [SerializeField]
		private Rigidbody ballPrefab;
		[FormerlySerializedAs("_startVelocity")] [SerializeField]
		private float startVelocity;
		[FormerlySerializedAs("_lifetime")] [SerializeField]
		private float lifetime;

		[FormerlySerializedAs("_respawnDelay")] [SerializeField]
		private float respawnDelay;
		
		[FormerlySerializedAs("_spawnOffset")] [SerializeField]
		private Vector3 spawnOffset = Vector3.zero; // Смещение точки спавна относительно игрока
		
		[FormerlySerializedAs("_gizmoRadius")] [SerializeField]
		private float gizmoRadius = 0.2f;

		private void Update()
		{
			if (!_ready) return;
			if (!Input.GetKey(KeyCode.Space)) return;
			StartCoroutine(Reloader());
			_ball.isKinematic = false;
				
			Transform parent = _ball.transform.parent;
			_ball.transform.parent = null;
			Destroy(parent.gameObject);
				
			_ball.velocity = transform.forward * startVelocity;
			Destroy(_ball.gameObject, lifetime);
		}

		private IEnumerator Reloader()
		{
			_ready = false;
			yield return new WaitForSeconds(respawnDelay);
			Spawn();
		}

		private void Spawn()
		{
			// Создаем мяч без родителя в мировых координатах (чтобы мячь не скейлился под игрока)
			var spawnPosition = transform.position + transform.TransformDirection(spawnOffset);
			_ball = Instantiate(ballPrefab, spawnPosition, transform.rotation);
			
			// пустой объект для отслеживания позиции
			var tracker = new GameObject("BallTracker");
			tracker.transform.SetParent(transform);
			tracker.transform.position = spawnPosition;
			
			// привязываем мяч к трекеру
			_ball.transform.SetParent(tracker.transform);
			
			_ball.isKinematic = true;
			_ready = true;
		}

		private void Start()
		{
			Spawn();
		}
		
		private void OnDrawGizmosSelected()
		{
			if (Application.isPlaying)
				return;
			
			Vector3 spawnPosition = transform.position + transform.TransformDirection(spawnOffset);
			
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(spawnPosition, gizmoRadius);
			
			Gizmos.color = Color.yellow;
			Gizmos.DrawRay(spawnPosition, transform.forward * 2f);
		}
	}
}