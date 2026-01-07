using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using GlobeTrottersAnimationStates;
using UnityEngine;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace GlobeTrottersFix;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    public Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
    public static string gtf = "gtf";

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        harmony.PatchAll();

        AddEventTemplates("globeTrotters", ref GlobeTrottersEventTemplates.templates);
        AddEventTemplates("globeTrottersFire", ref GlobeTrottersFireEventTemplates.templates);
    }

    private static void AddEventTemplates(string gameName, ref MixtapeEventTemplate[] templates)
    {
        MixtapeEventTemplate[] newTemplates =
        [
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_zoomOutRotateBackgroundSlow",
                length = 0.5f,
                properties = new Dictionary<string, object> { ["_beatOffset"] = 0f }
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_rotateBackgroundSlow",
                length = 0.5f,
                properties = new Dictionary<string, object> { ["_beatOffset"] = 0f }
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_zoomOutRotatePlanets",
                length = 0.5f,
                properties = new Dictionary<string, object> { ["_beatOffset"] = 0f }
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/_rotatePlanets",
                length = 0.5f,
                properties = new Dictionary<string, object> { ["_beatOffset"] = 0f }
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_zoomOutMultiplePlanets",
                length = 0.5f,
                properties = new Dictionary<string, object> { ["_beatOffset"] = 0f }
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/_rotateBackgroundFast",
                length = 0.5f,
                properties = new Dictionary<string, object> { ["_beatOffset"] = 0f }
            },

            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_zoomIntro1",
                length = 0.5f,
                properties = []
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_zoomIntro2",
                length = 0.5f,
                properties = []
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_zoomIntro3",
                length = 0.5f,
                properties = []
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_zoomClose",
                length = 0.5f,
                properties = []
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/_zoomEnd1",
                length = 0.5f,
                properties = []
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/_zoomEnd2",
                length = 0.5f,
                properties = []
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_zoomManual",
                length = 0.5f,
                properties = new Dictionary<string, object>
                {
                    ["x"] = 0f,
                    ["y"] = 0f,
                    ["z"] = -10f
                }
            },

            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_setState",
                length = 0.5f,
                properties = new Dictionary<string, object>
                {
                    ["_beatOffset"] = 0f,
                    ["state"] = new MixtapeEventTemplates.ChoiceField<string>(
                    [
                        "ZoomedInNotRotating",
                        "ZoomedOutBackgroundRotatingSlow",
                        "ZoomedInBackgroundRotatingSlow",
                        "ZoomedOutPlanetsRotatingFast",
                        "ZoomedInPlanetsRotatingFast",
                        "ZoomedOutMultiplePlanetsRotatingFast",
                        "ZoomedInBackgroundRotatingFast"
                    ]),
                    ["zoom"] = new MixtapeEventTemplates.ChoiceField<string>(
                    [
                        "intro1",
                        "intro2",
                        "intro3",
                        "gameClose",
                        "gameFar",
                        "outro1",
                        "outro2",
                        "manual",
                    ]),
                    ["x"] = 0f,
                    ["y"] = 0f,
                    ["z"] = -10f
                }
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_planetDistance",
                length = 0.5f,
                properties = new Dictionary<string, object>
                {
                    ["d"] = 9.150001f,
                }
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_flip",
                length = 0.5f,
                properties = new Dictionary<string, object>
                {
                    ["flip"] = false,
                    ["immediate"] = true,
                    ["which"] = -1
                }
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_activateMarbles",
                length = 0.5f,
                properties = []
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_deactivateMarbles",
                length = 0.5f,
                properties = []
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_swapPlanet",
                length = 0.5f,
                properties = new Dictionary<string, object>
                {
                    ["topPlanet"] = false,
                    ["index"] = 0
                }
            },
            new MixtapeEventTemplate
            {
                dataModel = $"{gameName}/{gtf}/_marcherAnimation",
                length = 0.5f,
                properties = new Dictionary<string, object>
                {
                    ["animation"] = new MixtapeEventTemplates.ChoiceField<string>(
                    [
                        "Bop",
                        "Ready"
                    ]),
                    ["which"] = -1
                }
            }
        ];

        MixtapeEventTemplate[] allTemplates = new MixtapeEventTemplate[templates.Length + newTemplates.Length];
        templates.CopyTo(allTemplates, 0);
        newTemplates.CopyTo(allTemplates, templates.Length);

        templates = allTemplates;
    }

    [HarmonyPatch(typeof(GlobeTrottersScript), "BeginInternal")]
    private static class GlobeTrottersScriptBeginInternalPatch
    {
        static readonly Type enumStateType = AccessTools.Inner(typeof(GlobeTrottersScript), "State");

        static readonly AccessTools.FieldRef<GlobeTrottersScript, Vector3> intro1PosRef =
            AccessTools.FieldRefAccess<GlobeTrottersScript, Vector3>("intro1Pos");

        static readonly AccessTools.FieldRef<GlobeTrottersScript, Vector3> intro2PosRef =
            AccessTools.FieldRefAccess<GlobeTrottersScript, Vector3>("intro2Pos");

        static readonly AccessTools.FieldRef<GlobeTrottersScript, Vector3> intro3PosRef =
            AccessTools.FieldRefAccess<GlobeTrottersScript, Vector3>("intro3Pos");

        static readonly AccessTools.FieldRef<GlobeTrottersScript, Vector3> gameClosePosRef =
           AccessTools.FieldRefAccess<GlobeTrottersScript, Vector3>("gameClosePos");

        static readonly AccessTools.FieldRef<GlobeTrottersScript, Vector3> gameFarPosRef =
           AccessTools.FieldRefAccess<GlobeTrottersScript, Vector3>("gameFarPos");

        static readonly AccessTools.FieldRef<GlobeTrottersScript, Vector3> outro1PosRef =
           AccessTools.FieldRefAccess<GlobeTrottersScript, Vector3>("outro1Pos");

        static readonly AccessTools.FieldRef<GlobeTrottersScript, Vector3> outro2PosRef =
           AccessTools.FieldRefAccess<GlobeTrottersScript, Vector3>("outro2Pos");

        static readonly AccessTools.FieldRef<GlobeTrottersScript, Animator[]> extraSmallMarchersRef =
           AccessTools.FieldRefAccess<GlobeTrottersScript, Animator[]>("extraSmallMarchers");

        static readonly AccessTools.FieldRef<GlobeTrottersScript, Animator[]> extraSmallMarchersTopRef =
           AccessTools.FieldRefAccess<GlobeTrottersScript, Animator[]>("extraSmallMarchersTop");

        static readonly MethodInfo isMixtapeOrCustomGetter = AccessTools.PropertyGetter(typeof(GameplayScript), "IsMixtapeOrCustom");
        static bool IsMixtapeOrCustom(GameplayScript obj) => (bool)isMixtapeOrCustomGetter.Invoke(obj, null);

        static readonly MethodInfo gameNameGetter = AccessTools.PropertyGetter(typeof(GlobeTrottersScript), "GameName");
        static string GameName(GlobeTrottersScript obj) => (string)gameNameGetter.Invoke(obj, null);

        static readonly MethodInfo setStateMethod = AccessTools.Method(typeof(GlobeTrottersScript), "SetState", [enumStateType, typeof(float), typeof(Vector3)]);
        static void SetState(GlobeTrottersScript obj, int state, float startBeat, Vector3 cameraPos) => setStateMethod.Invoke(obj, [Enum.ToObject(enumStateType, state), startBeat, cameraPos]);

        static readonly MethodInfo flipPlayerMethod = AccessTools.Method(typeof(GlobeTrottersScript), "FlipPlayer", [typeof(bool), typeof(bool)]);
        static void FlipPlayer(GlobeTrottersScript obj, bool flip, bool immediate = false) => flipPlayerMethod.Invoke(obj, [flip, immediate]);

        static readonly MethodInfo flipMethod = AccessTools.Method(typeof(GlobeTrottersScript), "Flip", [typeof(bool)]);
        static void Flip(GlobeTrottersScript obj, bool flip) => flipMethod.Invoke(obj, [flip]);

        static void Prefix(GlobeTrottersScript __instance, Entity[] entities)
        {
            if (IsMixtapeOrCustom(__instance))
            {
                foreach (var entity in entities)
                {
                    string[] entityArray = entity.dataModel.Split(['/'], 3);
                    if (entityArray.Length < 3)
                    {
                        continue;
                    }
                    string gameName = entityArray[0];
                    string command = entityArray[1];
                    string subcommand = entityArray[2];
                    if (gameName != GameName(__instance) || command != gtf)
                    {
                        continue;
                    }
                    switch (subcommand)
                    {
                        case "_zoomIntro1":
                            __instance.scheduler.Schedule(entity.beat, delegate
                            {
                                __instance.mainCamera.transform.localPosition = intro1PosRef(__instance);
                            });
                            break;
                        case "_zoomIntro2":
                            __instance.scheduler.Schedule(entity.beat, delegate
                            {
                                __instance.mainCamera.transform.localPosition = intro2PosRef(__instance);
                            });
                            break;
                        case "_zoomIntro3":
                            __instance.scheduler.Schedule(entity.beat, delegate
                            {
                                __instance.mainCamera.transform.localPosition = intro3PosRef(__instance);
                            });
                            break;
                        case "_zoomClose":
                            __instance.scheduler.Schedule(entity.beat, delegate
                            {
                                __instance.mainCamera.transform.localPosition = gameClosePosRef(__instance);
                            });
                            break;
                        case "_zoomManual":
                            float x = entity.GetFloat("x");
                            float y = entity.GetFloat("y");
                            float z = entity.GetFloat("z");
                            __instance.scheduler.Schedule(entity.beat, delegate
                            {
                                __instance.mainCamera.transform.localPosition = new Vector3(x, y, z);
                            });
                            break;
                        case "_zoomOutRotateBackgroundSlow":
                            __instance.scheduler.Schedule(entity.beat, delegate
                            {
                                SetState(__instance, 1, entity.beat - entity.GetFloat("_beatOffset"), gameFarPosRef(__instance));
                                __instance.marbles2.SetActive(value: false);
                                __instance.marbles3.SetActive(value: false);
                            });
                            break;
                        case "_rotateBackgroundSlow":
                            __instance.scheduler.Schedule(entity.beat, delegate
                            {
                                SetState(__instance, 2, entity.beat - entity.GetFloat("_beatOffset"), gameClosePosRef(__instance));
                                __instance.planet.transform.SetY(-9.150001f);
                                __instance.topPlanet.transform.SetY(9.150001f);
                            });
                            break;
                        case "_zoomOutRotatePlanets":
                            __instance.scheduler.Schedule(entity.beat, delegate
                            {
                                SetState(__instance, 3, entity.beat - entity.GetFloat("_beatOffset"), gameFarPosRef(__instance));
                                __instance.marbles2.SetActive(value: false);
                                __instance.marbles3.SetActive(value: false);
                            });
                            break;
                        case "_zoomOutMultiplePlanets":
                            __instance.scheduler.Schedule(entity.beat, delegate
                            {
                                SetState(__instance, 5, entity.beat - entity.GetFloat("_beatOffset"), gameFarPosRef(__instance));
                                __instance.marbles2.SetActive(value: true);
                                __instance.marbles3.SetActive(value: true);
                            });
                            break;

                        case "_setState":
                            string stateName = entity.GetString("state");
                            int state;
                            switch (stateName)
                            {
                                case "ZoomedInNotRotating":
                                    state = 0;
                                    break;
                                case "ZoomedOutBackgroundRotatingSlow":
                                    state = 1;
                                    break;
                                case "ZoomedInBackgroundRotatingSlow":
                                    state = 2;
                                    break;
                                case "ZoomedOutPlanetsRotatingFast":
                                    state = 3;
                                    break;
                                case "ZoomedInPlanetsRotatingFast":
                                    state = 4;
                                    break;
                                case "ZoomedOutMultiplePlanetsRotatingFast":
                                    state = 5;
                                    break;
                                case "ZoomedInBackgroundRotatingFast":
                                    state = 6;
                                    break;
                                default:
                                    state = -1;
                                    break;
                            }

                            string zoomName = entity.GetString("zoom");
                            Vector3 zoom;
                            switch (zoomName)
                            {
                                case "intro1":
                                    zoom = intro1PosRef(__instance);
                                    break;
                                case "intro2":
                                    zoom = intro2PosRef(__instance);
                                    break;
                                case "intro3":
                                    zoom = intro3PosRef(__instance);
                                    break;
                                case "gameClose":
                                    zoom = gameClosePosRef(__instance);
                                    break;
                                case "gameFar":
                                    zoom = gameFarPosRef(__instance);
                                    break;
                                case "outro1":
                                    zoom = outro1PosRef(__instance);
                                    break;
                                case "outro2":
                                    zoom = outro2PosRef(__instance);
                                    break;
                                default:
                                    float x2 = entity.GetFloat("x");
                                    float y2 = entity.GetFloat("y");
                                    float z2 = entity.GetFloat("z");
                                    zoom = new Vector3(x2, y2, z2);
                                    break;
                            }
                            __instance.scheduler.Schedule(entity.beat, delegate
                            {
                                SetState(__instance, state, entity.beat - entity.GetFloat("_beatOffset"), zoom);
                            });
                            break;

                        case "_planetDistance":
                            float d = entity.GetFloat("d");
                            __instance.scheduler.Schedule(entity.beat, delegate
                            {
                                __instance.planet.transform.SetY(-d);
                                __instance.topPlanet.transform.SetY(d);
                            });
                            break;

                        case "_flip":
                            {
                                var which = entity.GetInt("which");
                                bool flip = entity.GetBool("flip");
                                bool immediate = entity.GetBool("immediate");
                                if (which == -1)
                                {
                                    __instance.scheduler.Schedule(entity.beat, delegate
                                    {
                                        FlipPlayer(__instance, flip, immediate);
                                        FlipMarchers(__instance, flip);
                                    });
                                }
                                else if (which == 0)
                                {
                                    __instance.scheduler.Schedule(entity.beat, delegate
                                    {
                                        FlipPlayer(__instance, flip, immediate);
                                    });
                                }
                                else if (which == -2)
                                {
                                    __instance.scheduler.Schedule(entity.beat, delegate
                                    {
                                        FlipMarchers(__instance, flip);
                                    });
                                }
                                else
                                {
                                    var (marcher, smallMarcher) = IndexToMarcherAnimator(__instance, which);
                                    __instance.scheduler.Schedule(entity.beat, delegate
                                    {
                                        FlipMarcher(__instance, flip, marcher, smallMarcher);
                                    });
                                }
                                break;
                            }

                        case "_activateMarbles":
                            __instance.scheduler.Schedule(entity.beat, delegate
                            {
                                __instance.marbles2.SetActive(value: true);
                                __instance.marbles3.SetActive(value: true);
                            });
                            break;
                        case "_deactivateMarbles":
                            __instance.scheduler.Schedule(entity.beat, delegate
                            {
                                __instance.marbles2.SetActive(value: false);
                                __instance.marbles3.SetActive(value: false);
                            });
                            break;

                        case "_swapPlanet":
                            int index = entity.GetInt("index");
                            if (index >= __instance.largePlanetSprites.Length)
                            {
                                throw new MixtapeException($"Invalid planet sprite: {index} (min 0, max {__instance.largePlanetSprites.Length - 1})");
                            }
                            if (entity.GetBool("topPlanet"))
                            {
                                __instance.scheduler.Schedule(entity.beat, delegate
                                {
                                    __instance.topPlanet.SetSprite(__instance.largePlanetSprites[index]);
                                });
                            }
                            else
                            {
                                __instance.scheduler.Schedule(entity.beat, delegate
                                {
                                    __instance.planet.SetSprite(__instance.largePlanetSprites[index]);
                                });
                            }
                            break;

                        case "_marcherAnimation":
                            {
                                var animState = entity.GetString("animation") == "Bop" ? Marcher.Bop : Marcher.Ready;
                                var which = entity.GetInt("which");
                                if (which == -1)
                                {
                                    __instance.scheduler.Schedule(entity.beat, delegate
                                    {
                                        __instance.marcher.SetState(animState);
                                        if (__instance.smallMarcher != null)
                                        {
                                            __instance.smallMarcher.SetState(animState);
                                        }
                                        Animator[] marchers = __instance.npcMarchers;
                                        for (int i = 0; i < marchers.Length; i++)
                                        {
                                            marchers[i].SetState(animState);
                                        }
                                        marchers = __instance.smallMarchers;
                                        for (int i = 0; i < marchers.Length; i++)
                                        {
                                            marchers[i].SetState(animState);
                                        }
                                    });
                                }
                                else
                                {
                                    var (marcher, smallMarcher) = IndexToMarcherAnimator(__instance, which);
                                    __instance.scheduler.Schedule(entity.beat, delegate
                                    {
                                        marcher.SetState(animState);
                                        if (smallMarcher != null)
                                        {
                                            smallMarcher.SetState(animState);
                                        }
                                    });
                                }
                                break;
                            }
                    }
                }
            }
        }

        private static (Animator, Animator) IndexToMarcherAnimator(GlobeTrottersScript __instance, int index)
        {
            if (index == 0)
            {
                return (__instance.marcher, __instance.smallMarcher);
            }
            int index2 = index - 1;
            Animator[] marchers;
            Animator[] smallMarchers;
            if (index2 < __instance.npcMarchers.Length)
            {
                marchers = __instance.npcMarchers;
                smallMarchers = __instance.smallMarchers;
            }
            else
            {
                index2 -= __instance.npcMarchers.Length;
                if (index2 < __instance.extraMarchers.Length)
                {
                    marchers = __instance.extraMarchers;
                    smallMarchers = extraSmallMarchersRef(__instance);
                }
                else
                {
                    index2 -= __instance.extraMarchers.Length;
                    if (index2 < __instance.extraMarchersTop.Length)
                    {
                        marchers = __instance.extraMarchersTop;
                        smallMarchers = extraSmallMarchersTopRef(__instance);
                    }
                    else
                    {
                        throw new MixtapeException($"Invalid marcher index: {index}");
                    }
                }
            }
            return (marchers[index2], index2 < smallMarchers.Length ? smallMarchers[index2] : null);
        }

        private static void FlipMarchers(GlobeTrottersScript __instance, bool flip)
        {
            var topPlanet = __instance.topPlanet;
            var planet = __instance.planet;
            var topSmallPlanet = __instance.topSmallPlanet;
            var smallPlanet = __instance.smallPlanet;

            Animator[] marchers = __instance.npcMarchers;
            foreach (Animator marcher in marchers)
            {
                marcher.transform.parent.SetParent(flip ? topPlanet.transform : planet.transform, worldPositionStays: false);
                float num = marcher.transform.parent.localEulerAngles.z - (flip ? (0f - planet.TargetZ) : topPlanet.TargetZ);
                marcher.transform.parent.localEulerAngles = new Vector3(0f, 0f, (flip ? topPlanet.TargetZ : (0f - planet.TargetZ)) + num);
            }
            marchers = __instance.smallMarchers;
            foreach (Animator marcher in marchers)
            {
                marcher.transform.parent.SetParent(flip ? topSmallPlanet.transform : smallPlanet.transform, worldPositionStays: false);
                float num2 = marcher.transform.parent.localEulerAngles.z - (flip ? (0f - smallPlanet.TargetZ) : topSmallPlanet.TargetZ);
                marcher.transform.parent.localEulerAngles = new Vector3(0f, 0f, (flip ? topSmallPlanet.TargetZ : (0f - smallPlanet.TargetZ)) + num2);
            }
        }

        private static void FlipMarcher(GlobeTrottersScript __instance, bool flip, Animator marcher, Animator smallMarcher)
        {
            var topPlanet = __instance.topPlanet;
            var planet = __instance.planet;

            marcher.transform.parent.SetParent(flip ? topPlanet.transform : planet.transform, worldPositionStays: false);
            float num = marcher.transform.parent.localEulerAngles.z - (flip ? (0f - planet.TargetZ) : topPlanet.TargetZ);
            marcher.transform.parent.localEulerAngles = new Vector3(0f, 0f, (flip ? topPlanet.TargetZ : (0f - planet.TargetZ)) + num);

            if (smallMarcher != null)
            {
                var topSmallPlanet = __instance.topSmallPlanet;
                var smallPlanet = __instance.smallPlanet;

                smallMarcher.transform.parent.SetParent(flip ? topSmallPlanet.transform : smallPlanet.transform, worldPositionStays: false);
                float num2 = smallMarcher.transform.parent.localEulerAngles.z - (flip ? (0f - smallPlanet.TargetZ) : topSmallPlanet.TargetZ);
                smallMarcher.transform.parent.localEulerAngles = new Vector3(0f, 0f, (flip ? topSmallPlanet.TargetZ : (0f - smallPlanet.TargetZ)) + num2);
            }
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var codeMatcher = new CodeMatcher(instructions, il);

            // fix beatsSinceSwap to not % 8
            codeMatcher.MatchForward(false, [
                Ldloc_S(24),
                Ldloc_S(5),
                new CodeMatch(OpCodes.Sub),
                new CodeMatch(OpCodes.Ldc_I4_8),
                new CodeMatch(OpCodes.Rem),
                Stloc_S(28)
                ])
                .ThrowIfNotMatch("Could not find \"int num6 = (num4 - num3) % 8;\"")
                .Advance(3)
                .SetOpcodeAndAdvance(OpCodes.Nop)
                .SetOpcodeAndAdvance(OpCodes.Nop)
                .Advance(1);

            // prevent beatsSinceSwap from == 6
            codeMatcher.MatchForward(false, [
                Ldloc_S(28),
                new CodeMatch(OpCodes.Ldc_I4_6),
                new CodeMatch(ci => ci.opcode == OpCodes.Bne_Un || ci.opcode == OpCodes.Bne_Un_S)
                ])
                .ThrowIfNotMatch("Could not find \"if (num6 == 6)\"")
                .Advance(1)
                .SetOpcodeAndAdvance(OpCodes.Ldc_I4_0)
                .Advance(1);

            // prevent beatsSinceSwap from == 7
            codeMatcher.MatchForward(false, [
                Ldloc_S(28),
                new CodeMatch(OpCodes.Ldc_I4_7),
                new CodeMatch(ci => ci.opcode == OpCodes.Bne_Un || ci.opcode == OpCodes.Bne_Un_S)
                ])
                .ThrowIfNotMatch("Could not find \"if (num6 == 7)\"")
                .Advance(1)
                .SetOpcodeAndAdvance(OpCodes.Ldc_I4_0)
                .Advance(1);

            return codeMatcher.InstructionEnumeration();
        }
    }

    private static CodeMatch Ldloc_S(int index)
    {
        return new CodeMatch(ci =>
            ci.opcode == OpCodes.Ldloc_S &&
            ci.operand is LocalBuilder lb &&
            lb.LocalIndex == index
            );
    }

    private static CodeMatch Stloc_S(int index)
    {
        return new CodeMatch(ci =>
            ci.opcode == OpCodes.Stloc_S &&
            ci.operand is LocalBuilder lb &&
            lb.LocalIndex == index
            );
    }
}
