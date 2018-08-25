using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDestroyTest : MonoBehaviour
{
    public delegate int GetInstanceIDDel();

    public GameObject TestGO;

    GetInstanceIDDel GetTestInstanceID;

    void Start()
    {
        Debug.Log("the test gomeobject instanceID is " + TestGO.GetInstanceID());
        GetTestInstanceID = TestGO.GetInstanceID;
        Destroy(TestGO);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("UPDATE the test gomeobject instanceID is " + GetTestInstanceID());
    }
}
