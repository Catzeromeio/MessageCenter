using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo2 : MonoBehaviour
{
    public GameObject TheDemo;

    MessageComponent TheMessageComponent;

    void Start()
    {
        TheMessageComponent = gameObject.AddComponent<MessageComponent>();
        TheMessageComponent.Monitor(TheDemo, "Update", UpdateHandler);
    }

    void UpdateHandler(object data)
    {
        var mesg = (string)data;
        Debug.Log(mesg);
    }
}
