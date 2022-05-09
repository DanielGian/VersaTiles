using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ControllerManager_SCR : MonoBehaviour
{
    private const int maxNumOfPlayers = 2;
    private int playerIdCounter = 0;
    private int[] IDs = new int[2];
    private Player systemPlayer;
    private bool allowLobbyControls = false;
    private bool battleMode = true;
    private GameObject GM;

    // Start is called before the first frame update
    void Start()
    {
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;

        systemPlayer = ReInput.players.GetSystemPlayer();
        GM = GameObject.Find("GameManager");
        DontDestroyOnLoad(this.gameObject);
    }

    // This function will be called when a controller is about to be disconnected
    // You can get information about the controller that is being disconnected via the args parameter
    void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
    {
        if (playerIdCounter > 0) { playerIdCounter--; }
        Joystick joyStk = ReInput.controllers.GetJoystick(args.controllerId);
        systemPlayer.controllers.RemoveController(joyStk);
        Debug.Log($"Counter: {playerIdCounter}");
    }

    // Update is called once per frame
    void Update()
    {
        if (allowLobbyControls || battleMode)
        {
            int playerIndex = 0;
            // Assign each Joystick to a Player initially
            foreach (Player p in ReInput.players.Players)
            {
                foreach (Joystick joyStk in ReInput.controllers.Joysticks)
                {
                    if (ReInput.players.Players[0].controllers.ContainsController(joyStk) || ReInput.players.Players[1].controllers.ContainsController(joyStk))
                    {
                        if (ReInput.players.Players[playerIndex].controllers.joystickCount != 0 && ReInput.players.Players[playerIndex].controllers.ContainsController(joyStk))
                        {
                            //if (joyStk.GetButtonDown(1)) { Debug.Log($"Player {playerIndex} Joy {joyStk.id}"); RemoveJoystickFromPlayer(p, joyStk, playerIndex); return; }
                        }
                        continue;
                    }
                    if (p.controllers.joystickCount == 0)
                    {
                        // Assign Joystick to first Player that doesn't have any assigned
                        if (joyStk.GetAnyButtonDown() && !systemPlayer.GetButtonDown("Back")) { Debug.Log($"Player {playerIndex} Joy {joyStk.id}"); AssignJoystickToPlayer(p, joyStk, playerIndex); return; }
                    }
                }
                playerIndex++;
            }

            if (systemPlayer.GetButtonDown("Back"))
            {
                //GM.GetComponent<GameManager_SCR>().ExitVersusLobby();
            }
        }
        foreach (Joystick joyStk in ReInput.controllers.Joysticks)
        {
            if (ReInput.players.GetSystemPlayer().controllers.joystickCount == 1) { break; }
            if (!ReInput.players.GetSystemPlayer().controllers.ContainsController(joyStk)) { systemPlayer.controllers.AddController(joyStk, false); }
            else break;
        }
    }

    private void AssignJoystickToPlayer(Player p, Joystick joyStk, int index)
    {
        if (playerIdCounter < maxNumOfPlayers)
        {
            if (p.controllers.joystickCount > 0) // player already has a joystick
            {
                Debug.Log("Player already has a joystick!");
                index++;
                return;
            }
            IDs[index] = joyStk.id;
            p.controllers.hasKeyboard = true;
            p.controllers.AddController(joyStk, false); // assign joystick to player
            Debug.Log("Controller Assigned!");
            //systemPlayer.controllers.AddController(joyStk, true);
            playerIdCounter++;
            return;
        }
        else { Debug.Log("TOO MANY PLAYERS!"); }
    }

    private void RemoveJoystickFromPlayer(Player p, Joystick joyStk, int index)
    {
        //Debug.Log("Controller Removed!");
        IDs[index] = -1;
        p.controllers.RemoveController(joyStk); // remove joystick to player
        if (playerIdCounter > 0) { playerIdCounter--; }
        Debug.Log($"Counter: {playerIdCounter}");
        return;
    }

    public bool CheckRequiredNumOfPlayers() { return playerIdCounter >= 2; }
    public bool LobbyControlsON() { return allowLobbyControls; }

    public void EnableLobbyControls() { allowLobbyControls = true; }
    public void DisableLobbyControls() { allowLobbyControls = false; }
}

