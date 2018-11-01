<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:import href="common.xslt"/>
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="param">
		<xsl:apply-imports/>
		<xsl:choose>
			<xsl:when test="@index='3'">
				<BreakType datatype="string">
					<xsl:value-of select="."/>
				</BreakType>
			</xsl:when>
			<xsl:when test="@index='4'">
				<StartEndFlag datatype="string">
					<xsl:value-of select="."/>
				</StartEndFlag>
			</xsl:when>
			<xsl:when test="@index='5'">
				<BreakDescription datatype="string">
					<xsl:value-of select="."/>
				</BreakDescription>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
