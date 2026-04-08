using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : SoundObgect
{
    public Material UnPressed;
    public Material Pressed;

    MeshRenderer MeshRenderer;

    private void Awake()
    {
        base.Awake();

        MeshRenderer = GetComponent<MeshRenderer>();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Charicter>() != null)
        {
            base.OnTriggerEnter(other);

            MeshRenderer.material = Pressed;
        }
        
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Charicter>() != null)
        {
            MeshRenderer.material = UnPressed;
        }
        
    }
}
