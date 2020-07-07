using UnityEngine;

namespace Array2DEditor
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Array2DGameObjects", menuName = "Array2D/ExampleEnum")]
    public class Array2DGameObjects : Array2D<Enums.ExampleEnum>
    {
        [SerializeField]
        CellRowExampleEnum[] cells = new CellRowExampleEnum[Consts.defaultGridSize];

        protected override CellRow<Enums.ExampleEnum> GetCellRow(int idx)
        {
            return cells[idx];
        }
    }

    [System.Serializable]
    public class CellRowExampleEnum : CellRow<Enums.ExampleEnum> { }
}