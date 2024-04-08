using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GameService : MonoBehaviour
{
    #region References
    public PlayerAction playerAction;
    public NetworkItems networkItems;
    public GameControl gameControl;
    public AIManagerSystem aiManagerSystem;
    #endregion



    private void Start()
    {
        //if (inAppPurchaseManager == null)
        //{
        //    inAppPurchaseManager = FindAnyObjectByType<IAPManager>();
        //}

        //if (appEntitlementManager == null)
        //{
        //    appEntitlementManager = FindAnyObjectByType<AppEntitlementCheck>();
        //}
    }

    #region Singleton Implementation

    public static GameService Instance { get; private set; }



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);

        }
    }

    #endregion
}
