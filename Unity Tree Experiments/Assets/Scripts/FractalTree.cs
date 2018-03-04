using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FractalTree : MonoBehaviour {

	float lineWidth=1;
	float rootThree=Mathf.Sqrt(3);

	Mesh fTree;
	int zVal=0;
	float widthAdjust=0.6f;
	List<Vector3> vertices;
	int layerCount=0;

	// Use this for initialization
	void Start () {
		fTree=GetComponent<MeshFilter>().mesh;
		fTree.name="fractal tree";
		vertices=new List<Vector3>();
		List<int> faces=new List<int>();
		float lineLength=4;
		Vector2 startPos=new Vector2(0,0);
		CreateTree (vertices,faces,startPos,lineLength);
		fTree.vertices=vertices.ToArray();
		fTree.triangles = faces.ToArray();
	}

    private void CreateTree(List<Vector3> vertices, List<int> faces,Vector2 startPos, float lineLength)
    {
        int vertexPointer=vertices.Count;
		float tipWidth=lineWidth*widthAdjust;
		float tipHeight=rootThree*tipWidth/2;
		vertices.Add(new Vector3(startPos.x-lineWidth/2,startPos.y,zVal));
		vertices.Add(new Vector3(startPos.x-tipWidth/2,startPos.y+lineLength,zVal));
		vertices.Add(new Vector3(startPos.x,startPos.y+lineLength+tipHeight,zVal));
		vertices.Add(new Vector3(startPos.x+tipWidth/2,startPos.y+lineLength,zVal));
		vertices.Add(new Vector3(startPos.x+lineWidth/2,startPos.y,zVal));
		
		faces.Add(vertexPointer);
		faces.Add(vertexPointer+1);
		faces.Add(vertexPointer+4);
		faces.Add(vertexPointer+4);
		faces.Add(vertexPointer+1);
		faces.Add(vertexPointer+3);
		faces.Add(vertexPointer+1);
		faces.Add(vertexPointer+2);
		faces.Add(vertexPointer+3);

		startPos.y+=lineLength;
		startPos.x+=lineWidth/2;
		bool isLeft=false;
		float angle=Mathf.PI/4;
		int vertexId=vertices.Count;
		int startVertexId=vertexId-3;
		int endVertexId=vertexId-2;
		CreateRotatedBranch (vertices,faces,startPos,lineLength,angle,isLeft, startVertexId, endVertexId);
		startPos.x-=lineWidth;
		isLeft=true;
		angle=-Mathf.PI/4;
		startVertexId=vertexId-4;
		endVertexId=vertexId-3;
		CreateRotatedBranch (vertices,faces,startPos,lineLength,angle, isLeft, startVertexId, endVertexId);
    }
	private void CreateRotatedBranch(List<Vector3> vertices, List<int> faces,Vector2 startPos, float lineLength, float angle, bool isLeft, int startVertexId, int endVertexId)
    {
        int vertexPointer=vertices.Count;
		Vector3 startVector=vertices[startVertexId];
		Vector3 endVector=vertices[endVertexId];
		
		float tipWidth=lineWidth*widthAdjust;
		float tipHeight=rootThree*tipWidth/2;
		vertices.Add(new Vector3(startPos.x-tipWidth/2,startPos.y+lineLength,zVal));
		vertices.Add(new Vector3(startPos.x,startPos.y+lineLength+tipHeight,zVal));
		vertices.Add(new Vector3(startPos.x+tipWidth/2,startPos.y+lineLength,zVal));
		
		faces.Add(startVertexId);
		faces.Add(vertexPointer);
		faces.Add(endVertexId);
		faces.Add(endVertexId);
		faces.Add(vertexPointer);
		faces.Add(vertexPointer+2);
		faces.Add(vertexPointer);
		faces.Add(vertexPointer+1);
		faces.Add(vertexPointer+2);
/* 
		layerCount++;
		if(layerCount<20){
			startPos.y+=lineLength;
			startPos.x+=lineWidth/2;
			isLeft=false;
			angle=Mathf.PI/4;
			vertexId=vertices.Count;
			CreateRotatedBranch (vertices,faces,startPos,lineLength,angle,isLeft, vertexId);
			startPos.x-=lineWidth;
			isLeft=true;
			angle=-Mathf.PI/4;
			CreateRotatedBranch (vertices,faces,startPos,lineLength,angle, isLeft,vertexId);
		}
		*/
    }


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