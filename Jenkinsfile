node
{
	stage 'Checkout'
		git 'https://github.com/ibrahimahmed-aurea/IMI_WMS.git'
	stage 'Build Project'
		bat "\"${tool 'MSBuild'}\" /p:Configuration=Release /p:Platform=\"Any CPU\" /v:m Test_Jenkins/Test_Jenkins.sln"

}