using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHelloCountChanged))] 
    int helloCount = 0;
    void HandleMovement()
    {
        if (isLocalPlayer)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal * 0.1f, moveVertical * 0.1f, 0f);
            transform.position = transform.position + movement;
        }
    }

    void Update()
    {
        HandleMovement();

        if (isLocalPlayer && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Sending to the server...");
            Hello();
        }

        if (isServer && transform.position.y > 50)
        {
            TooHigh();
        }
    }

    [Command] void Hello()
    {
        Debug.Log("Received hello from client");
        helloCount += 1;
        ReplyHello();
    }

    [TargetRpc] void ReplyHello()
    {
        Debug.Log("Received hello from server");
    }

    [ClientRpc] void TooHigh()
    {
        Debug.Log("Too high");
    }

    void OnHelloCountChanged(int oldCount, int newCount)
    {
        Debug.Log($"We had {oldCount}, but now we have {newCount}");
    }
}
