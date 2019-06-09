using UnityEngine;
using System.Collections;

public class __SOURCES : MonoBehaviour {

	/*
	 * MODELS
	 * [Taxi Car] http://www.turbosquid.com/3d-models/free-max-model-car-rig/648165
	 * [Clock] http://www.tf3dm.com/3d-model/wall-clock-11050.html
	 * 
	 * MATERIALS
	 * [Reflection] http://wiki.unity3d.com/index.php/MirrorReflection4
	 * 
	 * SCRIPTS
	 * [Reflection] http://wiki.unity3d.com/index.php/MirrorReflection4
	 * 
	 * SPRITES
	 * [Bee] Icon made by http://www.flaticon.com/authors/freepik from www.flaticon.com
	 * [Hand] Icon made by http://www.flaticon.com/authors/zlatko-najdenovski from www.flaticon.com
	 * [Sax] Icon made by http://www.flaticon.com/authors/freepik from www.flaticon.com
	 * [Record] Icon made by http://www.flaticon.com/authors/madebyoliver from www.flaticon.com
	 * [Mic] Icon made by http://www.flaticon.com/authors/dave-gandy from www.flaticon.com
	 * [No Entry] Icon made by http://www.flaticon.com/authors/freepik from www.flaticon.com
	 * [Alert] Icon made by http://www.flaticon.com/authors/freepik from www.flaticon.com
	 * [Circular Arrow] Icon made by http://www.flaticon.com/authors/freepik from www.flaticon.com
	 * [Pickup] http://www.flaticon.com/free-icon/package-cube-box-for-delivery_45806#term=boxes&page=2&position=24
	 * [Coin] http://www.flaticon.com/free-icon/coin_126094
	 * 
	 * TEXTURES
	 * 
	 * CODE
	 * [1] https://forum.unity3d.com/threads/converting-float-to-integer.27511/
	 * [2] http://answers.unity3d.com/questions/356920/copy-an-list-in-c.html
	 * [3] http://answers.unity3d.com/questions/416164/vector3-and-transformrotation.html
	 * [4] https://forum.unity3d.com/threads/click-drag-camera-movement.39513
	 * [5] http://answers.unity3d.com/questions/286072/stringtrim-and-stringsplit.html
	 * [6] https://docs.unity3d.com/ScriptReference/UI.Slider-onValueChanged.html
	 * [7] http://blog.teamtreehouse.com/make-loading-screen-unity
	 * [8] http://answers.unity3d.com/questions/525106/c-enum-wont-show-in-inspector.html
	 * [9] http://wiki.unity3d.com/index.php/Server_Side_Highscores
	 * [10] http://wiki.unity3d.com/index.php?BoolPrefs
	 * [11] https://docs.unity3d.com/ScriptReference/Color.Lerp.html
	 * 
	 * TUTORIALS
	 * [Animation Controller] https://www.youtube.com/watch?v=HsKtxPmtvbY
	 * [Main Menu] https://www.youtube.com/watch?v=OWtQnZsSdEU
	 * [Rain Particles] https://laboratoriesx86.wordpress.com/2013/06/08/rainfx/
	 * [Snow Particles] https://www.youtube.com/watch?v=b8oVAS9IdZM
	 * 
	 * AUDIO
	 * [Driving Car] http://soundbible.com/1397-Car-Driving.html
	 * [Enter/Exit Car] http://www.freesfx.co.uk/download/?type=mp3&id=16357
	 * [Pickup] https://www.freesound.org/people/Wagna/sounds/325805/
	 * [Success] https://www.freesound.org/people/GabrielAraujo/sounds/242501
	 * [Failure] https://www.freesound.org/people/ProjectsU012/sounds/333785/
	 * [Handbrake] http://www.freesound.org/people/MorneDelport/sounds/326394/
	 * [Car Horn] http://www.freesound.org/people/bigmanjoe/sounds/349922/
	 * [Countdown] http://www.freesound.org/people/daveincamas/sounds/27081/
	 * [Game Over] http://www.freesound.org/people/jivatma07/sounds/122255/
	 * [Rain] http://www.freesound.org/people/Stevious42/sounds/259627/
	 * 
	 * RADIO - 1.03 Variety
	 * [Pop Dance - Bensound] https://www.youtube.com/watch?v=b54NnWZHrQs
	 * [We Are One - Vexento] https://www.youtube.com/watch?v=Ssvu2yncgWU
	 * [Adventures - A Himitsu] https://www.youtube.com/watch?v=MkNeIUgNPQ8
	 * [Buddha - Kontekst] https://www.youtube.com/watch?v=b6jK2t3lcRs
	 * [High - JPB] https://www.youtube.com/watch?v=Tv6WImqSuxA
	 * [Colors - Tobu] https://www.youtube.com/watch?v=MEJCwccKWG0
	 * [Funk City - Reatch] https://www.youtube.com/watch?v=J5JZNdb50B8
	 * [Circles - Lensko] https://www.youtube.com/watch?v=ztvIhqVtrrw
	 * 
	 * RADIO - Radio Buzz
	 * [Paint the Sky - AVE & EFX & Varun] https://www.youtube.com/watch?v=MwhVsOKxoko
	 * [Know Better - JayKode] https://www.youtube.com/watch?v=STgM16LNsMk
	 * [Undone - Spaces] https://www.youtube.com/watch?v=A5T2KeW4zcc
	 * [Give Me Love - Kyle Braun] https://www.youtube.com/watch?v=25Oq6tEUPIs
	 * [Make Dem - JRND & VMK] https://www.youtube.com/watch?v=w9jrC_oJQNM
	 * [You & I - Ellusive & TELYKast] https://www.youtube.com/watch?v=3c4GVDe9Zwg
	 * [Tell Me - Take/Five] https://www.youtube.com/watch?v=Kx1QIlGmhPo
	 * [Take Off - Meric] https://www.youtube.com/watch?v=VcUWXvdsfOc
	 * 
	 * RADIO - Saxy FM
	 * [Alto Sax Stroll - MusicByPedro] https://www.youtube.com/watch?v=DKltXgQAYeQ
	 * [Very Chill Saxophone - Spike's Vibes] https://www.youtube.com/watch?v=mWwf4LMfaTQ
	 * [Sax Appeal - Gee] https://www.youtube.com/watch?v=VuN6q3ALy5A
	 * [Electroswing Revival - Gee] https://www.youtube.com/watch?v=ZkkNgbkcvog
	 * [Swing Time - MusicByPedro] https://www.youtube.com/watch?v=nDDfWDvVROI
	 * [Smooth Jazz - Gee] https://www.youtube.com/watch?v=KeD33eft2Do
	 * [New York, 1924 - Ross Bugden] https://www.youtube.com/watch?v=wnPLbKmYCKQ
	 * [Jazz in Paris - Media Right Productions] https://www.youtube.com/watch?v=ZFgu73S16nk
	 * [Rainy November - Teknoaxe] https://www.youtube.com/watch?v=yAnzVd_mgjw
	 * 
	*/

}
