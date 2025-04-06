using UnityEngine;

[CreateAssetMenu(menuName = "StaticData/Spells/New Spell Data", fileName = "New Spell Data", order = 54)]
public class SpellData : ScriptableObject
{
    [field: SerializeField] public SpellNames Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public MagicElementsPattern Pattern { get; private set; }
    [field: SerializeField] public AnimationNames CastAnimationName { get; private set; }
}

