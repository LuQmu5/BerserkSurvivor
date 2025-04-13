using UnityEngine;

public abstract class Spell
{
    public SpellData Data { get; private set; }

    protected CharacterStats CasterStats { get; }

    protected SpellsViewFactory SpellsViewVfactory;

    private SpellViewType _spellViewType;

    public Spell(SpellData data, SpellsViewFactory projectileFactory, CharacterStats casterStats, SpellViewType spellViewType)
    {
        Data = data;
        SpellsViewVfactory = projectileFactory;
        CasterStats = casterStats;
        _spellViewType = spellViewType;
    }

    public virtual void Use(ICaster caster)
    {
        SpellView spellView = SpellsViewVfactory.GetItem(_spellViewType);
        spellView.Init(caster);
    }
}

public class FireballSpell : Spell
{
    public FireballSpell(SpellData data, SpellsViewFactory projectileFactory, CharacterStats casterStats, SpellViewType spellViewType)
        : base(data, projectileFactory, casterStats, spellViewType)
    {
    }
}

public class ArcaneLaserOfDeathSpell : Spell
{
    public ArcaneLaserOfDeathSpell(SpellData data, SpellsViewFactory projectileFactory, CharacterStats casterStats, SpellViewType spellViewType)
        : base(data, projectileFactory, casterStats, spellViewType)
    {
    }
}

public class FrostSpikeSpell : Spell
{
    public FrostSpikeSpell(SpellData data, SpellsViewFactory projectileFactory, CharacterStats casterStats, SpellViewType spellViewType) : base(data, projectileFactory, casterStats, spellViewType)
    {
    }
}

public class HasteBuff : Spell
{
    public HasteBuff(SpellData data, SpellsViewFactory projectileFactory, CharacterStats casterStats, SpellViewType spellViewType)
        : base(data, projectileFactory, casterStats, spellViewType)
    {
    }
}

public class ArcaneMissileSpell : Spell
{
    public ArcaneMissileSpell(SpellData data, SpellsViewFactory projectileFactory, CharacterStats casterStats, SpellViewType spellViewType)
        : base(data, projectileFactory, casterStats, spellViewType)
    {
    }
}

public class HealSpell : Spell
{
    public HealSpell(SpellData data, SpellsViewFactory projectileFactory, CharacterStats casterStats, SpellViewType spellViewType)
        : base(data, projectileFactory, casterStats, spellViewType)
    {
    }
}

public class TrailOfLifeSpell : Spell
{
    public TrailOfLifeSpell(SpellData data, SpellsViewFactory projectileFactory, CharacterStats casterStats, SpellViewType spellViewType)
        : base(data, projectileFactory, casterStats, spellViewType)
    {
    }
}

public class EarthQuake : Spell
{
    public EarthQuake(SpellData data, SpellsViewFactory projectileFactory, CharacterStats casterStats, SpellViewType spellViewType)
        : base(data, projectileFactory, casterStats, spellViewType)
    {
    }
}
