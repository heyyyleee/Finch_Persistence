using FinchAPI;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System;

namespace Finch_Starter
{
    //
    // User Commands
    //

    public enum Command
    {
        NONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        WAIT,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF,
        GETTEMPERATURE,
        DANCE,
        DONE
    }

    class Program
    {
        // *************************************************************
        // Application:     Finch Control
        // Author:          Hailey McGuire
        // Description:     Finch Starter - Persistence
        // Application Type: Console
        // Date Created:    02/09/2021
        // Date Revised:    03/28/2021
        // *************************************************************

        static void Main(string[] args)
        {
            //
            // create a new Finch object
            //
            Finch myFinch;
            myFinch = new Finch();

            //
            // call the connect method
            //
            myFinch.connect();

            //
            // begin your code
            //

            MainAppTheme();

            DisplayWelcomeScreen();

            MainMenu();

            //
            // call the disconnect method
            //
            myFinch.disConnect();
        }

        #region APP THEMES
        static void MainAppTheme()
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Clear();
        }

        static void SecondaryAppTheme()
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
        }
        #endregion

        static void DisplayContinuePrompt()
        {
            Console.WriteLine("\tPress any key to continue: ");
            Console.ReadKey();
        }

        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t" + headerText);
            Console.WriteLine();
        }

        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            SecondaryAppTheme();

            Console.WriteLine();
            Console.WriteLine("\tWelcome to your new Finch Robot!");
            Console.WriteLine();
            Console.WriteLine("\tThis app will show you all the things your Finch can do.");
            Console.WriteLine();
            DisplayContinuePrompt();
        }

        static void DisplayClosingScreen()
        {
            Console.Clear();
            SecondaryAppTheme();
            DisplayScreenHeader("\tClosing Screen");

            Console.WriteLine();
            Console.WriteLine("\tThank you for using the Finch app!");
            DisplayContinuePrompt();
            Console.ReadLine();
        }

        static void MainMenu()
        {

            bool quit = false;
            string userInput;

            Console.Clear();
            MainAppTheme();

            Finch myFinch = new Finch();

            do
            {
                DisplayScreenHeader("\tMain Finch Menu");
                Console.WriteLine();
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tg) Feedback Menu");
                Console.WriteLine("\th) Set User Theme");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t Enter your selection: ");
                userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case "a":
                        DisplayConnectFinchRobot(myFinch);
                        break;

                    case "b":

                        TalentShowDisplayMenu(myFinch);
                        break;

                    case "c":

                        DataRecorderDisplayMenuScreen(myFinch);
                        break;

                    case "d":

                        AlarmMainMenu(myFinch);
                        break;

                    case "e":

                        UserProgrammingDisplayMenuScreen(myFinch);
                        break;

                    case "f":

                        DisplayDisconnectFinchRobot(myFinch);
                        break;

                    case "g":

                        FeedbackMenu();
                        break;

                    case "h":
                        DisplaySetTheme();
                        break;

                    case "q":
                        quit = true;
                        DisplayClosingScreen();
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease select a letter from the menu");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quit);
        }

        #region FINCH CONNECTIONS

        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tDisconnecting from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine();
            Console.WriteLine("\tThe Finch robot is now disconnected.");
            DisplayContinuePrompt();

            MainMenu();
        }

        static bool DisplayConnectFinchRobot(Finch myfinch)
        {

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\tConnecting the Finch robot now. Please be sure the USB cable is connected to the robot and computer.");
            Console.WriteLine();
            DisplayContinuePrompt();

            robotConnected = myfinch.connect();

            myfinch.setLED(0, 0, 0);
            myfinch.noteOff();

            return robotConnected;

        }

        #endregion

        #region SET THEME

        static void DisplaySetTheme()
        {
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            bool themeChosen = false;

            themeColors = ReadThemeData();
            Console.ForegroundColor = themeColors.foregroundColor;
            Console.BackgroundColor = themeColors.backgroundColor;
            Console.Clear();
            DisplayScreenHeader("Set Application Theme");

            Console.WriteLine($"\tCurrent foreground color: {Console.ForegroundColor}");
            Console.WriteLine($"\tCurrent background color: {Console.BackgroundColor}");
            Console.WriteLine();

            Console.Write("\tIs this the theme you would like? Yes/No: ");
            if (Console.ReadLine().ToLower() == "no")
            {
                do
                {
                    themeColors.foregroundColor = GetConsoleColorFromUser("foreground");
                    themeColors.backgroundColor = GetConsoleColorFromUser("background");

                    Console.ForegroundColor = themeColors.foregroundColor;
                    Console.BackgroundColor = themeColors.backgroundColor;
                    Console.Clear();
                    DisplayScreenHeader("Set Application Theme");
                    Console.WriteLine($"\tNew foreground color: {Console.ForegroundColor}");
                    Console.WriteLine($"\tNew background color: {Console.BackgroundColor}");

                    Console.CursorVisible = true;
                    Console.WriteLine();
                    Console.Write("\tIs this the color theme you would like? ");
                    if (Console.ReadLine().ToLower() == "yes")
                    {
                        themeChosen = true;
                        WriteThemeData(themeColors.foregroundColor, themeColors.backgroundColor);
                    }
                } while (!themeChosen);
            }
        }

        static (ConsoleColor foregroundColor, ConsoleColor backgroundColor) ReadThemeData()
        {
            string dataPath = @"Theme/Data.txt";
            string[] themeColors;

            ConsoleColor foregroundColor;
            ConsoleColor backgroundColor;

            themeColors = File.ReadAllLines(dataPath);

            Enum.TryParse(themeColors[0], true, out foregroundColor);
            Enum.TryParse(themeColors[1], true, out backgroundColor);

            return (foregroundColor, backgroundColor);
        }

        static void WriteThemeData(ConsoleColor foreground, ConsoleColor background)
        {
            string dataPath = @"Theme/Data.txt";

            File.WriteAllText(dataPath, foreground.ToString() + "\n");
            File.AppendAllText(dataPath, background.ToString());
        }

        static ConsoleColor GetConsoleColorFromUser(string property)
        {
            ConsoleColor consoleColor;
            bool validConsoleColor;

            do
            {
                Console.Write($"\tEnter a value for the {property}: ");
                validConsoleColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out consoleColor);

                if (!validConsoleColor)
                {
                    Console.WriteLine("\n\t***** It appears you did not provide a valid color. Please try again. *****\n");
                }
                else
                {
                    validConsoleColor = true;
                }

            } while (!validConsoleColor);

            return consoleColor;

        }

        #endregion

        #region TALENT SHOW

        static void TalentShowDisplayMenu(Finch myFinch)
        {
            string userInput;
            bool quitTalentShowMenu = false;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                Console.WriteLine();
                Console.WriteLine("\tMake a Selection:");
                Console.WriteLine("\ta. Light and Sound");
                Console.WriteLine("\tb. Dance");
                Console.WriteLine("\tc. Mixing it Up");
                Console.WriteLine("\td. Return to Main Menu");
                Console.WriteLine();
                Console.Write("\tEnter your Selection: ");
                userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case "a":
                        TalentShowDisplayLightAndSound(myFinch);
                        break;

                    case "b":
                        TalentShowDisplayDance(myFinch);
                        break;

                    case "c":
                        TalentShowDisplayMixingItUp(myFinch);
                        break;

                    case "d":
                        MainMenu();
                        quitTalentShowMenu = true;
                        break;
                }

            } while (!quitTalentShowMenu);


        }

        public static void TalentShowDisplayMixingItUp(Finch myFinch)
        {
            Console.Clear();
            Console.WriteLine();
            DisplayScreenHeader("Mixing it Up");
            Console.WriteLine();
            Console.WriteLine("\tHere are all of your Finch's talents all together!");

            for (int ledValue = 100; ledValue < 255; ledValue++)
            {
                myFinch.setLED(ledValue, ledValue, ledValue);
            }

            for (int ledValue = 255; ledValue > 100; ledValue--)
            {
                myFinch.setLED(ledValue, ledValue, ledValue);
            }

            myFinch.setLED(0, 255, 255);
            myFinch.noteOn(523);
            myFinch.wait(200);
            myFinch.noteOn(698);
            myFinch.wait(400);
            myFinch.noteOn(698);
            myFinch.wait(400);
            myFinch.noteOn(880);
            myFinch.wait(600);
            myFinch.noteOff();
            //myFinch.wait(100);
            myFinch.noteOn(523);
            myFinch.wait(200);
            myFinch.noteOn(698);
            myFinch.wait(400);
            myFinch.noteOn(698);
            myFinch.wait(400);
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOff();

            myFinch.setLED(255, 255, 0);
            myFinch.setMotors(80, 80);
            myFinch.wait(200);
            myFinch.setLED(0, 255, 255);
            myFinch.setMotors(-80, -80);
            myFinch.wait(200);
            myFinch.setLED(255, 255, 0);
            myFinch.setMotors(80, 80);
            myFinch.wait(200);
            myFinch.setLED(0, 255, 255);
            myFinch.setMotors(-80, -80);
            myFinch.wait(200);
            myFinch.setMotors(0, 0);

            myFinch.setLED(255, 255, 0);
            myFinch.noteOn(1047);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.noteOn(1047);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOn(1047);
            myFinch.wait(400);
            myFinch.noteOn(1174);
            myFinch.wait(200);
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOn(784);
            myFinch.wait(600);

            myFinch.noteOn(523);
            myFinch.wait(200);
            myFinch.noteOn(659);
            myFinch.wait(400);
            myFinch.noteOn(659);
            myFinch.wait(400);
            myFinch.noteOn(784);
            myFinch.wait(600);
            myFinch.noteOff();
            myFinch.noteOn(523);
            myFinch.wait(200);
            myFinch.noteOn(659);
            myFinch.wait(400);
            myFinch.noteOn(659);
            myFinch.wait(400);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOff();

            myFinch.setLED(255, 255, 0);
            myFinch.setMotors(80, 80);
            myFinch.wait(200);
            myFinch.setLED(0, 255, 255);
            myFinch.setMotors(-80, -80);
            myFinch.wait(200);
            myFinch.setLED(255, 255, 0);
            myFinch.setMotors(80, 80);
            myFinch.wait(200);
            myFinch.setLED(0, 255, 255);
            myFinch.setMotors(-80, -80);
            myFinch.wait(200);
            myFinch.setMotors(0, 0);

            myFinch.setLED(255, 255, 0);
            myFinch.noteOn(1047);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.noteOn(1047);
            myFinch.wait(200);
            myFinch.noteOff();
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOn(1047);
            myFinch.wait(400);
            myFinch.noteOn(1174);
            myFinch.wait(200);
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOn(784);
            myFinch.wait(600);

            myFinch.noteOff();
            myFinch.setLED(0, 0, 0);

            DisplayContinuePrompt();
        }

        static void TalentShowDisplayLightAndSound(Finch myFinch)
        {
            DisplayScreenHeader("Light and Sound");
            Console.WriteLine("\tHere is a light show and a song from your Finch!");

            myFinch.setLED(255, 0, 255);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOn(880);
            myFinch.wait(200);
            myFinch.noteOn(988);
            myFinch.wait(400);
            myFinch.noteOn(587);
            myFinch.wait(400);
            myFinch.noteOn(587);
            myFinch.wait(400);

            myFinch.setLED(255, 0, 0);
            myFinch.noteOn(659);
            myFinch.wait(400);
            myFinch.setLED(255, 0, 255);
            myFinch.noteOn(784);
            myFinch.wait(400);
            myFinch.setLED(0, 0, 255);
            myFinch.noteOn(587);
            myFinch.wait(400);

            myFinch.setLED(255, 0, 0);
            myFinch.noteOn(659);
            myFinch.wait(400);
            myFinch.setLED(255, 0, 255);
            myFinch.noteOn(784);
            myFinch.wait(400);
            myFinch.setLED(0, 0, 255);
            myFinch.noteOn(587);
            myFinch.wait(400);

            myFinch.setLED(255, 0, 255);
            myFinch.noteOn(659);
            myFinch.wait(400);
            myFinch.noteOn(784);
            myFinch.wait(400);
            myFinch.noteOn(784);
            myFinch.wait(200);
            myFinch.noteOn(880);
            myFinch.wait(200);

            myFinch.noteOn(988);
            myFinch.wait(400);
            myFinch.noteOn(587);
            myFinch.wait(400);
            myFinch.noteOn(587);
            myFinch.wait(400);

            myFinch.setLED(255, 0, 0);
            myFinch.noteOn(659);
            myFinch.wait(400);
            myFinch.setLED(255, 0, 255);
            myFinch.noteOn(784);
            myFinch.wait(400);
            myFinch.setLED(0, 0, 255);
            myFinch.noteOn(587);
            myFinch.wait(400);
            myFinch.setLED(255, 0, 0);
            myFinch.noteOn(659);

            myFinch.wait(400);
            myFinch.noteOn(659);
            myFinch.setLED(255, 0, 255);
            myFinch.wait(400);
            myFinch.noteOn(784);
            myFinch.setLED(0, 0, 255);
            myFinch.wait(400);
            myFinch.noteOn(880);
            myFinch.setLED(255, 0, 0);
            myFinch.wait(400);
            myFinch.noteOn(784);
            myFinch.setLED(255, 0, 255);
            myFinch.wait(800);

            myFinch.setLED(0, 0, 0);
            myFinch.noteOff();

            DisplayContinuePrompt();

        }

        static void TalentShowDisplayDance(Finch myFinch)
        {
            Console.Clear();
            Console.WriteLine();
            DisplayScreenHeader("Dance");
            Console.WriteLine();
            Console.WriteLine("\tYour finch will now do a dance!");

            myFinch.setMotors(-80, 80);
            myFinch.wait(2000);
            myFinch.setMotors(80, -80);
            myFinch.wait(2000);
            myFinch.setMotors(100, 100);
            myFinch.wait(1000);
            myFinch.setMotors(-100, -100);
            myFinch.wait(1000);
            myFinch.setMotors(-100, 100);
            myFinch.wait(3000);

            myFinch.setMotors(80, -80);
            myFinch.wait(2000);
            myFinch.setMotors(-80, 80);
            myFinch.wait(2000);
            myFinch.setMotors(-100, -100);
            myFinch.wait(1000);
            myFinch.setMotors(100, 100);
            myFinch.wait(1000);
            myFinch.setMotors(100, -100);
            myFinch.wait(3000);

            myFinch.setMotors(300, 300);

            myFinch.setMotors(0, 0);

            DisplayContinuePrompt();
        }

        #endregion

        #region USER PROGRAMMING
        static void UserProgrammingDisplayMenuScreen(Finch myFinch)
        {

            string menuChoice;
            bool quitMenu = false;

            //
            // tuple to store the three command parameters
            //
            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            List<Command> commands = new List<Command>();

            do
            {
                DisplayScreenHeader("User Programming Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Command Parameters");
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Execute Commands");
                Console.WriteLine("\tq) Quit");
                Console.WriteLine();
                Console.Write("\tEnter your choice: ");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        commandParameters = UserProgrammingDisplayGetCommandParameters();
                        break;

                    case "b":
                        UserProgrammingDisplayGetFinchCommands(commands);
                        break;

                    case "c":
                        UserProgrammingDisplayFinchCommands(commands);
                        break;

                    case "d":
                        UserProgrammingDisplayExecuteFinchCommands(myFinch, commands, commandParameters);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine("Please enter a selection from the menu.");
                        break;
                }



            } while (!quitMenu);
        }

        private static void UserProgrammingDisplayExecuteFinchCommands(Finch myFinch, List<Command> commands, (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters)
        {
            int motorSpeed = commandParameters.motorSpeed;
            int ledBrightness = commandParameters.ledBrightness;
            int waitMilliseconds = (int)(commandParameters.waitSeconds * 1000);
            string commandFeedback = "";
            const int TURNING_MOTOR_SPEED = 100;

            DisplayScreenHeader("Execute Finch Controls");

            Console.WriteLine("\tThe Finch is now ready to execute the commands.");
            DisplayContinuePrompt();

            foreach (Command command in commands)
            {
                switch (command)
                {
                    case Command.NONE:
                        break;

                    case Command.MOVEFORWARD:
                        myFinch.setMotors(motorSpeed, motorSpeed);
                        commandFeedback = Command.MOVEFORWARD.ToString();
                        ; break;

                    case Command.MOVEBACKWARD:
                        myFinch.setMotors(-motorSpeed, -motorSpeed);
                        commandFeedback = Command.MOVEBACKWARD.ToString();
                        break;

                    case Command.STOPMOTORS:
                        myFinch.setMotors(0, 0);
                        commandFeedback = Command.STOPMOTORS.ToString();
                        break;

                    case Command.WAIT:
                        myFinch.wait(waitMilliseconds);
                        commandFeedback = Command.WAIT.ToString();
                        break;

                    case Command.TURNRIGHT:
                        myFinch.setMotors(TURNING_MOTOR_SPEED, -TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNRIGHT.ToString();
                        break;

                    case Command.TURNLEFT:
                        myFinch.setMotors(-TURNING_MOTOR_SPEED, TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNLEFT.ToString();
                        break;

                    case Command.LEDON:
                        myFinch.setLED(ledBrightness, ledBrightness, ledBrightness);
                        commandFeedback = Command.LEDON.ToString();
                        break;

                    case Command.LEDOFF:
                        myFinch.setLED(0, 0, 0);
                        commandFeedback = Command.LEDOFF.ToString();
                        break;

                    case Command.GETTEMPERATURE:
                        commandFeedback = $"Temperature: {myFinch.getTemperature().ToString("n2")}\n";
                        break;

                    case Command.DANCE:
                        TalentShowDisplayMixingItUp(myFinch);
                        commandFeedback = Command.DANCE.ToString();
                        break;

                    case Command.DONE:
                        myFinch.setLED(0, 0, 0);
                        myFinch.setMotors(0, 0);
                        commandFeedback = Command.DONE.ToString();
                        break;

                    default:
                        break;

                }

                Console.WriteLine($"\t{commandFeedback}");
            }

            DisplayContinuePrompt();
        }

        private static void UserProgrammingDisplayFinchCommands(List<Command> commands)
        {
            //
            // display user commands
            //
            DisplayScreenHeader("Finch Robot Commands");

            foreach (Command command in commands)
            {
                Console.WriteLine($"\t{command}");
            }

            DisplayContinuePrompt();
        }

        private static void UserProgrammingDisplayGetFinchCommands(List<Command> commands)
        {
            Command command = Command.NONE;

            DisplayScreenHeader("Finch Robot Commands");

            //
            // list commands
            //

            int commandCount = 1;
            Console.WriteLine("\tList of Available Commands");
            Console.WriteLine();
            Console.WriteLine("\t-");
            foreach (string commandName in Enum.GetNames(typeof(Command)))
            {
                Console.Write($"-{commandName.ToUpper()}-");
                if (commandCount % 5 == 0) Console.Write("-\n-");
                commandCount++;
            }

            Console.WriteLine();
            Console.WriteLine();

            while (command != Command.DONE)
            {
                Console.Write("\t Enter Command: ");

                if (Enum.TryParse(Console.ReadLine().ToUpper(), out command))
                {
                    commands.Add(command);
                }
                else
                {
                    Console.WriteLine("\t\t***************************************");
                    Console.WriteLine("\t\t*Please eneter a command from the list*");
                    Console.WriteLine("\t\t***************************************");
                }
            }


            DisplayContinuePrompt();

        }

        static (int motorSpeed, int ledBrightness, double waitSeconds) UserProgrammingDisplayGetCommandParameters()
        {
            DisplayScreenHeader("Command Parameters");

            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            int motorInput;
            int ledInput;
            double waitInput;

            Console.Write("\tEnter motor speed [1-255]: ");
            Int32.TryParse(Console.ReadLine(), out motorInput);
            if (motorInput >= 0 && motorInput <= 255)
            {
                commandParameters.motorSpeed = motorInput;
            }

            else
            {
                Console.WriteLine("Please enter a number between 1-255: ");
            }

            Console.Write("\tEnter LED brightness [1 - 255]: ");
            Int32.TryParse(Console.ReadLine(), out ledInput);
            if (ledInput >= 0 && ledInput <= 255)
            {
                commandParameters.ledBrightness = ledInput;
            }

            else
            {
                Console.WriteLine("Please enter a number between 1-255: ");
            }

            Console.Write("\tEnter wait in seconds: ");
            Double.TryParse(Console.ReadLine(), out waitInput);
            if (waitInput >= 0 && waitInput <= 10)
            {
                commandParameters.waitSeconds = waitInput;
            }

            else
            {
                Console.Write("Please enter a number between 1-10: ");
            }

            Console.WriteLine();
            Console.WriteLine($"\tMotor Speed: {commandParameters.motorSpeed}");
            Console.WriteLine($"\tLED Brightness: {commandParameters.ledBrightness}");
            Console.WriteLine($"\tWait Command Duration: {commandParameters.waitSeconds}");

            DisplayContinuePrompt();

            return commandParameters;
        }

        #endregion

        #region FEEDBACK MENU
        static void FeedbackMenu()
        {
            int userFeedback;
            string userResponse;
            bool validResponse;

            DisplayScreenHeader("Feedback Menu");

            do
            {
                Console.WriteLine();
                Console.WriteLine("\tThank you for using your Finch robot.");
                Console.WriteLine("\tPlease rate your experience from 1-5.");
                Console.WriteLine("\t1 is the worst, 5 is the best.");
                Console.WriteLine();
                Console.Write("\tYour rating: ");
                userResponse = Console.ReadLine();

                validResponse = int.TryParse(userResponse, out userFeedback);

                if (validResponse)
                {

                    if (userFeedback >= 3)
                    {
                        validResponse = true;
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("\tThank you for your feedback! We are glad you enjoy your Finch robot.");
                        DisplayContinuePrompt();
                    }

                    else if (userFeedback < 3)
                    {
                        validResponse = true;
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("\tWe are sorry that you are not enjoying your Finch robot.");
                        DisplayContinuePrompt();

                    }
                }

                else if (!validResponse)
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter an integer from 1 to 5.");
                }
            } while (!validResponse);
        }

        #endregion

        #region DATA RECORDER

        // ****************
        // * Data Recorder*
        // ****************

        static void DataRecorderDisplayMenuScreen(Finch myFinch)
        {
            int numberOfDataPoints = 0;
            double dataPointFrequency = 0;
            double[] temperatures = null;

            bool quitMenu = false;
            string userInput;

            Console.Clear();
            MainAppTheme();

            //Finch myFinch = new Finch();

            do
            {
                DisplayScreenHeader("\tData Recorder Menu");
                Console.WriteLine();
                Console.WriteLine("\ta) Number of Data Points");
                Console.WriteLine("\tb) Frequency of Data Points");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Show Data");
                Console.WriteLine("\te) Light Sensor");
                Console.WriteLine("\tq) Return to Main Menu");
                Console.Write("\t Enter your selection: ");
                userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case "a":
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        break;

                    case "c":
                        temperatures = DataRecorderDisplayGetData(numberOfDataPoints, dataPointFrequency, myFinch);
                        break;

                    case "d":
                        DataRecorderDisplayData(temperatures);
                        break;

                    case "e":
                        LightSensorMenu(myFinch);
                        break;

                    case "q":
                        MainMenu();
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease select a letter from the menu");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static void DataRecorderDisplayData(double[] temperatures)
        {
            DisplayScreenHeader("Display Data");

            DataRecorderDisplayTable(temperatures);

            DisplayContinuePrompt();
        }

        static void DataRecorderDisplayTable(double[] temperatures)
        {
            DisplayScreenHeader("Your Data in Farenheit");

            //
            // Table Headers
            //

            Console.WriteLine(
                "Recording Number".PadLeft(15) +
                "Temperature".PadLeft(15)
                );
            Console.WriteLine(
                "----------------".PadLeft(15) +
                "------------".PadLeft(15)
                );

            //
            // Display Table Data
            //
            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine(
                    (index + 1).ToString().PadLeft(15) +
                    ((temperatures[index]) * 1.8 + 32).ToString("n2").PadLeft(15)
                );
            }

            Console.WriteLine();

        }

        static double[] DataRecorderDisplayGetData(int numberOfDataPoints, double dataPointFrequency, Finch myFinch)
        {
            double[] temperatures = new double[numberOfDataPoints];

            DisplayScreenHeader("Get Data");

            Console.WriteLine("\tNumber of data points:{0}", numberOfDataPoints);
            Console.WriteLine("\tFrequency of data points:{0}", dataPointFrequency);
            Console.WriteLine();
            Console.WriteLine("\tThe Finch robot is ready to begin recording the temperature data.");
            DisplayContinuePrompt();

            for (int index = 0; index < numberOfDataPoints; index++)
            {
                temperatures[index] = myFinch.getTemperature();
                Console.WriteLine("\tReading {0}: {1}", index + 1, (temperatures[index]).ToString("n2"));
                int waitInSeconds = (int)(dataPointFrequency * 1000);
                myFinch.wait(waitInSeconds);
            }

            DisplayContinuePrompt();

            return temperatures;
        }

        //
        // Get and store frequency of data points
        //
        static double DataRecorderDisplayGetDataPointFrequency()
        {
            double dataPointFrequency;
            string userResponse;
            bool validResponse;

            DisplayScreenHeader("Data Point Frequency");

            do
            {
                Console.Write("\tEnter the frequency of data points: ");
                userResponse = Console.ReadLine();
                validResponse = double.TryParse(userResponse, out dataPointFrequency);

                if (validResponse)
                {

                }

                else if (!validResponse)
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter an integer.");
                }

            } while (!validResponse);


            DisplayContinuePrompt();

            return dataPointFrequency;
        }

        //
        //Get and store number of data points
        //
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;
            string userResponse;
            bool validResponse;

            DisplayScreenHeader("Number of Data Points");

            do
            {
                Console.Write("\tEnter number of data points: ");
                userResponse = Console.ReadLine();
                validResponse = int.TryParse(userResponse, out numberOfDataPoints);

                if (validResponse)
                {

                }

                else if (!validResponse)
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter an integer.");
                }

            } while (!validResponse);


            DisplayContinuePrompt();

            return numberOfDataPoints;

        }

        static void LightSensorMenu(Finch myFinch)
        {
            int[] lightReading = null;
            bool quitMenu = false;
            string userInput;

            Console.Clear();
            MainAppTheme();

            do
            {
                DisplayScreenHeader("\tLight Reading Menu");
                Console.WriteLine();
                Console.WriteLine("\ta) Get Data");
                Console.WriteLine("\tb) Display Data");
                Console.WriteLine("\tq) Return to Main Menu");
                Console.Write("\t Enter your selection: ");
                userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case "a":
                        lightReading = LightSensorGetData(myFinch);
                        break;

                    case "b":
                        LightSensorDisplayData(lightReading);
                        break;

                    case "q":
                        MainMenu();
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease select a letter from the menu");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static int[] LightSensorGetData(Finch myFinch)
        {
            int[] lightReading = new int[5];

            Console.Clear();
            DisplayScreenHeader("Light Reading");
            Console.WriteLine("\tThe Finch robot is ready to begin recording the light data.");
            DisplayContinuePrompt();

            for (int index = 0; index < 5; index++)
            {
                lightReading[index] = myFinch.getRightLightSensor();
                Console.WriteLine("\tReading {0}: {1}", index + 1, lightReading[index]);
                myFinch.wait(1000);
            }

            DisplayContinuePrompt();
            return lightReading;

        }

        static void LightSensorDisplayData(int[] lightReading)
        {
            //
            // Table Headers
            //
            DisplayScreenHeader("Light Data Display");

            Console.Clear();
            Console.WriteLine(
                "Recording Number".PadLeft(15) +
                "Light Reading".PadLeft(15)
                );
            Console.WriteLine(
                "----------------".PadLeft(15) +
                "------------".PadLeft(15)
                );

            //
            // Display Table Data
            //
            for (int index = 0; index < lightReading.Length; index++)
            {
                Console.WriteLine(
                    (index + 1).ToString().PadLeft(15) +
                    lightReading[index].ToString("n2").PadLeft(15)
                );
            }

            int i = 0;
            int sum = 0;
            float average = 0.0F;

            for (i = 0; i < lightReading.Length; i++)
            {
                sum += lightReading[i];
            }

            average = (float)sum / lightReading.Length;

            Console.WriteLine("The average of the light reading is {0}", average);


            Console.WriteLine();
            DisplayContinuePrompt();
        }


        #endregion

        #region ALARM SYSTEM

        // ****************
        // * Alarm System *
        // ****************

        static void AlarmMainMenu(Finch myFinch)
        {
            bool quitMenu = false;
            string userInput;

            Console.Clear();

            do
            {
                DisplayScreenHeader("Alarm Main Menu");
                Console.WriteLine();
                Console.WriteLine("\tChoose what you would like to monitor.");
                Console.WriteLine("\ta) Temperature");
                Console.WriteLine("\tb) Light");
                Console.WriteLine("\tq) Return to Main Menu");
                userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case "a":
                        TemperatureAlarmDisplayMenuScreen(myFinch);
                        break;

                    case "b":
                        LightAlarmDisplayMenuScreen(myFinch);
                        break;

                    case "q":
                        MainMenu();
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Please select an option from the menu");
                        Console.WriteLine();
                        DisplayContinuePrompt();
                        break;
                }
            } while (!quitMenu);


        }

        static void LightAlarmDisplayMenuScreen(Finch myFinch)
        {

            {

                bool quitMenu = false;
                string userInput;

                string sensorsToMonitor = "";
                string rangeType = "";
                int minMaxThresholdValue = 0;
                int timeToMonitor = 0;


                Console.Clear();
                MainAppTheme();

                //Finch myFinch = new Finch();

                do
                {
                    DisplayScreenHeader("\tLight Alarm Menu");
                    Console.WriteLine();
                    Console.WriteLine("\ta) Sensors to Monitor");
                    Console.WriteLine("\tb) Set Range Type");
                    Console.WriteLine("\tc) Set Minimum/Maximum Threshold");
                    Console.WriteLine("\td) Set Time to Monitor");
                    Console.WriteLine("\te) Set Alarm");
                    Console.WriteLine("\tq) Return to Main Menu");
                    Console.Write("\t Enter your selection: ");
                    userInput = Console.ReadLine().ToLower();

                    switch (userInput)
                    {
                        case "a":
                            sensorsToMonitor = LightAlarmDisplaySetSensorsToMonitor(myFinch);
                            break;

                        case "b":
                            rangeType = LightAlarmDisplaySetRangeType();
                            break;

                        case "c":
                            minMaxThresholdValue = LightAlarmSetMinMaxThresholdValue(rangeType, myFinch);
                            break;

                        case "d":
                            timeToMonitor = LightAlarmSetTimeToMonitor();

                            break;

                        case "e":
                            LightAlarmSetAlarm(myFinch, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor);
                            break;

                        case "q":
                            MainMenu();
                            quitMenu = true;
                            break;

                        default:
                            Console.WriteLine();
                            Console.WriteLine("\tPlease select a letter from the menu");
                            DisplayContinuePrompt();
                            break;
                    }

                } while (!quitMenu);
            }

        }

        static void LightAlarmSetAlarm(
            Finch myFinch,
            string sensorsToMonitor,
            string rangeType,
            int minMaxThresholdValue,
            int timeToMonitor)
        {
            int secondsElapsed = 0;
            bool thresholdExceeded = false;
            int currentLightSensorValue = 0;

            DisplayScreenHeader("Set Alarm");

            Console.WriteLine("\tSensors to monitor: {0}", sensorsToMonitor);
            Console.WriteLine("\tRange type: {0}", rangeType);
            Console.WriteLine("\tMinimum/Maximum Threshold Value: {0}", minMaxThresholdValue);
            Console.WriteLine("\tTime to monitor: {0}", timeToMonitor);
            Console.WriteLine("\tCurrent light reading is {0} on the left and {1} on the right", myFinch.getLeftLightSensor(), myFinch.getRightLightSensor());
            Console.WriteLine();

            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
            Console.WriteLine();

            while ((secondsElapsed < timeToMonitor) && (!thresholdExceeded))
            {

                switch (sensorsToMonitor)
                {
                    case "left":
                        currentLightSensorValue = myFinch.getLeftLightSensor();
                        break;

                    case "right":
                        currentLightSensorValue = myFinch.getRightLightSensor();
                        break;

                    case "both":
                        currentLightSensorValue = (myFinch.getLeftLightSensor() + myFinch.getRightLightSensor()) / 2;
                        break;
                }

                switch (rangeType)
                {
                    case "minimum":
                        if (currentLightSensorValue < minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;

                    case "maximum":
                        if (currentLightSensorValue > minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }

                        break;
                }

                myFinch.wait(1000);
                secondsElapsed++;
            }

            if (thresholdExceeded)
            {
                Console.WriteLine("\t****************************************************");
                Console.WriteLine("\t* The {0} value has been exceeded by a value of {1}*", rangeType, currentLightSensorValue);
                Console.WriteLine("\t****************************************************");
            }
            else
            {
                Console.WriteLine("\t**********************************************");
                Console.WriteLine("\t* The {0} value of {1} has not been exceeded *", rangeType, minMaxThresholdValue);
                Console.WriteLine("\t**********************************************");
            }

            Console.WriteLine();
            DisplayContinuePrompt();

        }

        static int LightAlarmSetMinMaxThresholdValue(string rangeType, Finch myFinch)
        {
            int minMaxThresholdValue;
            Console.CursorVisible = true;

            DisplayScreenHeader("Minimum/Maximum Threshold Value");

            Console.WriteLine("\tLeft Ambient Light Value: {0}", myFinch.getLeftLightSensor());
            Console.WriteLine("\tRight Ambient Light Value: {0}", myFinch.getRightLightSensor());
            Console.WriteLine();

            Console.Write("\tEnter the {0} light sensor threshold value: ", rangeType);
            bool parseSuccess = int.TryParse(Console.ReadLine(), out minMaxThresholdValue);

            if (parseSuccess)
            {
                Console.WriteLine();
                Console.WriteLine("\tYour {0} value is {1}", rangeType, minMaxThresholdValue);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("\tPlease enter an integer.");
            }



            DisplayContinuePrompt();
            return minMaxThresholdValue;


        }

        static string LightAlarmDisplaySetSensorsToMonitor(Finch myFinch)
        {
            string sensorsToMonitor = null;
            string userInput;
            bool quitMenu = false;

            DisplayScreenHeader("Sensors to Monitor");


            do
            {
                Console.WriteLine("\tChoose which sensors to monitor.");
                Console.WriteLine();
                Console.WriteLine("\ta) Left");
                Console.WriteLine("\tb) Right");
                Console.WriteLine("\tc) Both");
                Console.WriteLine("\tq) Return to Main Menu");

                Console.WriteLine();
                Console.WriteLine();
                Console.SetCursorPosition(20, 10);
                Console.WriteLine("\tLeft Ambient Light Value: {0}", myFinch.getLeftLightSensor());
                Console.SetCursorPosition(55, 10);
                Console.WriteLine("\tRight Ambient Light Value: {0}", myFinch.getRightLightSensor());
                Console.WriteLine();
                Console.WriteLine();
                Console.CursorVisible = true;
                Console.Write("\t Enter your selection: ");

                userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case "a":
                        sensorsToMonitor = "left";
                        quitMenu = true;
                        break;

                    case "b":
                        sensorsToMonitor = "right";
                        quitMenu = true;
                        break;

                    case "c":
                        sensorsToMonitor = "both";
                        quitMenu = true;
                        break;

                    case "q":
                        MainMenu();
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease select a letter from the menu");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);

            Console.WriteLine();
            Console.WriteLine("\tSensors to monitor set to {0}", sensorsToMonitor);
            Console.WriteLine();
            DisplayContinuePrompt();
            return sensorsToMonitor;
        }

        static string LightAlarmDisplaySetRangeType()
        {

            DisplayScreenHeader("Set Range Type");

            string rangeType = null;
            string userInput;
            bool quitMenu = false;

            do
            {
                Console.WriteLine("\tChoose which range type to monitor:");
                Console.WriteLine();
                Console.WriteLine("\ta) Minimum");
                Console.WriteLine("\tb) Maximum");
                Console.WriteLine("\tq) Return to Main Menu");
                Console.Write("\t Enter your selection: ");
                userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case "a":
                        rangeType = "minimum";
                        quitMenu = true;
                        break;

                    case "b":
                        rangeType = "maximum";
                        quitMenu = true;
                        break;

                    case "q":
                        MainMenu();
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease select a letter from the menu");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);


            Console.WriteLine();
            Console.WriteLine("\tRange type set to {0}", rangeType);
            DisplayContinuePrompt();
            return rangeType;
        }

        static int LightAlarmSetTimeToMonitor()
        {
            int timeToMonitor;
            Console.CursorVisible = true;

            DisplayScreenHeader("Set Time to Monitor");

            Console.Write("\tEnter time to monitor in seconds: ");
            //int.TryParse(Console.ReadLine(), out timeToMonitor);
            bool parseSuccess = int.TryParse(Console.ReadLine(), out timeToMonitor);

            if (parseSuccess)
                Console.WriteLine("\tYour time to monitor is {0}", timeToMonitor);

            else
                Console.WriteLine("\tPlease enter an integer.");

            DisplayContinuePrompt();
            return timeToMonitor;


        }

        static void TemperatureAlarmDisplayMenuScreen(Finch myFinch)
        {

            {

                bool quitMenu = false;
                string userInput;

                string sensorsToMonitor = "";
                string rangeType = "";
                double minMaxThresholdValue = 0;
                int timeToMonitor = 0;


                Console.Clear();


                do
                {
                    DisplayScreenHeader("\tTemperature Alarm Menu");
                    Console.WriteLine();
                    Console.WriteLine("\ta) Set Range Type");
                    Console.WriteLine("\tb) Set Minimum/Maximum Threshold");
                    Console.WriteLine("\tc) Set Time to Monitor");
                    Console.WriteLine("\td) Set Alarm");
                    Console.WriteLine("\tq) Return to Main Menu");
                    Console.Write("\t Enter your selection: ");
                    userInput = Console.ReadLine().ToLower();

                    switch (userInput)
                    {
                        case "a":
                            rangeType = TemperatureAlarmDisplaySetRangeType();
                            break;

                        case "b":
                            minMaxThresholdValue = TemperatureAlarmSetMinMaxThresholdValue(rangeType, myFinch);
                            break;

                        case "c":
                            timeToMonitor = TemperatureAlarmSetTimeToMonitor();
                            break;

                        case "d":
                            TemperatureAlarmSetAlarm(myFinch, rangeType, minMaxThresholdValue, timeToMonitor);
                            break;


                        case "q":
                            MainMenu();
                            quitMenu = true;
                            break;

                        default:
                            Console.WriteLine();
                            Console.WriteLine("\tPlease select a letter from the menu");
                            DisplayContinuePrompt();
                            break;
                    }

                } while (!quitMenu);
            }

        }

        static double TemperatureAlarmSetAlarm(Finch myFinch,
            string rangeType,
            double minMaxThresholdValue,
            int timeToMonitor)
        {
            int secondsElapsed = 0;
            bool thresholdExceeded = false;
            double currentTemperature = 0;
            bool sensorsToMonitor = true;
            double currentTempInFarenheit = 0;

            DisplayScreenHeader("Set Alarm");

            Console.WriteLine("\tSensors to monitor: {0}", sensorsToMonitor);
            Console.WriteLine("\tRange type: {0}", rangeType);
            Console.WriteLine("\tMinimum/Maximum Threshold Value: {0}", minMaxThresholdValue);
            Console.WriteLine("\tTime to monitor: {0}", timeToMonitor);
            Console.WriteLine("\tCurrent temperature reading is {0}", GetTempInFarenheit(myFinch));
            Console.WriteLine();

            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
            Console.WriteLine();

            while ((secondsElapsed < timeToMonitor) && (!thresholdExceeded))
            {

                switch (sensorsToMonitor)
                {
                    case true:
                        currentTempInFarenheit = GetTempInFarenheit(myFinch);
                        break;

                }

                switch (rangeType)
                {
                    case "minimum":
                        if (currentTemperature < minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;

                    case "maximum":
                        if (currentTemperature > minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }

                        break;
                }

                myFinch.wait(1000);
                secondsElapsed++;
            }

            if (thresholdExceeded)
            {
                Console.WriteLine("\t****************************************************");
                Console.WriteLine("\t* The {0} value has been exceeded by a value of {1}*", rangeType, currentTempInFarenheit);
                Console.WriteLine("\t****************************************************");
            }
            else
            {
                Console.WriteLine("\t**********************************************");
                Console.WriteLine("\t* The {0} value of {1} has not been exceeded *", rangeType, minMaxThresholdValue);
                Console.WriteLine("\t**********************************************");
            }

            Console.WriteLine();
            DisplayContinuePrompt();

            return currentTemperature;
        }

        static double GetTempInFarenheit(Finch myFinch)
        {
            double currentTempInFarenheit;
            double currentTemp;

            currentTemp = myFinch.getTemperature();

            currentTempInFarenheit = currentTemp * 1.8 + 32;

            return currentTempInFarenheit;
        }

        private static int TemperatureAlarmSetTimeToMonitor()
        {
            int timeToMonitor;
            Console.CursorVisible = true;

            DisplayScreenHeader("Set Time to Monitor");

            Console.Write("\tEnter time to monitor in seconds: ");
            //int.TryParse(Console.ReadLine(), out timeToMonitor);
            bool parseSuccess = int.TryParse(Console.ReadLine(), out timeToMonitor);

            if (parseSuccess)
                Console.WriteLine("\tYour time to monitor is {0}", timeToMonitor);

            else
                Console.WriteLine("\tPlease enter an integer.");

            DisplayContinuePrompt();
            return timeToMonitor;
        }

        private static double TemperatureAlarmSetMinMaxThresholdValue(string rangeType, Finch myFinch)
        {
            double minMaxThresholdValue;
            Console.CursorVisible = true;

            DisplayScreenHeader("Minimum/Maximum Threshold Value");

            Console.WriteLine("\tAmbient Temperature Value: {0}", GetTempInFarenheit(myFinch));
            Console.WriteLine();

            Console.Write("\tEnter the {0} temperature threshold value: ", rangeType);
            bool parseSuccess = double.TryParse(Console.ReadLine(), out minMaxThresholdValue);

            if (parseSuccess)
            {
                Console.WriteLine();
                Console.WriteLine("\tYour {0} value is {1}", rangeType, minMaxThresholdValue);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("\tPlease enter an integer.");
            }



            DisplayContinuePrompt();
            return minMaxThresholdValue;
        }

        private static string TemperatureAlarmDisplaySetRangeType()
        {
            DisplayScreenHeader("Set Range Type");

            string rangeType = null;
            string userInput;
            bool quitMenu = false;

            do
            {
                Console.WriteLine("\tChoose which range type to monitor:");
                Console.WriteLine();
                Console.WriteLine("\ta) Minimum");
                Console.WriteLine("\tb) Maximum");
                Console.WriteLine("\tq) Return to Main Menu");
                Console.Write("\t Enter your selection: ");
                userInput = Console.ReadLine().ToLower();

                switch (userInput)
                {
                    case "a":
                        rangeType = "minimum";
                        quitMenu = true;
                        break;

                    case "b":
                        rangeType = "maximum";
                        quitMenu = true;
                        break;

                    case "q":
                        MainMenu();
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease select a letter from the menu");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);


            Console.WriteLine();
            Console.WriteLine("\tRange type set to {0}", rangeType);
            DisplayContinuePrompt();
            return rangeType;
        }





        #endregion



    }


}