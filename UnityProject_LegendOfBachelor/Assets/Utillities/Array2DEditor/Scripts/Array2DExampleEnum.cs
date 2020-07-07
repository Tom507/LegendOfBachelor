using UnityEngine;

namespace Array2DEditor
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Array2D_ExampleEnum", menuName = "Array2D/ExampleEnum")]
    public class Array2DExampleEnum : Array2D<Enums.ExampleEnum>
    {
        [SerializeField]
        CellRowGameObjekt[] cells = new CellRowGameObjekt[Consts.defaultGridSize];

        protected override CellRow<Enums.ExampleEnum> GetCellRow(int idx)
        {
            return cells[idx];
        }
    }

    [System.Serializable]
    public class CellRowGameObjekt : CellRow<Enums.ExampleEnum> { }
}