using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class LaserBeam : MonoBehaviour
{
    public float noise = 1.0f;
    public float maxLength = 50.0f;


    LineRenderer lineRenderer;
    int length;
    Vector3[] position;
    //Cache any transforms here
    Transform myTransform;
    //The particle system, in this case sparks which will be created by the Laser
    private ParticleSystem endEffect;
    Vector3 offset;


    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        myTransform = transform;
        offset = new Vector3(0, 0, 0);
        endEffect = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        RenderLaser();
    }

    void RenderLaser()
    {

        //Shoot our laserbeam forwards!
        UpdateLength();
        
        //Move through the Array
        for (int i = 0; i < length; i++)
        {
            //Set the position here to the current location and project it in the forward direction of the object it is attached to
            //offset.x = myTransform.position.x + i * myTransform.forward.x + Random.Range(-noise, noise);
            offset.y = myTransform.position.y + i * myTransform.up.y + Random.Range(-noise, noise);
            //offset.z = i * myTransform.forward.z + Random.Range(-noise, noise) + myTransform.position.z;
            position[i] = offset;
            position[0] = Vector3.zero;

            lineRenderer.SetPosition(i, position[i]);

        }



    }

    void UpdateLength()
    {
        //Raycast from the location of the cube upwards
        RaycastHit[] hit;
        hit = Physics.RaycastAll(myTransform.position, myTransform.up, maxLength);
        int i = 0;

        //while (i < hit.Length)
        //{
        //    //Check to make sure we aren't hitting triggers but colliders
        //    if (!hit[i].collider.isTrigger)
        //    {
        //        length = (int)Mathf.Round(hit[i].distance) + 2;
        //        position = new Vector3[length];
        //        //Move our End Effect particle system to the hit point and start playing it
        //        if (endEffect)
        //        {
        //            endEffectTransform.position = hit[i].point;
        //            if (!endEffect.isPlaying)
        //                endEffect.Play();
        //        }
        //        lineRenderer.SetVertexCount(length);
        //        return;
        //    }
        //    i++;
        //}

        //If we're not hitting anything, don't play the particle effects
        if (endEffect)
        {
            if (endEffect.isPlaying)
                endEffect.Stop();
        }
        length = (int)maxLength;
        position = new Vector3[length];
        lineRenderer.SetVertexCount(length);


    }
}