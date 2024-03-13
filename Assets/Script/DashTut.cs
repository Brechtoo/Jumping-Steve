using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTut : MonoBehaviour
{
    public void Setup()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
