# BlockStrategy1.0
This is a simple attempt of mine to create a RTS game. After a few unfinished Unity projects I wanted to prove to myself, that I can develop a game until it reaches a playable state. In that regard this project was a success.
 
## What can you do in the game?
The game has fixed map with two bases located on it. One is yours and one belongs the AI/enemy. Also there are 5 resource points to capture. Capturing those points will generate you gold. And gold can be used to build three different units, a soldier, an archer and a lancer, which have different stats. The goal of the game is to destroy the enemy base. Since its a really simplistic concept a round usually will only take around 5 minutes. 

## The current state of the game
As of writing this the code side of things is done. There may be some bugs and quality of live improvements that I didn't discover/thought of so far. On the small chance someone will ever play this game, you are free to open an issue and I may fix stuff.
There are still issues open for visuals and sound. But currently I am not in a position to introduce those two. Im simply no modeler or sound designer. So those issue will be left open until I feel capable of tackling them.

## Some data
Here is some data for a look behind the stats that or not shown inside the game.

### Damage calculation
Damage calculation is done by this formula (attack means the attack stat of the attacker, while the armor is the stat of the defender):
damage = (attack/armor) * 10

### Unit stats
<table> 
<tr> <th></th> <th>attack</th> <th>armor</th> <th>range</th> <th>health</th><th>attackspeed</th><th>Movement speed</th></tr>
 <tr> <th>Soldier</th> <td>20</td> <td>10</td> <td>1</td> <td>100</td><td>1</td> <td>5</td></tr>
 <tr> <th>Archer</th> <td>15</td> <td>5</td> <td>20</td> <td>50</td><td>1,5</td> <td>3.5</td></tr>
 <tr> <th>Lancer</th> <td>10</td> <td>20</td> <td>5</td> <td>150</td><td>1</td> <td>4</td></tr>
 <tr> <th>Base</th> <td>(50)</td> <td>10</td> <td>(20)</td> <td>5000</td><td>1</td> <td>-</td></tr>
</table>

### Resource points
The resource points in the corner of the map generate 5 gold/second and the one in the middle generates 10 gold/second.
