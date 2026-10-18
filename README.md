# Sailing Ship Simulation (Unity 6)

A technical recruitment task featuring a kinematics-based sailing simulation built in Unity 6 (6000.5.11f1) using URP, UI Toolkit, and the New Input System.

---

## Overview

The project demonstrates a ship navigating water without using Rigidbody physics or colliders, relying purely on custom mathematical modeling for wind propulsion, hydrodynamic drag, and sail trimming.

![image](https://github.com/Pattxx/LittleBoatGame/blob/main/Docs/BoatScreenShot1.png)

## Demo video


<video src="Docs/BoatDemo.mp4" width="100%" controls autoplay loop muted>
  Your browser does not support the video tag.
</video>

---

## Controls

The controls are implemented via the new Input System:

| Action | Key / Binding | Description |
| :--- | :--- | :--- |
| **Steer** | `A` / `D` (or Left / Right Arrow) | Turns the rudder. Turning authority scales dynamically with forward speed through the water. |
| **Trim Sail** | `Q` / `E` | Rotates the sail boom between `-45°` and `+45°`. |
| **Unfurl / Furl Sail** | `W` / `S` | Adjusts sail deployment percentage (`0%` to `100%`). |

---

## Technical Details & Architecture

### 1. Sailing & Hydrodynamic Model (No Rigidbody)
Real sailboats cannot sail straight into the wind and rely on hydrodynamic lift from the keel to convert lateral wind force into forward motion:
* **Dead Zone / Headwind Penalty:** Sailing directly into the eye of the wind drastically cuts sail thrust down to zero.
* **Sail Trim Efficiency:** Thrust is computed based on the alignment between the sail normal and the global wind vector (`Mathf.Abs(Vector3.Dot(sailNormal, windDir))`).
* **Directional Drag (Keel Simulation):** Velocity is decomposed into forward (`transform.forward`) and lateral (`transform.right`) vectors. Applying heavy side-drag simulates keel foil resistance, letting the ship cut forward smoothly while cancelling leeway drift.
* **Speed-Dependent Steering:** Rudder turning responsiveness scales with the ship's current forward velocity (a dead-stop vessel cannot steer).

### 2. UI Toolkit
* Built entirely with UI Toolkit (`UIDocument`, UXML, USS) without legacy UGUI or TextMeshPro.
* A single HUD displays real-time sail angle (degrees) and sail openness (percentage).

### 3. Build & Optimization Setup
* **Unity Version:** `6000.5.11f1` (URP)
* **Scripting Backend:** `IL2CPP`
* **Managed Stripping Level:** `High`
* **Code Style:** Strictly adheres to [Microsoft C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions) (`sealed` classes, explicit access modifiers, clean private field prefixes, and comprehensive comments).

### 4. Art & 3D Assets
- **Ship Model:** Custom 3D model crafted and rigged from scratch in **Blender**.
- **Texturing:** Authored in **Adobe Substance 3D Painter** with packed PBR maps configured for the URP Lit shader.

![image](https://github.com/Pattxx/LittleBoatGame/blob/main/Docs/BoatSub.png)

---

## Project Structure

```text
Assets/
├── Input/              # Input Actions configuration
├── Materials/          # URP Lit materials
├── Models/             # Ship FBX asset and meshes
├── Resources/          # Dynamic project resources
├── Scenes/             # Main gameplay scene
├── Scripts/            # Core scripts (PlayerController, SailController, ShipMovement, WindController, ShipHUDController)
├── Settings/           # URP pipeline & project settings
├── Textures/           # Imported texture maps (made in Substance Painter)
├── UI/                 # UI layouts & stylesheets
└── UI Toolkit/         # Panel settings & theme assets
