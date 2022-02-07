using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float dashSpeed;

    private Rigidbody playerRigidBody;

    public void InitializePlayer(GameObject gameObject, CharacterInfo info)
    {
        playerRigidBody = gameObject.GetComponent<Rigidbody>();
    }

    public void PlayerWalk(CharacterInfo info)
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            Debug.Log("right");
        }
        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            Debug.Log("left");
        }

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * walkSpeed;

        playerRigidBody.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    public void PlayerAttack(CharacterInfo info)
    {
        switch (info.id)
        {
            case 1:
                break;

            default:

                break;
        }
    }
}
