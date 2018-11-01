<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:import href="common.xslt"/>
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="param">
		<xsl:apply-imports/>
		<xsl:choose>
			<xsl:when test="@index='3'">
				<LocationId datatype="string">
					<xsl:value-of select="."/>
				</LocationId>
			</xsl:when>
			<xsl:when test="@index='4'">
				<ItemNumber datatype="string">
					<xsl:value-of select="."/>
				</ItemNumber>
			</xsl:when>
			<xsl:when test="@index='5'">
				<Quantity datatype="double">
					<xsl:value-of select="."/>
				</Quantity>
			</xsl:when>
			<xsl:when test="@index='6'">
				<UnitOfMeasure datatype="string">
					<xsl:value-of select="."/>
				</UnitOfMeasure>
			</xsl:when>
			<xsl:when test="@index='7'">
				<SkippedSlot datatype="string">
					<xsl:value-of select="."/>
				</SkippedSlot>
			</xsl:when>
		</xsl:choose>
	</xsl:template>	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
</xsl:stylesheet>
