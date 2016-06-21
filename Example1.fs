// This script implements the "PASA" QoS prediction approach
﻿//Paper title: Analysing Multiple QoS Attributes in Parallel Design Patterns-based Applications. 
// Authors: Antonio Brogi, Marco Danelutto, Daniele De Sensi, Ahmad Ibrahim, Jacopo Soldani, Massimo Torquati
// @copyright Computer Science Department, University of Pisa, Italy

namespace skeleton
open System.Linq 
open Program

//Motivating Example 1 (image blurring application with a feedback loop)
module Example1=
    let mutable ph = 0.0  
    let g = new System.Random()
    let samplingFunction idx=
        printf "-"
        let number = g.NextDouble()
        if number < ph then
            Heavy
        else
            Light

    let inline read_QoS (workload):QoS =
        match workload with
        | Light ->
            (1.84,0.144) //(1.84 is Energy and 0.144 is Time)
        | Heavy ->
             (1.84,0.144)

    let inline blur1_QoS (workload):QoS =
        match workload with
        | Light ->
            (4.99,0.377)
        | Heavy ->
            (14.44,3.991)

    let inline blur2_QoS (workload):QoS =
        match workload with
        | Light ->
             (4.41,0.335)
        | Heavy ->
             (13.45,3.523)

    let inline write_QoS (workload):QoS =
        match workload with
        | Light ->
             (1.95,0.151)
        | Heavy ->
             (1.95,0.151)
    
    let read = Node (read_QoS)
    let blur1 = Node (blur1_QoS)
    let blur2 = Node (blur2_QoS)
    let write = Node (write_QoS)

    let startSimulation i monteCarloSimulation = 
        let pf_list = [0.1; 0.2; 0.3; 0.4] //Prob. of feedback
        let ph_list = [0.1; 0.2; 0.3; 0.4] //Perc. of heavy items
        
        for ph1 in ph_list do //for all heavy items
            ph <- ph1
            for probF in pf_list do //for all feedback
                let motivatingExample = Pipe[read;Feedback(Pipe[blur1;blur2],"blurred?",probF);write]
                let mutable w =0
                let mutable cL = Array.empty
                while (w < monteCarloSimulation) do
                    let a = Array.init (5) (fun _ ->   predictQoS(motivatingExample,i,samplingFunction))
                    w <- w + 5
                    cL <-Array.append cL a
                Visualizer.show cL (float i)
