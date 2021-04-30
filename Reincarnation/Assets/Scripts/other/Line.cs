using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public EdgeCollider2D edgeCollider;
    public List<Vector2> fingerPositions;
    //public TrailRenderer trailRenderer;
    public GameObject drawPrefab;
    public Transform LinePrefab;
    GameObject theTrail;

    public List<GameObject> TrailList;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fingerPositions.Clear();
            LineCollider.ColliderNumber = 0;
            theTrail = (GameObject)Instantiate(drawPrefab, transform.position, Quaternion.identity,transform);
            TrailList.Add(theTrail);
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
        }

        if (Input.GetMouseButtonUp(0))
        {
            //LineCollider.ColliderNumber = 0;
            theTrail.transform.parent = LinePrefab;
            fingerPositions.Clear();
            for (int i = 0; i < TrailList.Count; i++)
            {
                Destroy(TrailList[i]);
                if (i == TrailList.Count - 1)
                {
                    TrailList.Clear();
                }
            }
            //TrailList.Add(theTrail);
        }
    }

    void UpdateLine(Vector2 newFingerPos)
    {
        fingerPositions.Add(newFingerPos);
        edgeCollider.points = fingerPositions.ToArray();
    }
}
