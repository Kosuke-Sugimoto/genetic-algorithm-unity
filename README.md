# Genetic Algorithm Unity
Training Ball agent by genetic algorithm on Unity.  
___This repository is the revised version of the commit messages from the repository created in 2022 April.___

## Usage
1. Import "genetic-algorithm-unity.unitypackage".
2. Drag and drop "level~"file in Scenes directory to Hierarchy.
3. Remove "SampleScene" in Hierarchy.
4. Push the start button.

## Feature
・I chose "Uniform Select" as selection method.  
・Mutation probability is 2 %.  
・If this program fall into local minimum, call the function "Escape()".  
------------- if there is no difference between top point and bottom one, Escape() is called.  
・There are three scenes per level.

## Attention
・In level3, this program may require 300 generation. 
