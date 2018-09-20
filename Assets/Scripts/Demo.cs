using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    MessageComponent TheMessageComponent;
    void Start()
    {
        TheMessageComponent = gameObject.AddComponent<MessageComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        TheMessageComponent.TriggerSelf("Update", "this is test");
    }
}
