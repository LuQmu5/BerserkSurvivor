using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private Transform _levelSpawnPoint;
    [SerializeField] private MainCameraController _mainCameraController;
    [SerializeField] private SpellBookView _spellBookView;

    private const string CharactersPath = "Characters";

    private void Awake()
    {
        CharacterBehaviour[] characters = Resources.LoadAll<CharacterBehaviour>(CharactersPath);
        PlayerInput input = new PlayerInput();
        CharacterStats characterStats = new CharacterStats();

        CharacterBehaviour mage = Instantiate(characters[0], _levelSpawnPoint.position, Quaternion.identity);
        mage.Init(input, characterStats, _spellBookView);
        _mainCameraController.Init(mage.transform);
    }
}
