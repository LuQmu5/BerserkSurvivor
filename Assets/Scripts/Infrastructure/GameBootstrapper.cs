using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private Transform _levelSpawnPoint;
    [SerializeField] private MainCameraController _mainCameraController;
    [SerializeField] private SpellBookView _spellBookView;

    private const string CharactersPath = "Characters";
    private const string CharactersStatsPath = "StaticData/Stats";

    private void Awake()
    {
        int mageCharacterIndex = 0;
        
        CharacterBehaviour[] characters = Resources.LoadAll<CharacterBehaviour>(CharactersPath);
        StatsData[] statsData = Resources.LoadAll<StatsData>(CharactersStatsPath);

        PlayerInput input = new PlayerInput();
        CharacterStats characterStats = new CharacterStats(statsData[mageCharacterIndex]);

        CharacterBehaviour mage = Instantiate(characters[mageCharacterIndex], _levelSpawnPoint.position, Quaternion.identity);
        mage.Init(input, characterStats, _spellBookView);
        _mainCameraController.Init(mage.transform);
    }
}
