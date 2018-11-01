<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:import href="common.xslt"/>
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="param">
		<xsl:apply-imports/>
		<xsl:choose>
			<xsl:when test="@index='3'">
				<SpokenOrScannedIndicator datatype="string">
					<xsl:value-of select="."/>
				</SpokenOrScannedIndicator>
			</xsl:when>
			<xsl:when test="@index='4'">
				<LocationId datatype="string">
					<xsl:value-of select="."/>
				</LocationId>
			</xsl:when>
			<xsl:when test="@index='5'">
				<LocationCheckDigit datatype="string">
					<xsl:value-of select="."/>
				</LocationCheckDigit>
			</xsl:when>
		</xsl:choose>
	</xsl:template>	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
</xsl:stylesheet>
