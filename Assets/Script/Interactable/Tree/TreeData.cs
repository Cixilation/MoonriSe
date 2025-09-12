using UnityEngine;

public class TreeData : MonoBehaviour
{
    private static readonly int Chop = Animator.StringToHash("Chop");
    public GameObject treeStandingPrefab;
    public GameObject treeFallenPrefab;

    private float _health = 3f;
    private float totalDamage = 1f;

    private GameObject currentTreeInstance;

    private enum TreeState { Standing, Fallen, Destroyed }
    private TreeState currentState = TreeState.Standing;
    
    void Start()
    {
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        currentTreeInstance = Instantiate(treeStandingPrefab, transform.position + new Vector3(0, 0, 0), randomRotation, transform);
    }

    internal void ApplyDamage()
    {
        _health -= totalDamage;
        if (_health <= 0)
        {
            _health = 0;
            ChangeStateToFallen();
            DropLoot();
        }
    }
    private void ChangeStateToFallen()
    {
        if (currentState == TreeState.Standing)
        {
            Destroy(currentTreeInstance);
            currentTreeInstance = Instantiate(treeFallenPrefab, transform.position + new Vector3(0, 0, 0), transform.rotation); 
            currentState = TreeState.Fallen;
            Invoke("DestroyTree", 3f);
            Destroy(this.gameObject, 6f);   
        }
    }
    private void DestroyTree()
    {
        if (currentState == TreeState.Fallen)
        {
            Destroy(currentTreeInstance); 
            currentState = TreeState.Destroyed;
        }
    }

    private void DropLoot()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject)
        {
            Player _player;
            _player = playerObject.GetComponent<Player>();
            _player.PlayerAnimator.SetBool(Chop, false);
            InstantiateDrops.instance.InstantiateDrop(this.transform.position, "wood", 0);
        }
    }
}
