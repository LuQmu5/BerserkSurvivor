public enum SpellNames
{
    None,
    Fireball,
    FrostSpike,
    ArcaneLaserOfDeath,
    DarkMatterBall
}

public enum SpellViewType
{
    None,
    Fireball,
    ArcaneLaserOfDeath,
    FrostSpike,
    DarkMatterBall
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
            SpellNames.Fireball => new FireballSpell(data, _viewFactory, _stats, SpellViewType.Fireball),
            SpellNames.FrostSpike => new FrostSpikeSpell(data, _viewFactory, _stats, SpellViewType.FrostSpike),
            SpellNames.ArcaneLaserOfDeath => new ArcaneLaserOfDeathSpell(data, _viewFactory, _stats, SpellViewType.ArcaneLaserOfDeath),
            SpellNames.DarkMatterBall => new FireballSpell(data, _viewFactory, _stats, SpellViewType.DarkMatterBall),

            _ => throw new System.NotImplementedException($"No spell class registered for {data.Name}")
        };
    }
}
