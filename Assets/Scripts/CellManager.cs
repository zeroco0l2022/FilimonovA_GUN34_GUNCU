using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public event System.Action<Cell> OnCellClicked;
    
    [SerializeField] private CellPaletteSettings paletteSettings;
    
    private List<Cell> _cells = new List<Cell>();
    private List<Unit> _units = new List<Unit>();
    private Dictionary<Vector2Int, List<Cell>> _cellGridMultiple = new Dictionary<Vector2Int, List<Cell>>();
    
    private Cell _lastSelectedCell;
    
    private void Start()
    {
        FindAllCells();
        FindAllUnits();
        SetupNeighbours();
        SetupUnitCellConnections();
    }
    
    private void FindAllCells()
    {
        _cells = new List<Cell>(FindObjectsOfType<Cell>());
        
        _cellGridMultiple.Clear();
        
        foreach (var cell in _cells)
        {
            if (!_cellGridMultiple.ContainsKey(cell.GridPosition))
            {
                _cellGridMultiple[cell.GridPosition] = new List<Cell>();
            }
            
            _cellGridMultiple[cell.GridPosition].Add(cell);
            
            cell.OnPointerClickEvent += HandleCellClick;
        }
    }
    
    private void FindAllUnits()
    {
        _units = new List<Unit>(FindObjectsOfType<Unit>());
    }
    
    private void SetupNeighbours()
    {
        foreach (var kvp in _cellGridMultiple)
        {
            var position = kvp.Key;
            var cells = kvp.Value;
            
            foreach (var cell in cells)
            {
                var neighbors = new Dictionary<NeighbourType, Cell>();
                
                CheckNeighbor(new Vector2Int(position.x - 1, position.y), NeighbourType.Left, neighbors, cell);
                CheckNeighbor(new Vector2Int(position.x + 1, position.y), NeighbourType.Right, neighbors, cell);
                CheckNeighbor(new Vector2Int(position.x, position.y + 1), NeighbourType.Top, neighbors, cell);
                CheckNeighbor(new Vector2Int(position.x, position.y - 1), NeighbourType.Bottom, neighbors, cell);
                CheckNeighbor(new Vector2Int(position.x - 1, position.y + 1), NeighbourType.LeftTop, neighbors, cell);
                CheckNeighbor(new Vector2Int(position.x - 1, position.y - 1), NeighbourType.LeftBottom, neighbors, cell);
                CheckNeighbor(new Vector2Int(position.x + 1, position.y + 1), NeighbourType.RightTop, neighbors, cell);
                CheckNeighbor(new Vector2Int(position.x + 1, position.y - 1), NeighbourType.RightBottom, neighbors, cell);
                
                cell.NeighboursDictionary = neighbors;
            }
        }
    }
    
    private void CheckNeighbor(Vector2Int neighborPos, NeighbourType type, Dictionary<NeighbourType, Cell> neighbors, Cell currentCell)
    {
        if (!_cellGridMultiple.TryGetValue(neighborPos, out var neighborCells) ||
            neighborCells.Count <= 0) return;
        foreach (var cell in neighborCells.Where(cell => cell.UniqueID != currentCell.UniqueID))
        {
            neighbors[type] = cell;
            break;
        }
    }
    
    private void SetupUnitCellConnections()
    {
        foreach (var unit in _units)
        {
            if (!Physics.Raycast(unit.transform.position + Vector3.up, Vector3.down, out var hit)) continue;
            var cell = hit.collider.GetComponent<Cell>();
            if (cell == null) continue;
            unit.Cell = cell;
            cell.Unit = unit;
        }
    }
    
    private void HandleCellClick(Cell clickedCell)
    {
        
            if (clickedCell == null)
            {
                return;
            }
            
            if (paletteSettings == null)
            {
                return;
            }
            
            if (paletteSettings.selectedMaterial == null)
            {
                return;
            }
            
            if (_lastSelectedCell == clickedCell)
            {
                return;
            }
            
            if (_lastSelectedCell != null)
            {
                _lastSelectedCell.ResetSelect();
            }
            
            clickedCell.SetSelect(paletteSettings.selectedMaterial, Cell.SelectionType.Selected);
            
            _lastSelectedCell = clickedCell;
            
            OnCellClicked?.Invoke(clickedCell);
        
    }
    
    public Cell GetCellAtPosition(Vector2Int position)
    {
        return _cellGridMultiple.TryGetValue(position, out var cellsAtPosition) ? cellsAtPosition.FirstOrDefault() : null;
    }
    
    private void OnDestroy()
    {
        if (_cells == null) return;
        foreach (var cell in _cells.Where(cell => cell != null))
        {
            cell.OnPointerClickEvent -= HandleCellClick;
        }
    }

    public void TestSelection()
    {
        if (_cells.Count == 0 || paletteSettings == null)
        {
            return;
        }
    }
}
