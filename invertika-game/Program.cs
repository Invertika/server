using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISL.Server.Common;
using invertika_game.Game;
using ISL.Server.Enums;
using CSCL;
using ISL.Server.Utilities;
using System.Threading;
using invertika_game.Utils;
using ISL.Server.Network;
using invertika_game.Common;
using invertika_game.Scripting;

namespace invertika_game
{
	class Program
	{
		// Default options that automake should be able to override.
		const string DEFAULT_LOG_FILE="manaserv-game.log";
		const string DEFAULT_ITEMSDB_FILE="items.xml";
		const string DEFAULT_EQUIPDB_FILE="equip.xml";
		const string DEFAULT_SKILLSDB_FILE="skills.xml";
		const string DEFAULT_ATTRIBUTEDB_FILE="attributes.xml";
		const string DEFAULT_MAPSDB_FILE="maps.xml";
		const string DEFAULT_MONSTERSDB_FILE="monsters.xml";
		const string DEFAULT_STATUSDB_FILE="status-effects.xml";
		const string DEFAULT_PERMISSION_FILE="permissions.xml";
		const string DEFAULT_GLOBAL_EVENT_SCRIPT_FILE="scripts/main.lua";

		const int WORLD_TICK_SKIP=2; /** tolerance for lagging behind in world calculation) **/

		/** Timer for world ticks */
		//utils::Timer worldTimer(WORLD_TICK_MS);
		static Utils.Timer worldTimer;

		static int worldTime=0;              /**< Current world time in ticks */
		static bool running=true;            /**< Determines if server keeps running */

		static StringFilter stringFilter; /**< Slang's Filter */

		static AttributeManager attributeManager=new AttributeManager(DEFAULT_ATTRIBUTEDB_FILE);
		static ItemManager itemManager=new ItemManager(DEFAULT_ITEMSDB_FILE, DEFAULT_EQUIPDB_FILE);
		static MonsterManager monsterManager=new MonsterManager(DEFAULT_MONSTERSDB_FILE);
		static SkillManager skillManager=new SkillManager(DEFAULT_SKILLSDB_FILE);

		///** Core game message handler */
		static GameHandler gameHandler;

		///** Account server message handler */
		static AccountConnection accountHandler;

		///** Post Man **/
		static PostMan postMan;

		///** Bandwidth Monitor */
		static BandwidthMonitor gBandwidth;

		/** Callback used when SIGQUIT signal is received. */
		static void closeGracefully(int sig)
		{
			running=false;
		}

		static void initializeServer()
		{
			// Used to close via process signals
			//#if (defined __USE_UNIX98 || defined __FreeBSD__)
			//    signal(SIGQUIT, closeGracefully);
			//#endif
			//    signal(SIGINT, closeGracefully);
			//    signal(SIGTERM, closeGracefully);

			// Initialize PhysicsFS
			//PHYSFS_init("");


			// --- Initialize the managers
			// Initialize the slang's and double quotes filter.
			stringFilter=new StringFilter();

			ResourceManager.initialize();

			if(MapManager.initialize(DEFAULT_MAPSDB_FILE)<1)
			{
				Logger.Add(LogLevel.Fatal, "The Game Server can't find any valid/available maps.");
				System.Environment.Exit((int)ExitValue.EXIT_MAP_FILE_NOT_FOUND);
			}

			attributeManager.initialize();
			skillManager.initialize();
			itemManager.initialize();
			monsterManager.initialize();
			StatusManager.initialize(DEFAULT_STATUSDB_FILE);
			PermissionManager.initialize(DEFAULT_PERMISSION_FILE);

			string mainScriptFile=Configuration.getValue("script_mainFile", DEFAULT_GLOBAL_EVENT_SCRIPT_FILE);
			Script.loadGlobalEventScript(mainScriptFile);

			// --- Initialize the global handlers
			// FIXME: Make the global handlers global vars or part of a bigger
			// singleton or a local variable in the event-loop
			gameHandler=new GameHandler();
			accountHandler=new AccountConnection();
			postMan=new PostMan();
			gBandwidth=new BandwidthMonitor();
		}


		static void deinitializeServer()
		{
			//// Write configuration file
			//Configuration::deinitialize();

			//// Stop world timer
			//worldTimer.stop();

			//// Quit ENet
			//enet_deinitialize();

			//// Destroy message handlers
			//delete gameHandler; gameHandler = 0;
			//delete accountHandler; accountHandler = 0;
			//delete postMan; postMan = 0;
			//delete gBandwidth; gBandwidth = 0;

			//// Destroy Managers
			//delete stringFilter; stringFilter = 0;
			//delete monsterManager; monsterManager = 0;
			//delete skillManager; skillManager = 0;
			//delete itemManager; itemManager = 0;
			//MapManager::deinitialize();
			//StatusManager::deinitialize();

			//PHYSFS_deinit();
		}


		/**
		 * Show command line arguments.
		 */
		static void printHelp()
		{
			Console.WriteLine("manaserv");
			Console.WriteLine("Options: ");
			Console.WriteLine("-h --help          : Display this help ");
			Console.WriteLine("      --config <path> : Set the config path to use.");
			Console.WriteLine("  (Default: ./manaserv.xml)");
			Console.WriteLine("      --verbosity <n> : Set the verbosity level");
			Console.WriteLine("                         - 0. Fatal Errors only.");
			Console.WriteLine("                         - 1. All Errors.");
			Console.WriteLine("                         - 2. Plus warnings.");
			Console.WriteLine("                         - 3. Plus standard information.");
			Console.WriteLine("                         - 4. Plus debugging information.");
			Console.WriteLine("      --port <n>      : Set the default port to listen on.");

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
					Logger.Add(LogLevel.Information, "Using log verbosity level {0}", options.verbosity);
				}

				if(param.GetBool("port")) //-port:1234
				{
					options.port=param.GetInt32("verbosity", 1);
					options.portChanged=true;
				}
			}
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
				Logger.Add(LogLevel.Error, "Refusing to run without configuration!");
				System.Environment.Exit((int)ExitValue.EXIT_CONFIG_NOT_FOUND);
			}

			string logFile=Configuration.getValue("log_accountServerFile", DEFAULT_LOG_FILE);
			Logger.Init(logFile);
			Logger.ChangeLogMode(LogMode.Debug); //TODO hier könnte auf File gestellt werden

			// Check inter-server password.
			if(Configuration.getValue("net_password", "")=="")
			{
				Logger.Add(LogLevel.Error, "SECURITY WARNING: 'net_password' not set!");
				System.Environment.Exit((int)ExitValue.EXIT_BAD_CONFIG_PARAMETER);
			}

			// General initialization
			initializeServer();

			Logger.Add(LogLevel.Information, "The Mana Account+Chat Server v{0}", Various.AssemblyVersion);
			Logger.Add(LogLevel.Information, "Manaserv Protocol version {0}, Database version {1}", ManaServ.PROTOCOL_VERSION, ManaServ.SUPPORTED_DB_VERSION);

			if(!options.verbosityChanged)
			{
				options.verbosity=(LogLevel)Configuration.getValue("log_accountServerLogLevel", (int)options.verbosity);
			}
			//Logger::setVerbosity(options.verbosity); //TODO wird das überhaupt benutzt? //es müssten wohl nicht dem Ding entsprehcende Level entfern werden


			// When the gameListenToClientPort is set, we use it.
			// Otherwise, we use the accountListenToClientPort + 3 if the option is set.
			// If neither, the DEFAULT_SERVER_PORT + 3 is used.
			if(!options.portChanged)
			{
				// Prepare the fallback value
				options.port=Configuration.getValue("net_accountListenToClientPort", 0)+3;
				if(options.port==3) options.port=Configuration.DEFAULT_SERVER_PORT+3;

				// Set the actual value of options.port
				options.port=Configuration.getValue("net_gameListenToClientPort", options.port);
			}

			// Make an initial attempt to connect to the account server
			// Try again after longer and longer intervals when connection fails.
			bool isConnected=false;
			int waittime=0;

			while(!isConnected&&running)
			{
				Logger.Add(LogLevel.Information, "Connecting to account server");
				isConnected=accountHandler.start(options.port);

				if(!isConnected)
				{
					Logger.Add(LogLevel.Information, "Retrying in {0} seconds", waittime);
					Thread.Sleep(waittime*1000);
				}
			}

			if(!gameHandler.startListen((ushort)options.port))
			{
				Logger.Add(LogLevel.Fatal, "Unable to create an server host.");
				System.Environment.Exit((int)ExitValue.EXIT_NET_EXCEPTION);
			}

			// Initialize world timer
			worldTimer.start();

			// Account connection lost flag
			bool accountServerLost=false;
			int elapsedWorldTicks=0;

			while(running)
			{
				elapsedWorldTicks=worldTimer.poll();
				if(elapsedWorldTicks==0) worldTimer.sleep();

				while(elapsedWorldTicks>0)
				{
					if(elapsedWorldTicks>WORLD_TICK_SKIP)
					{
						Logger.Add(LogLevel.Warning, "Skipped {0} world tick due to insufficient CPU time.", elapsedWorldTicks-1);
						elapsedWorldTicks=1;
					}

					worldTime++;
					elapsedWorldTicks--;

					// Print world time at 10 second intervals to show we're alive
					if(worldTime%100==0)
					{
						Logger.Add(LogLevel.Information, "World time: {0}", worldTime);

					}

					if(accountHandler.isConnected())
					{
						accountServerLost=false;

						// Handle all messages that are in the message queues
						accountHandler.process();

						if(worldTime%100==0)
						{
							accountHandler.syncChanges(true);
							// force sending changes to the account server every 10 secs.
						}

						if(worldTime%300==0)
						{
							accountHandler.sendStatistics();
							Logger.Add(LogLevel.Information, "Total Account Output: {0} Bytes", gBandwidth.totalInterServerOut());
							Logger.Add(LogLevel.Information, "Total Account Input: {0} Bytes", gBandwidth.totalInterServerIn());
							Logger.Add(LogLevel.Information, "Total Client Output: {0} Bytes", gBandwidth.totalClientOut());
							Logger.Add(LogLevel.Information, "Total Client Input: {0} Bytes", gBandwidth.totalClientIn());
						}
					}
					else
					{
						// If the connection to the account server is lost.
						// Every players have to be logged out
						if(!accountServerLost)
						{
							Logger.Add(LogLevel.Warning, "The connection to the account server was lost.");
							accountServerLost=true;
						}

						// Try to reconnect every 200 ticks
						if(worldTime%200==0)
						{
							accountHandler.start(options.port);
						}
					}

					gameHandler.process();

					// Update all active objects/beings
					GameState.update(worldTime);

					// Send potentially urgent outgoing messages
					gameHandler.flush();
				}
			}

			Logger.Add(LogLevel.Information, "Received: Quit signal, closing down...");

			gameHandler.stopListen();
			accountHandler.stop();
			deinitializeServer();

			return (int)ExitValue.EXIT_NORMAL;
		}
	}
}
