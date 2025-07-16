# SunSensor

Open-source project for determing the spatial orientation relative to a light source (e.g., the Sun) and is housed in a 3D-printed CubeSat format (U1 standard).

## Project Structure

The SunSensor project is organized into two GitHub repositories, each responsible for specific functional modules:

* [`sun-sensor-hardware`](https://github.com/SEEK-Academy/sun-sensor-hardware):

  * **Mechanics** – Design and 3D printing of the CubeSat structure.
  * **Electronics** – Selection and integration of electronic components.
  * **Firmware** – Low-level software responsible for hardware communication and orientation calculations based on light-source input.

* `sun-sensor-software`:

  * **Software** – A real-time 3D simulation of the SunSensor device and the light source.

## sun-sensor-software

This repository contains the Software module of the SunSensor project. It delivers a real-time simulation environment developed in Unity, intended to run on single-board computers.

### Features

* Connects to the SunSensor hardware via USB and receives live orientation data.
* Renders the SunSensor device and a dynamic light source in a 3D scene.
* Displays a visual error margin in the form of a directional cone.
* Updates the simulation scene in real time based on incoming data.

## Setup and Configuration

Follow these steps to get the project up and running:

1. Install Unity Hub 

- Download and install [Unity Hub](https://unity.com/download).

2. Install Unity Editor

- In Unity Hub, switch to the "Installs/Add Unity Editor".  
- Install version **2022.3.19f1**.

3. Install Visual Studio

- If you don’t already have IDE, download and install [Visual Studio](https://visualstudio.microsoft.com/).
- During installation, select the "Game development with Unity" workload to get the Unity editor integration and C# tools.

4. Clone repository

```bash
git clone https://github.com/SEEK-Academy/sun-sensor-software.git
```

5. Project in Unity Editor

- In Unity Hub, click "Add", navigate to the cloned project folder and select it.
- Double-click to launch the project in the Unity Editor.

6. Configure external script editor

- In Unity Editor, go to "Edit/Preferences…/External Tools".
- Set External Script Editor to your IDE (e.g. Visual Studio).

## How to use

> 🚧 **Work in Progress**
