# Game Basic Information #

## Summary ##

**A paragraph-length pitch for your game.**

## Gameplay Explanation ##

### Controls ###
*Overworld*

WASD - Movement
Colliding with an enemy initiates the Combat phase

*Combat*

In combat, each player unit is able to take three actions during their turn: Attack, Swap and Special. 

- Attack (sword icon) simply damages the targeted enemy within your unit's range based on a max(1, ATK-DEF) formula.
- Swap (arrow icon) allows you to switch places with an adjacent allied unit, allowing your units to change their available targets for their Attack and Special actions. Swapping also heals 10% of your unit's maximum HP.
- Special (star icon) is a unit's special ability! Its range of targets and effects are unique to each player class!

Here they are listed below: 

- Tank - Taunt (Range: all enemy units) Provoke the targeted enemy to attack you for one turn. Expends 15 steam. 
- Support - Buff (Range: adjacent allied units) Increase the attack of an adjacent ally by 40% of their base attack. Expends 15 steam. 
- Healer - Heal (Range: adjacent allied units) Heal 40% of an adjacent ally's max health points. Generates 15 steam. 
- Ranger - Snipe (Range: all enemy units) Deal 2x base damage against an enemy. Generates 15 steam. 

The steam bar is an ever-present mechanic in the game! Depending on the section of the bar you're on, your party may get buffed or debuffed. Pay attention to the musical cues and take care to stay in the Overclocked zone! All attacks generate 10 steam. Swapping expends 10 steam. 



- 2D Positional Turn-Based RPG with character classes that dictate actions / stats / attack range in the vein of Darkest Dungeon
- Overworld vs. Battle Phase
- In the overworld, the party is able to move in a grid-based system. Enemies / obstacles are littered across the grid and encountering enemies in the overworld leads to a battle. 
- In battle, party members and enemies each have four positions that they can take up.
- Depending on the class of the party member / type of enemy, their attack range determines which entities they can target on the battlefield.  
- The battle system also incorporates a "steam bar", which has various zones that either benefit or disadvantage the entire party. Each action a party member takes as well as their position during their turn affects the progress of this bar. The bar represents an integer from 0-100, and is divided into three main parts:
    - Default Zone (0-40 steam): No buffs or debuffs are applied to the party. The starting zone. 
    - Overclocked Zone (40-75 steam): Your party gets a 1.5x boost to ATK and AGI! 
    - Short-circuited Zone (75-100 steam): Your party's DEF and AGI are halved! 
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

Morgan:

[**Link to UI Design Document**- (includes written documentation, sketches, wireframes, mockups, prototypes, and images of assets)](https://docs.google.com/document/d/1kTopceyNEWOaCYavN3E1oi3YmahnAdp1_5BOAVHhUrs/edit?usp=sharing)

There are three main screens related to the user interfaces created:
- **Title screen/pause menu**: includes play button, instructions button, quit button, text backdrop, and blur overlay (pause menu only)
- **Overworld**: includes pause button, small headshot backdrop, large headshot backdrop, and stat backdrop
- **Combat**: includes health bar, turn indicator, attack button, swap button, special ability button, back arrow, headshot backdrop, enemy indicator, ally indicator, out of range overlay, unit hover indicator

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

## Movement/Physics

**Describe the basics of movement and physics in your game. Is it the standard physics model? What did you change or modify? Did you make your movement scripts that do not use the physics system?**

## Animation and Visuals

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

## Narrative Design

**Document how the narrative is present in the game via assets, gameplay systems, and gameplay.** 

## Press Kit and Trailer

**Include links to your presskit materials and trailer.**

**Describe how you showcased your work. How did you choose what to show in the trailer? Why did you choose your screenshots?**



## Game Feel

**Document what you added to and how you tweaked your game to improve its game feel.**



## Unused Content
