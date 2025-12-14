using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SimpleTopDownAnimator : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sr;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        bool isMoving = (Mathf.Abs(x) > 0.01f || Mathf.Abs(y) > 0.01f);
        anim.SetBool("IsMoving", isMoving);

        // Flip sprite left/right
        if (Mathf.Abs(x) > 0.01f)
        {
            sr.flipX = x < 0f;
        }
    }
}
