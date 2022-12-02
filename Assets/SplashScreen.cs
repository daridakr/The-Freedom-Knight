using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
	public string startMenu = "MainMenu";
	public float fadeSpeed = 0.5f;
	private bool start;
	private Image rend;

	void Start()
	{
		start = true;
		//Cursor.visible = false;
		rend = GetComponent<Image>();
		rend.color = Color.clear;
	}

	void Update()
	{
		if (start) rend.color = Color.Lerp(rend.color, Color.white, fadeSpeed * Time.deltaTime);
		else rend.color = Color.Lerp(rend.color, Color.clear, fadeSpeed * Time.deltaTime);
		if (rend.color.a >= 0.95f) start = false;
		if (rend.color.a <= 0.1f && !start)
		{
			//Cursor.visible = true;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}
}
