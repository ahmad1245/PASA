// This script implements the "PASA" QoS prediction approach
﻿//Paper title: Analysing Multiple QoS Attributes in Parallel Design Patterns-based Applications. 
// Authors: Antonio Brogi, Marco Danelutto, Daniele De Sensi, Ahmad Ibrahim, Jacopo Soldani, Massimo Torquati
// @copyright Computer Science Department, University of Pisa, Italy

namespace skeleton
open System.Linq 
open Program

//Motivating Example 2 (qualitative analysis)
module Example2=
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

    let probF = 0.2 //Prob. of feedback
    let probHeavy = 0.45 //Perc. of heavy items


    // (C) a composition of Comp and Feedback
    let C = Comp[read;Feedback(Comp[blur1;blur2],"blurred?",probF);write]

    // (P) a composition of Pipe and Feedback
    let P = Pipe[read;Feedback(Pipe[blur1;blur2],"blurred?",probF);write]
    
    //Farm, each of whose workers is a composition of Comp and Feedback
    let F2C = Farm(Comp[read;Feedback(Comp[blur1;blur2],"blurred?",probF);write],2)
    let F3C = Farm(Comp[read;Feedback(Comp[blur1;blur2],"blurred?",probF);write],3)
    let F4C = Farm(Comp[read;Feedback(Comp[blur1;blur2],"blurred?",probF);write],4)
    let F5C = Farm(Comp[read;Feedback(Comp[blur1;blur2],"blurred?",probF);write],5)
    let F6C = Farm(Comp[read;Feedback(Comp[blur1;blur2],"blurred?",probF);write],6)
    let F7C = Farm(Comp[read;Feedback(Comp[blur1;blur2],"blurred?",probF);write],7)
    let F8C = Farm(Comp[read;Feedback(Comp[blur1;blur2],"blurred?",probF);write],8)
    let F9C = Farm(Comp[read;Feedback(Comp[blur1;blur2],"blurred?",probF);write],9)
    let F10C = Farm(Comp[read;Feedback(Comp[blur1;blur2],"blurred?",probF);write],10)
    let F11C = Farm(Comp[read;Feedback(Comp[blur1;blur2],"blurred?",probF);write],11)
    let F12C = Farm(Comp[read;Feedback(Comp[blur1;blur2],"blurred?",probF);write],12)
    let F13C = Farm(Comp[read;Feedback(Comp[blur1;blur2],"blurred?",probF);write],13)
    let F14C = Farm(Comp[read;Feedback(Comp[blur1;blur2],"blurred?",probF);write],14)


    //Farm whose workers are compositions of Pipe and Feedback
    let F2P = Farm(Pipe[read;Feedback(Pipe[blur1;blur2],"blurred?",probF);write],2)
    let F3P = Farm(Pipe[read;Feedback(Pipe[blur1;blur2],"blurred?",probF);write],3)

    //Pipe whose first and last stages are Read and Write, whose intermediate stage is a Farm whose workers are a Comp of Blur and Blur2
    let PF2C = Pipe[read;Farm(Feedback(Comp[blur1;blur2],"blurred?",probF),2);write]
    let PF3C = Pipe[read;Farm(Feedback(Comp[blur1;blur2],"blurred?",probF),3);write]
    let PF4C = Pipe[read;Farm(Feedback(Comp[blur1;blur2],"blurred?",probF),4);write]
    let PF5C = Pipe[read;Farm(Feedback(Comp[blur1;blur2],"blurred?",probF),5);write]
    let PF6C = Pipe[read;Farm(Feedback(Comp[blur1;blur2],"blurred?",probF),6);write]
    let PF7C = Pipe[read;Farm(Feedback(Comp[blur1;blur2],"blurred?",probF),7);write]
    let PF8C = Pipe[read;Farm(Feedback(Comp[blur1;blur2],"blurred?",probF),8);write]
    let PF9C = Pipe[read;Farm(Feedback(Comp[blur1;blur2],"blurred?",probF),9);write]
    let PF10C = Pipe[read;Farm(Feedback(Comp[blur1;blur2],"blurred?",probF),10);write]
    let PF11C = Pipe[read;Farm(Feedback(Comp[blur1;blur2],"blurred?",probF),11);write]
    let PF12C = Pipe[read;Farm(Feedback(Comp[blur1;blur2],"blurred?",probF),12);write]


    //Pipe whose first and last stages are Read and Write, whose intermediate stage is a Farm whose workers are a Pipe of Blur and Blur2
    let PF2P = Pipe[read;Farm(Feedback(Pipe[blur1;blur2],"blurred?",probF),2);write]
    let PF3P = Pipe[read;Farm(Feedback(Pipe[blur1;blur2],"blurred?",probF),3);write]
    let PF4P = Pipe[read;Farm(Feedback(Pipe[blur1;blur2],"blurred?",probF),4);write]
    let PF5P = Pipe[read;Farm(Feedback(Pipe[blur1;blur2],"blurred?",probF),5);write]
    let PF6P = Pipe[read;Farm(Feedback(Pipe[blur1;blur2],"blurred?",probF),6);write]



    let list_examples = [   C;
                            P;
                            F2C;F3C;F4C;F5C;F6C;F7C;F8C;F9C;F10C;F11C;F12C;F13C;F14C;
                            F2P;F3P;
                            PF2C;PF3C;PF4C;PF5C;PF6C;PF7C;PF8C;PF9C;PF10C;PF11C;PF12C;
                            PF2P;PF3P;PF4P;PF5P;PF6P
                        ]

    let startSimulation i monteCarloSimulation = 
        ph <- probHeavy
        for example in list_examples do //for all examples
            let mutable w =0
            let mutable cL = Array.empty
            while (w < monteCarloSimulation) do
                let a = Array.init (5) (fun _ -> predictQoS(example,i,samplingFunction))
                w <- w + 5
                cL <-Array.append cL a
            Visualizer.show cL (float i)
