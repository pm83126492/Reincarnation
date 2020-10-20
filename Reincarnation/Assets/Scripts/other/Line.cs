using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public EdgeCollider2D edgeCollider;
    public List<Vector2> fingerPositions;
    //public TrailRenderer trailRenderer;
    public GameObject drawPrefab;
    GameObject theTrail;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            theTrail = (GameObject)Instantiate(drawPrefab, transform.position, Quaternion.identity,transform);
        }

        if (Input.GetMouseButton(0))
        {
            //trailRenderer.enabled = true;
            Plane objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
                this.transform.position = mRay.GetPoint(rayDistance);
            UpdateLine((Vector2)transform.position);
            edgeCollider.points = fingerPositions.ToArray();
        }

        if (Input.GetMouseButtonUp(0))
        {
            theTrail.transform.parent = null;
            edgeCollider.enabled = false;
        }
    }

    void UpdateLine(Vector2 newFingerPos)
    {
        fingerPositions.Add(newFingerPos);
        edgeCollider.points = fingerPositions.ToArray();
    }
}
