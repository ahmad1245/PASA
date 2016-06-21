# PASA
This repository contains the source code of an F\# implementation of the PASA probabilistic approach for predicting QoS of parallel design patterns-based applications. Such approach has been presented in
> _A. Brogi, M. Danelutto, D. De Sensi, A. Ibrahim, J. Soldani, M. Torquati <br>
> **Predicting the QoS of Parallel Design Patterns-based Applications.** <br>
> [Accepted in 9th International Symposium on High-Level Parallel Programming and Applications (HLPP 2016)]._ 

If you wish to reuse the sources in this repository, please properly cite the above mentioned paper. Below you can find the BibTex reference:
```
@article{PASA,
 author = {Antonio Brogi and Marco Danelutto and Daniele De~Sensi and Ahmad Ibrahim and Jacopo Soldani and Massimo Torquati},
 title = {Predicting the QoS of Parallel Design Patterns-based Applications},
 journal = {},
 volume = {},
 pages = {},
 year = {},
 issn = {}
} 
```
In order to use this program, you need to do following steps
Step 1. Install Microsoft Visual Studio 2013. 
Step 2. Create a Visual F# Console Application project.
Step 3. Right click on project name in "Solution Explorer" window to add existing code files.
Step 4. In F# project, order of the code files is very important (files can be Move Up or Down). Please use the following order to run PASA:
      Visualizer.fs
      PASA.fs
      Example1.fs
      Example2.fs
      Main.fs
(You also need to add FSharp.Charting, FSharp.Core, FSharp.Data etc in the "References" depending upon your system configuration)
```
