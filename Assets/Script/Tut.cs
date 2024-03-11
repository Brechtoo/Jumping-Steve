using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Tut : MonoBehaviour
{
    public bool active = false;
    public void Setup()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive (false);
    }

}
