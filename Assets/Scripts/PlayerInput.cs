using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour {

    Player player;

    void Start ()
    {
        player = GetComponent<Player>();
    }

    void Update ()
    {
        // If input is locked (UI popup open), freeze controls
        if (player.inputLocked)
        {
            player.SetDirectionalInput(Vector2.zero);
            return;
        }

        // Normal movement
        Vector2 directionalInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
        player.SetDirectionalInput(directionalInput);

        // Jump controls
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.OnJumpInputDown();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.OnJumpInputUp();
        }
    }
}
