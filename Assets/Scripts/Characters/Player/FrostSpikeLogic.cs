public class FrostSpikeLogic : SpellView
{
    public override void Init(ICaster caster)
    {
        Destroy(gameObject, 3); // object pool
    }
}