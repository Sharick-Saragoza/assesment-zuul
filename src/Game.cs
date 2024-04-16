using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;
	// Constructor
	public Game()
	{
		parser = new Parser();
		// Player instance
		player = new Player();
		
		
		
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms
		Room bathroom = new Room("in the bathroom. The air is filled with the scent of poop.");
		Room classroom = new Room("in the classroom");
		Room library = new Room("in the library");
		Room principaloffice = new Room ("in the principal's office");
		Room technologyroom = new Room("in the technologyroom");
		Room canteen = new Room("in the canteen");
		Room outside = new Room("outside the main entrance of the university");
		Room theatre = new Room("in a lecture theatre");
		Room pub = new Room("in the underground campus pub");
		Room lab = new Room("in a science lab");
		Room office = new Room("in the teachers office");
		Room balcony = new Room("on the balcony of the principal's office there seems to be a key here!");
		Room basement = new Room("in a dusty lab basement");
	
		// Initialise room exits
		outside.Lock();
		bathroom.AddExit("north", library);
		bathroom.AddExit("east", classroom);
		bathroom.AddExit("south", office);
		bathroom.AddExit("west", canteen);

		library.AddExit("south", bathroom);
		library.AddExit("east", lab);

		lab.AddExit("west", library);
		lab.AddExit("east", technologyroom);
		lab.AddExit("south", classroom);

		technologyroom.AddExit("west", lab);
		
		classroom.AddExit("north", lab);
		classroom.AddExit("west", bathroom);
		classroom.AddExit("east", theatre);

		theatre.AddExit("north", classroom);

		canteen.AddExit("east", bathroom);
		canteen.AddExit("west", outside);

		office.AddExit("north", bathroom);

		principaloffice.AddExit("east", balcony);

		balcony.AddExit("west", principaloffice);

		// Kamers naar boven
		basement.AddExit("up", technologyroom);
		pub.AddExit("up", canteen);
		office.AddExit("up", principaloffice);

		// Kamers naar beneden
		technologyroom.AddExit("down", basement);
		canteen.AddExit("down", pub);
		principaloffice.AddExit("down", office);

		// Create your Items here
		Item key = new Item(5, "rusty key");
		Item lockpick = new Item(10, "lockpick");
		Item bandage = new Item(5, "bandage");
		Item apple = new Item(1, "almost rotten apple");
		Item crowbar = new Item(15, "crowbar");

		// And add them to the Rooms
		canteen.Chest.Put("apple", apple);
		lab.Chest.Put("bandage", bandage);

		// basement.Chest.Put("crowbar", crowbar);  
		balcony.Chest.Put("key", key);
		// pub.Chest.Put("lockpick", lockpick);
		

		
		// Start game outside
		player.currentRoom = bathroom;
	}
		
	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		while (!finished && !player.isDead)
		{
			Command command = parser.GetCommand();
			finished = ProcessCommand(command);
		}
		
		Console.WriteLine("You've lost too much blood and died!");
		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to continue.");
		Console.ReadLine();
		
	}

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to Zuul!");
		Console.WriteLine("You fell asleep in the bathroom and wake up at 3 AM needing to escape without getting caught or injured.");
		Console.WriteLine("The school is deserted as it's just started the holidays.");
		Console.WriteLine("Type 'help' if you need help.");
		Console.WriteLine();
		Console.WriteLine(player.currentRoom.GetLongDescription());
	}

	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;
		
		if(command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit; // false
		}

		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				break;
			case "quit":
				wantToQuit = true;
				break;
			case "look":
				LookAround();
				break;
			case "status":
				GiveStatus();
				break;
			case "take":
				Take(command);
				break;
			case "drop":
				Drop(command);
				break;
			case "use":
				Use(command);
				break;
		}

		return wantToQuit;
	}

	private void Use(Command command)
	{
		Console.WriteLine(player.Use(command));
	}

	private void Take(Command command)
    {
        if (!command.HasSecondWord())
        {
            Console.WriteLine("Take what item?");

            return;
        }

        player.TakeFromChest(command.SecondWord);
    }

    private void Drop(Command command)
    {
        if (!command.HasSecondWord())
        {
            Console.WriteLine("Drop what item?");

            return;
        }

        player.DropToChest(command.SecondWord);
    }







	// ######################################
	// implementations of user commands:
	// ######################################
	
	// Print out some help information.
	// Here we print the mission and a list of the command words.
	private void PrintHelp()
	{
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You wander around at the university.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}

	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	public void GoRoom(Command command)
	{
		int damage = player.Damage(5);
		if(!command.HasSecondWord())
		{
			// if there is no second word, we don't know where to go...
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.currentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to "+direction+"!");
			return;
		}

		if(nextRoom.IsLocked())
		{
			Console.WriteLine("This room is locked");
			return;
		}

		player.currentRoom = nextRoom;
		Console.WriteLine(player.currentRoom.GetLongDescription());
		
	}

	//  Gedetailleerd overzicht van de omgeving en welke items er liggen.
	private void LookAround()
	{
		Console.WriteLine(player.currentRoom.GetLongDescription());
	}

	// Status van player
	public void GiveStatus()
	{
		player.PlayerStatus();
	}
}

