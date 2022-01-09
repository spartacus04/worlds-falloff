using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public bool canMove = true;
    public float speed = 10;
    public new Camera camera;
    public float cameraSpeed = 0.3f;

    [HideInInspector]
    public Vector2 mousePos;
    private Vector2 movement;
    [HideInInspector]
    public Rigidbody2D rb;
    public bool cameraFollowTileMode = true;
    public Transform target;
    private static Movement instance;

    public void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
        {
            movement = Vector2.zero;
            return;
        }
        
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        mousePos = camera.ScreenToWorldPoint(Input.mousePosition);

        // Camera movement
        if(cameraFollowTileMode)
        {
            Vector2 cameraPos = camera.transform.position;

            if (rb.position.y - cameraPos.y > 5)
            {
                StartCoroutine(LerpFromTo(cameraPos, cameraPos + new Vector2(0, 10), cameraSpeed));
            }
            else if (rb.position.y - cameraPos.y < -5)
            {
                StartCoroutine(LerpFromTo(cameraPos, cameraPos + new Vector2(0, -10), cameraSpeed));
            }

            if (rb.position.x - cameraPos.x > 9)
            {
                StartCoroutine(LerpFromTo(cameraPos, cameraPos + new Vector2(18, 0), cameraSpeed));
            }
            else if (rb.position.x - cameraPos.x < -9)
            {
                StartCoroutine(LerpFromTo(cameraPos, cameraPos + new Vector2(-18, 0), cameraSpeed));
            }
        }
        else
        {
            Vector2 lerpedPos = Vector2.Lerp(transform.position, target.position, 0.5f);
            camera.transform.position = new Vector3(lerpedPos.x, lerpedPos.y, camera.transform.position.z);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = movement * speed * 200 * Time.fixedDeltaTime;

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    IEnumerator LerpFromTo(Vector2 pos1, Vector2 pos2, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            Vector2 lerped = Vector2.Lerp(pos1, pos2, t / duration);
            camera.transform.position = new Vector3(lerped.x, lerped.y, -10);
            yield return 0;
        }
        camera.transform.position = new Vector3(pos2.x, pos2.y, -10);
    }
}
