using HarmonyLib;
using Overload;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;


////////////////////////////////////////////////////////////////////////////////////////////////////
/// Author: luponix                                                                              ///
/// Issue: When dead the hud displays two black bars at the top and bottom.                      ///
///        These bars have a fixed width of 1000 which is to little at very high aspect ratios.  ///
////////////////////////////////////////////////////////////////////////////////////////////////////


namespace GameMod
{
    class FixWidescreenDeathBars
    {
        
        [HarmonyPatch(typeof(UIManager), "DrawOverlay")]
        class FixWidescreenDeathBars_UIManager_DrawOverlay
        {
            static IEnumerable<CodeInstruction> Transpiler(ILGenerator ilGen, IEnumerable<CodeInstruction> instructions)   
            {
              int state = 0;
              var codes = new List<CodeInstruction>(instructions);
              for( int i = 0; i < codes.Count; i++)
              {
                // look for -460
                if( state == 0 && codes[i].opcode == OpCodes.Ldc_R4 && (float)codes[i].operand == -460f)
                {
                  state = 1;
                }

                // and change the 1000 after it
                if( state == 1 && codes[i].opcode == OpCodes.Ldc_R4 && (float)codes[i].operand == 1000f)
                {
                  codes[i].operand = 3000f;
                  state = 2;
                }

                // look for 460
                if( state == 2 && codes[i].opcode == OpCodes.Ldc_R4 && (float)codes[i].operand == 460f)
                {
                  state = 3;
                }
               
                // and change the 1000 after it
                if( state == 3 && codes[i].opcode == OpCodes.Ldc_R4 && (float)codes[i].operand == 1000f)
                {
                  codes[i].operand = 3000f;
                  state = 4;
                }
                yield return codes[i];
                
              }
            }

        }
        
    }
}

