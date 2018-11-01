<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:import href="common.xslt"/>
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="param">
		<xsl:apply-imports/>
		<xsl:choose>
			<xsl:when test="@index='3'">
				<MaxNumberOfAssignments datatype="integer">
					<xsl:value-of select="."/>
				</MaxNumberOfAssignments>
			</xsl:when>
		</xsl:choose>
	</xsl:template>	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
</xsl:stylesheet>
