# ⏱️ Second Hand

**Second Hand** is a fast-paced 2D game created in **Unity / C#** for a 4-day game jam based around the theme **"Count Down"**.

In Second Hand, **time is your life**.

Every character in the arena has their own countdown timer. The player must steal time from NPCs, manage their remaining seconds, use a position-swapping ability strategically, and survive long enough for the exit to open.

> Built with Unity, C#, Aseprite and custom gameplay systems.

---

## 🎮 Gameplay

Every character starts with a limited amount of time.

When the timer reaches zero, that character is eliminated.

The player can increase their remaining time by interacting with NPCs and collecting watches around the arena. However, NPCs are also competing for time and can steal it from each other.

Once the evacuation countdown finishes, the exit becomes available and the player must reach it before their own timer runs out.

### Core Gameplay Loop

- ⏱️ Survive while your timer continuously counts down
- 👥 Steal time from NPCs
- 🏃 Avoid stronger NPCs that can steal time from you
- 🔄 Swap positions with another character
- ⌚ Find watches that provide additional time
- 🚪 Survive until the exit opens
- 🏁 Reach the exit before your timer reaches zero

---

## 🕹️ Main Features

### ⏱️ Time as Health

Instead of using a traditional health system, every character has a countdown timer.

Time functions as both the player's **health and main resource**.

Taking time from another character increases your timer while reducing theirs.

This creates a constant risk/reward decision between avoiding danger and approaching NPCs to gain additional time.

---

### 🤖 NPC Behaviour

NPCs use different behaviours depending on their situation.

They can:

- Wander around the arena
- Chase characters with less time
- Flee from characters that are more dangerous
- Target the player
- Target other NPCs
- Search for watches
- Avoid walls and obstacles

NPCs are therefore competing with both the player and each other instead of existing purely as enemies.

---

### 🔄 Position Swap Ability

The player has an ability that allows them to **swap positions with another character**.

Potential targets are highlighted before the swap.

The ability can be used to:

- Escape dangerous situations
- Move closer to the exit
- Avoid pursuing NPCs
- Reposition strategically
- Reach areas of the arena more quickly

The ability uses a cooldown to prevent constant swapping.

---

### ⌚ Watch Pickups

Watches periodically appear around the arena.

Collecting one adds additional time to the character that reaches it.

NPCs can also target watches, meaning the player must sometimes compete with them for the same resource.

The spawn rate and amount of time provided by watches were balanced during development to prevent them from becoming more valuable than interacting with NPCs.

---

### 🚪 Evacuation System

At the beginning of the match, an evacuation countdown starts.

The exit remains unavailable until the evacuation timer reaches zero.

Once the exit opens, the player still has to physically reach it before their personal timer expires.

This creates a final high-pressure section where the player's remaining time becomes especially important.

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
