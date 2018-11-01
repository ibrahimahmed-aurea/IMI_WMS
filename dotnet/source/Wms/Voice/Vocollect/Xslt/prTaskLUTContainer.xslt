<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:import href="common.xslt"/>
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="param">
		<xsl:apply-imports/>
		<xsl:choose>
			<xsl:when test="@index='3'">
				<AssignmentId datatype="string">
					<xsl:value-of select="."/>
				</AssignmentId>
			</xsl:when>
			<xsl:when test="@index='4'">
				<WorkId datatype="string">
					<xsl:value-of select="."/>
				</WorkId>
			</xsl:when>
			<xsl:when test="@index='5'">
				<TargetContainer datatype="integer">
					<xsl:value-of select="."/>
				</TargetContainer>
			</xsl:when>
			<xsl:when test="@index='6'">
				<SystemGeneratedContainerID datatype="string">
					<xsl:value-of select="."/>
				</SystemGeneratedContainerID>
			</xsl:when>
			<xsl:when test="@index='7'">
				<OperatorSpecifiedContainerID datatype="string">
					<xsl:value-of select="."/>
				</OperatorSpecifiedContainerID>
			</xsl:when>
			<xsl:when test="@index='8'">
				<Operation datatype="integer">
					<xsl:value-of select="."/>
				</Operation>
			</xsl:when>
			<xsl:when test="@index='9'">
				<NoOfLabels datatype="integer">
					<xsl:value-of select="."/>
				</NoOfLabels>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
