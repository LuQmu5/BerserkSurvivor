using UnityEngine;

[CreateAssetMenu(menuName = "StaticData/Items/New Item Data", fileName = "New Item Data", order = 54)]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Item Prefab { get; private set; }
}
