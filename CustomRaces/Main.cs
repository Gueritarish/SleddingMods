using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;
using CustomCommandApi;
using Il2Cpp_Scripts.Systems.Chat;

namespace CustomRaces
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg($"Initialized {Info.Name}.");
            CommandHandler.NewCommand("Start", CreateStartRace, 0);
            CommandHandler.NewCommand("START", CreateStartRace, 0);
            CommandHandler.NewCommand("start", CreateStartRace, 0);
            CommandHandler.NewCommand("st", CreateStartRace, 0);
            CommandHandler.NewCommand("ST", CreateStartRace, 0);
            CommandHandler.NewCommand("St", CreateStartRace, 0);
            CommandHandler.NewCommand("sT", CreateStartRace, 0);


            CommandHandler.NewCommand("Reset", ResetEndFlag, 0);
            CommandHandler.NewCommand("reset", ResetEndFlag, 0);
            CommandHandler.NewCommand("RESET", ResetEndFlag, 0);
            CommandHandler.NewCommand("rst", ResetEndFlag, 0);
            CommandHandler.NewCommand("RST", ResetEndFlag, 0);
            CommandHandler.NewCommand("Rst", ResetEndFlag, 0);

            CommandHandler.NewCommand("Finish", CreateEndRace, 0);
            CommandHandler.NewCommand("FINISH", CreateEndRace, 0);
            CommandHandler.NewCommand("finish", CreateEndRace, 0);
            CommandHandler.NewCommand("Fin", CreateEndRace, 0);
            CommandHandler.NewCommand("fin", CreateEndRace, 0);
            CommandHandler.NewCommand("FIN", CreateEndRace, 0);
            CommandHandler.NewCommand("END", CreateEndRace, 0);
            CommandHandler.NewCommand("End", CreateEndRace, 0);
            CommandHandler.NewCommand("end", CreateEndRace, 0);
        }
        
        static public string CreateStartRace(string[] args)
        {
            BuiltObjectManager builtObjectManager = GameObject.Find("Managers / Handlers/Networked/Built Object Manager").GetComponent<BuiltObjectManager>();
            Vector3 playerPosition = PlayerReferenceManager.Instance.GetLocalPlayerReference().PlayerControl.transform.position;
            
            // Enable template race
            GameObject race = GameObject.Find("World/Races/race (6)");
            race.SetActive(true);

            
            // Grabbing Starting flag from race 6 which is a template race used for testing
            BuildableObject startingRaceFlag = GameObject.Find("World/Races/race (6)/RaceFlag").GetComponent<BuildableObject>();
            startingRaceFlag.transform.position = playerPosition;
            
            MelonLogger.Msg("Custom Start Created!");
            return "Custom Start Created!";
        }
        static public void ResetEndFlagFun()
        {
            // Grabbing finishes flag from race 6 because it has two finish badly placed
            BuildableObject raceFinishFlag1 = GameObject.Find("World/Races/race (6)/RaceFlag (1)").GetComponent<BuildableObject>();
            BuildableObject raceFinishFlag2 = GameObject.Find("World/Races/race (6)/RaceFlag (2)").GetComponent<BuildableObject>();
            UnityEngine.Vector3 redFinishFlagPosition = GameObject.Find("World/Races/race (2)/RaceFlag (1)").GetComponent<BuildableObject>().transform.position;
            raceFinishFlag1.transform.position = redFinishFlagPosition;
            raceFinishFlag2.transform.position = redFinishFlagPosition;
            MelonLogger.Msg("EndFlag Position Reset!");
        }
        static public string ResetEndFlag(string[] args)
        {
            ResetEndFlagFun();
            return "EndFlag Position Reset!";
        }
        static public string CreateEndRace(string[] args)
        {
            // Grabbing finishes flag from race 6 because it has two finish badly placed
            BuildableObject raceFinishFlag1 = GameObject.Find("World/Races/race (6)/RaceFlag (1)").GetComponent<BuildableObject>();
            BuildableObject raceFinishFlag2 = GameObject.Find("World/Races/race (6)/RaceFlag (2)").GetComponent<BuildableObject>();
            Vector3 playerPosition = PlayerReferenceManager.Instance.GetLocalPlayerReference().PlayerControl.transform.position;
            raceFinishFlag1.transform.position = playerPosition;
            raceFinishFlag2.transform.position = playerPosition;
            MelonLogger.Msg("Custom End Created!");
            return "Custom End Created!";
        }

        [HarmonyPatch(typeof(ChatManager), nameof(Il2Cpp_Scripts.Systems.Chat.ChatManager.SendChatMessage))]
        public static class SendChatMessagePatch
        {
            [HarmonyPrefix]
            public static bool PreFix(string cleanedString)
            {
                return CommandHandler.RunCommand(cleanedString);
            }
        }

        [HarmonyPatch(typeof(ChatCommands), nameof(ChatCommands.GetCommand))]
        public static class GetCommandPatch
        {
            [HarmonyPrefix]
            public static bool PreFix(string message, out string arguments, ref CommandType __result)
            {
                arguments = "";

                __result = (CommandType)68;

                return CommandHandler.RunCommand(message);
            }
        }

        [HarmonyPatch(typeof(LobbySettingsManager), nameof(LobbySettingsManager.OnStartServer))]
        public static class OnStartServerPatch
           {
            [HarmonyPostfix]
            public static void PostFix()
            {
                ResetEndFlagFun();
                GameObject.Find("Managers / Handlers/Networked/Yeti Manager").active = false;
            }
        }
    }
}