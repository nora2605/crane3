using System.Collections;
using System.Collections.Generic;

namespace Assets.Source.Scripts.Game
{
    public static class BytecodeInterpreter
    {
        public static bool win;
        public static int totalInstructions = 0;
        public static int instructionPointer = 0;

        public static IEnumerator Execute(string code, Crane crane)
        {
            if (win)
            {
                yield break;
            }

            bool result = false; // Condition
            bool exif = false; // Check for the condition in result?
            int looplevel = 0; // Nested Loops
            int loopn = 0; // Amount to loop for, -1 is infinite (until break;
            List<int> loopAmount = new();
            instructionPointer = 0;
            while (instructionPointer < code.Length)
            {
                if ((exif && result) || !exif || (exif && (code[instructionPointer] == '!'))) // Either the condition is met, or there's no checking for conditions
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
                            Level.crane.ExecuteSwitchMap(Level.switchMap);
                            break;
                        case ',':
                            crane.Detach();
                            Level.crane.ExecuteSwitchMap(Level.switchMap);
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
                            goto SkipDelay;
                        case ']':
                            loopAmount[looplevel - 1]--;
                            if (loopAmount[looplevel - 1] == 0)
                            {
                                looplevel--;
                                loopAmount.RemoveAt(looplevel);
                                break;
                            }
                            else if (loopAmount[looplevel - 1] < 0)
                            {
                                loopAmount[looplevel - 1] = -1;
                            }

                            instructionPointer = code.LastIndexOf('[', instructionPointer - 1);
                            goto SkipDelay;
                        case '(':
                            if (!result)
                            {
                                instructionPointer = code.IndexOf(')', instructionPointer);
                            }
                            goto SkipDelay;
                        case ')':
                            goto SkipDelay;
                        case 'C':
                            result = crane.IsAboveTile(Tile.Crate);
                            exif = true;
                            goto SkipDelay;
                        case '!':
                            result = !result;
                            goto SkipDelay;
                        case 'T':
                            result = crane.IsAboveTile(Tile.Target);
                            exif = true;
                            goto SkipDelay;
                        case 'S':
                            result = crane.IsAboveTile(Tile.Switch);
                            exif = true;
                            goto SkipDelay;
                        case '*':
                            loopn = -1;
                            goto SkipDelay;
                        case ';':
                            loopAmount.RemoveAt(--looplevel);
                            instructionPointer = code.IndexOf("]", instructionPointer);
                            goto SkipDelay;
                        case '|':
                            result = crane.IsAtEnd(true);
                            exif = true;
                            goto SkipDelay;
                        case '_':
                            result = crane.IsAtEnd(false);
                            exif = true;
                            goto SkipDelay;
                        default:
                            if (code[instructionPointer] is not (> '0' and <= '9'))
                            {
                                goto SkipDelay;
                            }
                            break;
                    }
                    if (code[instructionPointer] is > '0' and <= '9') // 2 Numbers after each other will overwrite the last one, useful for if statements
                    {
                        loopn = code[instructionPointer] - '0';
                        goto SkipDelay;
                    }
                    else if (loopn > 0) // Direct Loop, doesn't work with star
                    {
                        loopn--;
                        if (loopn > 0)
                        {
                            instructionPointer--;
                        }
                    }
                    yield return null;
                }
                else
                {
                    exif = false;
                }

                SkipDelay:
                instructionPointer++;
                totalInstructions++;
                if (totalInstructions > 32000)
                {
                    break;
                }
            }
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