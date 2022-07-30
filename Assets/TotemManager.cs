using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using TotemEntities;
using TotemServices;
using TMPro;
using Unity.VisualScripting;

namespace TotemDemo
{
    public class TotemManager : MonoBehaviour
    {
        public static TotemManager Instance;
        private TotemDB totemDB;

        [Header("Demo")]
        public string _gameId; // Id of your game, used for legacy records identification

        [SerializeField] private GameObject loginButton;

        [Header("Login UI")]
        [SerializeField] private GameObject googleLoginObject;
        [SerializeField] private GameObject profileNameObject;
        [SerializeField] private TextMeshProUGUI profileNameText;

        [Header("Legacy UI")]
        [SerializeField] private UIAssetsList assetList;
        [SerializeField] private TextMeshProUGUI milestoneText;

        [Header("bolt")]
        [SerializeField] private GameObject boltGameScript;

        //Meta Data
        private string _accessToken;
        private string _publicKey;
        private List<TotemAvatar> _userAvatars;
        private List<TotemLegacyRecord> legacyRecordsList;

        //Default Avatar reference - use for your game
        private TotemAvatar firstAvatar;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            totemDB = new TotemDB(_gameId);

            totemDB.OnSocialLoginCompleted.AddListener(OnTotemUserLoggedIn);
            totemDB.OnUserProfileLoaded.AddListener(OnUserProfileLoaded);
            totemDB.OnAvatarsLoaded.AddListener(OnAvatarsLoaded);
        }

        #region USER AUTHENTICATION
        public void OnLoginButtonClick()
        {
            UILoadingScreen.Instance.Show();
            totemDB.AuthenticateCurrentUser();
        }

        private void OnTotemUserLoggedIn(TotemAccountGateway.SocialLoginResponse loginResult)
        {
            //googleLoginObject.SetActive(false);
            //profileNameObject.SetActive(true);
            //profileNameText.SetText(loginResult.profile.username);

            _accessToken = loginResult.accessToken;
            totemDB.GetUserProfile(_accessToken);
        }

        private void OnUserProfileLoaded(string publicKey)
        {
            _publicKey = publicKey;
            totemDB.GetUserAvatars(_publicKey);
        }

        private void OnAvatarsLoaded(List<TotemAvatar> avatars)
        {
            _userAvatars = avatars;

            //Reference the first Avatar in the list
            firstAvatar = avatars[0];

            CheckAztec();
            CheckBrazilian();
            CheckCeltic();
            CheckGreek();
            CheckRomanian();
            CheckTest();
            UILoadingScreen.Instance.Hide();
        }

        #endregion

        #region LEGACY RECORDS
        /// <summary>
        /// Add a new Legacy Record to a specific Totem Asset.
        /// </summary>
        public void AddLegacyRecord(ITotemAsset asset, int data)
        {
            UILoadingScreen.Instance.Show();
            totemDB.AddLegacyRecord(asset, data.ToString(), (record) =>
            {
                UILoadingScreen.Instance.Hide();
            });
        }

        /// <summary>
        /// Add a new Legacy Record to the first Totem Avatar.
        /// </summary>
        public void AddLegacyToFirstAvatar(int data)
        {
            AddLegacyRecord(firstAvatar, data);
            Debug.Log($"Add Legacy: {data}");
        }

        public void GetLegacyRecords(ITotemAsset asset, UnityAction<List<TotemLegacyRecord>> onSuccess, string gameID)
        {
            totemDB.GetLegacyRecords(asset, onSuccess, gameID);
        }

        public void GetLastLegacyRecord(UnityAction<TotemLegacyRecord> onSuccess, string gameID)
        {
            GetLegacyRecords(firstAvatar, (records) =>
            {
                if (records.Count == 0)
                {
                    onSuccess.Invoke(null);
                }
                else
                {
                    onSuccess.Invoke(records[records.Count - 1]);
                }
            }, gameID);
        }

        public void GodOfArts()
        {
            GetLastLegacyRecord((record) =>
            {
                if (record == null)
                {
                    AddLegacyRecord(firstAvatar, 1);
                }
                else
                {
                    int lastRecordData = int.Parse(record.data);
                    if (lastRecordData % 10 != 1)
                    {
                        AddLegacyRecord(firstAvatar, lastRecordData + 1);
                    }
                }
            }, _gameId);
        }

        public void GodOfFarts(int newEvent)
        {
            GetLastLegacyRecord((record) =>
            {
                if (record == null)
                {
                    AddLegacyRecord(firstAvatar, 10);
                }
                else
                {
                    int lastRecordData = int.Parse(record.data);
                    if (lastRecordData % 100 - lastRecordData % 10 != 10)
                    {
                        AddLegacyRecord(firstAvatar, lastRecordData + 10);
                    }
                }
            }, _gameId);
        }


        

        public void CheckAztec()
        {
            GetLastLegacyRecord((record) =>
            {
                if (record != null)
                {
                    Debug.Log("Aztec works");
                    CustomEvent.Trigger(boltGameScript, "aztec");
                }
                else
                {
                    Debug.Log("Aztec doesn't work");
                }
            }, "legacyjam2-aztec"
            );
        }

        public void CheckBrazilian()
        {
            GetLastLegacyRecord((record) =>
            {
                if (record != null)
                {
                    Debug.Log("Brazilian works");
                    CustomEvent.Trigger(boltGameScript, "brazilian");
                }
                else
                {
                    Debug.Log("Brazilian doesn't work");
                }
            }, "legacyjam2-brazilian"
            );
        }

        public void CheckCeltic()
        {
            GetLastLegacyRecord((record) =>
            {
                if (record != null)
                {
                    Debug.Log("Celtic works");
                    CustomEvent.Trigger(boltGameScript, "celtic");
                }
                else
                {
                    Debug.Log("Celtic doesn't work");
                }
            }, "legacyjam2-celtic"
            );
        }

        public void CheckGreek()
        {
            GetLastLegacyRecord((record) =>
            {
                if (record != null)
                {
                    Debug.Log("Greek works");
                    CustomEvent.Trigger(boltGameScript, "greek");
                }
                else
                {
                    Debug.Log("Greek doesn't work");
                }
            }, "legacyjam2-greek"
            );
        }

        public void CheckRomanian()
        {
            GetLastLegacyRecord((record) =>
            {
                if (record != null)
                {
                    Debug.Log("Romanian works");
                    CustomEvent.Trigger(boltGameScript, "romanian");
                }
                else
                {
                    Debug.Log("Romanian doesn't work");
                }
            }, "legacyjam2-romanian"
            );
        }

        public void CheckTest()
        {
            GetLastLegacyRecord((record) =>
            {
                if (record != null)
                {
                    Debug.Log("Test works");
                    CustomEvent.Trigger(boltGameScript, "test");
                }
                else
                {
                    Debug.Log("Test doesn't work");
                }
            }, "legacyjam2-test"
            );
        }
        #endregion
    }
}