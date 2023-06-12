# Game Basic Information #

## Summary ##

In a post-apocalyptic world, humans have long gone extinct and robots rule the Earth, establishing steampunk-esque societies centered around coal as an energy source. With the dwindling supplies of coal looming over the horizon, you take charge of a robot expeditionary team in search of new sources of energy for robotkind's continued sustenance. Initial reports from the frontier tell of a pre-war ruin supposedly filled with mysterious beings operating without coal. Perhaps there you will find what you seek... 

## Gameplay Explanation ##

### Controls ###
*Overworld*

WASD - Movement
ESC - Pause Game

*Combat*

Point-and-click user input - Selecting action and targeted unit during player unit's turn

### Gameplay Mechanics ###
*Overworld*

The player party traverses the overworld, encountering obstacles and enemy units along the way. Their goal is to reach a specified exit tile within the level in order to discover the purported energy source and win the game. Colliding with any enemy unit on the map kickstarts a transition to the Combat phase of the game. 

*Combat*

Combat is turn-based. A unit's turn order is determined by its initial AGI; the higher the AGI stat, the faster the unit moves in battle. In addition, much like in Darkest Dungeon, the battlefield is composed of two blocks of four positions each. One block corresponds to player units whereas the other block contains enemy units. The position of the player units dictate the targets that are available to them via their actions.

Each player unit is able to take three actions during their turn: Attack, Swap and Special. 

- Attack (sword icon) simply damages the targeted enemy within your unit's range based on a max(1, ATK-DEF) formula.
- Swap (arrow icon) allows you to switch places with an adjacent allied unit, allowing your units to change their available targets for their Attack and Special actions. Swapping also heals 10% of your unit's maximum HP.
- Special (star icon) is a unit's special ability! Its range of targets and effects are unique to each player class!

Here they are listed below: 

- Tank - Taunt (Range: all enemy units) Provoke the targeted enemy to attack you for one turn. Expends 15 steam. 
- Support - Buff (Range: adjacent allied units) Increase the attack of an adjacent ally by 40% of their base attack. Effect stacks. Expends 15 steam. 
- Healer - Heal (Range: adjacent allied units) Heal 40% of an adjacent ally's max health points. Generates 15 steam. 
- Ranger - Snipe (Range: all enemy units) Deal 2x base damage against an enemy. Generates 15 steam. 

Each enemy unit is only able to Attack. However, they have unlimited range! 

Heavily inspired by the action bar system of Chained Echoes, the steam bar is an ever-present mechanic in the game! Depending on the section of the bar you're on, your party may receive various positive or negative effects. Pay attention to the musical cues and take care to stay in the Overclocked zone! All basic attacks generate 10 steam. Swapping expends 10 steam. Special abilities have variable effects on the steam bar. Refer to the special abilities list above for more information.

The various states of the steam bar are listed below:

- Default Zone (0-40 steam): No buffs or debuffs are applied to the party. The starting zone.
- Overclocked Zone (40-75 steam): Your party gets a 1.5x boost to ATK and AGI!
- Shortcircuited Zone (75-100 steam): Your party's DEF and AGI are halved! 

**If you did work that should be factored in to your grade that does not fit easily into the proscribed roles, add it here! Please include links to resources and descriptions of game-related material that does not fit into roles here.**

# Main Roles #

Russell

Main Role: Combat Logic

Sub Role: Game Direction

Jordan

Main Role: Input / Movement

Sub Role: Narrative Design

Aron

Main Role: Combat Logic

Sub Role: Technical Artist

Morgan

Main Role: User Interface

Sub Role: Gameplay Testing

Clarissa

Main Role: Environmental Art

Sub Role: Level Design

Chloe

Main Role: Character Sprites/Animations

Sub Role: Sound Effects

Josh

Main Role: Music/Sound Effects 

Sub Role: Press Kit & Trailer

Your goal is to relate the work of your role and sub-role in terms of the content of the course. Please look at the role sections below for specific instructions for each role.

Below is a template for you to highlight items of your work. These provide the evidence needed for your work to be evaluated. Try to have at least 4 such descriptions. They will be assessed on the quality of the underlying system and how they are linked to course content. 

*Short Description* - Long description of your work item that includes how it is relevant to topics discussed in class. [link to evidence in your repository](https://github.com/dr-jam/ECS189L/edit/project-description/ProjectDocumentTemplate.md)

Here is an example:  
*Procedural Terrain* - The background of the game consists of procedurally-generated terrain that is produced with Perlin noise. This terrain can be modified by the game at run-time via a call to its script methods. The intent is to allow the player to modify the terrain. This system is based on the component design pattern and the procedural content generation portions of the course. [The PCG terrain generation script](https://github.com/dr-jam/CameraControlExercise/blob/513b927e87fc686fe627bf7d4ff6ff841cf34e9f/Obscura/Assets/Scripts/TerrainGenerator.cs#L6).

You should replace any **bold text** with your relevant information. Liberally use the template when necessary and appropriate.

## Combat Logic

Russell:

In this role, I focused more on the implementation of the combat systems in code, including the turn-based engine, combat UI, steam bar, class system, attacking, swapping and special abilities. The specific implementations for each of these systems are detailed as follows:

*Turn-based Engine* - The turn-based system in this game relies on the use of a hierarchy of unit state machines, an overall combat state machine and a UI state machine to run smoothly (loosely based on [xOctomanx's Unity Turn-based Tutorial Series](https://www.youtube.com/playlist?list=PLj0TSSTwoqAypUgag6HJoVZD-RmbpDtjF)). With the combination of these state machines, units are able to properly perform their specified actions during their turn, whether reliant on user input or enemy AI. The primary components involved in this process are as follows:

- **Combat State Machine** - Found in [CombatStateMachine.cs](https://github.com/LeUmbono/189L-Game-Project/blob/main/189L-Game/Assets/Scripts/Combat/StateMachines/CombatStateMachine.cs). Stores, initializes and updates combat-related information including allies and enemies in battle, the turn order queue, as well as the positions of each unit in battle. The state machine dictates the macroscopic flow of battle, ensuring the proper units are able to take their turns in order, perform their specified actions and that the combat instance is properly terminated depending on the amount of allies/enemies left standing in battle. 

- **Unit State Machines** - Found in [PlayerStateMachine.cs](https://github.com/LeUmbono/189L-Game-Project/blob/main/189L-Game/Assets/Scripts/Combat/StateMachines/PlayerStateMachine.cs) and [EnemyStateMachine.cs](https://github.com/LeUmbono/189L-Game-Project/blob/main/189L-Game/Assets/Scripts/Combat/StateMachines/EnemyStateMachine.cs). Inheriting from the abstract class [GenericUnitStateMachine](https://github.com/LeUmbono/189L-Game-Project/blob/main/189L-Game/Assets/Scripts/Combat/StateMachines/GenericUnitStateMachine.cs), these state machines deal more specifically with the actions each unit is able to during their turn; attacking, swapping and executing special abilities for player units and simply attacking for enemy units (these actions were done as their respective coroutines). They also deal with damage calculations and unit death, ensuring that the deceased unit is properly removed from the turn order queue and that the game proceeds as normal. Dead units are also 'swapped' to the end of their respective formations so as to ensure soft-locking does not occur due to out-of-range issues. Player-specific and enemy-specific variables can also be found in their respective state machines, such as buff amount or whether an enemy has been taunted or not. These state machines also play the appropriate animations/sound effects when units perform an action, take damage or die.

- **UI State Machine** - Found in [UIStateMachine.cs](https://github.com/LeUmbono/189L-Game-Project/blob/main/189L-Game/Assets/Scripts/Combat/StateMachines/UIStateMachine.cs). This state machine contains information about all the UI elements used in combat including health bars, the unit info panel, and the action and target selection panels. It ensures that the proper displays are shown during the correct combat phase, such as when selecting the type of action to take for the player. The UI state machine also takes in input from the buttons in the action and target selection panels and relays that information to the Player State Machine when executing actions. Furthermore, it renders stats in the unit info panel as being red/green depending on whether they have been buffed/debuffed from the base stats of the player displayed. 

- **Enemy AI** -  When performing their attacks, enemy units will more often target player units in the front of the allied formation rather than those in the back, based on the targeting engine found in [EnemyTargetingEngine.cs](https://github.com/LeUmbono/189L-Game-Project/blob/main/189L-Game/Assets/Scripts/Combat/EnemyTargetingEngine.cs). Originally, enemies attacked player units at random but results from our playtesting session revealed that players often felt it was unfair when some of their squishier units (i.e. the Ranger and the Healer) were decimated almost immediately during the enemy units' turns. Thus, this "fairer" AI system was formed in response to that and to further encourage strategic swapping of units. 

*Steam Bar* - The steam bar's functionality can be found in [SteamBar.cs](https://github.com/LeUmbono/189L-Game-Project/blob/main/189L-Game/Assets/Scripts/Combat/UI/SteamBar.cs). The class specifies the state thresholds, the steam colors and music themes/transitions associated with each state of the steam bar, as well as the current steam bar state. Whenever the steam bar's value changes, a check is performed to see which state the steam bar is now in. If the steam bar's state has changed, the effects of being in the new state are applied to the player party and the combat music and appearance of the steam bar are changed appropriately. 

*Class System* - Since each class in the game was designed to store the initial starting stats of each unit, I decided that storing that information in a scriptable object (in [ClassData.cs](https://github.com/LeUmbono/189L-Game-Project/blob/main/189L-Game/Assets/Scripts/Combat/Classes/ClassData.cs)) would be the best way to do so as it is lightweight and the information for each class is meant to be read-only. I also made it so that class assets for the SO can be made directly from the editor, simply through editing the values in the inspector, thus ensuring that classes for all units can be easily created. Each instance of ClassData stores a class name, sprite information, base stat values for HP, ATK, DEF, AGI, and Range, as well as a field for the special ability unique to the class (which is also stored as a SO).

*Basic Attack* - Implemented in [PlayerStateMachine.cs](https://github.com/LeUmbono/189L-Game-Project/blob/main/189L-Game/Assets/Scripts/Combat/StateMachines/PlayerStateMachine.cs) and [EnemyStateMachine.cs](EnemyStateMachine.cs). It involved using the `TakeDamage()` and `DoDamage()` methods to perform a basic max(1, ATK-DEF) calculation to that subtracts the resultant damage from a targeted unit's current HP. This ensured that any attack always dealt at least 1 point of damage regardless of one's ATK and target's DEF values. 

*Swapping*  - Implemented in [PlayerStateMachine.cs](https://github.com/LeUmbono/189L-Game-Project/blob/main/189L-Game/Assets/Scripts/Combat/StateMachines/PlayerStateMachine.cs) and [GenericUnitStateMachine](https://github.com/LeUmbono/189L-Game-Project/blob/main/189L-Game/Assets/Scripts/Combat/StateMachines/GenericUnitStateMachine.cs). Swapping involved switching the positions of the player unit performing the swap and the target ally via the `DoSwap()` method in GenericUnitStateMachine. It also swaps the relevant UI references to each unit's game objects so as to ensure functionality of the combat UI is intact. `DoSwap()` is also called when units die, regardless of whether they were a player or enemy unit, to prevent soft-locking. Hence, it's location in GenericUnitStateMachine. This action was found initially unpopular in the playtest due to a lack of utility and the random enemy targeting system; thus, I added more incentive to use it by introducing a 10% healing effect when swapping units and adjusting the enemy AI to favor attacking player units in the front. 

*Special Abilities* - In implementing special abilities into the combat, I made use of a modified version of the Command Pattern we learned in Exercise 1 that relied on an abstract class in [SpecialAbility.cs](https://github.com/LeUmbono/189L-Game-Project/blob/main/189L-Game/Assets/Scripts/Combat/Classes/SpecialAbility.cs) rather than an interface. Essentially, `SpecialAbility` is an abstract class that inherited from ScriptableObject and acted as a parent class for all subsequent special abilities; for its abstract methods, it contained an `Execute()` method to perform the functionality of the ability, a `SelectTargets()` method to return a list of possible targets from the 8 positions available in battle, and a `GetSteamBarChangeValue()` method that returned a value for which the ability would affect the progress of the steam bar. Using this `SpecialAbility` class, I was able to implement the special abilities for each player class (i.e. Snipe, Buff, Heal and Taunt).

## User Interface

**Describe your user interface and how it relates to gameplay. This can be done via the template.**

Morgan:

[**Link to UI Design Document**- (includes written documentation, sketches, wireframes, mockups, prototypes, and images of assets)](https://docs.google.com/document/d/1kTopceyNEWOaCYavN3E1oi3YmahnAdp1_5BOAVHhUrs/edit?usp=sharing)

There are three main screens related to the user interfaces created:
- **Title screen/pause menu** (involves point-and-click user input): includes play button, instructions button, quit button, text backdrop, and blur overlay (pause menu only)
- **Overworld** (involves keyboard user input): includes pause button, small headshot backdrop, large headshot backdrop, and stat backdrop
- **Combat** (involves point-and-click user input): includes health bar, turn indicator, attack button, swap button, special ability button, back arrow, headshot backdrop, enemy indicator, ally indicator, out of range overlay, unit hover indicator

This list contains key combat UI components as this is the most relevant part of our game. Please see the design document for all assets and images of these assets.
- **Health bar**: Indicates health of player's team and enemies. The background is static whereas the inner bar is dynamic. The heart is used to further reiterate relation to health.

![health static](https://github.com/LeUmbono/189L-Game-Project/blob/bcca53680e28f1a97548234a03d02efa329f5f69/189L-Game/Assets/Art/Combat/UI/FinalUI/health%20static.png)

![health dynamic](https://github.com/LeUmbono/189L-Game-Project/blob/bcca53680e28f1a97548234a03d02efa329f5f69/189L-Game/Assets/Art/Combat/UI/FinalUI/health%20dynamic.png)


- **Turn indicator**: A unit's turn is indicated by the yellow arrow above their head. The arrow is meant to "hover" to draw the user's attention and add dynamic motion to the screen.

![turn](https://github.com/LeUmbono/189L-Game-Project/blob/bcca53680e28f1a97548234a03d02efa329f5f69/189L-Game/Assets/Art/Combat/UI/FinalUI/indicator%20final.png)


- **Attack button**: Allows the player to choose which enemy they'd like to attack once pressed. Clicking on it switches to the unit selection screen.

![attack](https://github.com/LeUmbono/189L-Game-Project/blob/bcca53680e28f1a97548234a03d02efa329f5f69/189L-Game/Assets/Art/Combat/UI/FinalUI/attack%20button%20final.png)


- **Swap button**: Allows the player to swap positions with another ally once pressed. Clicking on it switches to the unit selection screen.

![swap](https://github.com/LeUmbono/189L-Game-Project/blob/bcca53680e28f1a97548234a03d02efa329f5f69/189L-Game/Assets/Art/Combat/UI/FinalUI/swap%20button%20final.png)


- **Special ability button**: Allows the player to use their special ability once pressed, such as taunt, buff, heal, or snipe. Clicking on it switches to the unit selection screen.

![star](https://github.com/LeUmbono/189L-Game-Project/blob/bcca53680e28f1a97548234a03d02efa329f5f69/189L-Game/Assets/Art/Combat/UI/FinalUI/star%20button%20final.png)


- **Back arrow**: Takes the user from the unit selection screen back to the action selection screen when pressed.

![back](https://github.com/LeUmbono/189L-Game-Project/blob/bcca53680e28f1a97548234a03d02efa329f5f69/189L-Game/Assets/Art/Combat/UI/FinalUI/back%20arrow.png)


- **Enemy outline**: Red outline around a unit's headshot that indicates enemy status to the player.

![enemy](https://github.com/LeUmbono/189L-Game-Project/blob/bcca53680e28f1a97548234a03d02efa329f5f69/189L-Game/Assets/Art/Combat/UI/FinalUI/highlight_enemy.png)


- **Ally outline**: Green outline around a unit's headshot that indicates ally status to the player.

![ally](https://github.com/LeUmbono/189L-Game-Project/blob/bcca53680e28f1a97548234a03d02efa329f5f69/189L-Game/Assets/Art/Combat/UI/FinalUI/highlight_ally.png)


- **Hover outline/overlay**: Cyan outline and overlay that indicates hover over a given unit; clicking while on hover will select this unit.

![hover](https://github.com/LeUmbono/189L-Game-Project/blob/bcca53680e28f1a97548234a03d02efa329f5f69/189L-Game/Assets/Art/Combat/UI/FinalUI/highlight_selected.png)


**Unit selection system**: **The most notable system that I created was the unit-select system in the combat screen**. I chose to designate allies with a green outline and enemies with a red outline to match common game UI color symbolization. Additionally, I chose to add a dark gray overlay (50% opacity) on top of “out-of-range” units in order to communicate that the player could not select these. Finally, I developed a bright blue outline and overlay (which I discovered was common in games with heavier UI) to indicate to the player which unit they were about to select; I felt high-noticeability was important because we decided not to implement a “submit” button, meaning if players click on the wrong unit by accident, they could not re-select. **My intention was that this dynamic hover/overlay would help prevent players from becoming frustrated with the game as a result of making that mistake**. You can find a prototype of this in the UI design document.

## Environmental Art

Clarissa:

**Combat Background**: blablabla.

## Sprites / Animations

**List your assets including their sources and licenses.**
pebbles.png -  Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License (https://creativecommons.org/licenses/by-nc-sa/3.0/deed.en_US)

**Describe how your work intersects with game feel, graphic design, and world-building. Include your visual style guide if one exists.**

*Shaders*

Aron: I realized that the steam bar would be a central mechanic, the "main appeal" of our game, so I paid great attention to its visuals. To emphasize it controlled the very "life-force" of our party, animating it was a must. Based on my scrapped steam shader, pixels on the left of the material had bigger clouds than the pixels on the right of the material. I wanted to give the impression that the steam bubbled up and disppated. This visual falls apart near the right end since I wanted a distinct cutoff point to indicate the value of the bar; no visual flare should get in the way of visual clarity.
https://www.shadertoy.com/view/DlcXWr

![Pebbles](https://github.com/LeUmbono/189L-Game-Project/blob/00ae6f72d6bac068c276995ddababd89b3e30f5b/189L-Game/Assets/Art/Materials/SteamBar/pebbles.jpg)

Aron: Pebbles were chosen as an interesting noise texture since they were bulbous and had strong outlines for where each cloud would begin and end. A binary simplex noise (through rounding) was tried at first, but its noisy edges made for an unclean cartoony effect. The pebble texture was rounded to give nice banding. See the shadertoy shader for more details.

![Transition](https://github.com/LeUmbono/189L-Game-Project/blob/00ae6f72d6bac068c276995ddababd89b3e30f5b/189L-Game/Assets/Art/Overworld/Transition/ShaderTexture%20-%20Copy.png)

Aron: The transition shader took a lot of iteration. I speculated having a factory-like piston transition, where boxes on a conveyor belt would scroll across and gradually cover the screen, and a steam nozzle shader where steam would "spray" from nozzles in the corner of the screen.. All these transitions felt clunky and not thematic. I ultimately went with a gear, a central character design element for our party members to provide simple visual language for initiating the combat phase.

*Sprite Art*

Great care was put into gears as a thematic element for the party members. Its design is very flexible in nature; our designs used the gear as a shield on the tank, an accessory on the ranger, and a radial crank on the support. Our visual design heavily leaned on strong shape language to communicate party purpose. 

The tank is sturdy, slow, and reliable. These traits are best communicated with a heavy square shape.

![Tank](https://github.com/LeUmbono/189L-Game-Project/blob/37db298b54b21f46af9d6708d36a3033c2614c3f/189L-Game/Assets/Art/Combat/Sprites/tankFINAL.png)

The support is friendly and takes a backseat to the rest of the bots. It is round to communicate a friendlier aura than the sharper and sturdier ranger and tank bot.

![Support](https://github.com/LeUmbono/189L-Game-Project/blob/37db298b54b21f46af9d6708d36a3033c2614c3f/189L-Game/Assets/Art/Combat/Sprites/supportFINAL.png)

![Ranger](https://github.com/LeUmbono/189L-Game-Project/blob/37db298b54b21f46af9d6708d36a3033c2614c3f/189L-Game/Assets/Art/Combat/Sprites/sniperFINAL.png)

![Healer](https://github.com/LeUmbono/189L-Game-Project/blob/dd6acc891590242862999f573f340b54f847e743/189L-Game/Assets/Art/Combat/Sprites/healerFINAL.png)

## Input

**Describe the default input configuration.**

**Add an entry for each platform or input style your project supports.**

## Music and Sound Effects

Josh: 

I was in charge of creating music and sound effects that enhanced experience of playing the game!

*Music* - To create the music, I use a DAW called Reaper and my MIDI controller/synth, the Arturia MiniLab 3.  There are two main themes I had to create; one for the overworld, and one for the combat theme. Given that the overworld was going to be an abandoned factory, I wanted to create music for it that would impart a lonely, melancholy tone. After settling on one of the many different sounds in the Analog Lab V soundbank, I added reverb and increased the bass to resemble the vast spaciousness of an abandoned factory. When it came to actually creating musical ideas, I didn't really take any inspiration from any sources. I started improvising and took notes of what I liked. I should note that while coming up with ideas for the overworld theme, I tried to sing what I played as I wanted the overworld theme to be more melodic. The process for creating the combat theme was a bit different though. Unlike the overworld theme, the combat theme was entirely improvised. I chose some musical scales and chords that I wanted to center the combat theme around and went ham! The sound patch I chose for the combat theme was interesting in the fact that when I pressed one note on the keyboard it'd play 2 different notes (at different times)! Without getting too much into the music theory, it created a level of uncontrolled chaos that I didn't try to control. It made me play more by ear rather than worrying about the music theory which I believed resulted in a more "raw" sound. I previous said that I didn't really draw inspiration from other music when trying to create the 2 main themes. That being said, I did draw inspiration when creating variations of the combat theme from the Mario franchise. In many Mario games, when you're almost out of time when completing a level, it will play a little blurb signaling that time's almost up and then it will play a more intense version of the original level theme. I tried implementing that in our game through the steam system since time wasn't an important factor. Whether or not the steam bar was in its default, overclocked, or shortcircuited zone would impact what theme was playing and the intensity of the them.

Here are some images of the software I used when producing the music:

![Analog Lab V](https://github.com/LeUmbono/189L-Game-Project/assets/102931991/7170f8f6-936e-437b-83e2-77a2aae29ba7)
![Reaper](https://github.com/LeUmbono/189L-Game-Project/assets/102931991/9ea5cf46-b328-4215-9644-a9359e9deb0e)
![Reaper Midi](https://github.com/LeUmbono/189L-Game-Project/assets/102931991/a7ad7230-703b-49c3-8ac3-f0bafff91f54)

All the sound effects were created by using whatever objects I had on me. I practically chose objects at random and took note of what interactions I liked and tried improving on prior experimentation. For example, one of the earlier sound effects was created by hitting a glass with different parts of the spoon and I later used a larger spoon in order to get a lower pitched sound. Each sound effect was rarely a singular audio file; most of the times it'd be 2 or 3 audio files layered. Lots of processing was done (distortion, reversals, reverb, and etc) on all of the sounds and all the processinng was done in Audacity.

Here are image of the software I used when producing the music:

![Buff](https://github.com/LeUmbono/189L-Game-Project/assets/102931991/4180308a-264c-44cc-8f0d-4f385f836004)

All the files can be found in this folder:
[![Music and Sound Effects]](https://github.com/LeUmbono/189L-Game-Project/tree/5e0ef6d322f8881ac412ca2eeebc8002dd77134a/189L-Game/Assets/Audio)

# Sub-Roles

## Game Direction

Russell:

In my role as Game Director, I was in charge of organizing regular weekly meetings to discuss progress on the game, establishing collaboration guidelines / task-tracking, inter-team communication and troubleshooting as well as ensuring that our collective vision for the game remained as close as possible from what we initially set out to accomplish. For all team-related communication, I created a [Discord server](https://discord.gg/MyrjvYtYJh) that had channels dedicated to each aspect of the game including programming, art/design and sound/music. These aspects will be discussed in more detail below:

*Organizing weekly progress checks* - Every Sunday at 12:30 pm, I would organize a team-wide meeting in our Discord server to discuss each member's expectations and progress in terms of their roles for the game. This ensured that I as the director knew where everyone was at during the project and that each member was clear on their reponsibilities for the week. Any concerns about the game were also brought to attention during this meeting, allowing us to come up with solutions as a team. If any members missed the meeting, I would update them privately via Discord. 

*Establishing collaboration guidelines / task-tracking* - I was also responsible for creating the [GitHub repository](https://github.com/LeUmbono/189L-Game-Project) for our project, ensuring that all team members were invited to collaborate and that it had the proper [Unity .gitignore file](https://github.com/LeUmbono/189L-Game-Project/blob/main/189L-Game/.gitignore) to prevent commit issues due to large file sizes. In managing GitHub, I instituted a policy whereby the main branch was not to be touched by any person under any circumstances unless I gave my explicit permission; instead, any new feature would have to be implemented on a separate branch that was to be named after the feature and the contributor to the feature (such as russell-umboh/combat-ui). This ensured that the number of merge conflicts would be kept to a minimum when it came to incorporating new features into the main branch. As the primary manager of the repo, I also merged all incoming pull requests into main, ensuring any conflicts are resolved in a satisfactory and responsible manner.

For task-tracking, I initially created a [workspace](https://trello.com/w/ecs189lfinalproject) in Trello, an online to-do list-making website, to keep track of our tasks for every week. This workspace was separated into three main taskboards: Programming, Music and General (for all other tasks) due to the relative complexity of each of those roles. In each board, there were To-Do, In Progress, and Completed sections between which members would move task cards once they have completed a certain phase of the task. However, I recognized that this was not very popular/confusing among the members because they had to switch between Trello and Discord and so I eventually just moved task-tracking completely into the Discord server, pinning the tasks in the #announcements channel and striking them out as they were completed.

*Inter-team communication and troubleshooting* - Whenever any team member encountered difficulties with aspects of their role, or had personal circumstances that prevented or limited their work, I would come up with solutions / accommodations to ensure that they could continue working on the project in a comfortable manner. For instance, we had initially wanted to design our Overworld by hand. However, that took too much time and so I suggested using some procedural content generation (PCG) to easily generate the initial layout of the dungeon automatically. Coincidentally, I was also doing an independent study on the topic and so I was able to easily find resources (see [here](https://www.youtube.com/playlist?list=PLcRSafycjWFenI87z7uZHFv6cUG2Tzu9v)) to implement this system into our game, using binary space partitioning to generate our factory level consisting of rooms and corridors via a room-first approach (found in [RoomFirstDungeonGenerator.cs](https://github.com/LeUmbono/189L-Game-Project/blob/main/189L-Game/Assets/Scripts/Overworld/PCG/RoomFirstDungeonGenerator.cs)). Then, the level designer could manually place enemies and obstacles throughout the entire layout. 

*Ensuring collective vision of game is maintained* - To ensure that our game remained as faithful as possible to the initial plan, I diligently monitored our progress throughout the quarter, ensuring that all incoming changes to the repository followed quality checks with regards to code organization (based on the [Microsoft C# style guide](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)), namespaces and naming conventions. I also organized all the files in the Unity project itself, grouping them into sections such as Art, Music/Sound, Prefabs, ScriptableObjects, Scripts, etc. as intuitively as possible. Furthermore, I made the executive decisions for the team in choosing which aspects of the game to prioritize in the interests of time, primarily focusing on the combat system as that was what initially made the entire game appealing to us. 

## Narrative Design

**Document how the narrative is present in the game via assets, gameplay systems, and gameplay.** 

## Technical Artist

## Gameplay Testing

**Add a link to the full results of your gameplay tests.**

[Playtesting Full Results](https://docs.google.com/spreadsheets/d/1n6ujxOvNEJYRFGBAkuFKevN88K4hgSQzI5bAZqSuePY/edit?usp=sharing)

**Summarize the key findings from your gameplay tests.**

[**Playtesting Summary, Report, & Analysis**](https://docs.google.com/document/d/1CTMEWQRnyL68Nw0RKID6Rn5qOjunhTB-_wRuw-S3gkI/edit?usp=sharing)

[Playtesting Instructions & Guidelines](https://docs.google.com/document/d/19JiA8req6dwKU2h-y9f5yDnb3djJPQiXp_C-QNy4oY0/edit?usp=sharing)

Morgan: To begin the play-testing process, I began by researching how to write helpful play testing follow-up questions as I’ve never actually held formal playtests before; I’ve only ever done user testing for apps. After adapting questions to fit the style of our game, I created a Google form that would be used post each playtesting session. **I made sure to address a variety of game-related concepts to make sure any feedback was as comprehensive as possible**:

1. What was your favorite part about the game?
2. What didn’t you like about the game?
3. What was confusing?
4. How emotionally invested in the game were you?
5. What was your strategy for combat?
6. Did anything hold you back from seeing your strategy or plans through?
7. Can you explain why the victorious parties won? Did you think any wins were well-deserved?
8. Did you think anything was “missing” in terms of game feel?
9. How long did you feel like you were playing the game? How long did you actually play for?
10. If you were to suggest that one change be made to the game, what would it be?

Based off of all of the feedback we received, **the three areas that we should focus on before the final due date are**:
1. **Clearer instructions**- potential solutions include detailed tutorial, continuous access to rules, and more supporting UI 
2. **More engaging gameplay**- potential solutions include faster combat sequences, reduced time of pre-existing animations/sound effects, and more animations to create visual interest/engagement
3. **Improved enemy AI**- potential solutions include less randomized turn order and attack sequence and incorporating limitations the player faces

Lastly, to further add to the analysis part of the play-testing process, **I included potential solutions to any key problems play-testers brought up**; some of these were “quick fixes” that prioritized the game’s upcoming deadline, whereas others were more comprehensive solutions that I considered to be “stretch goals”. **Many of these suggestions were implemented during the final week of development, including faster combat animations, shorter SFX, more text-UI, enhanced steambar UI, and improved enemy AI**. You can find all of these suggestions in the play testing report linked at the top of this section.

Aron: There were a lot of moments in playtesting where players would use the ranger's snipe ability during the overclocked phase of the steam bar on an oil slime. At the current attack of the ranger, the slime would live at 1 HP! This was mildly annoying for the players, and it slipped by as I was balancing the stat tables. It brought about the importance of *damage thresholds*, where dealing 39 damage to a 40 health unit was a lot different from dealing 40 damage to a 40 health unit.

There was a little bit of polishing needed to round off the rough edges such as speeding up animations and sounds to aid a sluggish game, adding visual indicators, and fixing minor visual bugs. I hadn't realized how important this process was until I felt how these changes worked in combination.. It made for a strictly more fun game with the new quality of life improvements!

## Level Design

## 

## Press Kit and Trailer

**Include links to your presskit materials and trailer.**

**Describe how you showcased your work. How did you choose what to show in the trailer? Why did you choose your screenshots?**

Josh: I was in charge of making the trailer! Given that there's not a lot of variation in the aesthetics of the gameplay and there's not much content to highlight for a full minute, I decided to create a trailer that moreso resembles a movie trailer (background image with text, then show footage, repeat!). That being said, I knew that I needed to highlight the overworld and especially the combat. I wanted footage of the combat had to include the moves of each character and to also showcase the different combat options (basic attacks, switching, and specials moves). To be brutally honest, my editing skills are NOT very good so I decided to double down on that and to make it obvious to the viewers that the trailer wasn't taking itself seriously at all (through the use of poorly edited memes). I focused on giving an entertaining product rather than a polished product. The editing software that I chose to use was Windows Movie Maker as it's lightweight and easy to use and I chose OBS to record the footage for the same reason. Another aspect of the game that I wanted to capture in the game was the difference between the desolate loneliness of the overworld and the chaoticness of the combat. I captured this by making the first part of the trailer much calmer and then suddenly making trailer have much more energy after a grand pause. The 
Here are pictures of the software I used to create the trailer:

![Windows Movie Maker](https://github.com/LeUmbono/189L-Game-Project/assets/102931991/86395def-da61-4c55-9703-80cfd6f3ae28)
![Windows Movie Maker 1](https://github.com/LeUmbono/189L-Game-Project/assets/102931991/6112b704-4d9f-44cc-bee5-13f2c9f345ac)


## Unused Content
