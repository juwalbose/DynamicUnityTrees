using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FractalTree : MonoBehaviour {

	float rootThree=Mathf.Sqrt(3);

	Mesh fTree;
	int zVal=0;
	public float sizeAdjust=0.7f;
	List<Vector3> vertices;
	public int maxLayers=1;
	public float branchAngle=Mathf.PI/4;

	// Use this for initialization
	void Start () {
		fTree=GetComponent<MeshFilter>().mesh;
		fTree.name="fractal tree";
		vertices=new List<Vector3>();
		List<int> faces=new List<int>();
		Vector2 startPos=new Vector2(0,0);
		CreateTree (vertices,faces,startPos);
		fTree.vertices=vertices.ToArray();
		fTree.triangles = faces.ToArray();
		//Debug.Log(vertices.Count);
		fTree.RecalculateNormals();
	}

    private void CreateTree(List<Vector3> vertices, List<int> faces,Vector2 startPos)
    {
        int layerCount=0;
		float lineWidth=1;
		float lineLength=4;
		
		int vertexPointer=vertices.Count;
		float tipWidth=lineWidth*sizeAdjust;
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
		float angle=branchAngle;
		int vertexId=vertices.Count;
		int startVertexId=vertexId-3;
		int endVertexId=vertexId-2;
		CreateRotatedBranch (vertices,faces,startPos,lineLength*sizeAdjust,tipWidth,angle,isLeft, startVertexId, endVertexId, layerCount);
		layerCount=0;
		startPos.x-=lineWidth;
		isLeft=true;
		angle=-branchAngle;
		startVertexId=vertexId-4;
		endVertexId=vertexId-3;
		CreateRotatedBranch (vertices,faces,startPos,lineLength*sizeAdjust,tipWidth,angle, isLeft, startVertexId, endVertexId,layerCount);
    }
	private void CreateRotatedBranch(List<Vector3> vertices, List<int> faces,Vector2 startPos, float lineLength,float lineWidth, float angle, bool isLeft, int startVertexId, int endVertexId, int layerCount)
    {
        int vertexPointer=vertices.Count;
		Vector3 startVector=vertices[startVertexId];
		Vector3 endVector=vertices[endVertexId];
		
		float tipWidth=lineWidth*sizeAdjust;
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

		float newBaseX=startPos.x;
		int vertexId=vertices.Count;
		layerCount++;
		if(layerCount<maxLayers){
			startPos.y+=lineLength;
			startPos.x=newBaseX+lineWidth/2;
			isLeft=false;
			angle=branchAngle;
			startVertexId=vertexId-2;
			endVertexId=vertexId-1;
			
			CreateRotatedBranch (vertices,faces,startPos,lineLength*sizeAdjust,tipWidth,angle,isLeft, startVertexId, endVertexId,layerCount);
			
			startPos.x=newBaseX-lineWidth;
			isLeft=true;
			angle=-branchAngle;
			startVertexId=vertexId-3;
			endVertexId=vertexId-2;
			CreateRotatedBranch (vertices,faces,startPos,lineLength*sizeAdjust,tipWidth,angle, isLeft, startVertexId, endVertexId,layerCount);
		
		}
		
    }

/* 
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
	*/
}