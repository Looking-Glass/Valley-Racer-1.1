Valley Racer 1.1
Sept 27 2016

Here you will find the source code and files for Valley Racer.

One unique aspect of Valley Racer is that it uses a customized version of the Hypercube. Specifically, I've added some code to the hypercubeCamera script to achieve visuals I couldn't achieve otherwise. I've had some pitfalls in maintaining a custom hypercube across multiple updates of the hypercube plugin, so I will share what I've found to be effective practices:

1. Keep step-by-step notes of exactly what modifications were done to the hypercube so you can replicate them quickly on an update.

2. Keep notes on what values your hypercube's components have in each scene. Sometimes after an update the best way to resolve problems that occur is to delete the hypercube in the scene and replace it with the prefab. You'll want to be able to quickly set the values to what they were before.

3. Avoid references directly to the hypercube. If you've noticed, I keep my hypercube parented by an object called "Hypercube Holder," which is an empty game object I make my references to. The hypercube holder never needs to be deleted or replaced, so my references never break during an update.

I hope this helps anyone working in volume, and if there are any curious visitors with questions on Valley Racer, feel free to email me at kyle@lookingglassfactory.com

Thanks for playing!