<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="message">
		<xsl:element name="{@type}" namespace="http://www.im.se/wms/voice/vocollect">
			<xsl:apply-templates select="params/param"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="param">
		<xsl:choose>
			<xsl:when test="@index='0'">
				<TimeStamp datatype="dateTime">
					<xsl:value-of select="."/>
				</TimeStamp>
			</xsl:when>
			<xsl:when test="@index='1'">
				<SerialNumber datatype="string">
					<xsl:value-of select="."/>
				</SerialNumber>
			</xsl:when>
			<xsl:when test="@index='2'">
				<Operator datatype="string">
					<xsl:value-of select="."/>
				</Operator>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
