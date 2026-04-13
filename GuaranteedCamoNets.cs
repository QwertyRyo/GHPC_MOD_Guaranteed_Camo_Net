using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using MelonLoader;
using GHPC;
using GHPC.Player;
using GHPC.Mission;
using GHPC.State;
using GHPC.Vehicle;


namespace GuaranteedCamoNets
{
   
    public class GuaranteedCamoNetsClass : MelonMod
    {
        public static GameObject gameManager;
        public static Dictionary<string, MelonPreferences_Entry<bool>> vehiclePrefs = new Dictionary<string, MelonPreferences_Entry<bool>>();

        static bool activeScene = false;
        static bool grafen = false;

        // cfg key (appears in melonpreferences.cfg) -> UniqueName (used to match vehicles)
        static readonly Dictionary<string, string> vehicleMap = new Dictionary<string, string>
        {
            { "T72",           "T72"           },
            { "T72_LEM",       "T72GILLS"      },
            { "T72_LEM_mod",   "T72ULEM"       },
            { "T72ÜV1",        "T72UV1"        },
            { "T72ÜV2",        "T72UV2"        },
            { "T72M",          "T72M"          },
            { "T72M1",         "T72M1"         },
            { "LEO1A3",        "LEO1A3"        },
            { "LEO1A3A1",      "LEO1A3A1"      },
            { "LEO1A3A2",      "LEO1A3A2"      },
            { "LEO1A3A3",      "LEO1A3A3"      },
            { "LEOA1A1",       "LEO1A1"       }, 
            { "LEOA1A2",       "LEO1A1A2"       },
            { "LEOA1A3",       "LEO1A1A3"       },
            { "LEOA1A4",       "LEO1A1A4"       },
            { "M1",            "M1"            },
            { "M1IP_Abrams",   "M1IP Abrams"   },
            { "M60A1",         "M60A1"         },
            { "M60A1_AOS",      "M60A1AOS"      },
            { "M60A1_RISE_PASSIVE",    "M60A1RISEP"    },
            { "M60A1_RISEP_77",  "M60A1RISEP77"  },
            { "M60A3",          "M60A3"         },
            { "M60A3TTS",      "M60A3TTS"      },
            { "PT76B",         "PT76B"         },
            { "MARDERA1PLUS",  "MARDERA1PLUS"  },
            { "MARDERA1MINUS", "MARDERA1MINUS" },
            { "MARDERA1MINUS_NO_ATGM", "MARDERA1_NO_ATGM" },
            { "M113G",         "M113G"         },
            { "BRDM2",         "BRDM2"         },
            { "BMP1_GDR",          "BMP1"          },
            { "BMP1_USSR",          "BMP1_SA"          },
            { "BMP1P_GDR",         "BMP1P"         },
            { "BMP1P_USSR",         "BMP1P_SA"         },
            { "BMP2_USSR",       "BMP2_SA"       },
            { "BMP2_GDR", "BMP2" },
            { "T62",           "T62"           },
            { "T64A_obr_1974",  "T64A74"          },
            { "T64A_obr_1979",  "T64A79"        },
            { "T64A_obr_1981",          "T64A81"        },
            { "T64A_obr_1983",          "T64A"          },
            { "T64A_obr_1984",        "T64A84"        },
            { "T64B_obr_1981",          "T64B81"},
            { "T64B1_obr_1981", "T64B181"},
            { "T64B1_obr_1984",          "T64B1"          },
            { "T64B_obr_1984",          "T64B"          },
            { "T80B",          "T80B"          },
            
        };

        public override void OnInitializeMelon()
        {
            MelonPreferences_Category cfg = MelonPreferences.CreateCategory("GuaranteedCamoNets");
            foreach (var kv in vehicleMap)
                vehiclePrefs[kv.Value] = cfg.CreateEntry(kv.Key, true);
            MelonPreferences.Save();
        }

       

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "MainMenu2_Scene" || sceneName == "t64_menu" || sceneName == "MainMenu2-1_Scene") 
            {
                activeScene = false;
                grafen = false;
                return; 
            }

            gameManager = GameObject.Find("_APP_GHPC_");
            if (gameManager == null) { return; }
            StateController.RunOrDefer(GameState.GameReady, new GameStateEventHandler(CamoFlagSet), GameStatePriority.Medium);
        }


        public IEnumerator CamoFlagSet(GameState _)
        {
            if (activeScene == true) { yield break; }
            activeScene = true;
            Vehicle[] list = GameObject.FindObjectsByType<Vehicle>(FindObjectsSortMode.None);

            foreach (var vehicle in list)
            {
                GameObject vehicle_go = vehicle.gameObject;
                if (vehicle_go == null) { continue; }
                //MelonLogger.Msg($"checking {vehicle.UniqueName}");
                if (!vehiclePrefs.ContainsKey(vehicle.UniqueName)) { continue; }
                bool enable = vehiclePrefs[vehicle.UniqueName].Value;

                //
                   

                switch (vehicle.UniqueName)
                {

                    case "T72":
                    {
                        Transform p;
                        p = vehicle.transform.Find("T72M_skirts_rig/HULL/t72m hull net");
                        if (p == null) MelonLogger.Error("T72: path not found: T72M_skirts_rig/HULL/t72m hull net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("T72M_skirts_rig/HULL/TURRET/t72m turret net");
                        if (p == null) MelonLogger.Error("T72: path not found: T72M_skirts_rig/HULL/TURRET/t72m turret net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("T72M_skirts_rig/HULL/TURRET/GUN/muzzle identity/t72m gun net");
                        if (p == null) MelonLogger.Error("T72: path not found: T72M_skirts_rig/HULL/TURRET/GUN/muzzle identity/t72m gun net"); else p.gameObject.SetActive(enable);
                        break;
                    }
                    case "T72ULEM":
                        {
                            Transform p;
                            p = vehicle.transform.Find("T72M_skirts_rig/HULL/t72m hull net");
                            if (p == null) MelonLogger.Error("T72 LEM mod: path not found: T72M_skirts_rig/HULL/t72m hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("T72M_skirts_rig/HULL/TURRET/t72m turret net");
                            if (p == null) MelonLogger.Error("T72 LEM mod: path not found: T72M_skirts_rig/HULL/TURRET/t72m turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("T72M_skirts_rig/HULL/TURRET/GUN/muzzle identity/t72m gun net");
                            if (p == null) MelonLogger.Error("T72 LEM mod: path not found: T72M_skirts_rig/HULL/TURRET/GUN/muzzle identity/t72m gun net"); else p.gameObject.SetActive(enable);
                            break;
                        }
                    case "T72UV1":
                        {
                            Transform p;
                            p = vehicle.transform.Find("T72M_skirts_rig/HULL/t72m hull net");
                            if (p == null) MelonLogger.Error("T72ÜV1: path not found: T72M_skirts_rig/HULL/t72m hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("T72M_skirts_rig/HULL/TURRET/t72m turret net");
                            if (p == null) MelonLogger.Error("T72ÜV1: path not found: T72M_skirts_rig/HULL/TURRET/t72m turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("T72M_skirts_rig/HULL/TURRET/GUN/muzzle identity/t72m gun net");
                            if (p == null) MelonLogger.Error("T72ÜV1: path not found: T72M_skirts_rig/HULL/TURRET/GUN/muzzle identity/t72m gun net"); else p.gameObject.SetActive(enable);
                            break;

                        }
                    case "T72UV2":
                        {
                            Transform p;
                            p = vehicle.transform.Find("T72M_skirts_rig/HULL/t72m hull net");
                            if (p == null) MelonLogger.Error("T72ÜV2: path not found: T72M_skirts_rig/HULL/t72m hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("T72M_skirts_rig/HULL/TURRET/t72m turret net");
                            if (p == null) MelonLogger.Error("T72ÜV2: path not found: T72M_skirts_rig/HULL/TURRET/t72m turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("T72M_skirts_rig/HULL/TURRET/GUN/muzzle identity/t72m gun net");
                            if (p == null) MelonLogger.Error("T72ÜV2: path not found: T72M_skirts_rig/HULL/TURRET/GUN/muzzle identity/t72m gun net"); else p.gameObject.SetActive(enable);
                            break;

                        }
                    case "T72M":
                        {
                            Transform p;
                            p = vehicle.transform.Find("T72M_skirts_rig/HULL/t72m hull net");
                            if (p == null) MelonLogger.Error("T72M: path not found: T72M_skirts_rig/HULL/t72m hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("T72M_skirts_rig/HULL/TURRET/t72m turret net");
                            if (p == null) MelonLogger.Error("T72M: path not found: T72M_skirts_rig/HULL/TURRET/t72m turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("T72M_skirts_rig/HULL/TURRET/GUN/muzzle identity/t72m gun net");
                            if (p == null) MelonLogger.Error("T72M: path not found: T72M_skirts_rig/HULL/TURRET/GUN/muzzle identity/t72m gun net"); else p.gameObject.SetActive(enable);

                            break;
                        }
                    case "T72M1":
                        {
                            Transform p;
                            p = vehicle.transform.Find("---MESH---/HULL/t72m hull net");
                            if (p == null) MelonLogger.Error("T72M1: path not found: ---MESH---/HULL/t72m hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---MESH---/HULL/TURRET/t72m turret net");
                            if (p == null) MelonLogger.Error("T72M1: path not found: ---MESH---/HULL/TURRET/t72m turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---MESH---/HULL/TURRET/GUN/muzzle identity/t72m gun net");
                            if (p == null) MelonLogger.Error("T72M1: path not found: ---MESH---/HULL/TURRET/GUN/muzzle identity/t72m gun net"); else p.gameObject.SetActive(enable);

                            break;

                        }
                    case "T72GILLS":
                        {
                            Transform p;
                            p = vehicle.transform.Find("T72M_skirts_rig/HULL/t72m hull net");
                            if (p == null) MelonLogger.Error("T72 LEM: path not found: T72M_skirts_rig/HULL/t72m hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("T72M_skirts_rig/HULL/TURRET/t72m turret net");
                            if (p == null) MelonLogger.Error("T72 LEM: path not found: T72M_skirts_rig/HULL/TURRET/t72m turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("T72M_skirts_rig/HULL/TURRET/GUN/muzzle identity/t72m gun net");
                            if (p == null) MelonLogger.Error("T72 LEM: path not found: T72M_skirts_rig/HULL/TURRET/GUN/muzzle identity/t72m gun net"); else p.gameObject.SetActive(enable);
                            break;
                        }
                    case "PT76B":
                    {
                        Transform p;
                        p = vehicle.transform.Find("PT76_rig/HULL/PT76 hull net");
                        if (p == null) MelonLogger.Error("PT76B: path not found: PT76_rig/HULL/PT76 hull net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("PT76_rig/HULL/TURRET/GUN/PT76 gun net");
                        if (p == null) MelonLogger.Error("PT76B: path not found: PT76_rig/HULL/TURRET/GUN/PT76 gun net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("PT76_rig/HULL/TURRET/PT76 turret net");
                        if (p == null) MelonLogger.Error("PT76B: path not found:PT76_rig/HULL/TURRET/PT76 turret net"); else p.gameObject.SetActive(enable);
                        break;
                    }
                    case "T64A":
                        {
                            Transform p;
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/t64a hull net");
                            if (p == null) MelonLogger.Error("T64A obr. 1983: path not found: ---T64A_MESH---/HULL/t64a hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/t64a turret net");
                            if (p == null) MelonLogger.Error("T64A obr. 1983: path not found: ---T64A_MESH---/HULL/TURRET/t64a turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net");
                            if (p == null) MelonLogger.Error("T64A obr. 1983: path not found: ---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net"); else p.gameObject.SetActive(enable);
                            break;

                        }
                    case "T64A81":
                        {
                            Transform p;
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/t64a hull net");
                            if (p == null) MelonLogger.Error("T64A obr. 1981: path not found: ---T64A_MESH---/HULL/t64a hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/t64a turret net");
                            if (p == null) MelonLogger.Error("T64A obr. 1981: path not found: ---T64A_MESH---/HULL/TURRET/t64a turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net");
                            if (p == null) MelonLogger.Error("T64A obr. 1981: path not found: ---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net"); else p.gameObject.SetActive(enable);
                            break;

                        }
                    case "T64A84":
                        {
                            Transform p;
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/t64a hull net");
                            if (p == null) MelonLogger.Error("T64A84: path not found: ---T64A_MESH---/HULL/t64a hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/t64a turret net");
                            if (p == null) MelonLogger.Error("T64A84: path not found: ---T64A_MESH---/HULL/TURRET/t64a turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net");
                            if (p == null) MelonLogger.Error("T64A84: path not found: ---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net"); else p.gameObject.SetActive(enable);
                            break;

                        }
                    case "T64A74":
                        {
                            Transform p;
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/t64a hull net");
                            if (p == null) MelonLogger.Error("T64A74: path not found: ---T64A_MESH---/HULL/t64a hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/t64a turret net");
                            if (p == null) MelonLogger.Error("T64A74: path not found: ---T64A_MESH---/HULL/TURRET/t64a turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net");
                            if (p == null) MelonLogger.Error("T64A74: path not found: ---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net"); else p.gameObject.SetActive(enable);
                            break;
                        }
                    case "T64A79":
                        {
                            Transform p;
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/t64a hull net");
                            if (p == null) MelonLogger.Error("T64A obr.1979: path not found: ---T64A_MESH---/HULL/t64a hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/t64a turret net");
                            if (p == null) MelonLogger.Error("T64A obr.1979: path not found: ---T64A_MESH---/HULL/TURRET/t64a turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net");
                            if (p == null) MelonLogger.Error("T64A obr.1979: path not found: ---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net"); else p.gameObject.SetActive(enable);
                            break;
                        }           
                    case "T64B":
                        {
                            Transform p;
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/t64a hull net");
                            if (p == null) MelonLogger.Error("T64B obr. 1984: path not found: ---T64A_MESH---/HULL/t64a hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/t64a turret net");
                            if (p == null) MelonLogger.Error("T64B obr. 1984: path not found: ---T64A_MESH---/HULL/TURRET/t64a turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net");
                            if (p == null) MelonLogger.Error("T64B obr. 1984: path not found: ---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net"); else p.gameObject.SetActive(enable);
                            break;

                        }
                    case "T64B81":
                        {
                            Transform p;
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/t64a hull net");
                            if (p == null) MelonLogger.Error("T64B obr. 1981: path not found: ---T64A_MESH---/HULL/t64a hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/t64a turret net");
                            if (p == null) MelonLogger.Error("T64B obr. 1981: path not found: ---T64A_MESH---/HULL/TURRET/t64a turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net");
                            if (p == null) MelonLogger.Error("T64B obr. 1981: path not found: ---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net"); else p.gameObject.SetActive(enable);
                            break;

                        }
                    case "T64B1":
                        {
                            Transform p;
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/t64a hull net");
                            if (p == null) MelonLogger.Error("T64B1 obr. 1984: path not found: ---T64A_MESH---/HULL/t64a hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/t64a turret net");
                            if (p == null) MelonLogger.Error("T64B1 obr. 1984: path not found: ---T64A_MESH---/HULL/TURRET/t64a turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net");
                            if (p == null) MelonLogger.Error("T64B1 obr. 1984: path not found: ---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net"); else p.gameObject.SetActive(enable);
                            break;
                        }
                    case "T64B181":
                        {
                            Transform p;
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/t64a hull net");
                            if (p == null) MelonLogger.Error("T64B1 obr. 1981: path not found: ---T64A_MESH---/HULL/t64a hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/t64a turret net");
                            if (p == null) MelonLogger.Error("T64B1 obr. 1981: path not found: ---T64A_MESH---/HULL/TURRET/t64a turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net");
                            if (p == null) MelonLogger.Error("T64B1 obr. 1981: path not found: ---T64A_MESH---/HULL/TURRET/Main gun/Muzzle identity/t64a gun net"); else p.gameObject.SetActive(enable);
                            break;
                        }
                    case "T80B":
                    {
                        Transform p;
                        p = vehicle.transform.Find("T80B_rig/HULL/TURRET/gun/---MAIN GUN SCRIPTS---/net_barrel");
                        if (p == null) MelonLogger.Error("T80B: path not found: T80B_rig/HULL/TURRET/gun/---MAIN GUN SCRIPTS---/net_barrel"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("T80B_rig/HULL/TURRET/---TURRET SCRIPTS---/net_turret");
                        if (p == null) MelonLogger.Error("T80B: path not found: T80B_rig/HULL/TURRET/---TURRET SCRIPTS---/net_turret"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("T80_camonet");
                        if (p == null) MelonLogger.Error("T80B: path not found: T80_camonet"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("T80_camonet/net_glacis");
                        if (p == null) MelonLogger.Error("T80B: path not found: T80_camonet/net_glacis"); else p.gameObject.SetActive(enable);
                        break;
                    }
                    case "M60A1":
                    {
                        Transform p;
                        p = vehicle.transform.Find("--RIG/hull/M60 hull net");
                        if (p == null) MelonLogger.Error("M60A1: path not found: --RIG/hull/M60 hull net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("--RIG/hull/turret/main gun mantlet/M60 mantlet net");
                        if (p == null) MelonLogger.Error("M60A1: path not found: --RIG/HULL/TURRET/GUN/main gun mantlet/M60 mantlet net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("--RIG/hull/turret/main gun mantlet/main gun mantlet001/M60 gun net");
                        if (p == null) MelonLogger.Error("M60A1: path not found: --RIG/HULL/TURRET/GUN/main gun mantlet/main gun mantlet001/M60 gun net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("--RIG/hull/turret/M60 turret net");
                        if (p == null) MelonLogger.Error("M60A1: path not found: --RIG/hull/turret/M60 turret net"); else p.gameObject.SetActive(enable);
                        break;

                    }
                    case "M60A1AOS":
                    {
                        Transform p;
                        p = vehicle.transform.Find("--RIG/hull/M60 hull net");
                        if (p == null) MelonLogger.Error("M60A1AOS: path not found: --RIG/hull/M60 hull net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("--RIG/hull/turret/main gun mantlet/M60 mantlet net");
                        if (p == null) MelonLogger.Error("M60A1AOS: path not found: --RIG/HULL/TURRET/GUN/main gun mantlet/M60 mantlet net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("--RIG/hull/turret/main gun mantlet/main gun mantlet001/M60 gun net");
                        if (p == null) MelonLogger.Error("M60A1AOS: path not found: --RIG/HULL/TURRET/GUN/main gun mantlet/main gun mantlet001/M60 gun net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("--RIG/hull/turret/M60 turret net");
                        if (p == null) MelonLogger.Error("M60A1AOS: path not found: --RIG/hull/turret/M60 turret net"); else p.gameObject.SetActive(enable);
                        break;
                    }
                    case "M60A1RISEP":
                    {
                        Transform p;
                        p = vehicle.transform.Find("--RIG/hull/M60 hull net");
                        if (p == null) MelonLogger.Error("M60A1RISEP: path not found: --RIG/hull/M60 hull net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("--RIG/hull/turret/main gun mantlet/M60 mantlet net");
                        if (p == null) MelonLogger.Error("M60A1RISEP: path not found: --RIG/HULL/TURRET/GUN/main gun mantlet/M60 mantlet net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("--RIG/hull/turret/main gun mantlet/main gun mantlet001/M60 gun net");
                        if (p == null) MelonLogger.Error("M60A1RISEP: path not found: --RIG/HULL/TURRET/GUN/main gun mantlet/main gun mantlet001/M60 gun net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("--RIG/hull/turret/M60 turret net");
                        if (p == null) MelonLogger.Error("M60A1RISEP: path not found: --RIG/hull/turret/M60 turret net"); else p.gameObject.SetActive(enable);
                        break;
                    }
                    case "M60A1RISEP77":
                        {
                        Transform p;
                        p = vehicle.transform.Find("--RIG/hull/M60 hull net");
                        if (p == null) MelonLogger.Error("M60A1RISEP77: path not found: --RIG/hull/M60 hull net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("--RIG/hull/turret/main gun mantlet/M60 mantlet net");
                        if (p == null) MelonLogger.Error("M60A1RISEP77: path not found: --RIG/HULL/TURRET/GUN/main gun mantlet/M60 mantlet net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("--RIG/hull/turret/main gun mantlet/main gun mantlet001/M60 gun net");
                        if (p == null) MelonLogger.Error("M60A1RISEP77: path not found: --RIG/HULL/TURRET/GUN/main gun mantlet/main gun mantlet001/M60 gun net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("--RIG/hull/turret/M60 turret net");
                        if (p == null) MelonLogger.Error("M60A1RISEP77: path not found: --RIG/hull/turret/M60 turret net"); else p.gameObject.SetActive(enable);
                        break;

                        }
                    case "M60A3":
                        {
                            Transform p;
                            p = vehicle.transform.Find("M60A3TTS_rig/hull/M60 hull net");
                            if (p == null) MelonLogger.Error("M60A3: path not found: M60A3TTS_rig/hull/M60 hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("M60A3TTS_rig/hull/turret/main gun mantlet/M60 mantlet net");
                            if (p == null) MelonLogger.Error("M60A3: path not found: M60A3TTS_rig/hull/turret/main gun mantlet/M60 mantlet net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("M60A3TTS_rig/hull/turret/main gun mantlet/main gun mantlet001/M60 gun net");
                            if (p == null) MelonLogger.Error("M60A3: path not found: M60A3TTS_rig/hull/turret/main gun mantlet/main gun mantlet001/M60 gun net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("M60A3TTS_rig/hull/turret/M60 turret net");
                            if (p == null) MelonLogger.Error("M60A3: path not found: --RIG/hull/turret/M60 turret net"); else p.gameObject.SetActive(enable);
                            break;
                        }
                    case "M60A3TTS":
                    {
                        Transform p;
                        p = vehicle.transform.Find("M60A3TTS_rig/hull/M60 hull net");
                        if (p == null) MelonLogger.Error("M60A3TTS: path not found: M60A3TTS_rig/hull/M60 hull net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("M60A3TTS_rig/hull/turret/main gun mantlet/M60 mantlet net");
                        if (p == null) MelonLogger.Error("M60A3TTS: path not found: M60A3TTS_rig/hull/turret/main gun mantlet/M60 mantlet net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("M60A3TTS_rig/hull/turret/main gun mantlet/main gun mantlet001/M60 gun net");
                        if (p == null) MelonLogger.Error("M60A3TTS: path not found: M60A3TTS_rig/hull/turret/main gun mantlet/main gun mantlet001/M60 gun net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("M60A3TTS_rig/hull/turret/M60 turret net");
                        if (p == null) MelonLogger.Error("M60A3TTS: path not found: --RIG/hull/turret/M60 turret net"); else p.gameObject.SetActive(enable);
                        break;
                    }
                    case "M1":
                    {
                        Transform p;
                        p = vehicle.transform.Find("IPM1_rig/HULL/TURRET/M1 camo net/turret_front_left");
                        if (p == null) MelonLogger.Error("M1: path not found: IPM1_rig/HULL/TURRET/M1 camo net/turret_front_left"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("IPM1_rig/HULL/TURRET/M1 camo net/turret_front_right");
                        if (p == null) MelonLogger.Error("M1: path not found: IPM1_rig/HULL/TURRET/M1 camo net/turret_front_right"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("IPM1_rig/HULL/TURRET/GUN/turret_gun");
                        if (p == null) MelonLogger.Error("M1: path not found: IPM1_rig/HULL/TURRET/GUN/turret_gun"); else p.gameObject.SetActive(enable);
                        break;
                    }
                    case "M1IP Abrams":
                    {
                        Transform turret = vehicle.transform.Find("IPM1_rig/HULL/TURRET");
                        if (turret == null) { MelonLogger.Error("M1IP Abrams: path not found: IPM1_rig/HULL/TURRET"); break; }
                        foreach (Transform child in turret)
                        {
                            if (child.name != "M1 camo net") { continue; }
                            Transform tfl = child.Find("turret_front_left");
                            if (tfl == null) MelonLogger.Error("M1IP Abrams: path not found: M1 camo net/turret_front_left"); else tfl.gameObject.SetActive(enable);
                            Transform tfr = child.Find("turret_front_right");
                            if (tfr == null) MelonLogger.Error("M1IP Abrams: path not found: M1 camo net/turret_front_right"); else tfr.gameObject.SetActive(enable);
                        }
                        Transform gun = vehicle.transform.Find("IPM1_rig/HULL/TURRET/GUN/turret_gun");
                        if (gun == null) MelonLogger.Error("M1IP Abrams: path not found: IPM1_rig/HULL/TURRET/GUN/turret_gun"); else gun.gameObject.SetActive(enable);
                        break;
                    }
                    case "LEO1A3A1":
                        {
                            Transform p;
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/1A3 hull net");
                            if (p == null) MelonLogger.Error("LEO1A3A1 path not found: LEO1A1A1_rig/HULL/1A3 hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A3 turret net");
                            if (p == null) MelonLogger.Error("LEO1A3A1 path not found: LEO1A1A1_rig/HULL/TURRET/1A3 turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 mantlet net");
                            if (p == null) MelonLogger.Error("LEO1A3A1 path not found: LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 mantlet net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 muzzle/1A3  gun net");
                            if (p == null) MelonLogger.Error("LEO1A3A1 path not found: LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 muzzle/1A3  gun net"); else p.gameObject.SetActive(enable);

                            break;
                        }
                    case "LEO1A3A2":
                        {
                            Transform p;
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/1A3 hull net");
                            if (p == null) MelonLogger.Error("LEO1A3A2 path not found: LEO1A1A1_rig/HULL/1A3 hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A3 turret net");
                            if (p == null) MelonLogger.Error("LEO1A3A2 path not found: LEO1A1A1_rig/HULL/TURRET/1A3 turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 mantlet net");
                            if (p == null) MelonLogger.Error("LEO1A3A2 path not found: LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 mantlet net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 muzzle/1A3  gun net");
                            if (p == null) MelonLogger.Error("LEO1A3A2 path not found: LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 muzzle/1A3  gun net"); else p.gameObject.SetActive(enable);

                            break;
                        }
                    case "LEO1A3A3":
                        {
                            Transform p;
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/1A3 hull net");
                            if (p == null) MelonLogger.Error("LEO1A3A3 path not found: LEO1A1A1_rig/HULL/1A3 hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A3 turret net");
                            if (p == null) MelonLogger.Error("LEO1A3A3 path not found: LEO1A1A1_rig/HULL/TURRET/1A3 turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 mantlet net");
                            if (p == null) MelonLogger.Error("LEO1A3A3 path not found: LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 mantlet net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 muzzle/1A3  gun net");
                            if (p == null) MelonLogger.Error("LEO1A3A3 path not found: LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 muzzle/1A3  gun net"); else p.gameObject.SetActive(enable);
                            break;

                        }
                    case "LEO1A3":
                        {
                            Transform p;
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/1A3 hull net");
                            if (p == null) MelonLogger.Error("LEO1A3: path not found: LEO1A1A1_rig/HULL/1A3 hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A3 turret net");
                            if (p == null) MelonLogger.Error("LEO1A3: path not found: LEO1A1A1_rig/HULL/TURRET/1A3 turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 mantlet net");
                            if (p == null) MelonLogger.Error("LEO1A3: path not found: LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 mantlet net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 muzzle/1A3  gun net");
                            if (p == null) MelonLogger.Error("LEO1A3: path not found: LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/1A3 muzzle/1A3  gun net"); else p.gameObject.SetActive(enable);

                            break;
                        }
                    case "LEO1A1A4":
                    {
                        Transform p;
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/1A1 hull net");
                        if (p == null) MelonLogger.Error("LEOA1A4: path not found: LEO1A1A1_rig/HULL/1A1 hull net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A1 turret net");
                        if (p == null) MelonLogger.Error("LEOA1A4: path not found: LEO1A1A1_rig/HULL/TURRET/1A1 turret net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/Mantlet/1A1 mantlet net");
                        if (p == null) MelonLogger.Error("LEOA1A4: path not found: LEO1A1A1_rig/HULL/TURRET/Mantlet/1A1 mantlet net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/Mantlet/gun/1A1  gun net");
                        if (p == null) MelonLogger.Error("LEOA1A4: path not found: LEO1A1A1_rig/HULL/TURRET/Mantlet/gun/1A1  gun net"); else p.gameObject.SetActive(enable);
                        break;
                    }
                    case "LEO1A1":
                    {
                        Transform p;
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/1A1 hull net");
                        if (p == null) MelonLogger.Error("LEOA1A1: path not found: LEO1A1A1_rig/HULL/1A1 hull net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A1 turret net");
                        if (p == null) MelonLogger.Error("LEOA1A1: path not found: LEO1A1A1_rig/HULL/TURRET/1A1 turret net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/Mantlet/1A1 mantlet net");
                        if (p == null) MelonLogger.Error("LEOA1A1: path not found: LEO1A1A1_rig/HULL/TURRET/Mantlet/1A1 mantlet net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/Mantlet/gun/1A1  gun net");
                        if (p == null) MelonLogger.Error("LEOA1A1: path not found: LEO1A1A1_rig/HULL/TURRET/Mantlet/gun/1A1  gun net"); else p.gameObject.SetActive(enable);
                        break;
                    }
                    case "LEO1A1A2":
                    {
                        Transform p;
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/1A1 hull net");
                        if (p == null) MelonLogger.Error("LEOA1A2: path not found: LEO1A1A1_rig/HULL/1A1 hull net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A1 turret net");
                        if (p == null) MelonLogger.Error("LEOA1A2: path not found: LEO1A1A1_rig/HULL/TURRET/1A1 turret net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/Mantlet/1A1 mantlet net");
                        if (p == null) MelonLogger.Error("LEOA1A2: path not found: LEO1A1A1_rig/HULL/TURRET/Mantlet/1A1 mantlet net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/Mantlet/gun/1A1  gun net");
                        if (p == null) MelonLogger.Error("LEOA1A2: path not found: LEO1A1A1_rig/HULL/TURRET/Mantlet/gun/1A1  gun net"); else p.gameObject.SetActive(enable);
                        break;
                    }
                    case "LEO1A1A3":
                    {
                        Transform p;
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/1A1 hull net");
                        if (p == null) MelonLogger.Error("LEOA1A3: path not found: LEO1A1A1_rig/HULL/1A1 hull net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/1A1 turret net");
                        if (p == null) MelonLogger.Error("LEOA1A3: path not found: LEO1A1A1_rig/HULL/TURRET/1A1 turret net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/Mantlet/1A1 mantlet net");
                        if (p == null) MelonLogger.Error("LEOA1A3: path not found: LEO1A1A1_rig/HULL/TURRET/Mantlet/1A1 mantlet net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("LEO1A1A1_rig/HULL/TURRET/Mantlet/gun/1A1  gun net");
                        if (p == null) MelonLogger.Error("LEOA1A3: path not found: LEO1A1A1_rig/HULL/TURRET/Mantlet/gun/1A1  gun net"); else p.gameObject.SetActive(enable);
                        break;
                    }
                    case "M113G":
                    {
                        Transform p;
                        p = vehicle.transform.Find("113G_nets");
                        if (p == null) MelonLogger.Error("M113G: path not found: 113G_nets"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("113G_nets/Box144");
                        if (p == null) MelonLogger.Error("M113G: path not found: 113G_nets/Box144"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("113G_nets/empty tie down rack001");
                        if (p == null) MelonLogger.Error("M113G: path not found: 113G_nets/empty tie down rack001"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("113G_nets/NatoCamoRoll");
                        if (p == null) MelonLogger.Error("M113G: path not found: 113G_nets/NatoCamoRoll"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("113G_nets/NatoCamoRoll001");
                        if (p == null) MelonLogger.Error("M113G: path not found: 113G_nets/NatoCamoRoll001"); else p.gameObject.SetActive(enable);
                        break;
                    }
                    case "MARDERA1PLUS":
                        {
                            Transform p;
                            p = vehicle.transform.Find("Marder1A1_rig/hull/Marder hull net");
                            if (p == null) MelonLogger.Error("MARDERA1PLUS: path not found: Marder1A1_rig/hull/Marder hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("Marder1A1_rig/hull/turret/Marder turret net");
                            if (p == null) MelonLogger.Error("MARDERA1PLUS: path not found: Marder1A1_rig/hull/turret/Marder turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("Marder1A1_rig/hull/turret/gun/Marder gun net");
                            if (p == null) MelonLogger.Error("MARDERA1PLUS: path not found: Marder1A1_rig/hull/turret/gun/Marder gun net"); else p.gameObject.SetActive(enable);
                            break;
                        }
                    case "MARDERA1MINUS":
                        {
                            Transform p;
                            p = vehicle.transform.Find("Marder1A1_rig/hull/Marder hull net");
                            if (p == null) MelonLogger.Error("MARDERA1MINUS: path not found: Marder1A1_rig/hull/Marder hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("Marder1A1_rig/hull/turret/Marder turret net");
                            if (p == null) MelonLogger.Error("MARDERA1MINUS: path not found: Marder1A1_rig/hull/turret/Marder turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("Marder1A1_rig/hull/turret/gun/Marder gun net");
                            if (p == null) MelonLogger.Error("MARDERA1MINUS: path not found: Marder1A1_rig/hull/turret/gun/Marder gun net"); else p.gameObject.SetActive(enable);
                            break;
                        }
                    case "MARDERA1_NO_ATGM":
                        {
                            Transform p;
                            p = vehicle.transform.Find("Marder1A1_rig/hull/Marder hull net");
                            if (p == null) MelonLogger.Error("MARDERA1_NO_ATGM: path not found: Marder1A1_rig/hull/Marder hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("Marder1A1_rig/hull/turret/Marder turret net");
                            if (p == null) MelonLogger.Error("MARDERA1_NO_ATGM: path not found: Marder1A1_rig/hull/turret/Marder turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("Marder1A1_rig/hull/turret/gun/Marder gun net");
                            if (p == null) MelonLogger.Error("MARDERA1_NO_ATGM: path not found: Marder1A1_rig/hull/turret/gun/Marder gun net"); else p.gameObject.SetActive(enable);
                            break;
                        }
                    case "MARDER1A2":
                        {
                            Transform p;
                            p = vehicle.transform.Find("Marder1A1_rig/hull/Marder hull net");
                            if (p == null) MelonLogger.Error("MARDER1A2: path not found: Marder1A1_rig/hull/Marder hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("Marder1A1_rig/hull/turret/Marder turret net");
                            if (p == null) MelonLogger.Error("MARDER1A2: path not found: Marder1A1_rig/hull/turret/Marder turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("Marder1A1_rig/hull/turret/gun/Marder gun net");
                            if (p == null) MelonLogger.Error("MARDER1A2: path not found: Marder1A1_rig/hull/turret/gun/Marder gun net"); else p.gameObject.SetActive(enable);
                            break;
                        }
                    case "BRDM2":
                    {
                        Transform p;
                        p = vehicle.transform.Find("BRDM2_rig/HULL/brdm hull net");
                        if (p == null) MelonLogger.Error("BRDM2: path not found: BRDM2_rig/HULL/brdm hull net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("BRDM2_rig/HULL/TURRET/brdm turret net");
                        if (p == null) MelonLogger.Error("BRDM2: path not found: BRDM2_rig/HULL/TURRET/brdm turret net"); else p.gameObject.SetActive(enable);
                        p = vehicle.transform.Find("BRDM2_rig/HULL/TURRET/GUN/brdm gun net");
                        if (p == null) MelonLogger.Error("BRDM2: path not found: BRDM2_rig/HULL/TURRET/GUN/brdm gun net"); else p.gameObject.SetActive(enable);
                        break;
                    }
                    case "T62":
                        {
                            Transform p;
                            p = vehicle.transform.Find("---T62_rig---/HULL/T62 hull net");
                            if (p == null) MelonLogger.Error("T62: path not found: ---T62_rig---/HULL/T62 hull net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T62_rig---/HULL/TURRET/T62 turret net");
                            if (p == null) MelonLogger.Error("T62: path not found: ---T62_rig---/HULL/TURRET/T62 turret net"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("---T62_rig---/HULL/TURRET/GUN/muzzle/T62 gun net");
                            if (p == null) MelonLogger.Error("T62: path not found: ---T62_rig---/HULL/TURRET/GUN/muzzle/T62 gun net"); else p.gameObject.SetActive(enable);
                        
                        break;
                        }
                    case "BMP2_SA":
                        {
                            Transform p;
                            p = vehicle.transform.Find("BMP2_rig/HULL/bmp2 net front");
                            if (p == null) MelonLogger.Error("BMP2_USSR: path not found: BMP2_rig/HULL/bmp2 net front"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP2_rig/HULL/bmp2 net side");
                            if (p == null) MelonLogger.Error("BMP2_USSR: path not found: BMP2_rig/HULL/bmp2 net side"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP2_rig/HULL/TURRET/bmp2 net turret");
                            if (p == null) MelonLogger.Error("BMP2_USSR: path not found: BMP2_rig/HULL/TURRET/bmp2 net turret"); else p.gameObject.SetActive(enable);

                            break;
                        }
                    case "BMP2":
                        {
                            Transform p;
                            p = vehicle.transform.Find("BMP2_rig/HULL/bmp2 net front");
                            if (p == null) MelonLogger.Error("BMP2_GDR: path not found: BMP2_rig/HULL/bmp2 net front"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP2_rig/HULL/bmp2 net side");
                            if (p == null) MelonLogger.Error("BMP2_GDR: path not found: BMP2_rig/HULL/bmp2 net side"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP2_rig/HULL/TURRET/bmp2 net turret");
                            if (p == null) MelonLogger.Error("BMP2_GDR: path not found: BMP2_rig/HULL/TURRET/bmp2 net turret"); else p.gameObject.SetActive(enable);

                            break;
                        }
                    case "BMP1":
                        {
                            Transform p;
                            p = vehicle.transform.Find("BMP1_rig/HULL/bmp1 net front");
                            if (p == null) MelonLogger.Error("BMP1: path not found: BMP1_rig/HULL/bmp1 net front"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP1_rig/HULL/bmp1 net sides");
                            if (p == null) MelonLogger.Error("BMP1: path not found: BMP1_rig/HULL/bmp1 net sides"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP1_rig/HULL/TURRET/bmp1 net turret");
                            if (p == null) MelonLogger.Error("BMP1: path not found: BMP1_rig/HULL/TURRET/bmp1 net turret"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP1_rig/HULL/TURRET/GUN/bmp1 net gun");
                            if (p == null) MelonLogger.Error("BMP1: path not found: BMP1_rig/HULL/TURRET/GUN/bmp1 net gun"); else p.gameObject.SetActive(enable);

                            break;
                        }
                    case "BMP1_SA":
                        {
                            Transform p;
                            p = vehicle.transform.Find("BMP1_rig/HULL/bmp1 net front");
                            if (p == null) MelonLogger.Error("BMP1_USSR: path not found: BMP1_rig/HULL/bmp1 net front"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP1_rig/HULL/bmp1 net sides");
                            if (p == null) MelonLogger.Error("BMP1_USSR: path not found: BMP1_rig/HULL/bmp1 net sides"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP1_rig/HULL/TURRET/bmp1 net turret");
                            if (p == null) MelonLogger.Error("BMP1_USSR: path not found: BMP1_rig/HULL/TURRET/bmp1 net turret"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP1_rig/HULL/TURRET/GUN/bmp1 net gun");
                            if (p == null) MelonLogger.Error("BMP1_USSR: path not found: BMP1_rig/HULL/TURRET/GUN/bmp1 net gun"); else p.gameObject.SetActive(enable);

                            break;
                        }
                    case "BMP1P":
                        {
                            Transform p;
                            p = vehicle.transform.Find("BMP1_rig/HULL/bmp1 net front");
                            if (p == null) MelonLogger.Error("BMP1P_GDR: path not found: BMP1_rig/HULL/bmp1 net front"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP1_rig/HULL/bmp1 net sides");
                            if (p == null) MelonLogger.Error("BMP1P_GDR: path not found: BMP1_rig/HULL/bmp1 net sides"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP1_rig/HULL/TURRET/bmp1 net turret");
                            if (p == null) MelonLogger.Error("BMP1P_GDR: path not found: BMP1_rig/HULL/TURRET/bmp1 net turret"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP1_rig/HULL/TURRET/GUN/bmp1 net gun");
                            if (p == null) MelonLogger.Error("BMP1P_GDR: path not found: BMP1_rig/HULL/TURRET/GUN/bmp1 net gun"); else p.gameObject.SetActive(enable);

                            break;
                        }
                    case "BMP1P_SA":
                    {
                            Transform p;
                            p = vehicle.transform.Find("BMP1_rig/HULL/bmp1 net front");
                            if (p == null) MelonLogger.Error("BMP1P_USSR: path not found: BMP1_rig/HULL/bmp1 net front"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP1_rig/HULL/bmp1 net sides");
                            if (p == null) MelonLogger.Error("BMP1P_USSR: path not found: BMP1_rig/HULL/bmp1 net sides"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP1_rig/HULL/TURRET/bmp1 net turret");
                            if (p == null) MelonLogger.Error("BMP1P_USSR: path not found: BMP1_rig/HULL/TURRET/bmp1 net turret"); else p.gameObject.SetActive(enable);
                            p = vehicle.transform.Find("BMP1_rig/HULL/TURRET/GUN/bmp1 net gun");
                            if (p == null) MelonLogger.Error("BMP1P_USSR: path not found: BMP1_rig/HULL/TURRET/GUN/bmp1 net gun"); else p.gameObject.SetActive(enable);

                            break;
                        }


                    default:
                        continue;
                }
            }

            if (grafen) {
                Unit newVic = Object.FindAnyObjectByType<Unit>();
                gameManager.GetComponent<PlayerInput>().SetPlayerUnit(newVic);
            }
            activeScene = false;
            yield break;            
        }
    }    
}
