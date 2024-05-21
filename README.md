# Shrouded
 
## Quest System 
#### Overview
This project implements a quest system for a Unity game. The system includes various components such as quest givers, quest assets, and dialogue nodes to manage and display quests within the game. Players can interact with characters to receive, progress, and complete quests. The system ensures quests are dynamically loaded and associated with the correct quest givers.

### Components
#### QuestLoader
The QuestLoader class is responsible for loading quests from predefined quest canvases, parsing dialogue nodes, and assigning quests to the appropriate quest givers in the game. It initializes the quest givers and speakers from the scene and manages the dialogue finite state machine (FSM) for quest interactions.

#### Enemy
The Enemy class defines the basic attributes and behaviors for enemies in the game, including health points (HP), movement speed, attack power, and the ability to take damage. It includes methods for taking damage and handling the enemy's death.

#### Log (Enemy Subclass)
The Log class is a specific type of enemy that inherits from Enemy. It includes additional logic for chasing and attacking the player based on proximity, as well as managing its animation states.

#### PlayerMovement
The PlayerMovement class handles the player's movement and attack logic. It detects collisions with enemies and triggers the damage mechanism.

#### QuestAsset
The QuestAsset scriptable object holds data related to individual quests, including references to quest givers and dialogue content. These assets are used by the QuestLoader to initialize and manage quests in the game.

#### QuestNodeCanvas
The QuestNodeCanvas class represents the structure of a quest, including various dialogue and choice nodes. It is used to define the flow of interactions for each quest.

#### Setup and Usage
Initialize Quest Givers and Speakers:

Ensure all characters in the game scene are tagged appropriately (e.g., "Character").
Assign the QuestGiver component to any game object that should act as a quest giver.

#### Create and Assign QuestAssets:

Create QuestAsset scriptable objects in the Unity Editor.
Assign the questGiver field to the appropriate game object in the scene.
Configure Dialogue and Quest Nodes:

Use QuestNodeCanvas to define the dialogue flow and choices for each quest.
Ensure all connections between nodes are correctly set up.
#### Load and Assign Quests:

The QuestLoader script will automatically load and assign quests at runtime.
Ensure the QuestLoader component is attached to a game object in the scene.
#### Error Handling
##### Common Errors
Null Reference Errors: Ensure all references, such as questGiver in QuestAsset, are correctly assigned in the Unity Editor.
Index Out of Range: Check that all lists and arrays accessed within scripts have valid indices and are properly initialized.
Missing References: Verify that all game objects and components expected by scripts are present and correctly configured in the scene.
#### Debugging
Use Unity's console for detailed error messages and stack traces to identify and fix issues.
Add additional debug logs in scripts to trace the flow of execution and validate the state of variables.
#### Future Improvements
Enhance the quest system with more complex conditions and branching dialogue.
Add visual indicators for quest statuses (e.g., active, completed) on the game HUD.
Implement save/load functionality to persist quest progress across game sessions.
#### Conclusion
This quest system provides a robust framework for managing quests and interactions within a Unity game. By following the setup instructions and ensuring proper configuration, you can extend and customize the system to fit your game's requirements.
