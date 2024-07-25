using UnityEditor;

using FuryLion;

public static class SceneSwitcherList
{
	[MenuItem("Scenes/Editor", false, 0)]
	private static void OpenScene_Editor()
	{
		SceneSwitcher.OpenScene("Assets/Scenes/Editor.unity");
	}
	[MenuItem("Scenes/Game", false, 0)]
	private static void OpenScene_Game()
	{
		SceneSwitcher.OpenScene("Assets/Scenes/Game.unity");
	}
	[MenuItem("Scenes/Main", false, 0)]
	private static void OpenScene_Main()
	{
		SceneSwitcher.OpenScene("Assets/Scenes/Main.unity");
	}
	[MenuItem("Scenes/Splash", false, 0)]
	private static void OpenScene_Splash()
	{
		SceneSwitcher.OpenScene("Assets/Scenes/Splash.unity");
	}
}