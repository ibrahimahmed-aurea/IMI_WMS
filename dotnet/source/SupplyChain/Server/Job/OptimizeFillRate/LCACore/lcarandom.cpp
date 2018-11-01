#include "lcarandom.h"
#include <math.h>

LCARandom::LCARandom(long seed)
{
	initSeed(seed);

} 

// Sets a seed for the random generator
void LCARandom::initSeed(long seed)
{
	mZ = seed;

} 

// Draws a "random" real value v where 0 < v < 1, from a more or less uniform distribution
float LCARandom::draw()
{
	static long a = 16807;
	static long m = 2147483647; 

	long q = m / a;
	long r = m % a;
	long gamma = a * (mZ % q) - r * (mZ / q);
	
	mZ = gamma + (gamma > 0 ? 0 : m);

	return mZ / (float) m;

} 

// Draws a number with lb <= number <= ub
long LCARandom::draw(long lb, long ub)
{
	/*    0 < LCARandom::draw() < 1
	 *
	 * == { under the assumption that [lb] is at most [ub] }
	 *
	 *    0 < LCARandom::draw() * (ub - lb + 1) < ub - lb + 1 
	 *
	 * == { ... }
	 *
	 *    lb < lb + LCARandom::draw() * (ub - lb + 1) < ub + 1
	 *
	 * However, the above implementation would draw [lb] with a slightly 
	 * smaller probability than the other values. 
	 *
	 * The following implementation seems to work better in practice, although
	 * it suffers from the same problem. Why does it (seem to) work better ? */

	double inputFloor = 0.5 + draw() * (ub - lb);
	return lb + (long) floor (inputFloor);

} 
