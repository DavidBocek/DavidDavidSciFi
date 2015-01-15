using UnityEngine;
using System.Collections;

public static class SPlayerSettings {

	public static string saveFileName;

	public static bool invertMouse;
	public static bool displayHUD;
	public static float mouseSensitivity;
	
	public static void LoadPlayerSettings(){
		if (PlayerPrefs.HasKey(saveFileName)){
			LoadFromSaveName();
		} else {
			LoadDefaultSettings();
		}
	}

	public static void LoadDefaultSettings(){
		invertMouse = false;
		displayHUD = true;
		mouseSensitivity = 1.0f;
	}

	private static void LoadFromSaveName(){
		invertMouse = PlayerPrefs.GetInt(saveFileName+"_INVERT_MOUSE",0) > 0;
		displayHUD = PlayerPrefs.GetInt(saveFileName+"_DISPLAY_HUD",1) > 1;
		mouseSensitivity = PlayerPrefs.GetFloat(saveFileName+"_MOUSE_SENSITIVITY",1.0f);
	}
}
