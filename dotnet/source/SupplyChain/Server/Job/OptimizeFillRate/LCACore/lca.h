#pragma once

#ifndef LCA_H
#define LCA_H

#include "lcapickbatchline.h"
#include "lcawarehouse.h"

// Twn: dit moet er nog uit
#include <stdio.h>

class LCAPickBatch;
class LCAParameters;
class LCAResult;

/* This class is the main class for the load carrier algorithm. It performs all steps
 * of the load carrier algorithm
 */
class LCA
{
	public:
	
		LCA()
		{
			//mpAnnealFile = 0;
		}

		~LCA()
		{
			//if ( mpAnnealFile ) fclose( mpAnnealFile );
		}

		int setWarehouse( LCAWarehouse warehouse, std::map<int, QStringList>& rsErrorMessages );
		LCAResult* processPickBatch( const LCAPickBatch *pPickbatch, const LCAParameters *pParameters );

		//void setAnnealFile( QString sFile ) 
		//{ 
			//if ( mpAnnealFile ) fclose (mpAnnealFile );
			//mpAnnealFile = fopen( sFile.toAscii().data(), "w" );
		//}


	protected:

		LCAResult* checkPickBatchAndParameters( const LCAPickBatch *pPickbatch, const LCAParameters *pParameters );
		LCAResult* processPickBatchAptean( const LCAPickBatch *pPickbatch, const LCAParameters *pParameters );
		LCAResult* processPickBatchAlternating( const LCAPickBatch *pPickbatch, const LCAParameters *pParameters );
		LCAResult* processPickBatchAisleDensity( const LCAPickBatch *pPickbatch, const LCAParameters *pParameters );

		LCAResult* processSortedVector( const LCAPickBatch *pPickbatch, 
										const LCAPickBatchLineVector& rVector, 
										const LCAParameters *pParameters );

		void createAlternatingVector( const LCAPickBatchLineVector& rSourcePickLines, 
			                          LCAPickBatchLineVector& rcAlternatingPickLines, 
									  const double densitySwitch );

		int calculateLowerBound( const LCAPickBatch *pPickbatch, const LCAParameters *pParameters );
		LCAResult* determineBest( LCAResult* pOption1, LCAResult* pOption2, const LCAParameters* pParameters );

		LCAWarehouse mLCAWarehouse;

		// FILE* mpAnnealFile;
};

#endif