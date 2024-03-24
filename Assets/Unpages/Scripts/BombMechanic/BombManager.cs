using Cysharp.Threading.Tasks;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

public class BombManager : MonoBehaviour
{

    [SerializeField] private NetworkObject bombPrefab;
    private bool _isInActivationDelay;



    [Header("Player Bomb Attributes")]
    public int bombSize = 1;
    public int bombCounter = 2;

    [Button]
    public void DropBomb(GameObject playerPreb, NetworkRunner Runner, PlayerRef playerRef)
    {

        if (!_isInActivationDelay && bombCounter > 0)
        {
            Runner.Spawn(bombPrefab, new Vector3(Mathf.RoundToInt(playerPreb.transform.position.x),
            bombPrefab.transform.position.y, Mathf.RoundToInt(playerPreb.transform.position.z)),
            bombPrefab.transform.rotation, playerRef);
            bombCounter--;
            //Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(playerPreb.transform.position.x),
            //bombPrefab.transform.position.y, Mathf.RoundToInt(playerPreb.transform.position.z)),
            //bombPrefab.transform.rotation);
        }
        StartActivationDelay();
    }

    public async void StartActivationDelay()
    {
        _isInActivationDelay = true;
        await UniTask.WaitForSeconds(1f, cancellationToken: destroyCancellationToken);
        _isInActivationDelay = false;
    }
}
