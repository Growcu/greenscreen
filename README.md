# Green Screen JA

## Project path
Clone the repository to the following path:<br>
`C:\projekty\`<br>
because you need to use the absolute path to the DLL in the code

## How start
1. Download [VS 2022 Community](https://visualstudio.microsoft.com/pl/)
2. Install
3. Add components:
   - .NET desktop development
   - Desktop development with C++
4. Clone repo:
    ```cmd
    git clone https://gitlab.aei.polsl.pl/dg300645/green-screen-ja
    ```
5. Change to the cloned repository's directory
    ```cmd
    cd Swap-DApp
    ```
6. Start VS
    ```cmd
    green-screen-ja.sln
    ```
    Everything should be connect, once you run the project you will be able to run the application


### <span style="color:indianred">Warning</span><br>
If you change the project path when cloning the repository, you must also change the absolute path to the DLL in the code:<br>
`.\green-screen-ja\Program.cs`<br>
```c#
// line18:
[DllImport(@"C:\projekty\green-screen-ja\x64\Debug\JAAsm.dll")]
```