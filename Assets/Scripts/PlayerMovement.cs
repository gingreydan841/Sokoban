using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Tilemap walls;
    public Tilemap obstacles;
    public Tilemap targets;
    public LayerMask crateLayer;
    public Transform crate1;
    public Transform crate2;
    public bool hasSecondCrate;
    public GameObject winPanel;

    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        anim.SetBool("IsWalking", input != Vector2.zero);
        Vector2 direction = Vector2.zero;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            direction = new Vector2(input.x, 0);
        }
        else if (input.y != 0)
        {
            direction = new Vector2(0, input.y);
        }

        if (direction != Vector2.zero)
        {
            Move(direction);
        }
    }

    void Move(Vector2 direction)
    {
        Vector3 targetPosition = transform.position + (Vector3)direction;

        Vector3Int targetCell = walls.WorldToCell(targetPosition);

        if (walls.HasTile(targetCell) || obstacles.HasTile(targetCell))
        {
            return;
        }

        Collider2D crate = Physics2D.OverlapPoint(targetPosition, crateLayer);

        if (crate != null)
        {
            Vector3 crateTargetPosition = crate.transform.position + (Vector3)direction;

            Vector3Int crateTargetCell = walls.WorldToCell(crateTargetPosition);

            if (walls.HasTile(crateTargetCell) || obstacles.HasTile(crateTargetCell) || Physics2D.OverlapPoint(crateTargetPosition, crateLayer) != null)
            {
                return;
            }

            crate.transform.position = crateTargetPosition;
            CheckWinCondition();
        }

        transform.position = targetPosition;
    }

    void CheckWinCondition()
    {
        for (int i = 0; i < 2; i++)
        {
            if (i == 1 && !hasSecondCrate)
            {
                break;
            }

            Transform crate;
            if (i == 0)
            {
                crate = crate1;
            }
            else
            {
                crate = crate2;
            }

            Vector3Int crateCell = targets.WorldToCell(crate.position);

            if (!targets.HasTile(crateCell))
            {
                return;
            }
        }

        winPanel.SetActive(true);
    }

    public void OnRestart(InputValue value)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnBacktoMain(InputValue value)
    {
        SceneManager.LoadScene("MainMenu");
    }
}