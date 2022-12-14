using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    public Rigidbody rb;
    public float impulseForce = 5f;

    private Vector3 startPos;
    public int perfectPass = 0;
    private bool ignoreNextCollision;
    public bool isSuperSpeedActive;

    private void Awake()
    {
        startPos = transform.position;
    }

    private void Start()
    {
        GameManager.Instance.ResetGame += ResetBall;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (ignoreNextCollision)
            return;

        if (isSuperSpeedActive)
        {
            if (other.transform.TryGetComponent(out Goal goal))
            {
                Destroy(other.transform.parent.gameObject);
            }

        }
        else
        {
            DeathPart deathPart = other.transform.GetComponent<DeathPart>();
            if (deathPart != null)
                deathPart.HittedDeathPart();
        }

        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);
        print("jump");

        ignoreNextCollision = true;
        StartCoroutine(AllowCollision());

        perfectPass = 0;
        isSuperSpeedActive = false;
    }

    private void Update()
    {
        if (perfectPass >= 3 && isSuperSpeedActive == false)
        {
            isSuperSpeedActive = true;
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }
    }

    public void ResetBall()
    {
        transform.position = startPos;
    }

    private IEnumerator AllowCollision()
    {
        yield return new WaitForSeconds(0.2f);
        ignoreNextCollision = false;
    }


}
