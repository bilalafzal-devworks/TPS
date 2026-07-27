using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class height : MonoBehaviour
{
    void Start()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.Log("No Renderers Found!");
            return;
        }

        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        Debug.Log("Character Height = " + bounds.size.y);
    }
}
