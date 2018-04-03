using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch {
	public List<Vector3> vertices=new List<Vector3>();
	public float angleInDegrees;
	public Branch(Vector3 origin,float branchLength, float baseWidth, float branchAngleinDegrees, Vector3 originOffset, float widthDecreaseFactor){
		angleInDegrees=branchAngleinDegrees;
		CreateBranch(origin,branchLength,baseWidth,angleInDegrees, originOffset,widthDecreaseFactor);
	}
	private void CreateBranch(Vector3 origin, float branchLength,float branchWidth, float branchAngle, Vector3 offset, float widthDecreaseFactor)
    {
        Vector3 bottomLeft=new Vector3(origin.x,origin.y,origin.z),bottomRight=new Vector3(origin.x,origin.y,origin.z),topLeft=new Vector3(origin.x,origin.y,origin.z),topRight=new Vector3(origin.x,origin.y,origin.z);
		bottomLeft.x-=branchWidth*0.5f;
		bottomRight.x+=branchWidth*0.5f;
		topLeft.y=topRight.y=origin.y+branchLength;
		float newWidth=branchWidth*widthDecreaseFactor;
		topLeft.x-=newWidth*0.5f;
		topRight.x+=newWidth*0.5f;
		
		Vector3 axis=Vector3.back;
		Quaternion rotationValue = Quaternion.AngleAxis(branchAngle, axis);
		vertices.Add((rotationValue*(bottomLeft))+offset);
		vertices.Add((rotationValue*(topLeft))+offset);
		vertices.Add((rotationValue*(topRight))+offset);
		vertices.Add((rotationValue*(bottomRight))+offset);
    }
}
