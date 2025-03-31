using UnityEngine;

[CreateAssetMenu(menuName = "StaticData/Spells/New Spell Data", fileName = "New Spell Data", order = 54)]
public class SpellData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public MagicElementsPattern Pattern { get; private set; }
}
