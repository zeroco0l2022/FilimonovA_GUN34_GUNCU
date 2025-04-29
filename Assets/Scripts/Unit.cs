using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public event System.Action OnMoveEndCallback;
    
    public Cell Cell { get; set; }
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Team team;
    
    public Team Team => team;
    
    private void Start()
    {
        if (!Physics.Raycast(transform.position + Vector3.up, Vector3.down, out var hit)) return;
        Cell = hit.collider.GetComponent<Cell>();
        if (Cell != null)
        {
            Cell.Unit = this;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cell?.OnPointerEnter(eventData);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        Cell?.OnPointerExit(eventData);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Cell?.OnPointerClick(eventData);
    }
    
    public void Move(Cell targetCell)
    {
        if (Cell != null)
        {
            Cell.Unit = null;
        }
        StartCoroutine(MoveCoroutine(targetCell));
    }
    
    private IEnumerator MoveCoroutine(Cell targetCell)
    {
        var startPos = transform.position;
        var targetPos = targetCell.transform.position + Vector3.up * 0.5f;
        var journeyLength = Vector3.Distance(startPos, targetPos);
        var startTime = Time.time;
        
        while (transform.position != targetPos)
        {
            var distanceCovered = (Time.time - startTime) * moveSpeed;
            var fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPos, targetPos, fractionOfJourney);
            yield return null;
        }
        
        Cell = targetCell;
        Cell.Unit = this;
        OnMoveEndCallback?.Invoke();
    }
}
