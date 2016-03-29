using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ChangeColor : MonoBehaviour
{
	public Color ObjectColor;
	
	private Color currentColor;
	private Material materialColored;
	
	void Update()
	{
		if (ObjectColor != currentColor)
		{
			//stop the leaks
			if (materialColored != null)

				#if UNITY_EDITOR
				UnityEditor.AssetDatabase.DeleteAsset(UnityEditor.AssetDatabase.GetAssetPath(materialColored));
				#endif


			//create a new material
			materialColored = new Material(Shader.Find("Diffuse"));
			materialColored.color = currentColor = ObjectColor;
			this.GetComponent<Renderer>().material = materialColored;
		}
	}
}