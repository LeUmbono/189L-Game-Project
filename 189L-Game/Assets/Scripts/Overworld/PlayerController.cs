using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private LayerMask collisionLayer;
        private Transform movePoint;
        private bool canMove = true;
        private const float tileDistance = 1.0f;

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

            if (canMove)
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
            if (Input.GetKeyDown(KeyCode.A) && NotCollisionTile(-tileDistance, 0.0f))
            {
                this.movePoint.position += new Vector3(-tileDistance, 0.0f, 0.0f);
                canMove = false;
            }
            else if (Input.GetKeyDown(KeyCode.D) && NotCollisionTile(tileDistance, 0.0f))
            {
                this.movePoint.position += new Vector3(tileDistance, 0.0f, 0.0f);
                canMove = false;
            }
            else if (Input.GetKeyDown(KeyCode.W) && NotCollisionTile(0.0f, tileDistance))
            {
                this.movePoint.position += new Vector3(0.0f, tileDistance, 0.0f);
                canMove = false;
            }
            else if (Input.GetKeyDown(KeyCode.S) && NotCollisionTile(0.0f, -tileDistance))
            {
                this.movePoint.position += new Vector3(0.0f, -tileDistance, 0.0f);
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
            if (Physics2D.OverlapBox(this.movePoint.position + new Vector3(displacementX, displacementY, 0.0f), new Vector2(tileDistance - 0.1f, tileDistance - 0.1f), 0.0f, collisionLayer))
            {
                return false;
            }
            return true;
        }
    }
}

