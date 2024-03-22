namespace Unpages.Network{
    using Fusion;
    using Sirenix.OdinInspector;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class NetworkPlayer : NetworkBehaviour
    {

        [Networked] public string PlayerName { get; set; }

        [Networked] public NetworkBool Ready { get; set; }

        public NetworkObject networkCharacter;

        [Button]
        public override void Spawned()
        {
            //NetworkManager.Instance.SetPlayer(Object.InputAuthority, this);
            //NetworkManager.Instance.SessionRunner.GetComponent<NetworkEvents>().PlayerJoined.AddListener(SetPlayer);
            SetPlayer(Runner,Object.InputAuthority);
            

            //NetworkManager.Instance.SetPlayer(Object.InputAuthority, this);

        }
        public void SetPlayer(NetworkRunner runner, PlayerRef playerRef)
        {
            NetworkManager.Instance.SetPlayer(playerRef, this);
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
