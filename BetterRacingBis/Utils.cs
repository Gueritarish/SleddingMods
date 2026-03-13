using UnityEngine;
using Il2Cpp;

namespace BetterRacingBis
{
    public class GeneralUtils
    {
        public static bool RestartingRace { get; set; } = false;

        public static void TeleportToRespawnAnchor()
        {
            PlayerReference localPlayer = PlayerReferenceManager.Instance.GetLocalPlayerReference();

            if(localPlayer.PlayerControl.movement.isUsingRespawnAnchor && !RestartingRace)
            {
                Vector3 respawnAnchorPosition = localPlayer.PlayerControl.movement.respawnAnchorPosition;
                localPlayer.PlayerControl.teleportationController.TeleportPlayer(respawnAnchorPosition, localPlayer.PlayerControl.movement.playerGraphics.transform.rotation);
            }
        }

        public static void TeleportToRaceStart()
        {
            PlayerReference localPlayer = PlayerReferenceManager.Instance.GetLocalPlayerReference();

            if(!RestartingRace)
            {
                Vector3 raceStartPosition = localPlayer.PlayerControl.racingController.GetCurrentRaceData().start.graphics.transform.position;
                localPlayer.PlayerControl.teleportationController.TeleportPlayer(raceStartPosition, localPlayer.PlayerControl.movement.playerGraphics.transform.rotation);
            }
        }

        public static void RestartRace()
        {
            if(RestartingRace) return;

            PlayerReference localPlayer = PlayerReferenceManager.Instance.GetLocalPlayerReference();

            if(localPlayer.PlayerControl.racingController.IsInRace())
            {
                PlaceableRaceInteractable currentRace = localPlayer.PlayerControl.racingController.GetCurrentRace();

                RestartingRace = true;
                localPlayer.PlayerControl.teleportationController.ResetPlayerToBeginningOfRace();
                localPlayer.PlayerControl.racingController.Button_LeaveRace();

                currentRace.Interact(localPlayer.PlayerControl);
            }
        }

        // Start Flags :
        // Ping Path : World/Races/race/RaceFlag
        // Green/Blue Path : World/Races/race (1)/RaceFlag
        // Blue : World/Races/race (2)/RaceFlag
        // Reverse Race : World/Races/race (3)/RaceFlag
        // Orange/Red Path : World/Races/race (4)/RaceFlag
        // Red Race : World/Races/race (5)/RaceFlag
        // Custom Race : World/Races/race (6)/RaceFlag

        // FOG Ping Path
        public static void teleportPingPath()
        {
            Vector3 flagPos = GameObject.Find("World/Races/race/RaceFlag").GetComponent<BuildableObject>().transform.position;
            PlayerReference localPlayer = PlayerReferenceManager.Instance.GetLocalPlayerReference();
            localPlayer.PlayerControl.teleportationController.TeleportPlayer(
                flagPos,
                localPlayer.PlayerControl.movement.playerGraphics.transform.rotation
            );
        }

        // FOG Green / Blue Path
        public static void teleportGreenBluePath()
        {
            Vector3 flagPos = GameObject.Find("World/Races/race (1)/RaceFlag").GetComponent<BuildableObject>().transform.position;
            PlayerReference localPlayer = PlayerReferenceManager.Instance.GetLocalPlayerReference();
            localPlayer.PlayerControl.teleportationController.TeleportPlayer(
                flagPos,
                localPlayer.PlayerControl.movement.playerGraphics.transform.rotation
            );
        }

        // Blue Race
        public static void teleportBlueRace()
        {
            Vector3 flagPos = GameObject.Find("World/Races/race (2)/RaceFlag").GetComponent<BuildableObject>().transform.position;
            PlayerReference localPlayer = PlayerReferenceManager.Instance.GetLocalPlayerReference();
            localPlayer.PlayerControl.teleportationController.TeleportPlayer(
                flagPos,
                localPlayer.PlayerControl.movement.playerGraphics.transform.rotation
            );
        }

        // Reverse Race
        public static void teleportReverseRace()
        {
            Vector3 flagPos = GameObject.Find("World/Races/race (3)/RaceFlag").GetComponent<BuildableObject>().transform.position;
            PlayerReference localPlayer = PlayerReferenceManager.Instance.GetLocalPlayerReference();
            localPlayer.PlayerControl.teleportationController.TeleportPlayer(
                flagPos,
                localPlayer.PlayerControl.movement.playerGraphics.transform.rotation
            );
        }

        // Orange / Red Path
        public static void teleportOrangeRedPath()
        {
            Vector3 flagPos = GameObject.Find("World/Races/race (4)/RaceFlag").GetComponent<BuildableObject>().transform.position;
            PlayerReference localPlayer = PlayerReferenceManager.Instance.GetLocalPlayerReference();
            localPlayer.PlayerControl.teleportationController.TeleportPlayer(
                flagPos,
                localPlayer.PlayerControl.movement.playerGraphics.transform.rotation
            );
        }

        // Red Race
        public static void teleportRedRace()
        {
            Vector3 flagPos = GameObject.Find("World/Races/race (5)/RaceFlag").GetComponent<BuildableObject>().transform.position;
            PlayerReference localPlayer = PlayerReferenceManager.Instance.GetLocalPlayerReference();
            localPlayer.PlayerControl.teleportationController.TeleportPlayer(
                flagPos,
                localPlayer.PlayerControl.movement.playerGraphics.transform.rotation
            );
        }

        // Custom Race
        public static void teleportCustomRace()
        {
            Vector3 flagPos = GameObject.Find("World/Races/race (6)/RaceFlag").GetComponent<BuildableObject>().transform.position;
            PlayerReference localPlayer = PlayerReferenceManager.Instance.GetLocalPlayerReference();
            localPlayer.PlayerControl.teleportationController.TeleportPlayer(
                flagPos,
                localPlayer.PlayerControl.movement.playerGraphics.transform.rotation
            );
        }


    }
}