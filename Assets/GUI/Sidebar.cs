//--------------------------------------------------
// Connor Janowiak
// Feb 22, 2013
//
// Class used to control the sidebar on the screen. Includes the drop down menus
// and individual instruments.
//
//--------------------------------------------------

using UnityEngine;
using System.Collections;

public class Sidebar : MonoBehaviour {

	public int sidebarWidth = 150;
	public int timebarHeight = 100;
	public GUISkin skin;
	public Texture dropdownArrow;

	//Used for dropdown selections
	public string[] bodies;
	private int bodySelected = 0;

	public string[] instruments;
	private int instrumentSelected = 0;

	//Used to determine which dropdown is open, and disabling everything else
	private int dropdownOpen = -1;

	public void OnGUI() {
		//Sidebar group
		GUI.BeginGroup(new Rect(Screen.width - sidebarWidth, 0, sidebarWidth, Screen.height - timebarHeight), skin.GetStyle("Box"));

			//NOTE Controls must be declared in bottom-to-top order to render and function properly
			//
			//Instrument selection
			GUI.Label(new Rect(10, 70, sidebarWidth, 30), "Instrument");
			DropDown(new Rect(15, 95, sidebarWidth - 30, 30), 1, instruments, ref instrumentSelected);

			//Primary selection
			GUI.Label(new Rect(10, 5, sidebarWidth, 30), "Viewing");
			DropDown(new Rect(15, 30, sidebarWidth - 30, 30), 0, bodies, ref bodySelected);

		GUI.EndGroup();
	}

	private int DropDown(Rect pos, int dropdown_id, string[] options, ref int selected) {
		if(dropdownOpen != dropdown_id) {
			if(dropdownOpen != -1) GUI.enabled = false;
			if(GUI.Button(pos, options[selected], skin.GetStyle("Dropdown")) && dropdownOpen == -1) {
				//Set current dropdown to this one.
				dropdownOpen = dropdown_id;
			}
			GUI.enabled = true;
			GUI.DrawTexture(new Rect(pos.x + pos.width - 25, pos.y + 5, 20, 20), dropdownArrow);
		}

		if(dropdownOpen == dropdown_id) {
			//Draw the dropdown
			//Have the current selected be first
			if(GUI.Button(pos, options[selected], skin.GetStyle("Dropdown Hover"))) {
				//Do nothing and close the dropdown, if we click on the top one.
				dropdownOpen = -1;
			}
			GUI.DrawTexture(new Rect(pos.x + pos.width - 25, pos.y + 5, 20, 20), dropdownArrow);

			//Go through the rest
			int vertpos = 0;
			for(int i = 0; i < options.Length; i++) {
				if(i != selected) {
					vertpos++;
					if(GUI.Button(new Rect(pos.x, pos.y + pos.height * vertpos, pos.width, pos.height),
																							options[i], 
																							skin.GetStyle("Dropdown Hover"))) {
							//Close the dropdown and select the correct one
						dropdownOpen = -1;
						selected = i;
					}
				}
			}
		}
		return selected;
	}
}
