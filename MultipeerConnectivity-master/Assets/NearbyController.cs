using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using System;
using GooglePlayGames.BasicApi.Nearby;

public class NearbyController : MonoBehaviour {


    private void Awake()
    {
        PlayGamesPlatform.InitializeNearby((client) =>
        {
            Debug.Log("Nearby connections initialized");
        });

    }

   public void Advertise (string Name )
    {
        List<string> appIdentifiers = new List< string> ();
        appIdentifiers.Add(PlayGamesPlatform.Nearby.GetAppBundleId());
        PlayGamesPlatform.Nearby.StartAdvertising(
                   Name,  // User-friendly name
                    appIdentifiers,  // App bundle Id for this game
                   TimeSpan.FromSeconds(0),// 0 = advertise forever
                   (AdvertisingResult result) =>
                   {
                       Debug.Log("OnAdvertisingResult: " + result);
                   },
                      (ConnectionRequest request) =>
                      {
                          Debug.Log("Received connection request: " +
                           request.RemoteEndpoint.ServiceId + " " +
                           request.RemoteEndpoint.EndpointId + " " +
                           request.RemoteEndpoint.Name);
   
                       }
                   );
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
