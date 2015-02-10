using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FacebookScript : MonoBehaviour {

	bool meh;

	protected string status = "Ready";

	public string title = "";
	public string message = "Hello";
	private string[] filterTypes = new string[] { "None (default)", "app_users", "app_non_users" };
	private int filterSelection = 0;
	private List<string> filterGroupNames = new List<string>();
	private List<string> filterGroupsIds = new List<string>();
	private int filterGroups = 0;
	public string data = "{}";
	public string selectorExcludeIds = "";
	public string selectorMax = "";

	void Awake()
	{
		FB.Init(onInitComplete: OnInitComplete, onHideUnity: OnHideUnity);
		//FB.ActivateApp();
	}

	private void OnInitComplete()
	{
		enabled = true;
		if (FB.IsLoggedIn) 
		{
			//OnLoggedIn();
			//loadingState = LoadingState.WAITING_FOR_INITIAL_PLAYER_DATA;
		}
		else
		{
			//loadingState = LoadingState.DONE;
		}
	}

	private void OnHideUnity(bool isGameShown)
	{
		//Util.Log("OnHideUnity");
		if (!isGameShown)
		{
			// pause the game - we will need to hide
			//Time.timeScale = 0;
		}
		else
		{
			// start the game back up - we're getting focus again
			//Time.timeScale = 1;
		}
	}

	void OnLoggedIn()
	{
		/*Util.Log("Logged in. ID: " + FB.UserId);
		
		// Reqest player info and profile picture
		FB.API(meQueryString, Facebook.HttpMethod.GET, APICallback);
		LoadPictureAPI(Util.GetPictureURL("me", 128, 128),MyPictureCallback);
		
		// Load high scores
		QueryScores();*/
	}

	void LogCallback(FBResult response) {
		Debug.Log(response.Text);
	}

	private void CallAppRequestAsFriendSelector()
	{
		// If there's a Max Recipients specified, include it
		int? maxRecipients = null;
		if (selectorMax != "")
		{
			try
			{
				maxRecipients = Int32.Parse(selectorMax);
			}
			catch (Exception e)
			{
				status = e.Message;
			}
		}
		
		// include the exclude ids
		string[] excludeIds = (selectorExcludeIds == "") ? null : selectorExcludeIds.Split(',');
		
		// Filter groups
		List<object> selectionFilters = new List<object>();
		if (filterSelection > 0)
		{
			selectionFilters.Add(filterTypes[filterSelection]);
		}
		if (filterGroups > 0)
		{
			for (int i = 0; i < filterGroups; i++)
			{
				selectionFilters.Add(
					new FBAppRequestsFilterGroup(
					filterGroupNames[i],
					filterGroupsIds[i].Split(',').ToList()
					)
					);
			}
		}
		
		FB.AppRequest(message, null, (selectionFilters.Count > 0) ? selectionFilters : null, excludeIds, maxRecipients, data, title, callback: LogCallback);
	}

	void Update()
	{
		Debug.Log("meh");
	}

	void LoginCallback(FBResult result)
	{
		if (result.Error != null)
		{
			//lastResponse = "Error Response:\n" + result.Error;
		}
		else if (!FB.IsLoggedIn)
		{
			//lastResponse = "Login cancelled by Player";
		}
		else
		{
			//lastResponse = "Login was successful!";
		}
	}

	void OnGUI()
	{
		Debug.Log("meh");

		if(GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 80, 150, 50), "Login"))
		{
			FB.Login("public_profile,email,user_friends", LoginCallback);
		}

		if(!FB.IsLoggedIn && FB.IsInitialized)
		{
			if(GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 80, 150, 50), "Login"))
			{
				FB.Login("public_profile,email,user_friends", LoginCallback);
			}
		}

		if(FB.IsLoggedIn)
		{
			if(GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2, 150, 50), "SendTo"))
			{
				CallAppRequestAsFriendSelector();
			}
		}

		if(GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 150, 50), "Meh"))
		{

		}
	}
}
