using MelonLoader;
using System.Reflection;
using HarmonyLib;
using Il2Cpp;
using UnityEngine;
using CustomKeybindApi;

namespace BetterRacingBis
{
    public class Main : MelonMod
    {

        public override void OnInitializeMelon()
        {
            var modColor = typeof(Main).Assembly.GetCustomAttribute<MelonColorAttribute>();
            string modColorString = (modColor != null)
                ? $"\x1b[38;2;{modColor.DrawingColor.R};{modColor.DrawingColor.G};{modColor.DrawingColor.B}m"
                : "\x1b[38;2;0;255;255m";

            LoggerInstance.Msg($"{modColorString}{Info.Name}\x1b[0m wishes you a good racing session!");

            Il2Cpp_Scripts.Managers.RaceManager.CELEBRATION_TIMER = 1.5f;
        }

        public override void OnUpdate() => RaceUI.Update();
        public override void OnGUI() => RaceUI.Draw();

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "Main Mountain Scene")
            {
                Transform raceContainer = GameObject.Find("World/Races").transform;

                for (int i = 0; i < raceContainer.childCount; i++)
                {
                    GameObject race = raceContainer.GetChild(i).gameObject;

                    if (!race.activeSelf && race.name != "race (6)")
                    {
                        race.SetActive(true);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PlayerRacingController), nameof(PlayerRacingController.SetRacingStatus))]
        public static class SetRacingStatusPatch
        {
            [HarmonyPostfix]
            public static void PostFix(RacingState newRacingState)
            {
                if (newRacingState == RacingState.Finished)
                {
                    GeneralUtils.TeleportToRaceStart();
                }

                if (newRacingState == RacingState.Countdown)
                {
                    GeneralUtils.RestartingRace = false;
                }
            }
        }
        [HarmonyPatch(typeof(LobbySettingsManager), nameof(LobbySettingsManager.OnStartServer))]
        public static class OnStartServerPatch
        {
            [HarmonyPostfix]
            public static void PostFix()
            {
                GameObject.Find("Managers / Handlers/Networked/Yeti Manager").active = false;
            }
        }
    }
}