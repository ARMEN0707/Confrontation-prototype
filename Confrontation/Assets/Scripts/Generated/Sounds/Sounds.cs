using UnityEngine;
using FuryLion;

public static class Sounds
{
	public static class Music
	{

		public static AudioClip MainMenu => SoundResources.Get((int)SoundsNames.MainMenu);
	}

}