using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class VampireFacePlayer : MonoBehaviour
{
    SpriteRenderer sr;
    Transform player;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("guy");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        // Player is to the LEFT of vampire
        if (player.position.x < transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;
    }
}
