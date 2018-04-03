using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FractalTreeProper : MonoBehaviour {
	public Vector3 treeOrigin;
	public float trunkLength;
	public float trunkBaseWidth;
	public int numLayers;
	public float branchAngle;

	public float widthDecreaseFactor;
	public float lengthDecreaseFactor;

	List<Branch> branches;
	bool isDebug=false;
	Mesh fTree;
	List<int> faces;
	List<Vector3> vertices;
	
	// Use this for initialization
	void Start () {
		branches=new List<Branch>();
		faces=new List<int>();
		vertices=new List<Vector3>();
		fTree=GetComponent<MeshFilter>().mesh;
		fTree.name="fractal tree";
		CreateTree();
	}
	private void CreateTree(){
		
		CreateBranch(0,treeOrigin,0,vertices.Count);
		
		fTree.vertices=vertices.ToArray();
		fTree.triangles = faces.ToArray();
		//Debug.Log(vertices.Count);
		fTree.RecalculateNormals();
		Debug.Log("Tree has "+vertices.Count.ToString()+" vertices");
	}

    private void CreateBranch(int currentLayer,Vector3 branchOffset, float angle, int baseVertexPointer)
    {
        if(currentLayer>=numLayers)return;
		
		float length=trunkLength;
		float width=trunkBaseWidth;
		for(int i=0;i<currentLayer;i++){
			length*=lengthDecreaseFactor;
			width*=widthDecreaseFactor;
		}
		
		Branch branch=new Branch(Vector3.zero,length,width,angle, branchOffset,widthDecreaseFactor);
		branches.Add(branch);
		Vector3 tipMidPoint=new Vector3();
		tipMidPoint.x=(branch.vertices[1].x+branch.vertices[2].x)*0.5f;
		tipMidPoint.y=(branch.vertices[1].y+branch.vertices[2].y)*0.5f;
		if(currentLayer==0){
			vertices.AddRange(branch.vertices);
			faces.Add(baseVertexPointer);
			faces.Add(baseVertexPointer+1);
			faces.Add(baseVertexPointer+3);
			faces.Add(baseVertexPointer+3);
			faces.Add(baseVertexPointer+1);
			faces.Add(baseVertexPointer+2);
		}else{
			int vertexPointer=vertices.Count;
			vertices.Add(branch.vertices[1]);
			vertices.Add(branch.vertices[2]);
			int indexDelta=3;
			if(currentLayer!=1){
				indexDelta=2;
			}
			faces.Add(baseVertexPointer-indexDelta);
			faces.Add(vertexPointer);
			faces.Add(baseVertexPointer-(indexDelta-1));
			faces.Add(baseVertexPointer-(indexDelta-1));
			faces.Add(vertexPointer);
			faces.Add(vertexPointer+1);
			
		}
		baseVertexPointer=vertices.Count;
		CreateBranch(currentLayer+1,tipMidPoint,angle+branchAngle,baseVertexPointer);
		CreateBranch(currentLayer+1,tipMidPoint,angle-branchAngle,baseVertexPointer);
    }

	private void OnDrawGizmos () {
		if(!isDebug)return;
		if (branches == null) {
			return;
		}
		Gizmos.color = Color.green;
		foreach(Branch br in branches){
			for (int i = 0; i < br.vertices.Count; i++) {
				Gizmos.DrawSphere(br.vertices[i], 0.1f);
			}
		}
		
	}
}
