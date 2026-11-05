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
        anim = GetComponentInChildren<Animator>(); //Animator on the Pig 
    }

    void OnMove(InputValue value) //Runs when the player presses a WASD key
    {
        Vector2 input = value.Get<Vector2>(); //Gets the movement direction
        anim.SetBool("IsWalking", input != Vector2.zero); //Plays Walk when moving and Idle when not moving
        Vector2 direction = Vector2.zero; //Starts with no movement direction

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) //Checks whether horizontal input is >
        {
            direction = new Vector2(Mathf.Sign(input.x), 0); //Sets the direction to left or right
        }
        else if (input.y != 0) //Checks whether vertical input was pressed
        {
            direction = new Vector2(0, Mathf.Sign(input.y)); //Sets the direction to up or down
        }

        if (direction != Vector2.zero) //Checks whether a direction was pressed
        {
            Move(direction); //Moves the player one tile
        }
    }

    void Move(Vector2 direction) //Moves the player one tile
    {
        Vector3 targetPosition = transform.position + (Vector3)direction; //Calculates the next position

        Vector3Int targetCell = walls.WorldToCell(targetPosition); //Converts the position into a tilemap cell

        if (walls.HasTile(targetCell) || obstacles.HasTile(targetCell)) //Checks for walls or water
        {
            return; //Stops movement if the next tile is blocked
        }

        Collider2D crate = Physics2D.OverlapPoint(targetPosition, crateLayer); //Checks whether a crate is in the next tile

        if (crate != null) //Checks whether the player found a crate
        {
            Vector3 crateTargetPosition = crate.transform.position + (Vector3)direction; //Calculates the crate's next position

            Vector3Int crateTargetCell = walls.WorldToCell(crateTargetPosition); //Converts the crate position into a tilemap cell

            if (walls.HasTile(crateTargetCell) || obstacles.HasTile(crateTargetCell) || Physics2D.OverlapPoint(crateTargetPosition, crateLayer) != null) //Checks whether the crate is blocked
            {
                return; //Stops if the crate cannot move
            }

            crate.transform.position = crateTargetPosition; //Moves the crate one tile
            CheckWinCondition(); //Checks whether all crates are on targets
        }

        transform.position = targetPosition; //Moves the player one tile
    }

    void CheckWinCondition() //Checks if all crates are on targets
    {
        for (int i = 0; i < 2; i++) //Loop through first and second crate
        {
            if (i == 1 && !hasSecondCrate) //does this level only has one crate?
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

            Vector3Int crateCell = targets.WorldToCell(crate.position); //Converts the crate position into a tilemap cell

            if (!targets.HasTile(crateCell)) //Checks whether the crate is not on a target
            {
                return; //Stops because the level is not complete
            }
        }

        winPanel.SetActive(true); 
    }

    public void OnRestart(InputValue value) //Runs when the player presses R
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void OnBacktoMain(InputValue value) //Runs when the player presses M
    {
        SceneManager.LoadScene("MainMenu"); 
    }
}

