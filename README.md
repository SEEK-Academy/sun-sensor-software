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

### Technology Stack

* **Unity** – For 3D graphics and simulation.
* **C#** – Core scripting language for Unity.

## How to use

> 🚧 **Work in Progress**
