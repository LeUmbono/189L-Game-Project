using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class PlayerController : OverworldEntity
    {
        // Movement variables.
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private LayerMask collisionLayer;
        private Transform movePoint;
        private bool canMove = true;
        private const float tileDistance = 1.0f;
        private bool isInCombat = false;

        public void DisableInput()
        {
            this.isInCombat = true;
        }
        public void EnableInput()
        {
            this.isInCombat = false;
        }

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

            if (canMove && !isInCombat)
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
            if (Input.GetButton("Left") && NotCollisionTile(-tileDistance, 0.0f))
            {
                this.GetComponent<SpriteRenderer>().flipX = true;
                this.movePoint.position += new Vector3(-tileDistance, 0.0f, 0.0f);
                this.GetComponent<AudioSource>().Play();
                canMove = false;
            }
            else if (Input.GetButton("Right") && NotCollisionTile(tileDistance, 0.0f))
            {
                this.GetComponent<SpriteRenderer>().flipX = false;
                this.movePoint.position += new Vector3(tileDistance, 0.0f, 0.0f);
                this.GetComponent<AudioSource>().Play();
                canMove = false;
            }
            else if (Input.GetButton("Up") && NotCollisionTile(0.0f, tileDistance))
            {
                this.movePoint.position += new Vector3(0.0f, tileDistance, 0.0f);
                this.GetComponent<AudioSource>().Play();
                canMove = false;
            }
            else if (Input.GetButton("Down") && NotCollisionTile(0.0f, -tileDistance))
            {
                this.movePoint.position += new Vector3(0.0f, -tileDistance, 0.0f);
                this.GetComponent<AudioSource>().Play();
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
            if (Physics2D.Raycast(this.transform.position, new Vector2(displacementX, displacementY).normalized, tileDistance, collisionLayer))
            {
                return false;
            }
            return true;
        }
    }
}
