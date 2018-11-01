#pragma once

#ifndef LCAAREA_H
#define LCAAREA_H

#include <QtCore/qstring.h>

class LCAArea
{
	public:

		LCAArea();
		LCAArea( const QString sWSId, const double xCord, const double yCord );


		double getXCord() const { return mXCord; }
		double getYCord() const { return mYCord; }

	protected:

		QString msWSId;
		double mXCord;
		double mYCord;
};

#endif