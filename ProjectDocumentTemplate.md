# Game Basic Information #

## Summary ##

**A paragraph-length pitch for your game.**

## Gameplay Explanation ##

### Controls ###
*Overworld*

WASD - Movement
Colliding with an enemy initiates the Combat phase

*Combat*

Select buttons with the mouse.

- 2D Positional Turn-Based RPG with character classes that dictate actions / stats / attack range in the vein of Darkest Dungeon
- Overworld vs. Battle Phase
- In the overworld, the party is able to move in a grid-based system. Enemies / obstacles are littered across the grid and encountering enemies in the overworld leads to a battle. 
- In battle, party members and enemies each have four positions that they can take up.
- Depending on the class of the party member / type of enemy, their attack range determines which entities they can target on the battlefield.  
- The battle system also incorporates a "steam bar", which has various zones that either benefit or disadvantage the entire party. Each action a party member takes as well as their position during their turn affects the progress of this bar. The bar represents an integer from 0-100, and is divided into three main parts:
  - Inert (0 - 40): 1x Attack
  - Overclocked (40 - 60): 1.5x Attack, 1.0 Defense
  - Short-circuited (60 - 100): 0.5x Attack, 0.5 Defense
- Inspiration: Action Bar in Chained Echoes

**If you did work that should be factored in to your grade that does not fit easily into the proscribed roles, add it here! Please include links to resources and descriptions of game-related material that does not fit into roles here.**

# Main Roles #

Russell
Main Role: Combat Logic
Sub Role: Game Director

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
Sub Role: Project Management

Chloe
Main Role: Character Sprites/Animations
Sub Role: Sound Effects

Josh
Main Role: Music 
Sub Role: Sound Effects

Your goal is to relate the work of your role and sub-role in terms of the content of the course. Please look at the role sections below for specific instructions for each role.

Below is a template for you to highlight items of your work. These provide the evidence needed for your work to be evaluated. Try to have at least 4 such descriptions. They will be assessed on the quality of the underlying system and how they are linked to course content. 

*Short Description* - Long description of your work item that includes how it is relevant to topics discussed in class. [link to evidence in your repository](https://github.com/dr-jam/ECS189L/edit/project-description/ProjectDocumentTemplate.md)

Here is an example:  
*Procedural Terrain* - The background of the game consists of procedurally-generated terrain that is produced with Perlin noise. This terrain can be modified by the game at run-time via a call to its script methods. The intent is to allow the player to modify the terrain. This system is based on the component design pattern and the procedural content generation portions of the course. [The PCG terrain generation script](https://github.com/dr-jam/CameraControlExercise/blob/513b927e87fc686fe627bf7d4ff6ff841cf34e9f/Obscura/Assets/Scripts/TerrainGenerator.cs#L6).

You should replay any **bold text** with your relevant information. Liberally use the template when necessary and appropriate.

## Producer

**Describe the steps you took in your role as producer. Typical items include group scheduling mechanism, links to meeting notes, descriptions of team logistics problems with their resolution, project organization tools (e.g., timelines, depedency/task tracking, Gantt charts, etc.), and repository management methodology.**

## User Interface

**Describe your user interface and how it relates to gameplay. This can be done via the template.**

## Movement/Physics

**Describe the basics of movement and physics in your game. Is it the standard physics model? What did you change or modify? Did you make your movement scripts that do not use the physics system?**

## Animation and Visuals

**List your assets including their sources and licenses.**
pebbles.png -  Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License (https://creativecommons.org/licenses/by-nc-sa/3.0/deed.en_US)

**Describe how your work intersects with game feel, graphic design, and world-building. Include your visual style guide if one exists.**

*Shaders*

I realized that the steam bar would be a central mechanic, the "main appeal" of our game, so I paid great attention to its visuals. To emphasize it controlled the very "life-force" of our party, animating it was a must. Based on my scrapped steam shader, pixels on the left of the material had bigger clouds than the pixels on the right of the material. I wanted to give the impression that the steam bubbled up and disppated. This visual falls apart near the right end since I wanted a distinct cutoff point to indicate the value of the bar; no visual flare should get in the way of visual clarity.
https://www.shadertoy.com/view/DlcXWr

![Pebbles](https://github.com/LeUmbono/189L-Game-Project/blob/00ae6f72d6bac068c276995ddababd89b3e30f5b/189L-Game/Assets/Art/Materials/SteamBar/pebbles.jpg)

Pebbles were chosen as an interesting noise texture since they were bulbous and had strong outlines for where each cloud would begin and end. A binary simplex noise (through rounding) was tried at first, but its noisy edges made for an unclean cartoony effect. The pebble texture was rounded to give nice banding. See the shadertoy shader for more details.

![Transition](https://github.com/LeUmbono/189L-Game-Project/blob/00ae6f72d6bac068c276995ddababd89b3e30f5b/189L-Game/Assets/Art/Overworld/Transition/ShaderTexture%20-%20Copy.png)

The transition shader took a lot of iteration. I speculated having a factory-like piston transition, where boxes on a conveyor belt would scroll across and gradually cover the screen, and a steam nozzle shader where steam would "spray" from nozzles in the corner of the screen.. All these transitions felt clunky and not thematic. I ultimately went with a gear, a central character design element for our party members to provide simple visual language for initiating the combat phase.

*Sprite Art*

Great care was put into gears as a thematic element for the party members. Its design is very flexible in nature; our designs used the gear as a shield on the tank, an accessory on the ranger, and a radial crank on the support. Our visual design heavily leaned on strong shape language to communicate party purpose. 

The tank is sturdy, slow, and reliable. These traits are best communicated with a heavy square shape.

![Tank](https://github.com/LeUmbono/189L-Game-Project/blob/37db298b54b21f46af9d6708d36a3033c2614c3f/189L-Game/Assets/Art/Combat/Sprites/tankFINAL.png)

The support is friendly and takes a backseat to the rest of the bots. It is round to communicate a friendlier aura than the sharper and sturdier ranger and tank bot.

![Support](https://github.com/LeUmbono/189L-Game-Project/blob/37db298b54b21f46af9d6708d36a3033c2614c3f/189L-Game/Assets/Art/Combat/Sprites/supportFINAL.png)


![Sniper](https://github.com/LeUmbono/189L-Game-Project/blob/37db298b54b21f46af9d6708d36a3033c2614c3f/189L-Game/Assets/Art/Combat/Sprites/sniperFINAL.png)

![Healer](https://github.com/LeUmbono/189L-Game-Project/blob/dd6acc891590242862999f573f340b54f847e743/189L-Game/Assets/Art/Combat/Sprites/healerFINAL.png)

## Input

**Describe the default input configuration.**

**Add an entry for each platform or input style your project supports.**

## Game Logic

**Document what game states and game data you managed and what design patterns you used to complete your task.**

# Sub-Roles

## Cross-Platform

**Describe the platforms you targeted for your game release. For each, describe the process and unique actions taken for each platform. What obstacles did you overcome? What was easier than expected?**

## Audio

**List your assets including their sources and licenses.**

**Describe the implementation of your audio system.**

**Document the sound style.** 

## Gameplay Testing

**Add a link to the full results of your gameplay tests.**

**Summarize the key findings from your gameplay tests.**

## Narrative Design

**Document how the narrative is present in the game via assets, gameplay systems, and gameplay.** 

## Press Kit and Trailer

**Include links to your presskit materials and trailer.**

**Describe how you showcased your work. How did you choose what to show in the trailer? Why did you choose your screenshots?**



## Game Feel

**Document what you added to and how you tweaked your game to improve its game feel.**



## Unused Content
