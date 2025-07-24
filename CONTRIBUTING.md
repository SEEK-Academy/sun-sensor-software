# Contributing

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

## Coding Standards

Official documentation [C# Coding Conventions](https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions)

- Use **PascalCase** for types and methods.
- Use **camelCase** for parameters and private fields.
- Use prefix `_` for private fields.
- Keep methods short and focused; adhere to the Single Responsibility Principle.

## GitHub Flow

Official documentation [GitHub Flow](https://docs.github.com/en/get-started/using-github/github-flow)

1. Branch

   ```bash
   git checkout main
   git pull
   git checkout -b feat/short-name
   ```

2. Commit

	[Conventional Commits 1.0.0](https://www.conventionalcommits.org/en/v1.0.0/)

3. Pull Request

	Push your branch and open a Pull Request against `main`.

4. Merge

   ```bash
   git checkout main
   git merge --no-ff feat/short-name
   git push
   ```

5. Cleanup

   ```bash
   git branch -d feat/short-name
   git push origin --delete feat/short-name
   ```
