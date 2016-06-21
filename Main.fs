// This script implements the "PASA" QoS prediction approach
// Authors: Antonio Brogi, Marco Danelutto, Daniele De Sensi, Ahmad Ibrahim, Jacopo Soldani, Massimo Torquati
// @copyright Computer Science Department, University of Pisa, Italy

module skeleton.Main
open Program
open System 
open System.IO
open System.Linq 


[<EntryPoint>]
let main argv =    
    let totalinputs = 1000 //Total number of inputs
    let monteCarloSimulation = 1000 //Number of Monte Carlo Simulation

    //Motivating Example 1 (image blurring application with a feedback loop). 
    let w = Example1.startSimulation totalinputs monteCarloSimulation
        
    //Motivating Example 2 (qualitative analysis) 
    let w = Example2.startSimulation totalinputs monteCarloSimulation
    
    printf "\n\n done. Press Enter to exit"
    Console.ReadLine() |>ignore
    0 // return an integer exit code
