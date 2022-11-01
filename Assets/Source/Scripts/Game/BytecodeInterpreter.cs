using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Scripts.Game
{
    public static class BytecodeInterpreter
    {
        public static bool win;

        public static IEnumerator Execute(string code, Crane crane)
        {
            if (win) yield break;
            int instructionPointer = 0; 
            bool result = false; // Condition
            bool exif = false; // Check for the condition in result?
            int looplevel = 0; // Nested Loops
            int loopn = 0; // Amount to loop for, -1 is infinite (until break;
            int totalInstructions = 0;
            List<int> loopAmount = new List<int>();
            while (instructionPointer < code.Length)
            {
                for (int x = 0; x < Level.dimensions.Item1; x++)
                {
                    for (int y = 0; y < Level.dimensions.Item2; y++)
                    {
                        if (Level.levelMap[x, y] == Tile.Target && Level.propMap[x, y] == Prop.Crate)
                        {
                            win = true;
                        }
                    }
                }
                Level.crane.ExecuteSwitchMap(Level.switchMap);
                if ((exif && result) || !exif || exif && (code[instructionPointer] == '!')) // Either the condition is met, or there's no checking for conditions
                {
                    exif = false;
                    switch (code[instructionPointer])
                    {
                        case '<': // Move Downwards/Leftwards
                            crane.MoveDownLeft();
                            break;
                        case '>': // Move Upwards/Rightwards
                            crane.MoveUpRight();
                            break;
                        case '/':
                            crane.HookUp();
                            break;
                        case '\\':
                            crane.HookDown();
                            break;
                        case '.':
                            crane.Attach();
                            break;
                        case ',':
                            crane.Detach();
                            break;
                        case '+':
                            crane.Extend();
                            break;
                        case '-':
                            crane.Retract();
                            break;
                        case 'd':
                            crane.TurnRight();
                            break;
                        case 'a':
                            crane.TurnLeft();
                            break;
                        case '[':
                            looplevel++;
                            loopAmount.Add(loopn);
                            loopn = 0;
                            break;
                        case ']':
                            loopAmount[looplevel - 1]--;
                            if (loopAmount[looplevel - 1] == 0)
                            {
                                looplevel--;
                                loopAmount.RemoveAt(looplevel - 1);
                            }
                            else if (loopAmount[looplevel - 1] < 0)
                                loopAmount[looplevel - 1] = -1;
                            else
                            {
                                instructionPointer = code.LastIndexOf('[', 0, instructionPointer) + 1;
                            }
                            break;
                        case '(':
                            if (!result)
                            {
                                instructionPointer = code.IndexOf(')', instructionPointer);
                            }
                            break;
                        case ')':

                            break;
                        case 'C':
                            result = crane.IsAboveTile(Tile.Crate);
                            exif = true;
                            break;
                        case '!':
                            result = !result;
                            break;
                        case 'T':
                            result = crane.IsAboveTile(Tile.Target);
                            exif = true;
                            break;
                        case 'S':
                            result = crane.IsAboveTile(Tile.Switch);
                            exif = true;
                            break;
                        case '*':
                            loopn = -1;
                            break;
                        case ';':
                            looplevel--;
                            loopAmount.RemoveAt(looplevel - 1);
                            instructionPointer = code.IndexOf("]", instructionPointer) + 1;
                            break;
                        case '|':
                            result = crane.IsAtEnd(true);
                            exif = true;
                            break;
                        case '_':
                            result = crane.IsAtEnd(false);
                            exif = true;
                            break;
                        default: 
                            break;
                    }
                    if (code[instructionPointer] > '0' && code[instructionPointer] <= '9') // 2 Numbers after each other will overwrite the last one, useful for if statements
                    {
                        loopn = code[instructionPointer] - '0';
                    }
                    else if (loopn > 0) // Direct Loop, doesn't work with star
                    {
                        loopn--;
                        if (loopn > 0) instructionPointer--;
                    }
                    instructionPointer++;
                }
                else exif = false;
                yield return null;
                totalInstructions++;
                if (totalInstructions > 32000) break;
            }
        }
    }
}



/* Example Program
 * 
 *  _________________
 *  | T |   | t |   |
 *  L___L___L___L___|
 *  |   |   | t |   |
 *  L___L___L___L___|
 *  | B |   | K |   |
 *  L___L___L___L___|
 *  | C |   | t |   |
 *  L___L___L___L___|
 * 
 * K = Crane
 * C = Crate
 * T = Target
 * B = Block (Obstacle)
 * t = Track
 * 
 * The Program:
 * 
 * <4d+.-3>+,
 * (Crane's initial position is pointing to the right)
 * 
 * <            Crane moves one position down 
 * 4            Sets loopn to 4                      
 * d            Performs a 45° rotation to the right --> repeats until loopn = 0, so 4 times
 * +            extends beam to 2 fiels (allows to reach 2 fields straight, or 1 field diagonally)
 * .            Attach lying Crate
 * -            retract beam to 1 field (allows to reach 1 field straight, or 1 field diagonally)
 * 3            Sets loopn to 3
 * >            Crane moves one position up --> repeats until loopn = 0, so 3 times
 * +            extends beam to 2 fields
 * ,            Sets down Crate on Target => Win
 * 
 * 
 * This of course only scratches the surface of the high level stuff the code can do but idk if i implement hard enough
 * levels anyway afterward. i'll have to see about that.
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */