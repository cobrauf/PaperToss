using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour
{
	public bool m_EnableContour = true;
	public GameObject m_MainCamera;
	private ContourDrawing[] m_Objs;
	
	void Start ()
	{
		QualitySettings.antiAliasing = 8;
		m_Objs = GameObject.FindObjectsOfType<ContourDrawing> ();
		for (int i = 0; i < m_Objs.Length; i++)
			m_Objs[i].Initialize ();
	}
	void Update ()
	{
		for (int i = 0; i < m_Objs.Length; i++)
		{
			if (m_EnableContour)
			{
				m_Objs[i].FxEnable ();
				m_Objs[i].UpdateSelfParameters ();
				m_MainCamera.GetComponent<Skybox> ().enabled = true;
			}
			else
			{
				m_Objs[i].FxDisable ();
				m_MainCamera.GetComponent<Skybox> ().enabled = false;
			}
		}
	}
	private void OnGUI()
	{
		//int w = 250;
		//GUI.Box (new Rect (Screen.width / 2 - w / 2, 10, w, 25), "NPR Contour Drawing Demo");
	}
}
