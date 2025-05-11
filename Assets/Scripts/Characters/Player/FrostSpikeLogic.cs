public class FrostSpikeLogic : SpellView
{
    public override void Init(ICaster caster, CharacterStats stats  )
    {
        Destroy(gameObject, 3); // object pool
    }
}