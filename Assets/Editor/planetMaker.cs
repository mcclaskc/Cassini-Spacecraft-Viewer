using UnityEngine;
using UnityEditor;
using System.Collections;

public class planetMaker : EditorWindow {
	
	string planetName = "Planet";
	Texture2D bodyTexture;
	float radius = 0.0f;
	float flattening = 0.0f;
	bool atmosphere;
	bool hasRing;
	float ringSize = 0.0f;
	
	[MenuItem("GameObject/Planet Maker")]
	public static void ShowWindow(){
		EditorWindow.GetWindow(typeof(planetMaker));	
	}
	
	void OnGUI() {
		GUILayout.Label("Enter Appropriate Settings Below:", EditorStyles.boldLabel);
		planetName = EditorGUILayout.TextField("TextField", planetName);
		bodyTexture = (Texture2D) EditorGUILayout.ObjectField("", bodyTexture, typeof(Texture2D), false);
		radius = EditorGUILayout.Slider("Radius", radius, 0f,1000f);
		flattening = EditorGUILayout.Slider("Flattening", flattening, 0f, 1f);
		atmosphere = EditorGUILayout.Toggle("Atmosphere", atmosphere);
		EditorGUILayout.Separator();
		hasRing = EditorGUILayout.BeginToggleGroup("Ring", hasRing);
		ringSize = EditorGUILayout.Slider("Ring Radius", ringSize, 0f, 1000f);
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.Separator();

		if(GUILayout.Button("Create Planet")) {
			createPlanet();
		}

		EditorGUILayout.Separator();
	}
	
	void createPlanet() {
		GameObject planet =  GameObject.CreatePrimitive(PrimitiveType.Sphere);
		planet.name = name;
		Material tex = new Material(Shader.Find("Diffuse"));
		AssetDatabase.CreateAsset(tex, "Assets/" + name + ".mat");
		tex.mainTexture = bodyTexture;
		planet.renderer.material = tex;
		planet.transform.localScale = new Vector3(radius,radius * (1.0f - flattening),radius);
	}
	
}
