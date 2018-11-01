<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:import href="common.xslt"/>
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="param">
		<xsl:apply-imports/>
		<xsl:choose>
			<xsl:when test="@index='3'">
				<LotId datatype="string">
					<xsl:value-of select="."/>
				</LotId>
			</xsl:when>
			<xsl:when test="@index='4'">
				<Qty datatype="integer">
					<xsl:value-of select="."/>
				</Qty>
			</xsl:when>			
			<xsl:when test="@index='5'">
				<WorkId datatype="string">
					<xsl:value-of select="."/>
				</WorkId>
			</xsl:when>			
			<xsl:when test="@index='6'">
				<Sequence datatype="integer">
					<xsl:value-of select="."/>
				</Sequence>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
