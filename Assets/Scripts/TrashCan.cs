using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour {
    public static TrashCan instance;

    public GameObject trashCanRB;

    void Awake ()
    {
        instance = this;
    }

    void Update ()
    {
        transform.position = trashCanRB.transform.position;
        transform.rotation = trashCanRB.transform.rotation;
    }   

}
