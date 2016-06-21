// This script implements the "PASA" QoS prediction approach
// Authors: Antonio Brogi, Marco Danelutto, Daniele De Sensi, Ahmad Ibrahim, Jacopo Soldani, Massimo Torquati
// @copyright Computer Science Department, University of Pisa, Italy
 
module Visualizer 
open FSharp.Charting
open System.Windows.Forms


let show samplesSeq input=
    let bucketCount = 50

    let bucketize rangeMin rangeMax : seq<float> -> seq<float*int> =
        let range = rangeMax - rangeMin
        let bucketWidth = range / (float bucketCount)
        let projection v =
            ((v - rangeMin) / bucketWidth |> truncate) * bucketWidth  + (rangeMin + bucketWidth/2.0)
        let filter v =
            v < rangeMax && v > rangeMin
        Seq.filter filter >> Seq.countBy projection
   
    let EnergyAvg =
        samplesSeq
        |> Seq.averageBy (fun (energy,time) -> (energy))
    let histogramEnergyAvg =
        let samplesSeq =
            samplesSeq
            |> Seq.map (fun ((energy,time)) -> (energy))
        let avg = Seq.average samplesSeq
        let sqavg = samplesSeq |> Seq.averageBy (fun x-> (x - avg) * (x-avg))
        let stddev = System.Math.Sqrt(sqavg)
        let rMina = Seq.min samplesSeq
        let rMaxa = Seq.max samplesSeq
        let rMin = avg - stddev * 4.0
        let rMax = avg + stddev * 4.0
        let range = rMax - rMin
        let rMin = ((rMin / range) * 100.0 |> truncate) * range / 100.0   
        let rMax = ((rMax / range) * 100.0 + 0.9999 |> truncate) * range / 100.0
        samplesSeq
        |> bucketize rMin rMax
        |> Seq.maxBy snd
        |> fun (_,my) ->[ EnergyAvg,0.0;EnergyAvg,float my * 1.1]
        |> fun x -> Chart.Line (x,null,null,["";sprintf "Average Energy (mJ): %.3f" EnergyAvg ],System.Drawing.Color.Red)
    let histogramEnergy =
        let samplesSeq =
            samplesSeq
            |> Seq.map (fun ((energy,time)) -> (energy))
        let avg = Seq.average samplesSeq
        let sqavg = samplesSeq |> Seq.averageBy (fun x-> (x - avg) * (x-avg))
        let stddev = System.Math.Sqrt(sqavg)
        let rMin = avg - stddev * 4.0
        let rMax = avg + stddev * 4.0
        let buckets =
            samplesSeq
            |> bucketize rMin rMax
            |> Seq.sortBy fst
        Chart.Column (buckets,Color=System.Drawing.Color.Blue)
        |> fun x -> Chart.Combine [x;histogramEnergyAvg]
        |> Chart.WithXAxis (true,"Energy (mJ)")
    let histogramEnergyNoBucket =
        let samplesSeq =
            samplesSeq
            |> Seq.map (fun ((energy,time)) -> (energy))
        let avg = Seq.average samplesSeq
        let sqavg = samplesSeq |> Seq.averageBy (fun x-> (x - avg) * (x-avg))
        let stddev = System.Math.Sqrt(sqavg)
        let rMin = avg - stddev * 4.0
        let rMax = avg + stddev * 4.0
        let filter v = v < rMax && v > rMin
        let buckets =
            samplesSeq
            |> Seq.filter filter
            |> Seq.countBy id
            |> Seq.sortBy fst
        Chart.Column (buckets,Color=System.Drawing.Color.Blue)
        |> Chart.WithXAxis (true,"Energy (mJ)")
        
    let timeAvg =
        samplesSeq
        |> Seq.averageBy (fun ((energy,time)) -> ( time))
    let histogramTimeAvg =
        let samplesSeq =
            samplesSeq
            |> Seq.map (fun ((energy,time)) -> ( time))
        let avg = Seq.average samplesSeq
        let sqavg = samplesSeq |> Seq.averageBy (fun x-> (x - avg) * (x-avg))
        let stddev = System.Math.Sqrt(sqavg)
        let rMin = avg - stddev * 4.0
        let rMax = avg + stddev * 4.0
        let range = rMax - rMin
        let rMin = ((rMin / range) * 100.0 |> truncate) * range / 100.0
        let rMax = ((rMax / range) * 100.0 + 0.9999 |> truncate) * range / 100.0
        let label = sprintf "Average time(msec): %.4f" timeAvg 
        let buckets = bucketize rMin rMax samplesSeq
        let maxBucket = Seq.maxBy snd buckets
        let height = float (snd maxBucket)
        Chart.Line ([ timeAvg,0.0;timeAvg,height * 1.1] ,null,null,["";label ],System.Drawing.Color.Red) 

    let histogramTime =
        let samplesSeq =
            samplesSeq
            |> Seq.map (fun ((energy,time)) -> ( time))
        let avg = Seq.average samplesSeq
        let sqavg = samplesSeq |> Seq.averageBy (fun x-> (x - avg) * (x-avg))
        let stddev = System.Math.Sqrt(sqavg)
        let rMin = avg - stddev * 4.0
        let rMax = avg + stddev * 4.0
        let buckets = bucketize rMin rMax samplesSeq |> Seq.sortBy fst
        let hist = Chart.Column (buckets,Color=System.Drawing.Color.Blue)
        Chart.Combine [hist;histogramTimeAvg] |> Chart.WithXAxis (true,"Time (msec)") 

 

    Chart.Rows (
        [
            histogramEnergy  
            histogramTime  
        ]
        ) |> Chart.Show

     
 