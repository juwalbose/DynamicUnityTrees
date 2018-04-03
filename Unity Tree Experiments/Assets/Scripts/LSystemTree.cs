using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystemTree : MonoBehaviour {

	List<Branch> branches;
	bool isDebug=true;
	Mesh lTree;
	List<int> faces;
	List<Vector3> vertices;

	public string[] vars;
	[SerializeField]
	public Dictionary<string,string> rules;
	public string lString;
	

	// Use this for initialization
	void Start () {
		//rules=new Dictionary<string, string>();
		lString=IterateLString(lString);
		Debug.Log(lString);

		branches=new List<Branch>();
		faces=new List<int>();
		vertices=new List<Vector3>();
		//lTree=GetComponent<MeshFilter>().mesh;
		//lTree.name="l system tree";
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
		
		/*
		lTree.vertices=vertices.ToArray();
		lTree.triangles = faces.ToArray();
		lTree.RecalculateNormals();
		Debug.Log("Tree has "+vertices.Count.ToString()+" vertices");
		*/
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
