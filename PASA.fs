// This script implements the "PASA" QoS prediction approach
// Authors: Antonio Brogi, Marco Danelutto, Daniele De Sensi, Ahmad Ibrahim, Jacopo Soldani, Massimo Torquati
// @copyright Computer Science Department, University of Pisa, Italy
namespace skeleton

open System

module Program =
    type QoS = float * float

    type InputType = Heavy | Light 

    type Activity =
        | Node of (InputType->QoS)
        | Comp of Activity list
        | Pipe of Activity list
        | Farm of Activity * int
        | Feedback of Activity * string * float

    let zero = 0.0,0.0:QoS

 

    let inline Both(a:QoS,b:QoS) =
        let energy1,time1 = a
        let energy2,time2 = b
        (energy1 + energy2), (max time1 time2):QoS

    let inline Delay(a:QoS,b:QoS) =
        let energy1,time1 = a
        let energy2,time2 = b
        (energy1), (time1 + time2):QoS

    let sampleCondition(condition:string, probability:float)=
        let g = new System.Random()
        let number = g.NextDouble()
        if number < probability then
            true
        else
            false

    let predictQoS(a:Activity, size:int, samplingFunction) =
        let inputStream = Array.init size samplingFunction

        let rec exec (a:Activity, startIndex:int, endIndex:int) :QoS=
            match a with
            | Node(evaluateQoS) ->
                let mutable nodeQoS = zero

                // For each data item "i" from the input stream
                for i=startIndex to endIndex do
                    // [1] Compute the QoS "iQoS" of "i"
                    let iQoS = evaluateQoS(inputStream.[i])
                    // [2] Update node QoS by adding "iQoS" 
                    nodeQoS <- Both(iQoS,Delay(nodeQoS,iQoS))
                nodeQoS
            | Comp(aList)->
                let mutable compQoS = zero             
                // For each data item "i" from the input stream
                for i = startIndex to endIndex do
                    let mutable aListQoS = zero
                    // [1] For each activity "a" in Comp
                    for a in aList do              
                        // [1.1] Compute the QoS "aQoS" required by
                        //       activity "a" for processing item "i"              
                        let aQoS = exec (a,i,i)
                        // [1.2] Update the QoS of Comp for proccesing 
                        //       item "i" by adding "aQoS" 
                        aListQoS <- Both(aListQoS,Delay(aQoS,aListQoS))
                    // [2] Update overall Comp QoS by adding "aListQoS" 
                    compQoS <- Both(compQoS, Delay(aListQoS,compQoS))
                compQoS
            | Pipe(aList) ->
                let mutable pipeQoS = zero
                for a in aList do 
                    let aQoS = exec(a,startIndex,endIndex)
                    pipeQoS <- Both(pipeQoS,aQoS)
                pipeQoS
            | Farm (a,n) ->
                let mutable farmQoS = zero
                let workerStreamSize = (endIndex - startIndex + 1) / n
                for w=0 to n-1 do
                    let wStartIndex = startIndex + w*workerStreamSize
                    let wEndIndex = wStartIndex + workerStreamSize - 1
                    let mutable wQoS = zero
                    if (wEndIndex < inputStream.Length) then
                        wQoS <- exec(a,wStartIndex,wEndIndex)
                    else 
                        wQoS <- exec(a,wStartIndex,inputStream.Length)
                    farmQoS <- Both(farmQoS,wQoS)  
                farmQoS
            | Feedback(a,condition,probability) ->                 
                // Compute QoS of feedback by evaluating "a" over all input stream
                let mutable feedbackQoS = exec(a,startIndex,endIndex)
                // Count the number of items routed back to the input stream
                let mutable f = 0
                for i=startIndex to endIndex do
                    if(sampleCondition(condition,probability)) then
                        f  <- f + 1
                // Update "feedbackQoS" by adding the QoS for re-processing "f" items
                // by re-executing "Feedback(a,condition,probability)
                if f > 0 then
                    let fQoS = exec(Feedback(a,condition,probability),endIndex-f+1,endIndex)
                    feedbackQoS <- Both(feedbackQoS,Delay(fQoS,feedbackQoS))
                feedbackQoS
        exec(a,0,inputStream.Length-1)