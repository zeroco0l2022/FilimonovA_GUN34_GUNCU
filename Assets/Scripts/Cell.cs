using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public event System.Action<Cell> OnPointerClickEvent;
    
    [SerializeField] private GameObject focus;
    [SerializeField] private GameObject select;
    
    public Unit Unit { get; set; }
    public Vector2Int GridPosition { get; private set; }
    public List<Cell> Neighbours { get; set; }
    public Unit OccupyingUnit { get; set; }
    public string UniqueID { get; private set; }
    public Dictionary<NeighbourType, Cell> NeighboursDictionary { get; set; } = new Dictionary<NeighbourType, Cell>();
    
    private bool _isSelected;
    private System.DateTime _lastSelectTime;
    private System.DateTime _lastResetTime;
    
    public enum SelectionType
    {
        None,
        Selected,
        Move,
        Attack,
        MoveAndAttack
    }
    
    private SelectionType _currentSelectionType = SelectionType.None;
    
    private void Awake()
    {
        focus.SetActive(false);
        select.SetActive(false);
        UniqueID = System.Guid.NewGuid().ToString();
        GridPosition = CalculateGridPosition();
    }
    
    private Vector2Int CalculateGridPosition()
    {
        var x = Mathf.FloorToInt(transform.position.x + 0.5f);
        var z = Mathf.FloorToInt(transform.position.z + 0.5f);
        
        var gridPos = new Vector2Int(x, z);
        return gridPos;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        focus.SetActive(true);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        focus.SetActive(false);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnPointerClickEvent != null)
        {
            OnPointerClickEvent.Invoke(this);
        }
    }
    
    public void SetSelect(Material material, SelectionType type = SelectionType.Selected)
    {
        if (select == null)
        {
            return;
        }
        
        _currentSelectionType = type;
        select.SetActive(true);
        _isSelected = true;
        
        var meshRenderer = select.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = true;
            
            if (material != null)
            {
                meshRenderer.material = material;
            }
        }
        
        StartCoroutine(MonitorSelectionState());
    }
    
    public void ResetSelect()
    {
        _lastResetTime = System.DateTime.Now;
        _isSelected = false;
        
        if (select == null)
        {
            return;
        }
        
        select.SetActive(false);
    }

    private IEnumerator MonitorSelectionState()
    {
        const float checkDuration = 5.0f;
        var startTime = Time.time;
        
        while (Time.time - startTime < checkDuration)
        {
            if (_isSelected && select && !select.activeSelf)
            {
                select.SetActive(true);
                
                var meshRenderer = select.GetComponent<MeshRenderer>();
                if (meshRenderer)
                {
                    meshRenderer.enabled = true;
                }
            }
            
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SimulateClick()
    {
        OnPointerClickEvent?.Invoke(this);
    }

    public bool HasClickSubscribers()
    {
        return OnPointerClickEvent != null;
    }
}
