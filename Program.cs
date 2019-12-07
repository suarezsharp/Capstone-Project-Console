using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Capstone_Project_Console
{
    class Program
        //************************
        //Title: RPG Battle Simulator
        //Application Type: Console .Net Framework
        //Description: A framework for creating basic RPG battle sequences.
        //Author: Alex Suarez
        //Date Created: 12/7/19
        //Last Modified: 12/7/19
        //************************
    {
        static void Main(string[] args)
        {
            DisplayTitleCard();
            List<FriendlyCharacter> party = new List<FriendlyCharacter>();
            party = ReadCharacterList();
            DoBattleSequence(party);
            DisplayExitCard();
        }

        static void DoBattleSequence(List<FriendlyCharacter> party)
        {
            ConsoleKey menuChoice;
            ConsoleKeyInfo menuKey;

            int[] mainSelectionIndex = { 0, 0 };
            int[] subSelectionIndex = { 0, 0 };
            int targetIndex = 7;
            int partyOrderIndex = 0;
            int useID;
            Random random = new Random();
            int enemyTarget;
            bool isContinued = true;
            FriendlyCharacter.StatusEffect[] checkStatus = new FriendlyCharacter.StatusEffect[2];
            EnemyCharacter.StatusEffect[] checkEnemyStatus = new EnemyCharacter.StatusEffect[2];


            RefreshCharacters(party);

            ChatLog chatLog = new ChatLog();
            chatLog.InitializeChat();

            List<(FriendlyCharacter, string, EnemyCharacter)> commandList
                = new List<(FriendlyCharacter, string, EnemyCharacter)>();
            List<(EnemyCharacter, string, FriendlyCharacter)> enemyCommandList
                = new List<(EnemyCharacter, string, FriendlyCharacter)>();

            List<String> itemList = new List<String>()
            {
                "Fire Mote",
                "Thunder Mote",
                "Blizzard Mote"
            };
            List<String> spellList = new List<String>()
            {
                "Fight",
                "Fire",
                "Thunder",
                "Blizzard",
                "Sleep"
            };

            List<EnemyCharacter> enemyParty = new List<EnemyCharacter>()
            {
                new EnemyCharacter(EnemyCharacter.EnemyType.SNAKE)
                {
                    Name = "Booper",
                },
                new EnemyCharacter(EnemyCharacter.EnemyType.SNAKE)
                {
                    Name = "Tanker",
                    MaxHealth = 50,
                    BaseResistance = 4
                },
                new EnemyCharacter(EnemyCharacter.EnemyType.SNAKE)
                {
                    Name = "Biter",
                }
            };
            RefreshCharacters(enemyParty);

            do
            {
                //Turn start - Loops until all commands are received
                partyOrderIndex = 0;
                if (party[partyOrderIndex].Status == FriendlyCharacter.StatusEffect.KO) partyOrderIndex += 1;
                if (party[partyOrderIndex].Status == FriendlyCharacter.StatusEffect.KO) partyOrderIndex += 1;
                commandList.Clear();
                enemyCommandList.Clear();
                targetIndex = -1;
                mainSelectionIndex[0] = 0;
                mainSelectionIndex[1] = 0;
                //BEGINNING OF COMMAND INPUT PHASE
                do
                {
                    //Main selection window - Main hub for each command
                    if (party[partyOrderIndex].Status == FriendlyCharacter.StatusEffect.KO) partyOrderIndex += 1;
                    if (party[partyOrderIndex].Status == FriendlyCharacter.StatusEffect.KO) partyOrderIndex += 1;
                    PrintBattleFrame(party, enemyParty, mainSelectionIndex, subSelectionIndex,
                        partyOrderIndex, targetIndex, chatLog);
                    menuKey = Console.ReadKey();
                    menuChoice = menuKey.Key;
                    //Input register switch case for MAIN MENU
                    switch (menuChoice)
                    {
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            mainSelectionIndex[1]++;
                            if (mainSelectionIndex[1] > 1) mainSelectionIndex[1] = 0;
                            break;
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            mainSelectionIndex[0]++;
                            if (mainSelectionIndex[0] > 1) mainSelectionIndex[0] = 0;
                            break;
                        case ConsoleKey.Enter:
                            if (mainSelectionIndex[0] == 0 && mainSelectionIndex[1] == 0)
                            {
                                // if "FIGHT"
                                mainSelectionIndex[0] = -1;
                                mainSelectionIndex[1] = -1;
                                subSelectionIndex[0] = 0;
                                subSelectionIndex[1] = 0;
                                do
                                {
                                    //Sub-selection window

                                    PrintBattleFrame(party, enemyParty, mainSelectionIndex, subSelectionIndex,
                                        partyOrderIndex, targetIndex, chatLog);
                                    PrintListBar(spellList, subSelectionIndex);

                                    menuKey = Console.ReadKey();
                                    menuChoice = menuKey.Key;
                                    //Input register for SUB-SELECTION SPELL LIST MENU
                                    switch (menuChoice)
                                    {
                                        case ConsoleKey.W:
                                        case ConsoleKey.UpArrow:
                                            subSelectionIndex[1]--;
                                            if (subSelectionIndex[1] < 0) subSelectionIndex[1] = (int)(spellList.Count - 1) / 3;
                                            if (subSelectionIndex[1] > spellList.Count - (3 * subSelectionIndex[1]) - subSelectionIndex[0])
                                                subSelectionIndex[1] = 0;
                                            break;
                                        case ConsoleKey.S:
                                        case ConsoleKey.DownArrow:
                                            subSelectionIndex[1]++;
                                            if (subSelectionIndex[1] > ((spellList.Count - 1) / 3)) subSelectionIndex[1] = 0;
                                            if (subSelectionIndex[1] > spellList.Count - (3 * subSelectionIndex[1]) - subSelectionIndex[0])
                                                subSelectionIndex[1] = 0;
                                            break;
                                        case ConsoleKey.A:
                                        case ConsoleKey.LeftArrow:
                                            subSelectionIndex[0]--;
                                            if (subSelectionIndex[0] < 0)
                                            {
                                                subSelectionIndex[0] = 2;
                                                if (spellList.Count <= 3 * subSelectionIndex[1] + subSelectionIndex[0]) subSelectionIndex[0] = 1;
                                                if (spellList.Count <= 3 * subSelectionIndex[1] + subSelectionIndex[0]) subSelectionIndex[0] = 0;

                                            }
                                            break;
                                        case ConsoleKey.D:
                                        case ConsoleKey.RightArrow:
                                            subSelectionIndex[0]++;
                                            if (subSelectionIndex[0] > 2) subSelectionIndex[0] = 0;
                                            if (subSelectionIndex[0] >= spellList.Count - (3 * subSelectionIndex[1])) subSelectionIndex[0] = 0;
                                            break;
                                        case ConsoleKey.Enter:
                                            useID = 3 * subSelectionIndex[1] + subSelectionIndex[0];
                                            if (spellList[useID] == "Sleep" && party[partyOrderIndex].Mana < 20)
                                            {
                                                chatLog.ChatLine = $"{party[partyOrderIndex].Name} doesn't have enough mana to cast {spellList[useID]}.";
                                                break;
                                            }
                                            else if (spellList[useID] != "Fight" && party[partyOrderIndex].Mana < 10)
                                            {
                                                chatLog.ChatLine = $"{party[partyOrderIndex].Name} doesn't have enough mana to cast {spellList[useID]}.";
                                                break;
                                            }
                                            //Targeting menu
                                            targetIndex = 3;
                                            if (enemyParty[targetIndex - 3].Status == EnemyCharacter.StatusEffect.KO) targetIndex += 1;
                                            if (enemyParty[targetIndex - 3].Status == EnemyCharacter.StatusEffect.KO) targetIndex += 1;
                                            do
                                            {
                                                PrintBattleFrame(party, enemyParty, mainSelectionIndex, subSelectionIndex,
                                                                                partyOrderIndex, targetIndex, chatLog);
                                                PrintListBar(spellList, subSelectionIndex);
                                                menuKey = Console.ReadKey();
                                                menuChoice = menuKey.Key;
                                                switch (menuChoice)
                                                {
                                                    case ConsoleKey.W:
                                                    case ConsoleKey.UpArrow:
                                                        targetIndex--;
                                                        if (targetIndex < 3) targetIndex += 3;
                                                        if (enemyParty[targetIndex - 3].Status == EnemyCharacter.StatusEffect.KO) targetIndex -= 1;
                                                        if (targetIndex < 3) targetIndex += 3;
                                                        if (enemyParty[targetIndex - 3].Status == EnemyCharacter.StatusEffect.KO) targetIndex -= 1;
                                                        if (targetIndex < 3) targetIndex += 3;
                                                        break;
                                                    case ConsoleKey.S:
                                                    case ConsoleKey.DownArrow:
                                                        targetIndex++;
                                                        if (targetIndex >= 6) targetIndex -= 3;
                                                        if (enemyParty[targetIndex - 3].Status == EnemyCharacter.StatusEffect.KO) targetIndex += 1;
                                                        if (targetIndex >= 6) targetIndex -= 3;
                                                        if (enemyParty[targetIndex - 3].Status == EnemyCharacter.StatusEffect.KO) targetIndex += 1;
                                                        if (targetIndex >= 6) targetIndex -= 3;
                                                        break;
                                                    case ConsoleKey.Enter:
                                                        (FriendlyCharacter, string, EnemyCharacter) command;
                                                        useID = 3 * subSelectionIndex[1] + subSelectionIndex[0];
                                                        command = (party[partyOrderIndex], spellList[useID], enemyParty[targetIndex - 3]);
                                                        commandList.Add(command);
                                                        targetIndex = -1;
                                                        partyOrderIndex++;
                                                        break;
                                                    case ConsoleKey.Escape:
                                                        targetIndex = -1;
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            } while (targetIndex != -1);
                                            subSelectionIndex[1] = -1;
                                            mainSelectionIndex[0] = 0;
                                            mainSelectionIndex[1] = 0;
                                            break;
                                        case ConsoleKey.Escape:
                                            subSelectionIndex[1] = -1;
                                            mainSelectionIndex[0] = 0;
                                            mainSelectionIndex[1] = 0;
                                            break;
                                        default:
                                            break;
                                    }
                                } while (subSelectionIndex[1] != -1);
                            }
                            if (mainSelectionIndex[0] == 1 && mainSelectionIndex[1] == 0)
                            {
                                //if "PARTY"
                                PartyTable(party);
                            }
                            if (mainSelectionIndex[0] == 0 && mainSelectionIndex[1] == 1)
                            {
                                //if "ITEM"
                                mainSelectionIndex[0] = -1;
                                mainSelectionIndex[1] = -1;
                                subSelectionIndex[0] = 0;
                                subSelectionIndex[1] = 0;

                                if (itemList.Count == 0)
                                {
                                    chatLog.ChatLine = "You do not have any items.";
                                    subSelectionIndex[1] = -1;
                                    mainSelectionIndex[0] = 0;
                                    mainSelectionIndex[1] = 0;
                                }
                                if (itemList.Count > 0)
                                    do
                                    {
                                        //Sub-selection window

                                        PrintBattleFrame(party, enemyParty, mainSelectionIndex, subSelectionIndex,
                                            partyOrderIndex, targetIndex, chatLog);
                                        PrintListBar(itemList, subSelectionIndex);

                                        menuKey = Console.ReadKey();
                                        menuChoice = menuKey.Key;
                                        //Input register for SUB-SELECTION ITEM MENU
                                        switch (menuChoice)
                                        {
                                            case ConsoleKey.W:
                                            case ConsoleKey.UpArrow:
                                                subSelectionIndex[1]--;
                                                if (subSelectionIndex[1] < 0) subSelectionIndex[1] = (int)(itemList.Count - 1) / 3;
                                                break;
                                            case ConsoleKey.S:
                                            case ConsoleKey.DownArrow:
                                                subSelectionIndex[1]++;
                                                if (subSelectionIndex[1] > ((itemList.Count - 1) / 3)) subSelectionIndex[1] = 0;
                                                break;
                                            case ConsoleKey.A:
                                            case ConsoleKey.LeftArrow:
                                                subSelectionIndex[0]--;
                                                if (subSelectionIndex[0] < 0)
                                                {
                                                    subSelectionIndex[0] = 2;
                                                    if (itemList.Count <= 3 * subSelectionIndex[1] + subSelectionIndex[0]) subSelectionIndex[0] = 1;
                                                    if (itemList.Count <= 3 * subSelectionIndex[1] + subSelectionIndex[0]) subSelectionIndex[0] = 0;

                                                }
                                                break;
                                            case ConsoleKey.D:
                                            case ConsoleKey.RightArrow:
                                                subSelectionIndex[0]++;
                                                if (subSelectionIndex[0] > 2) subSelectionIndex[0] = 0;
                                                if (subSelectionIndex[0] >= itemList.Count - (3 * subSelectionIndex[1])) subSelectionIndex[0] = 0;
                                                break;
                                            case ConsoleKey.Enter:
                                                //Targeting menu for ITEM MENU
                                                targetIndex = 3;
                                                if (enemyParty[targetIndex - 3].Status == EnemyCharacter.StatusEffect.KO) targetIndex += 1;
                                                if (enemyParty[targetIndex - 3].Status == EnemyCharacter.StatusEffect.KO) targetIndex += 1;
                                                do
                                                {
                                                    PrintBattleFrame(party, enemyParty, mainSelectionIndex, subSelectionIndex,
                                                                                    partyOrderIndex, targetIndex, chatLog);
                                                    PrintListBar(itemList, subSelectionIndex);
                                                    menuKey = Console.ReadKey();
                                                    menuChoice = menuKey.Key;
                                                    switch (menuChoice)
                                                    {
                                                        case ConsoleKey.W:
                                                        case ConsoleKey.UpArrow:
                                                            targetIndex--;
                                                            if (targetIndex < 3) targetIndex += 3;
                                                            if (enemyParty[targetIndex - 3].Status == EnemyCharacter.StatusEffect.KO) targetIndex -= 1;
                                                            if (targetIndex < 3) targetIndex += 3;
                                                            if (enemyParty[targetIndex - 3].Status == EnemyCharacter.StatusEffect.KO) targetIndex -= 1;
                                                            if (targetIndex < 3) targetIndex += 3;
                                                            break;
                                                        case ConsoleKey.S:
                                                        case ConsoleKey.DownArrow:
                                                            targetIndex++;
                                                            if (targetIndex >= 6) targetIndex -= 3;
                                                            if (enemyParty[targetIndex - 3].Status == EnemyCharacter.StatusEffect.KO) targetIndex += 1;
                                                            if (targetIndex >= 6) targetIndex -= 3;
                                                            if (enemyParty[targetIndex - 3].Status == EnemyCharacter.StatusEffect.KO) targetIndex += 1;
                                                            if (targetIndex >= 6) targetIndex -= 3;
                                                            break;
                                                        case ConsoleKey.Enter:
                                                            (FriendlyCharacter, string, EnemyCharacter) command;
                                                            useID = 3 * subSelectionIndex[1] + subSelectionIndex[0];
                                                            command = (party[partyOrderIndex], itemList[useID], enemyParty[targetIndex - 3]);
                                                            commandList.Add(command);
                                                            itemList.Remove($"{itemList[useID]}");
                                                            targetIndex = -1;
                                                            partyOrderIndex++;
                                                            break;
                                                        case ConsoleKey.Escape:
                                                            targetIndex = -1;
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                } while (targetIndex != -1);
                                                subSelectionIndex[1] = -1;
                                                mainSelectionIndex[0] = 0;
                                                mainSelectionIndex[1] = 0;
                                                break;
                                            case ConsoleKey.Escape:
                                                subSelectionIndex[1] = -1;
                                                mainSelectionIndex[0] = 0;
                                                mainSelectionIndex[1] = 0;
                                                break;
                                            default:
                                                break;
                                        }
                                    } while (subSelectionIndex[1] != -1);
                            }
                            if (mainSelectionIndex[0] == 1 && mainSelectionIndex[1] == 1)
                            {
                                //if "PASS"
                                partyOrderIndex++;
                                if (partyOrderIndex == 3) break;
                                if (party[partyOrderIndex].Status == FriendlyCharacter.StatusEffect.KO) partyOrderIndex += 1;
                                if (partyOrderIndex == 3) break;
                                if (party[partyOrderIndex].Status == FriendlyCharacter.StatusEffect.KO) partyOrderIndex += 1;
                            }
                            break;
                        default:
                            break;
                    }
                } while (partyOrderIndex != 3);
                mainSelectionIndex[0] = -1;
                mainSelectionIndex[1] = -1;

                //BEGINNING OF BATTLE PHASE
                //Friendly units act
                foreach ((FriendlyCharacter, string, EnemyCharacter) command in commandList)
                {
                    FriendlyCharacter actor = command.Item1;
                    string action = command.Item2;
                    EnemyCharacter target = command.Item3;

                    //Interpretation of unit commands for friendly units
                    switch (action)
                    {
                        case "Fight":
                            if (target.Status != EnemyCharacter.StatusEffect.KO)
                            {
                                checkEnemyStatus[0] = target.Status;
                                actor.DoAttack(target);
                                if (target.Status == EnemyCharacter.StatusEffect.SLEEP)
                                    target.Status = EnemyCharacter.StatusEffect.NONE;
                                checkEnemyStatus[1] = target.Status;
                                chatLog.ChatLine = $"{actor.Name} hits {target.Name} for {actor.ReportDamage(target)} damage.";
                                if (checkEnemyStatus[0] != checkEnemyStatus[1])
                                    switch (checkEnemyStatus[1])
                                    {
                                        case EnemyCharacter.StatusEffect.KO:
                                            chatLog.ChatLine = $"{target.Name} was knocked out!";
                                            break;
                                        case EnemyCharacter.StatusEffect.NONE:
                                            chatLog.ChatLine = $"{target.Name} woke up!";
                                            break;
                                        default:
                                            break;
                                    }
                                PrintBattleFrame(party, enemyParty, mainSelectionIndex, subSelectionIndex,
                                    partyOrderIndex, targetIndex, chatLog);
                                Console.ReadKey();
                            }
                            break;
                        case "Fire":
                            if (target.Status != EnemyCharacter.StatusEffect.KO)
                            {
                                checkEnemyStatus[0] = target.Status;
                                actor.CastFire(target);
                                if (target.Status == EnemyCharacter.StatusEffect.SLEEP)
                                    target.Status = EnemyCharacter.StatusEffect.NONE;
                                checkEnemyStatus[1] = target.Status;
                                chatLog.ChatLine = $"{actor.Name} casts Fire! {target.Name} takes {actor.ReportFire(target)} damage.";
                                if (checkEnemyStatus[0] != checkEnemyStatus[1])
                                    switch (checkEnemyStatus[1])
                                    {
                                        case EnemyCharacter.StatusEffect.KO:
                                            chatLog.ChatLine = $"{target.Name} was knocked out!";
                                            break;
                                        case EnemyCharacter.StatusEffect.NONE:
                                            chatLog.ChatLine = $"{target.Name} woke up!";
                                            break;
                                        default:
                                            break;
                                    }
                                PrintBattleFrame(party, enemyParty, mainSelectionIndex, subSelectionIndex,
                                    partyOrderIndex, targetIndex, chatLog);
                                Console.ReadKey();
                            }
                            break;
                        case "Thunder":
                            if (target.Status != EnemyCharacter.StatusEffect.KO)
                            {
                                checkEnemyStatus[0] = target.Status;
                                actor.CastThunder(target);
                                if (target.Status == EnemyCharacter.StatusEffect.SLEEP)
                                    target.Status = EnemyCharacter.StatusEffect.NONE;
                                checkEnemyStatus[1] = target.Status;
                                chatLog.ChatLine = $"{actor.Name} casts Thunder! {target.Name} takes {actor.ReportThunder(target)} damage.";
                                if (checkEnemyStatus[0] != checkEnemyStatus[1])
                                    switch (checkEnemyStatus[1])
                                    {
                                        case EnemyCharacter.StatusEffect.KO:
                                            chatLog.ChatLine = $"{target.Name} was knocked out!";
                                            break;
                                        case EnemyCharacter.StatusEffect.NONE:
                                            chatLog.ChatLine = $"{target.Name} woke up!";
                                            break;
                                        default:
                                            break;
                                    }
                                PrintBattleFrame(party, enemyParty, mainSelectionIndex, subSelectionIndex,
                                    partyOrderIndex, targetIndex, chatLog);
                                Console.ReadKey();
                            }
                            break;
                        case "Blizzard":
                            if (target.Status != EnemyCharacter.StatusEffect.KO)
                            {
                                checkEnemyStatus[0] = target.Status;
                                actor.CastBlizzard(target);
                                if (target.Status == EnemyCharacter.StatusEffect.SLEEP)
                                    target.Status = EnemyCharacter.StatusEffect.NONE;
                                checkEnemyStatus[1] = target.Status;
                                chatLog.ChatLine = $"{actor.Name} casts Blizzard! {target.Name} takes {actor.ReportBlizzard(target)} damage.";
                                if (checkEnemyStatus[0] != checkEnemyStatus[1])
                                    switch (checkEnemyStatus[1])
                                    {
                                        case EnemyCharacter.StatusEffect.KO:
                                            chatLog.ChatLine = $"{target.Name} was knocked out!";
                                            break;
                                        case EnemyCharacter.StatusEffect.NONE:
                                            chatLog.ChatLine = $"{target.Name} woke up!";
                                            break;
                                        default:
                                            break;
                                    }
                                PrintBattleFrame(party, enemyParty, mainSelectionIndex, subSelectionIndex,
                                    partyOrderIndex, targetIndex, chatLog);
                                Console.ReadKey();
                            }
                            break;
                        case "Sleep":
                            if (target.Status != EnemyCharacter.StatusEffect.KO)
                            {
                                checkEnemyStatus[0] = target.Status;
                                actor.CastSleep(target);
                                switch (checkEnemyStatus[0])
                                {
                                    case EnemyCharacter.StatusEffect.KO:
                                        chatLog.ChatLine =
                                            $"{actor.Name} casts Sleep! It had no effect.";
                                        break;
                                    case EnemyCharacter.StatusEffect.NONE:
                                        chatLog.ChatLine =
                                            $"{actor.Name} casts Sleep! {target.Name} was put to sleep.";
                                        break;
                                    case EnemyCharacter.StatusEffect.SLEEP:
                                        chatLog.ChatLine =
                                            $"{actor.Name} casts Sleep! {target.Name}'s sleep was extended.";
                                        break;
                                    default:
                                        break;
                                }
                                PrintBattleFrame(party, enemyParty, mainSelectionIndex, subSelectionIndex,
                                    partyOrderIndex, targetIndex, chatLog);
                                Console.ReadKey();
                            }
                            break;
                        case "Fire Mote":
                            if (target.Status != EnemyCharacter.StatusEffect.KO)
                            {
                                //todo Make motes scale to the highest Magic of the party. (post-turnin)
                                checkEnemyStatus[0] = target.Status;
                                target.Health = target.Health - (10 - target.Resistance);
                                if (target.Status == EnemyCharacter.StatusEffect.SLEEP)
                                    target.Status = EnemyCharacter.StatusEffect.NONE;
                                checkEnemyStatus[1] = target.Status;
                                chatLog.ChatLine = $"{actor.Name} uses a Fire Mote! {target.Name} takes {10 - target.Resistance} damage.";
                                if (checkEnemyStatus[0] != checkEnemyStatus[1])
                                    switch (checkEnemyStatus[1])
                                    {
                                        case EnemyCharacter.StatusEffect.KO:
                                            chatLog.ChatLine = $"{target.Name} was knocked out!";
                                            break;
                                        case EnemyCharacter.StatusEffect.NONE:
                                            chatLog.ChatLine = $"{target.Name} woke up!";
                                            break;
                                        default:
                                            break;
                                    }
                                PrintBattleFrame(party, enemyParty, mainSelectionIndex, subSelectionIndex,
                                    partyOrderIndex, targetIndex, chatLog);
                                Console.ReadKey();
                            }
                            break;
                        case "Thunder Mote":
                            if (target.Status != EnemyCharacter.StatusEffect.KO)
                            {
                                checkEnemyStatus[0] = target.Status;
                                target.Health = target.Health - (int)(3 * (5 - target.Resistance));
                                if (target.Status == EnemyCharacter.StatusEffect.SLEEP)
                                    target.Status = EnemyCharacter.StatusEffect.NONE;
                                checkEnemyStatus[1] = target.Status;
                                chatLog.ChatLine = $"{actor.Name} uses a Thunder Mote! {target.Name} takes {(int)(3 * (5 - target.Resistance))} damage.";
                                if (checkEnemyStatus[0] != checkEnemyStatus[1])
                                    switch (checkEnemyStatus[1])
                                    {
                                        case EnemyCharacter.StatusEffect.KO:
                                            chatLog.ChatLine = $"{target.Name} was knocked out!";
                                            break;
                                        case EnemyCharacter.StatusEffect.NONE:
                                            chatLog.ChatLine = $"{target.Name} woke up!";
                                            break;
                                        default:
                                            break;
                                    }
                                PrintBattleFrame(party, enemyParty, mainSelectionIndex, subSelectionIndex,
                                    partyOrderIndex, targetIndex, chatLog);
                                Console.ReadKey();
                            }
                            break;
                        case "Blizzard Mote":
                            if (target.Status != EnemyCharacter.StatusEffect.KO)
                            {
                                checkEnemyStatus[0] = target.Status;
                                target.Health = target.Health - (int)(10 * 0.75);
                                if (target.Status == EnemyCharacter.StatusEffect.SLEEP)
                                    target.Status = EnemyCharacter.StatusEffect.NONE;
                                checkEnemyStatus[1] = target.Status;
                                chatLog.ChatLine = $"{actor.Name} uses a Blizzard Mote! {target.Name} takes {(int)(10 * 0.75)} damage.";
                                if (checkEnemyStatus[0] != checkEnemyStatus[1])
                                    switch (checkEnemyStatus[1])
                                    {
                                        case EnemyCharacter.StatusEffect.KO:
                                            chatLog.ChatLine = $"{target.Name} was knocked out!";
                                            break;
                                        case EnemyCharacter.StatusEffect.NONE:
                                            chatLog.ChatLine = $"{target.Name} woke up!";
                                            break;
                                        default:
                                            break;
                                    }
                                PrintBattleFrame(party, enemyParty, mainSelectionIndex, subSelectionIndex,
                                    partyOrderIndex, targetIndex, chatLog);
                                Console.ReadKey();
                            }
                            break;
                        default:
                            break;
                    }
                }

                //Creation of enemy commands
                for (int i = 0; i < 3; i++)
                {
                    if (enemyParty[i].Status == EnemyCharacter.StatusEffect.NONE)
                    {
                        do
                        {
                            enemyTarget = random.Next(3);
                        } while (party[enemyTarget].Status == FriendlyCharacter.StatusEffect.KO);
                        (EnemyCharacter, string, FriendlyCharacter) command =
                            (enemyParty[i], "Fight", party[enemyTarget]);
                        enemyCommandList.Add(command);
                    }
                    if (enemyParty[i].Status == EnemyCharacter.StatusEffect.SLEEP)
                    {
                        enemyTarget = random.Next(3);
                        if (enemyTarget == 0) enemyCommandList.Add((enemyParty[i], "Wake Up", party[0]));
                    }
                }

                //Interpretation of enemy commands
                foreach ((EnemyCharacter, string, FriendlyCharacter) command in enemyCommandList)
                {

                    EnemyCharacter actor = command.Item1;
                    string action = command.Item2;
                    FriendlyCharacter target = command.Item3;

                    switch (action)
                    {
                        case "Fight":
                            if (target.Status != FriendlyCharacter.StatusEffect.KO)
                            {
                                checkStatus[0] = target.Status;
                                actor.DoAttack(target);
                                if (target.Status == FriendlyCharacter.StatusEffect.SLEEP)
                                    target.Status = FriendlyCharacter.StatusEffect.NONE;
                                checkStatus[1] = target.Status;
                                chatLog.ChatLine = $"{actor.Name} hits {target.Name} for {actor.ReportDamage(target)} damage.";
                                if (checkStatus[0] != checkStatus[1])
                                    switch (checkStatus[1])
                                    {
                                        case FriendlyCharacter.StatusEffect.KO:
                                            chatLog.ChatLine = $"{target.Name} was knocked out!";
                                            break;
                                        case FriendlyCharacter.StatusEffect.NONE:
                                            chatLog.ChatLine = $"{target.Name} woke up!";
                                            break;
                                        default:
                                            break;
                                    }
                                PrintBattleFrame(party, enemyParty, mainSelectionIndex, subSelectionIndex,
                            partyOrderIndex, targetIndex, chatLog);
                                if (enemyCommandList.IndexOf(command) + 1 != enemyCommandList.Count) Console.ReadKey();
                            }
                            break;
                        case "Wake Up":
                            actor.Status = EnemyCharacter.StatusEffect.NONE;
                            chatLog.ChatLine = $"{actor.Name} woke up!";
                            break;
                        default:
                            break;
                    }

                }
                //If there's a reason for combat to end, it will end here.
                isContinued = isBattleSequenceContinued(party, enemyParty);
            } while (isContinued);
            //Updates the screen for if combat is over.
            PrintBattleFrame(party, enemyParty, mainSelectionIndex, subSelectionIndex, partyOrderIndex, targetIndex, chatLog);
            Console.WriteLine();
            Console.WriteLine("\t\tVictory!");
            Console.WriteLine();
            Console.WriteLine("\t\tPress a key to continue.");
            Console.ReadKey();
        }

        #region FullscreenMenus

        static void DisplayTitleCard()
        {
            Console.Clear();
            Console.WriteLine("\t\t RPG Battle Demo");
            Console.WriteLine();
            Console.WriteLine("\t\tDefeat all the snakes.");
            Console.WriteLine("\t\tPress any key to start.");
            Console.ReadKey();
        }

        static void DisplayExitCard()
        {
            Console.Clear();
            Console.WriteLine("\t\t RPG Battle Demo");
            Console.WriteLine();
            Console.WriteLine("\t\tYou defeated all the snakes! That's the end of this test demo.");
            Console.WriteLine("\t\tPress any key to exit.");
            Console.ReadKey();
        }

        

        static void PrintBattleFrame(List<FriendlyCharacter> party, List<EnemyCharacter> enemyParty, int[] mainSelectionIndex,
            int[] subSelectionIndex, int partyOrderIndex, int targetIndex, ChatLog chatLog)
        {
            Console.Clear();
            PrintBattleScreen(party, enemyParty, partyOrderIndex, targetIndex);
            PrintMenuBar(party, enemyParty, mainSelectionIndex, subSelectionIndex, partyOrderIndex);
            chatLog.DisplayChat();
        }

        static void PartyTable(List<FriendlyCharacter> party)
        {
            Console.Clear();
            Console.WriteLine("\t\t Party View");
            Console.WriteLine();
            Console.WriteLine();
            string asteriskBar = new string('*', 80);
            Console.WriteLine("{0,10} {1,9} {2,9} {3,9} {4,9} {5,9} {6,12} {7,6}", "Name", "HP", "Mana", "Attack", "Defense",
                "Magic", "Resistance", "Status");
            Console.WriteLine(asteriskBar);
            Console.WriteLine();
            foreach (FriendlyCharacter character in party)
            {
                if (character.Status != FriendlyCharacter.StatusEffect.NONE) Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0,10} {1,9} {2,9} {3,9} {4,9} {5,9} {6,12} {7,6}",
                    character.Name, $"{character.Health}/{character.MaxHealth}", $"{character.Mana}/{character.MaxMana}",
                    $"{character.Attack}/{character.BaseAttack}", $"{character.Defense}/{character.BaseDefense}",
                    $"{character.Magic}/{character.BaseMagic}", $"{character.Resistance}/{character.BaseResistance}",
                    $"{character.Status}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine();
            }
            Console.WriteLine(asteriskBar);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\t Press a key to return.");
            Console.ReadKey();
        }

        #endregion

        #region PartialScreens

        static void PrintBattleScreen(List<FriendlyCharacter> party, List<EnemyCharacter> enemyParty,
            int partyOrderIndex, int targetIndex)
        {
            string asteriskBar = new string('*', 55);
            Console.WriteLine();
            if (partyOrderIndex == 0) Console.ForegroundColor = ConsoleColor.Yellow;
            if (party[0].Status == FriendlyCharacter.StatusEffect.KO)
                Console.ForegroundColor = ConsoleColor.Red;
            if (party[0].Status == FriendlyCharacter.StatusEffect.SLEEP)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            if (targetIndex == 0) Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("{0,11}", party[0].Name);
            Console.ForegroundColor = ConsoleColor.Gray;

            if (enemyParty[0].Status == EnemyCharacter.StatusEffect.KO)
                Console.ForegroundColor = ConsoleColor.Red;
            if (enemyParty[0].Status == EnemyCharacter.StatusEffect.SLEEP)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            if (targetIndex == 3) Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0,30}", enemyParty[0].Name.PadRight(5));
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine();

            if (partyOrderIndex == 1) Console.ForegroundColor = ConsoleColor.Yellow;
            if (party[1].Status == FriendlyCharacter.StatusEffect.KO)
                Console.ForegroundColor = ConsoleColor.Red;
            if (party[1].Status == FriendlyCharacter.StatusEffect.SLEEP)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            if (targetIndex == 1) Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("{0,11}", party[1].Name);
            Console.ForegroundColor = ConsoleColor.Gray;

            
            if (enemyParty[1].Status == EnemyCharacter.StatusEffect.KO)
                Console.ForegroundColor = ConsoleColor.Red;
            if (enemyParty[1].Status == EnemyCharacter.StatusEffect.SLEEP)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            if (targetIndex == 4) Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0,30}", enemyParty[1].Name.PadRight(5));
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine();

            if (partyOrderIndex == 2) Console.ForegroundColor = ConsoleColor.Yellow;
            if (party[2].Status == FriendlyCharacter.StatusEffect.KO)
                Console.ForegroundColor = ConsoleColor.Red;
            if (party[2].Status == FriendlyCharacter.StatusEffect.SLEEP)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            if (targetIndex == 2) Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("{0,11}", party[2].Name);
            Console.ForegroundColor = ConsoleColor.Gray;

            if (enemyParty[2].Status == EnemyCharacter.StatusEffect.KO)
                Console.ForegroundColor = ConsoleColor.Red;
            if (enemyParty[2].Status == EnemyCharacter.StatusEffect.SLEEP)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            if (targetIndex == 5) Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0,30}", enemyParty[2].Name.PadRight(5));
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine();
        }

        static void PrintMenuBar(List<FriendlyCharacter> party, List<EnemyCharacter> enemyParty, int[] mainSelectionIndex,
            int[] subSelectionIndex, int partyOrderIndex)
        {
            string asteriskBar = new string('*', 56);
            Console.WriteLine(asteriskBar);

            WriteCharacterBar(party[0]);
            if (mainSelectionIndex[0] == 0 && mainSelectionIndex[1] == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("{0,-8}", "Fight");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else Console.Write("{0,-8}", "Fight");
            if (mainSelectionIndex[0] == 1 && mainSelectionIndex[1] == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("{0,-8}", "Party");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else Console.Write("{0,-8}", "Party");
            Console.WriteLine("*");

            WriteCharacterBar(party[1]);
            Console.WriteLine("{0,16}*", " ");

            WriteCharacterBar(party[2]);
            if (mainSelectionIndex[0] == 0 && mainSelectionIndex[1] == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("{0,-8}", "Items");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else Console.Write("{0,-8}", "Items");
            if (mainSelectionIndex[0] == 1 && mainSelectionIndex[1] == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("{0,-8}", "Pass");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else Console.Write("{0,-8}", "Pass");
            Console.WriteLine("*");

            Console.WriteLine(asteriskBar);
        }

        static void PrintListBar(List<string> list, int[] subSelectionIndex)
        {
            //todo Tooltips
            string toolTip = "";
            string toolTipCost = "";
            string asteriskBar = new string('*', 56);
            switch (list[3 * subSelectionIndex[1] + subSelectionIndex[0]])
            {
                case "Fight":
                    toolTip = "A basic attack.";
                    toolTipCost = "Cost: Free";
                    break;
                case "Fire":
                    toolTip = "A basic spell that deals magic damage 1 time.";
                    toolTipCost = "Cost: 10 MP";
                    break;
                case "Thunder":
                    toolTip = "A spell that deals 1/2 magic damage 3 times.";
                    toolTipCost = "Cost: 10 MP";
                    break;
                case "Blizzard":
                    toolTip = "A spell that pierces resistances for 3/4 magic damage.";
                    toolTipCost = "Cost: 10 MP";
                    break;
                case "Sleep":
                    toolTip = "A potent spell that puts the target to sleep.";
                    toolTipCost = "Cost: 20 MP";
                    break;
                case "Fire Mote":
                    toolTip = "A mote containing a cast of Fire.";
                    toolTipCost = "Cost: 1 mote";
                    break;
                case "Thunder Mote":
                    toolTip = "A mote containing a cast of Thunder.";
                    toolTipCost = "Cost: 1 mote";
                    break;
                case "Blizzard Mote":
                    toolTip = "A mote containing a cast of Blizzard";
                    toolTipCost = "Cost: 1 mote";
                    break;
                default:
                    break;
            }
            Console.Write(asteriskBar);
            Console.WriteLine($"  {toolTip}");
            //Height
            for (int i = 0; i <= ((list.Count - 1) / 3); i++)
            {
                //Width
                Console.Write("* ");
                for (int j = 0; j < 3; j++)
                {
                    if (list.Count > 3 * i + j)
                    {
                        if (subSelectionIndex[1] == i && subSelectionIndex[0] == j)
                            Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("{0,-17}", list[3 * i + j]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        Console.Write("{0,-17}", "");
                    }
                }

                if (i == 0) Console.WriteLine($"  *  {toolTipCost}");
                if (i != 0) Console.WriteLine("  *");

            }
            Console.WriteLine(asteriskBar);
        }

        #endregion

        #region UnitFunctions

        static void RefreshCharacters(List<FriendlyCharacter> FriendlyCharacters)
        {
            foreach (FriendlyCharacter character in FriendlyCharacters)
            {
                character.InitializeCharacter();
            }
        }
        static void RefreshCharacters(List<EnemyCharacter> EnemyCharacters)
        {
            foreach (EnemyCharacter character in EnemyCharacters)
            {
                character.InitializeCharacter();
            }
        }

        static void WriteCharacterBar(FriendlyCharacter character)
        {
            Console.Write("* {0,6}: HP {1,4}/{2,-4} MP {3,4}/{4,-4} *  ",
            character.Name, character.Health, character.MaxHealth, character.Mana, character.MaxMana);
        }

        static bool isBattleSequenceContinued(List<FriendlyCharacter> party, List<EnemyCharacter> enemyParty)
        {
            bool isContinued = true;
            if (party[0].Status == FriendlyCharacter.StatusEffect.KO
                && party[1].Status == FriendlyCharacter.StatusEffect.KO
                && party[2].Status == FriendlyCharacter.StatusEffect.KO) isContinued = false;
            if (enemyParty[0].Status == EnemyCharacter.StatusEffect.KO
                && enemyParty[1].Status == EnemyCharacter.StatusEffect.KO
                && enemyParty[2].Status == EnemyCharacter.StatusEffect.KO) isContinued = false;
            return isContinued;
        }

        #endregion

        #region FileIO

        static List<FriendlyCharacter> ReadCharacterList()
        {
            //FILE FORMAT: Name,MaxHealth,MaxMana,BaseAttack,BaseDefense,BaseMagic,BaseResistance,BaseSpeed
            List<FriendlyCharacter> characters = new List<FriendlyCharacter>();
            string[] characterData = File.ReadAllLines("Data\\Characters.txt");
            string[] characterProperties;
            foreach (string character in characterData)
            {
                characterProperties = character.Split(',');

                FriendlyCharacter newCharacter = new FriendlyCharacter()
                {
                    Name = characterProperties[0],
                    MaxHealth = Int32.Parse(characterProperties[1]),
                    MaxMana = Int32.Parse(characterProperties[2]),
                    BaseAttack = Int32.Parse(characterProperties[3]),
                    BaseDefense = Int32.Parse(characterProperties[4]),
                    BaseMagic = Int32.Parse(characterProperties[5]),
                    BaseResistance = Int32.Parse(characterProperties[6]),
                    BaseSpeed = Int32.Parse(characterProperties[7])
                };
                characters.Add(newCharacter);
            }
            return characters;
        }

        #endregion
    }
}
