using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CellPaletteSettings", menuName = "Game/Cell Palette Settings")]
public class CellPaletteSettings : ScriptableObject
{
    [Header("Selection Materials")]
    public Material selectedMaterial;
    public Material moveMaterial;
    public Material attackMaterial;
    public Material moveAndAttackMaterial;
    
    [Header("Visual Settings")]
    public Color player1Color = Color.blue;
    public Color player2Color = Color.red;
    public float selectionAlpha = 0.5f;
}
