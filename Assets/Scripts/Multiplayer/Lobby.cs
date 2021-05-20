using System;
using System.Collections.Generic;
using API;
using API.Models;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnitySocketIO;
using agora_gaming_rtc;
using UnityEngine.Android;

public class Lobby : DiscardCardsScript
{
    public GameObject playerPrefab;
    public Transform playerContainer;
    public Dropdown gameModesDropdown;

    public bool reloadLobby;
    public bool startGame;

    public GameObject[] lobbyLeaderObjects;

    private SocketIOController socketIOController;
    private List<string> _gameModes;
    
    // voice chat things here
    public Button leaveChannel;
    public Button muteButton;
    private IRtcEngine mRtcEngine = null;
    [SerializeField]
    private string AppID = "4bee75478c0344709d0a58b68170517f";
    
    
    // full voice thing this awake
    
    void Start()
    {
        base.Start();

        socketIOController = SocketClient.Client;
        
        socketIOController.On("lobby-update", response =>
        {
            GameData.Instance.GameInfo = Helper.DeserializeGameInfo(response.data);

            reloadLobby = true;
        });
        
        socketIOController.On("start", response =>
        {
            GameData.Instance.GameInfo = Helper.DeserializeGameInfo(response.data);
            GameData.Instance.IsMultiplayer = true;

            startGame = true;
        });

        var routine = APIClient.Instance.GetGameModes(gameModes =>
        {
            _gameModes = new List<string>() { "undefined" };
            foreach (var gameMode in gameModes)
            {
                if (!(gameMode.Equals("singleplayer")))
                {
                    var optionItem = new Dropdown.OptionData(gameMode);
                    gameModesDropdown.options.Add(optionItem);
                    _gameModes.Add(gameMode);
                }
            }
        });

        StartCoroutine(routine);

        InitLobbyData();
        
        // voice from here
#if (UNITY_2018_3_OR_NEWER)
        if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
			
        } 
        else 
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
        mRtcEngine = IRtcEngine.GetEngine(AppID);
        leaveChannel.onClick.AddListener(LeaveChannel);
        muteButton.onClick.AddListener(MuteButtonTapped);
        muteButton.enabled = true;
        
        mRtcEngine.OnLeaveChannel += (RtcStats stats) =>
        {
            muteButton.enabled = false;
            // reset the mute button state
            if (isMuted)
            {
                MuteButtonTapped();
            }
        };
        mRtcEngine.SetLogFilter(LOG_FILTER.INFO);

        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_COMMUNICATION);
        var gameInfo = GameData.Instance.GameInfo;
        JoinChannel(gameInfo.Lobby().lobbyCode);
    }

    private void Update()
    {
        if (reloadLobby)
        {
            InitLobbyData();
            reloadLobby = false;
        }

        if (startGame)
        {
            var gameMode = GameData.Instance.GameInfo.GameModeId();

            switch (gameMode)
            {
                case "basic":
                {
                    SceneManager.LoadScene("Scenes/Multiplayer/Basic/BasicGame");
                    break;
                }
                case "concurrent":
                {
                    SceneManager.LoadScene("Scenes/Multiplayer/Concurrent/Concurrent");
                    break;
                }
                default:
                {
                    SceneManager.LoadScene("GameScene");
                    break;
                }
            }
        }
    }

    private void InitLobbyData()
    {
        var childCount = playerContainer.childCount;
        
        for (var i = childCount - 1; i >= 0; i--)
        {
            Destroy(playerContainer.GetChild(i).gameObject);
        }

        var gameInfo = GameData.Instance.GameInfo;
        
        foreach (var player in gameInfo.Lobby().players)
        {
            var lobbyOwner = player.sessionId == gameInfo.Lobby().lobbyOwnerId;
            AddPlayer(player, lobbyOwner);

            if (player.sessionId == socketIOController.SocketID)
            {
                ShowLobbyLeaderObjects(lobbyOwner);
            }
        }
    }

    private void AddPlayer(Player player, bool lobbyOwner)
    {
        var playerObject = Instantiate(playerPrefab, playerContainer);

        var text = playerObject.GetComponentInChildren<Text>();
        text.text = player.name;

        if (lobbyOwner)
        {
            text.fontStyle = FontStyle.BoldAndItalic;
        }
    }

    public void StartGame()
    {
        var options = getGameStartParam();
        options.gameMode = getSelectedGameMode();


        socketIOController.Emit("start", JsonUtility.ToJson(options));
    }

    private string getSelectedGameMode()
    {
        var gameModeValue = gameModesDropdown.value;

        if (gameModeValue == 0)
        {
            throw new Exception("Choose correct game mode");
        }
        
        return _gameModes[gameModeValue];
    }

    private void ShowLobbyLeaderObjects(bool active)
    {
        foreach (var gameObject in lobbyLeaderObjects)
        {
            gameObject.SetActive(active);
        }
    }
    
    // here come the voice chat methods
    public void JoinChannel(string lobbyId)
    {
        mRtcEngine.JoinChannel(lobbyId, "extra", 0);
    }

    public void LeaveChannel()
    {
        mRtcEngine.LeaveChannel();
    }

    void OnApplicationQuit()
    {
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();
        }
    }

    bool isMuted = false;
    void MuteButtonTapped()
    {
        string labeltext = isMuted ? "Mute" : "Unmute";
        Text label = muteButton.GetComponentInChildren<Text>();
        if (label != null)
        {
            label.text = labeltext;
        }
        isMuted = !isMuted;
        mRtcEngine.EnableLocalAudio(!isMuted);
    }
}
