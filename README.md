# ⏱️ Second Hand

**Second Hand** is a fast-paced 2D game created in **Unity / C#** for a 4-day game jam based around the theme **"Count Down"**.

In Second Hand, **time is your life**.

Every character in the arena has their own countdown timer. The player must manage their remaining seconds, avoid NPCs that can steal their time, strategically swap timers with NPCs, collect temporary power-ups, and survive long enough for the exit to open.

> Built with Unity, C#, Aseprite and custom gameplay systems.

---

## 🎮 Gameplay

Every character starts with a limited amount of time.

When the timer reaches zero, that character is eliminated.

NPCs can steal **1 second** from the player when they get close enough, adding that second to their own timer.

The player can recover from dangerous situations using the game's main mechanic: **swapping timers with NPCs**.

When the player gets close enough to an NPC, a box appears around them to indicate that they are within swapping range. Clicking that NPC exchanges the two countdown timers.

For example:

```text
Player: 6 seconds
NPC:    13 seconds

        ↓ SWAP ↓

Player: 13 seconds
NPC:     6 seconds
```

NPCs also interact with each other, compete for time and search for pickups around the arena.

Once the evacuation countdown finishes, the exit becomes available and the player must reach it before their own timer runs out.

### Core Gameplay Loop

- ⏱️ Survive while your timer continuously counts down
- 👥 Avoid NPCs that can steal 1 second of your time
- 🔄 Swap timers with NPCs when it benefits you
- 🎁 Search for temporary power-ups
- 🤖 React to NPCs that chase, flee and interact with each other
- 🚪 Survive until the exit opens
- 🏁 Reach the exit before your timer reaches zero

---

## 🕹️ Main Features

### ⏱️ Time as Health

Instead of using a traditional health system, every character has a countdown timer.

Time functions as both the player's **health and main resource**.

If the player's timer reaches zero, the game is over.

NPCs can steal 1 second from the player when they get close enough, increasing their own timer while reducing the player's.

The player can also completely change their situation by swapping their remaining time with an NPC.

This creates a constant risk/reward decision between staying away from dangerous characters and getting close enough to an NPC with more time to perform a swap.

---

### 🤖 NPC Behaviour

NPCs use different behaviours depending on their situation.

They can:

- Wander around the arena
- Chase characters with less time
- Flee from characters that are more dangerous
- Target the player
- Target other NPCs
- Steal time when close enough
- Search for pickups
- Avoid walls and obstacles

NPCs are therefore competing with both the player and each other instead of existing purely as enemies.

Their behaviour can change throughout the match depending on their timer and what is happening around them.

---

### 🔄 Timer Swap Ability

The main player ability allows you to **swap your countdown timer with an NPC's timer**.

When the player gets close enough to an NPC, a visible box appears around that character to show that they are within swapping range.

The player can then click the highlighted NPC to exchange timers.

For example:

```text
BEFORE

Player: 6 seconds
NPC:    13 seconds


        ↓ SWAP ↓


AFTER

Player: 13 seconds
NPC:     6 seconds
```

**Only their remaining countdown timers are exchanged.**

This can be used to:

- Recover when the player's timer is dangerously low
- Take advantage of NPCs with large amounts of remaining time
- Give a low timer to another NPC
- Make strategic decisions about which NPCs are worth approaching
- Turn a losing situation into a potential escape

Getting close enough to perform a swap is also dangerous because NPCs can steal time from the player.

The ability uses a cooldown to prevent constant swapping.

---

## 🎁 Pickups

There are **three different pickups** that can appear during a match.

NPCs can also search for pickups, meaning the player may have to compete with other characters to reach them first.

Each pickup has its own description displayed in-game and its effect lasts for **4 seconds**.

### ⏱️ Time Watch

The **Time Watch** contains additional time that can be added to the player's countdown.

However, the amount of time available on the watch decreases while it remains on the floor, rewarding the player for collecting it quickly.

The player's timer is capped at **30 seconds**.

---

### 👟 Hermes Boot

The **Hermes Boot** gives the player a temporary **50% movement-speed increase for 4 seconds**.

This can help the player escape NPCs, reach pickups, or close the distance to an NPC they want to swap timers with.

---

### 🔒 Time Lock

The **Time Lock** protects the player's timer for **4 seconds**.

While active, NPCs cannot steal time from the player.

It also affects timer swapping — if an NPC currently has a Time Lock active, the player cannot swap timers with that NPC until the effect expires.

---
A temporary power-up that affects the countdown system and gives the player a short opportunity to act without the usual time pressure.

All pickup effects last for:

```text
4 Seconds
```

The short duration means pickups create temporary opportunities rather than permanent upgrades.

Their spawn rate was balanced so that the player still needs to interact with NPCs and make use of the timer-swapping mechanic rather than relying entirely on pickups.

---

### 🚪 Evacuation System

At the beginning of the match, an evacuation countdown starts.

The exit remains unavailable until the evacuation timer reaches zero.

Once the exit opens, the player still has to physically reach it before their personal timer expires.

Opening the exit does not automatically win the game.

This creates a final high-pressure section where every remaining second becomes especially important.

---

## 🧠 AI System

The NPC AI is built around multiple behaviour states.

```text
Wander
   ↓
Evaluate Nearby Targets
   ↓
 ┌───────────────┐
 │               │
Chase           Flee
 │               │
 └───────┬───────┘
         ↓
      Wander
```

NPC decision-making considers nearby:

```text
Player
Other NPCs
Pickups
Walls / Obstacles
```

A major part of their decision-making is based on comparing their remaining time with nearby characters.

For example:

```text
NPC has LESS time than the player or another NPC
                    ↓
                  CHASE

NPC has MORE time than the player or another NPC
                    ↓
                   FLEE
```

An NPC with less time becomes aggressive because stealing time from another character can help it survive.

An NPC with more time instead tries to protect its advantage by fleeing from characters that could take time from it.

Because timers constantly change through stealing, swapping and pickups, an NPC can switch between **chasing and fleeing during the same match**.

This creates situations where a character that was previously chasing the player may suddenly begin running away once the balance of time changes.

---

## 🛠️ Systems Implemented

The project includes several gameplay systems written in **C#**:

- Player movement
- Player countdown timer
- NPC countdown timers
- NPC time-stealing system
- Timer swapping
- NPC swap-range detection
- NPC highlighting and selection
- Mouse-based NPC targeting
- Swap cooldown
- Wander / Chase / Flee AI states
- NPC awareness system
- NPC target selection
- NPC wall avoidance
- Pickup system
- Pickup spawning
- Timer pickup
- Hermes Boots pickup
- Freeze Time pickup
- Temporary 4-second pickup effects
- In-game pickup descriptions
- Match countdown
- Evacuation timer
- Exit activation
- Win / Lose states
- Main menu
- Scene transitions
- Floating time feedback
- Audio management
- Countdown sound effects
- Low-time warnings
- Game-state management

---

## 🎨 Art & Animation

The game uses a simple pixel-art visual style built around a bright arena.

Character sprites were created using **Aseprite**.

The player is represented by a stick character with a scarf, while NPCs use characters based on the same visual style.

Character animations include:

```text
Idle
Walk Up
Walk Down
Walk Left
Walk Right
```

The animations use a small number of frames to maintain the minimalist pixel-art aesthetic.

---

## 🔊 Audio

Audio is used throughout the game to reinforce the countdown mechanics and make the player more aware of their remaining time.

The game includes:

- Menu music
- Gameplay music
- Clock ticking
- Countdown beeps
- Timer swap sound effects
- Time-stealing sound effects
- Low-time warnings
- Pickup sounds
- UI interaction sounds

Audio becomes increasingly important as the player's timer gets closer to zero, helping reinforce the pressure of the countdown.

---

## ✨ Polish

Several additional systems were added during the final stages of the game jam.

### 🎬 Pixel Scene Transition

Scenes transition using a pixel-block effect that moves across the screen.

This was added to make transitions between the menu and gameplay feel more polished.

---

### ⏳ Starting Countdown

Every match begins with:

```text
3
2
1
GO!
```

Player movement and abilities remain disabled until the countdown finishes.

This ensures every run begins consistently and gives the player a moment to prepare.

---

### ➕ Floating Time Feedback

When time is gained or lost, floating text provides immediate visual feedback.

For example:

```text
+1
-1
```

This allows the player to understand when their timer changes without constantly needing to watch the main timer UI.

---

### 🎯 NPC Swap Highlight

When the player gets close enough to an NPC to perform a timer swap, a visible box appears around that NPC.

This shows the player that the NPC is currently selectable.

The player can then click the highlighted character to exchange timers.

---

## 💻 Technologies Used

| Technology | Usage |
|---|---|
| **Unity** | Game engine |
| **C#** | Gameplay programming |
| **Aseprite** | Pixel art and animation |
| **Audacity** | Audio editing |
| **Git / GitHub** | Version control |
| **WebGL** | Browser build |

---

## 🎯 What I Learned

Second Hand was developed under a very short **4-day game jam deadline**, meaning I had to prioritise mechanics that directly supported the game's central idea.

The project gave me experience with:

- Designing a game around one central mechanic
- Building timer-based gameplay systems
- Connecting multiple systems to the same shared resource
- Implementing state-based NPC AI
- Creating interactions between multiple autonomous NPCs
- Designing risk/reward gameplay mechanics
- Implementing temporary power-ups
- Managing different game states
- Creating clear gameplay feedback
- Debugging AI behaviour
- Balancing gameplay systems
- Rapid iteration
- Working within a strict deadline
- Polishing a prototype into a complete playable game

One of the most interesting parts of development was making NPCs interact with **each other**, rather than only reacting to the player.

Their remaining time can influence whether they chase or flee, allowing their behaviour to change throughout a match.

Another major design challenge was making **time affect almost every part of the game**.

The player's life, NPC interactions, timer swapping, pickups and the pressure to reach the exit all revolve around the same countdown mechanic.

This helped keep the game focused around the game jam's **"Count Down"** theme.

---

## 🎥 Gameplay

Gameplay screenshots and footage can be displayed here.

Example:

```html
<p align="center">
  <img src="Images/gameplay1.png" width="45%" />
  <img src="Images/gameplay2.png" width="45%" />
</p>
```

A gameplay GIF can also be displayed directly inside the README:

```markdown
![Second Hand Gameplay](Images/gameplay.gif)
```

---

## 🎮 Play the Game

Second Hand is available as a **WebGL browser game on itch.io**.

**Play Second Hand:**  
https://alexziou.itch.io/second-hand

---

## 📦 Running the Project

To open the project locally:

1. Clone this repository.
2. Open **Unity Hub**.
3. Select **Add project from disk**.
4. Select the cloned project folder.
5. Open the project using the appropriate Unity version.
6. Open the main menu scene.
7. Press **Play**.
