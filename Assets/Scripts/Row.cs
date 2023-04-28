using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    [HideInInspector] public List<Cell> cellsHavingCar = new();
    public Cell[] rowCells;
    public int rowNumber;
    public GameObject particalParent;


    public bool IsCarColorSameInRow()
    {
        foreach (Cell cell in rowCells) { if (cell.isOccupide) { cellsHavingCar.Add(cell); } }
        if (cellsHavingCar.Count == 0) { return true; }
        Color firstColor = cellsHavingCar[0].puzzleCar.carColor;
        foreach (Cell cell in cellsHavingCar) { if (cell.puzzleCar.carColor != firstColor) { cellsHavingCar.Clear(); return false; } }
        cellsHavingCar.Clear();
        //particalParent.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        //particalParent.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        return true;
    }
}
