using UnityEngine;
using Zenject;

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private Transform _levelSpawnPoint;
    [SerializeField] private MainCameraController _mainCameraController;
    [SerializeField] private SpellBookView _spellBookView;
    [SerializeField] private StatsData _characterStatsData;

    private const string CharactersPath = "Prefabs/Characters";

    [Inject]
    public void Construct(ProjectileFactory projectileFactory)
    {
        int mageCharacterIndex = 0;
        
        CharacterBehaviour[] characters = Resources.LoadAll<CharacterBehaviour>(CharactersPath);

        PlayerInput input = new PlayerInput();

        CharacterBehaviour mage = Instantiate(characters[mageCharacterIndex], _levelSpawnPoint.position, Quaternion.identity);
        mage.Init(input, _spellBookView, _characterStatsData, projectileFactory);
        _mainCameraController.Init(mage.transform);
    }
}
