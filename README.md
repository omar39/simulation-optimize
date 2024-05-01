# My approach for solving the problem
## Optimizing graphics
### For Marbles
GPU draw calls were at first exceeding 1000, which increases CPU bottleneck.
So I managed to make the Marbles prefab static, and enabled GPU instancing for its materials to achieve static batching which makes draw calls nearly 40.
### For Actors
This was a hassle to be honest, as I got rid of the Actors prefab and instead, I used GPU instancing for making them, and managed to update their hunt/search behaviors by a manager script.
## Optimizing CPU
### For Actors
After getting rid of the Actors behavior scripts, things got much better, by managing their behavior in one script which updates Actors positions on GPU based on their Marble targets, but the problem still exists.
After careful debugging and monitoring resources, the search algorithm for Marbles is making a HUGE overhead.
To solve that, I have to use another data structure to make the process much more quicker, by using KDTree, a data structure like the binary search tree that features the same time complexity, O(log(n)) which increase performance drastically.

### For Marbles
I used Object Pooling to reuse that huge amount of Marbles. I updated the code to suit the Object Pool. A believe I can achieve a better performance by implementing a custom Object Pool.

### Results
Using 10,000 Actors, 10,000 Marbles
![10K Bench](/benchmarks/bench%2010K.png)
Using 200,000 Actors, 200,000 Marbles
![200K Bench](/benchmarks/bench%20200K.png)

### Hardware
CPU: Intel Core i5 3100 3GHtz
GPU: Nvidia GTX 1050
