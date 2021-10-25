using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private Camera cam;

    public float sizeNormal;
    public float sizeZoomedIn;
    public float zoomProgress = 0f;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.health <= 0)
            animator.SetBool("Dead", true);
        else
            animator.SetBool("Dead", false);
        cam.orthographicSize = (zoomProgress * sizeZoomedIn) + (1f - zoomProgress) * sizeNormal;
    }
}
