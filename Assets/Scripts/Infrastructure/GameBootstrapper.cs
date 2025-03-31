using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private Transform _levelSpawnPoint;
    [SerializeField] private MainCameraController _mainCameraController;

    private const string CharactersPath = "Characters";

    private void Awake()
    {
        CharacterBehaviour[] characters = Resources.LoadAll<CharacterBehaviour>(CharactersPath);
        PlayerInput input = new PlayerInput();
        CharacterStats characterStats = new CharacterStats();

        CharacterBehaviour mage = Instantiate(characters[0], _levelSpawnPoint.position, Quaternion.identity);
        mage.Init(input, characterStats);
        _mainCameraController.Init(mage.transform);
    }
}
