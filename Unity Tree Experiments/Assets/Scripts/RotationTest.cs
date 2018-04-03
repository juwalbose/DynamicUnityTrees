using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour {

	List<Vector3> vertices=new List<Vector3>();
	public float widthDecreaseFactor=0.8f;
	public float length;
	public float width;
	public float angle;
	public Vector3 originOffset;

	// Use this for initialization
	void Start () {
		Branch branch=new Branch(new Vector3(0,0,0),length,width,angle, originOffset,widthDecreaseFactor);
		vertices=branch.vertices;
	}


    //to display vertices
    private void OnDrawGizmos () {
		if (vertices == null) {
			return;
		}
		Gizmos.color = Color.black;
		for (int i = 0; i < vertices.Count; i++) {
			Gizmos.DrawSphere(vertices[i], 0.1f);
		}
	}
	
}
