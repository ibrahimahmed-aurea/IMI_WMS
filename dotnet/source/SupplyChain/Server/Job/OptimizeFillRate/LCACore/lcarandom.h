#pragma once

#ifndef LCARANDOM_H
#define LCARANDOM_H

class LCARandom
{
	public:

		LCARandom(long seed = 314159);

		void  initSeed(long seed);
		float draw();
		long  draw(long lowerBound, long upperBound);

	private:

		long mZ;

};


#endif