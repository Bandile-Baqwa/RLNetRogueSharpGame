using OpenTK.Graphics.ES20;
using RLNETConsoleGame.Core;
using RogueSharp.DiceNotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNETConsoleGame.Systems
{
    public class CommandSystem
    {
        public bool MovePlayer(Direction direction)     // return true if the player was abel to moveand reutrn false if not ie hit a wall or seomthing 
        {
            int x = Game.Player.X;
            int y = Game.Player.Y;

            switch (direction)
            {
                case Direction.Up:
                    {
                        y = Game.Player.Y - 1;
                        break;
                    }
                    case Direction.Down:
                    {
                        y = Game.Player.Y + 1;
                        break;
                    }
                case Direction.Left:
                    {
                        x = Game.Player.X - 1;
                        break;
                    }
                case Direction.Right:
                    {
                        x = Game.Player.X + 1;
                            break;
                    }
                default:
                    {
                        return false;
                    }
            }
            if (Game.DungeonMap.SetActorPosition(Game.Player, x, y))    //this moves the plaer to a new position and return the result ,true if moved false if didnt 
            {
                return true;
            }
            return false;

            Monster monster = Game.DungeonMap.GetMonsterAt(x, y);
            if (monster != null)
            {
                Attack(Game.Player, monster);
                return true;
            }
        }

        public void Attack(Actor attacker, Actor defender)
        {
            StringBuilder attackMessage = new StringBuilder();
            StringBuilder defenseMessage = new StringBuilder();

            int hits = ResolveAttack(attacker, defender, attackMessage);    // these willl be the para passed into the respective methods 
            int blocks = ResolveDefense(defender, hits, attackMessage , defenseMessage);

            Game.MessageLog.Add(attackMessage.ToString());
            if (!string.IsNullOrWhiteSpace(defenseMessage.ToString()))
            {
                Game.MessageLog.Add(defenseMessage.ToString());
            }

            int damage = hits - blocks;
            ResolveDamage(defender, damage);
        }

        //the attacker rolls based on his stats to see if her gets any hits
        public static int ResolveAttack(Actor attacker, Actor defender, StringBuilder attackMessage)
        {
            int hits = 0;

            attackMessage.AppendFormat("{0} attacks {1} and rolls :", attacker.Name, defender.Name);

            //Roll a number of 100 sided dice to get the attack value of the attacking actor 
            DiceExpression attackDice = new DiceExpression().Dice(attacker.Attack, 100);
            DiceResult attackResult = attackDice.Roll();

            foreach (TermResult termResult in attackResult.Results)
            {
                //compare the value to 100 subtracted by attack chance
               if (termResult.Value >= 100 - attacker.AttackChance)
                {
                    hits++;
                }
            }
                return hits;
        }

        public static int ResolveDefense(Actor defender, int hits, StringBuilder attackMessage, StringBuilder defenseMessage)
        {
            int blocks = 0;
            if (hits > 0)
            {
                defenseMessage.AppendFormat("Scoring {0} hits ", hits);
                defenseMessage.AppendFormat("{0} defends and rolls : ", defender.Name);

                //same as the attack dice expression but for defence ,, 100 side dice fo rattack value subtrack the defending actor 
                DiceExpression defenseDice = new DiceExpression().Dice(defender.Defense, 100);
                DiceResult defenseResult = defenseDice.Roll();

                foreach (TermResult termResult in defenseResult.Results)
                {
                    defenseMessage.Append(termResult.Value + ",");

                    if (termResult.Value >= 100 - defender.DefenseChance)
                    {
                        blocks++;
                    }
                }
                defenseMessage.AppendFormat("resulting in {0} blocks", blocks);
            }
            else
            {
                attackMessage.Append("and misses completely");
            }
            return blocks;
        }

        private static void ResolveDamage(Actor defender, int damage) // daamage taken by the defender
        {
            if (damage > 0)
            {
                defender.Health = defender.Health - damage;

                Game.MessageLog.Add($"{defender.Name} was a hit for {damage} damage");

                if (defender.Health <= 0)
                {
                    ResolveDeath(defender);
                }
            }
            else
            {
                Game.MessageLog.Add($"DAMM ,{defender.Name} blocked all your attacks");
            }
        }

        private static void ResolveDeath(Actor defender)            // the dealth of the player or monster
        {
            if (defender is Player)
            {
                Game.MessageLog.Add($"{defender.Name} was killed but remember Sterrings never die Try Again, GAME OVER !!");
            }
            else if (defender is Monster)
            {
                Game.DungeonMap.RemoveMonsters((Monster)defender);
                Game.MessageLog.Add($"You Killed a {defender.Name} and it dropped {defender.Gold} Gold");
            }
        }
        }
    }

