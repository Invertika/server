//
//  Program.cs
//
//  This file is part of Invertika (http://invertika.org)
// 
//  Based on The Mana Server (http://manasource.org)
//  Copyright (C) 2004-2012  The Mana World Development Team 
//
//  Author:
//       seeseekey <seeseekey@googlemail.com>
// 
//  Copyright (c) 2011, 2012 by Invertika Development Team
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using invertika_account.Account;
using invertika_account.Chat;
using invertika_account.Common;
using invertika_account.Utilities;
using CSCL;
using System.IO;
using System.Timers;
using System.Threading;
using ISL.Server.Utilities;
using ISL.Server.Common;
using ISL.Server.Enums;
using ISL.Server.Network;
using System.Diagnostics;

namespace invertika_account
{
	class Program
	{
		// Default options that automake should be able to override.
		const string DEFAULT_LOG_FILE="manaserv-account.log";
		const string DEFAULT_STATS_FILE="manaserv.stats";
		const string DEFAULT_ATTRIBUTEDB_FILE="attributes.xml";

		static bool running=true;        /**< Determines if server keeps running */

		public static StringFilter stringFilter; /**< Slang's Filter */

		static string statisticsFile="";

		/** Database handler. */
		public static Storage storage;

		/** Communications (chat) message handler */
		public static ChatHandler chatHandler;

		public static ChatChannelManager chatChannelManager;
		public static GuildManager guildManager;
		public static PostManager postalManager;
		static BandwidthMonitor gBandwidth;

		//static Configuration Configuration;

		/** Callback used when SIGQUIT signal is received. */
		static void closeGracefully(int val)
		{
			running=false;
		}

		/**
		 * Initializes the server.
		 */
		static void initialize()
		{
			// TODO Abfangen des Abbruchssignales und entsprechend darauf reagieren
			// Used to close via process signals
			//#if (defined __USE_UNIX98 || defined __FreeBSD__)
			//    signal(SIGQUIT, closeGracefully);
			//#endif
			//    signal(SIGINT, closeGracefully);
			//    signal(SIGTERM, closeGracefully);

			//string logFile=Configuration.getValue("log_accountServerFile", DEFAULT_LOG_FILE);

			//// Initialize PhysicsFS
			////PHYSFS_init("");

			//Logger.Init(logFile);

			// Indicate in which file the statistics are put.
			statisticsFile=Configuration.getValue("log_statisticsFile", DEFAULT_STATS_FILE);
			Logger.Write(LogLevel.Information, "Using statistics file: {0}", statisticsFile);

			//ResourceManager::initialize(); //TODO Check

			// Open database //TODO wieder einkommentieren
//#if! DEBUG
//			try
//			{
//#endif
				storage=new Storage();
				storage.open();
//#if! DEBUG
//			}
//			catch(Exception ex)
//			{
//				Logger.Add(LogLevel.Error, "Error opening the database: {0}", ex.ToString());
//				System.Environment.Exit((int)ExitValue.EXIT_DB_EXCEPTION);
//			}
//#endif

			// --- Initialize the managers
			stringFilter = new StringFilter();  // The slang's and double quotes filter.
			chatChannelManager=new ChatChannelManager();
			guildManager=new GuildManager();
			postalManager=new PostManager();
			gBandwidth=new BandwidthMonitor();

			// --- Initialize the global handlers
			// FIXME: Make the global handlers global vars or part of a bigger
			// singleton or a local variable in the event-loop
			chatHandler=new ChatHandler();

			// Initialize the processor utility functions
			//utils::processor::init(); //TODO Überprüfen

			// Seed the random number generator
			//std::srand( time(NULL) ); //TODO Überprüfen
		}


		/**
		 * Deinitializes the server.
		 */
		static void deinitializeServer() //TODO Funktion nochmals überprüfen
		{
			// Write configuration file
			Configuration.deinitialize(); 

			// Destroy message handlers.
			AccountClientHandler.deinitialize();
			GameServerHandler.deinitialize();

			// Quit ENet
			//enet_deinitialize();

			//delete chatHandler;

			// Destroy Managers
			//delete stringFilter;
			//delete chatChannelManager;
			//delete guildManager;
			//delete postalManager;
			//delete gBandwidth;

			// Get rid of persistent data storage
			//delete storage;

			//PHYSFS_deinit();
		}

		/**
		 * Dumps statistics.
		 */
		static void dumpStatistics(string accountAddress, int accountClientPort, int accountGamePort, int chatClientPort)
		{
			StreamWriter sw=new StreamWriter(statisticsFile);

			sw.WriteLine("<statistics>");

			// Print last heartbeat
			sw.WriteLine("<heartbeat=\"{0}_{1}\" />", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
	
			//// Add account server information
			sw.WriteLine("<accountserver address=\"{0}\" clientport=\"{1}\" gameport=\"{2}\" chatclientport=\"{3}\" />", accountAddress, accountClientPort, accountGamePort, chatClientPort);

			//// Add game servers information
			GameServerHandler.dumpStatistics(sw); //TODO überprüfen
			sw.WriteLine("</statistics>");

			sw.Close();
		}

		/**
		 * Show command line arguments
		 */
		static void printHelp()
		{
			Console.WriteLine("manaserv");
			Console.WriteLine("Options: ");
			Console.WriteLine("  -h --help          : Display this help");
			Console.WriteLine("     --config <path> : Set the config path to use.");
			Console.WriteLine(" (Default: ./manaserv.xml)");
			Console.WriteLine("     --verbosity <n> : Set the verbosity level");
			Console.WriteLine("                        - 0. Fatal Errors only.");
			Console.WriteLine("                        - 1. All Errors.");
			Console.WriteLine("                        - 2. Plus warnings.");
			Console.WriteLine("                        - 3. Plus standard information.");
			Console.WriteLine("                        - 4. Plus debugging information.");
			Console.WriteLine("     --port <n>      : Set the default port to listen on");

			System.Environment.Exit((int)ExitValue.EXIT_NORMAL);
		}

		/**
		 * Parse the command line arguments
		 */
		static void parseOptions(string[] args, out CommandLineOptions options)
		{
			Parameters param=Parameters.InterpretCommandLine(args);

			options=new CommandLineOptions();

			if(param.GetBool("help"))
			{
				//Print help.
				printHelp();
			}
			else
			{
				//TODO überprüfen ob getBool auch funktioniert wenn danach ein Doppelpunkt kommt
				if(param.GetBool("config")) //-config:invertika.xml
				{
					options.configPath=param.GetString("config", "");
				}

				if(param.GetBool("verbosity")) //-verbosity:3
				{
					options.verbosity=(LogLevel)(param.GetInt32("verbosity", 1));
					options.verbosityChanged=true; //TODO richtig so?
					Logger.Write(LogLevel.Information, "Using log verbosity level {0}", options.verbosity);
				}

				if(param.GetBool("port")) //-port:1234
				{
					options.port=param.GetInt32("verbosity", 1);
					options.portChanged=true;
				}
			}
		}

		public static void statTimerEvent(object sender, ElapsedEventArgs e)
		{
			object obj=((AdvancedTimer)(sender)).Parameter;
			List<object> parameters=(List<object>)obj;

			string accountHost=parameters[0].ToString();
			int port=(int)parameters[1];
			int accountGamePort=(int)parameters[2];
			int chatClientPort=(int)(ushort)parameters[3];

			dumpStatistics(accountHost, port, accountGamePort, chatClientPort);
		}

		public static void banTimerEvent(object sender, ElapsedEventArgs e)
		{
			storage.checkBannedAccounts();
		}

		/**
		 * Main function, initializes and runs server.
		 */
		static int Main(string[] args)
		{
			//Logger
			Logger.ChangeLogMode(LogMode.Debug);

			 //Parse command line options
			CommandLineOptions options;
			parseOptions(args, out options);

			try
			{
				if(options.configPath==null) options.configPath=Configuration.DEFAULT_CONFIG_FILE;
				Configuration.Init(options.configPath);
			}
			catch
			{
				Logger.Write(LogLevel.Error, "Refusing to run without configuration!");
				System.Environment.Exit((int)ExitValue.EXIT_CONFIG_NOT_FOUND);
			}

			string logFile=Configuration.getValue("log_accountServerFile", DEFAULT_LOG_FILE);
			Logger.Init(logFile);
			Logger.ChangeLogMode(LogMode.Debug); //TODO hier könnte auf File gestellt werden

			// Check inter-server password.
			if (Configuration.getValue("net_password", "")=="")
			{
				Logger.Write(LogLevel.Error, "SECURITY WARNING: 'net_password' not set!");
				System.Environment.Exit((int)ExitValue.EXIT_BAD_CONFIG_PARAMETER);
			}

			// General initialization
			initialize();

			Logger.Write(LogLevel.Information, "The Mana Account+Chat Server v{0}", Various.AssemblyVersion);
			Logger.Write(LogLevel.Information, "Manaserv Protocol version {0}, Database version {1}", ManaServ.PROTOCOL_VERSION, ManaServ.SUPPORTED_DB_VERSION);

			if (!options.verbosityChanged)
			{
			    options.verbosity = (LogLevel)Configuration.getValue("log_accountServerLogLevel", (int)options.verbosity);
			}
			//Logger::setVerbosity(options.verbosity); //TODO wird das überhaupt benutzt? //es müssten wohl nicht dem Ding entsprehcende Level entfern werden

			string accountHost=Configuration.getValue("net_accountHost", "localhost");

			// We separate the chat host as the chat server will be separated out
			// from the account server.
			string chatHost=Configuration.getValue("net_chatHost", "localhost");

			// Setting the listen ports
			// Note: The accountToGame and chatToClient listen ports auto offset
			// to accountToClient listen port when they're not set,
			// or to DEFAULT_SERVER_PORT otherwise.
			if(!options.portChanged)
			{
			    options.port = Configuration.getValue("net_accountListenToClientPort", options.port);
			}

			int accountGamePort = Configuration.getValue("net_accountListenToGamePort", options.port + 1);
			ushort chatClientPort = (ushort)Configuration.getValue("net_chatListenToClientPort", options.port + 2);

			if (!AccountClientHandler.initialize(DEFAULT_ATTRIBUTEDB_FILE, options.port, accountHost) || 
				!GameServerHandler.initialize(accountGamePort, accountHost) || 
				!chatHandler.startListen(chatClientPort, chatHost))
			{
			    Logger.Write(LogLevel.Error, "Unable to create an ENet server host.");
			    System.Environment.Exit((int)ExitValue.EXIT_NET_EXCEPTION);
			}

			// Dump statistics every 10 seconds.
			AdvancedTimer statTimer=new AdvancedTimer();

			List<object> paramsStatTimer=new List<object>();
			paramsStatTimer.Add(accountHost);
			paramsStatTimer.Add(options.port);
			paramsStatTimer.Add(accountGamePort);
			paramsStatTimer.Add(chatClientPort);

			statTimer.Parameter=paramsStatTimer;

			statTimer.Elapsed+=new ElapsedEventHandler(statTimerEvent);
			statTimer.Interval=10000;
			statTimer.AutoReset=true;
			statTimer.Start();

			// Check for expired bans every 30 seconds
			AdvancedTimer banTimer=new AdvancedTimer();

			banTimer.Elapsed+=new ElapsedEventHandler(banTimerEvent);
			banTimer.Interval=30000;
			banTimer.AutoReset=true;
			banTimer.Start();

			// -------------------------------------------------------------------------
			// FIXME: for testing purposes only...
			// writing accountserver startup time and svn revision to database as global
			// world state variable
			DateTime startup=DateTime.Now;
			storage.setWorldStateVar("accountserver_startup", startup.ToString());
			const string revision = "$Revision$";
			storage.setWorldStateVar("accountserver_version", revision);
			// -------------------------------------------------------------------------

			//AccountClientHandler
			Thread accountServerThread;	// Der Thread in dem die Process Funktion läuft
			accountServerThread=new Thread(AccountClientHandler.process);
			accountServerThread.Name="AccountClientHandler Thread";
			accountServerThread.Start();

			//GameServerHandler
			Thread gameServerThread;	// Der Thread in dem die Process Funktion läuft
			gameServerThread=new Thread(GameServerHandler.process);
			gameServerThread.Name="GameServerHandler Thread";
			gameServerThread.Start();

			//chatHandler
			Thread chatServerThread;	// Der Thread in dem die Process Funktion läuft
			chatServerThread=new Thread(chatHandler.process);
			chatServerThread.Name="Chathandler Thread";
			chatServerThread.Start();

			while(running)
			{
				//AccountClientHandler.process();
				//GameServerHandler.process();
				//chatHandler.process(50);

				Thread.Sleep(50); //ssk
			}

			Logger.Write(LogLevel.Information, "Received: Quit signal, closing down...");
			chatHandler.stopListen();
			deinitializeServer();

			return (int)ExitValue.EXIT_NORMAL;
		}
	}
}
