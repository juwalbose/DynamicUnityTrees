using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LSystemTree : MonoBehaviour {

	List<Branch> branches;
	bool isDebug=false;
	Mesh lTree;
	List<int> faces;
	List<Vector3> vertices;

	public string[] vars;
	Dictionary<string,string> rules;
	string lString;
	public int iterations=3;
	public int angle=25;
	public float startLength=2;
	public float startWidth=1;

	List<LevelState> levelStates;
	public Vector3 treeOrigin;
	public float widthDecreaseFactor;
	public float lengthDecreaseFactor;
	

	// Use this for initialization
	void Start () {
		lString=vars[0];
		rules=new Dictionary<string, string>();
		rules[vars[0]]="FF+[+F-F-F]-[-F+F+F]";
		for(int i=0;i<iterations;i++){
			lString=IterateLString(lString);
		}
		
		
		//lString="F[+F[+F]-F]-F[+F]-F";
		//lString="F[+F]--F";
		Debug.Log(lString);

		branches=new List<Branch>();
		faces=new List<int>();
		vertices=new List<Vector3>();
		lTree=GetComponent<MeshFilter>().mesh;
		lTree.name="l system tree";
		CreateTree();
	}
	string IterateLString(string currentLString){
		string newLString="";
		char[] chars=currentLString.ToCharArray();
		for(int i=0;i<chars.Length;i++){
			if(rules.ContainsKey(chars[i].ToString())){
				newLString+=rules[chars[i].ToString()];
			}else{
				newLString+=chars[i].ToString();
			}

		}
		return newLString;
	}
	private void CreateTree(){
		levelStates=new List<LevelState>();
		char[] chars=lString.ToCharArray();
		float currentRotation=0;
		float currentLength=startLength;
		float currentWidth=startWidth;
		Vector3 currentPosition=treeOrigin;
		int levelIndex=0;
		LevelState levelState=new LevelState();
		levelState.position=currentPosition;
		levelState.levelIndex=levelIndex;
		levelState.width=currentWidth;
		levelState.length=currentLength;
		levelState.rotation=currentRotation;
		levelState.logicBranches=new List<LevelState.BranchState>();
		levelStates.Add(levelState);
		Vector3 tipPosition=new Vector3();
		Queue<LevelState> savedStates=new Queue<LevelState>();
		for(int i=0;i<chars.Length;i++){
			switch(chars[i]){
				case 'F':
					if(levelState.levelIndex!=levelIndex){
						foreach(LevelState ls in levelStates){
							if(ls.levelIndex==levelIndex){
								levelState=ls;
								break;
							}
						}	
					}
					tipPosition.x=levelState.position.x+(currentLength*Mathf.Sin(Mathf.Deg2Rad*currentRotation));
					tipPosition.y=levelState.position.y+(currentLength*Mathf.Cos(Mathf.Deg2Rad*currentRotation));
					levelIndex++;
					LevelState.BranchState branchState=new LevelState.BranchState();
					branchState.rotation=currentRotation;
					branchState.length=currentLength;
					levelState.logicBranches.Add(branchState);

					currentWidth*=widthDecreaseFactor;

					currentPosition=tipPosition;
					levelState=new LevelState();
					levelState.position=currentPosition;
					levelState.levelIndex=levelIndex;
					levelState.width=currentWidth;
					levelState.length=currentLength;
					levelState.rotation=currentRotation;
					levelState.logicBranches=new List<LevelState.BranchState>();
					levelStates.Add(levelState);

					currentLength*=lengthDecreaseFactor;
					
				break;
				case '+':
					currentRotation+=angle;
				break;
				case '-':
					currentRotation-=angle;
				break;
				case '[':
					savedStates.Enqueue(levelState);
				break;
				case ']':
					levelState=savedStates.Dequeue();
					currentPosition=levelState.position;
					currentRotation=levelState.rotation;
					currentLength=levelState.length;
					currentWidth=levelState.width;
					levelIndex=levelState.levelIndex;
				break;
			}
		}
		
		for(int i=0;i<levelStates.Count;i++){
			levelState=levelStates[i];
			foreach(LevelState.BranchState bs in levelState.logicBranches){
				Branch branch=new Branch(Vector3.zero,bs.length,levelState.width,bs.rotation, levelState.position,widthDecreaseFactor);
				branches.Add(branch);
				int baseVertexPointer=vertices.Count;
				vertices.AddRange(branch.vertices);
				faces.Add(baseVertexPointer);
				faces.Add(baseVertexPointer+1);
				faces.Add(baseVertexPointer+3);
				faces.Add(baseVertexPointer+3);
				faces.Add(baseVertexPointer+1);
				faces.Add(baseVertexPointer+2);
			}
		}
		 
		lTree.vertices=vertices.ToArray();
		lTree.triangles = faces.ToArray();
		lTree.RecalculateNormals();
		//Debug.Log("Tree has "+vertices.Count.ToString()+" vertices");
		
	}

	private void OnDrawGizmos () {
		if(!isDebug)return;
		if (branches == null) {
			return;
		}
		Gizmos.color = Color.green;
		foreach(Branch br in branches){
			for (int i = 0; i < br.vertices.Count; i++) {
				Gizmos.DrawSphere(br.vertices[i], 0.05f);
			}
		}
		
	}
	
}
