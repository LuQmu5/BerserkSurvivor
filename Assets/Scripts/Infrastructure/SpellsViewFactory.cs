
using System.Collections.Generic;
using UnityEngine;

public class SpellsViewFactory : MonoBehaviourFactory<SpellView>
{
    public SpellsViewFactory(SpellView[] projectilesPrefabsList) : base(projectilesPrefabsList)
    {
    }
}

public enum SpellViewType
{
    Fireball,
}
