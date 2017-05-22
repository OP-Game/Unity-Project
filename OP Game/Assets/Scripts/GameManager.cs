using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


namespace Com.OP.MyGame
{
    public class GameManager : Photon.PunBehaviour
    {
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        private GameObject spawner;
        #region Photon Messages

        public override void OnPhotonPlayerConnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected


                LoadArena();
            }
        }


        public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects


            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected


                LoadArena();
            }
        }
        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        void OnJoinedRoom()
        {
            GameObject playerChar = PhotonNetwork.Instantiate(this.playerPrefab.name, spawner.transform.position, Quaternion.identity, 0);
            PlayerManager pManager = playerChar.GetComponent<PlayerManager>();
            pManager.enabled = true;
            UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsController = playerChar.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
            fpsController.enabled = true;
            Camera fpsCamera = playerChar.GetComponentInChildren<Camera>();
            fpsCamera.enabled = true;
        }

        #endregion

        private void Start()
        {
            spawner = GameObject.Find("Spawner");
            

        }

        #region Public Methods


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        #endregion

        #region Private Methods


        void LoadArena()
        {
            if (!PhotonNetwork.isMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount);
            PhotonNetwork.LoadLevel("Arena1 1");
        }


        #endregion
    }
}