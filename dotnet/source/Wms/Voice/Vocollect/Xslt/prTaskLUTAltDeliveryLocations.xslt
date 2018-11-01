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
				<ContainerId datatype="string">
					<xsl:value-of select="."/>
				</ContainerId>
			</xsl:when>
			<xsl:when test="@index='6'">
				<AlternateDeliveryLocation datatype="string">
					<xsl:value-of select="."/>
				</AlternateDeliveryLocation>
			</xsl:when>
      <xsl:when test="@index='7'">
        <AlternateDeliveryArea datatype="string">
          <xsl:value-of select="."/>
        </AlternateDeliveryArea>  
      </xsl:when>                
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
