using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   [SerializeField] private float moveSpeed = 5.0f;
   [SerializeField] private LayerMask collisionLayer;
   private Transform movePoint;
   private bool canMove = true;

    private void Awake()
    {
        movePoint = this.transform.Find("MovePoint");
        if (movePoint == null)
        {
            Debug.LogError("MovePoint not found. Please attach a MovePoint to object.");
        }
        movePoint.parent = null;
    }

    private void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.movePoint.position, moveSpeed * Time.deltaTime);

        if(canMove)
        {
            CheckForInput();
        }
        else
        {
            CheckOnTile();
        }
    }

    private void CheckForInput()
    {
        if ((Mathf.Abs(Input.GetAxisRaw("Horizontal")) >= 0.5f) && NotCollisionTile(Input.GetAxisRaw("Horizontal"), 0.0f))
        {
            movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, 0.0f);
            canMove = false;
        }
        else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) >= 0.5f && NotCollisionTile(0.0f, Input.GetAxisRaw("Vertical")))
        {
            movePoint.position += new Vector3(0.0f, Input.GetAxisRaw("Vertical"), 0.0f);
            canMove = false;
        }
    }

    private void CheckOnTile()
    {
        if (Vector3.Distance(this.transform.position, this.movePoint.position) <= 0.05f)
        {
            canMove = true;
        }
    }
    private bool NotCollisionTile(float displacementX, float displacementY)
    {
        if (Physics2D.OverlapCircle(this.movePoint.position + new Vector3(displacementX, displacementY, 0.0f), 0.5f, collisionLayer))
        {
            return false;
        }
        return true;
    }
}
