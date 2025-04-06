public enum SpellNames
{
    Fireball,
    Frostbolt,
    ArcaneMissile,
    Heal,
    TrailOfLife,
    EarthQuake,
    HasteBuff
}

public class SpellFactory
{
    private readonly SpellsViewFactory _viewFactory;
    private readonly CharacterStats _stats;

    public SpellFactory(SpellsViewFactory viewFactory, CharacterStats stats)
    {
        _viewFactory = viewFactory;
        _stats = stats;
    }

    public Spell CreateSpell(SpellData data)
    {
        return data.Name switch
        {
            SpellNames.Fireball => new FireballSpell(data, _viewFactory, _stats),
            SpellNames.Frostbolt => new FrostboltSpell(data, _viewFactory, _stats),
            SpellNames.ArcaneMissile => new ArcaneMissileSpell(data, _viewFactory, _stats),
            SpellNames.Heal => new HealSpell(data, _viewFactory, _stats),
            SpellNames.TrailOfLife => new TrailOfLifeSpell(data, _viewFactory, _stats),
            SpellNames.EarthQuake => new EarthQuake(data, _viewFactory, _stats),
            SpellNames.HasteBuff => new HasteBuff(data, _viewFactory, _stats),


            _ => throw new System.NotImplementedException($"No spell class registered for {data.Name}")
        };
    }
}
