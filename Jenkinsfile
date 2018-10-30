node
{
	stage 'Checkout'
		git 'https://github.com/ibrahimahmed-aurea/IMI_WMS.git'
	stage 'Build Project'
		bat 'set MsBuild=C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe'
		bat '%MsBuild%  /p:Configuration=Release /p:Platform=\"Any CPU\" /v:m Test_Jenkins/Test_Jenkins.sln"'
		

}
