pipeline {
  agent { 
    label 'win-slave-node'
  }
  stages {
	stage('Checkout the project') {
		git 'https://github.com/ibrahimahmed-aurea/IMI_WMS.git'
	}
    stage('Build') {
      steps {
        script {
          def msbuild = tool name: 'MSBuild', type: 'hudson.plugins.msbuild.MsBuildInstallation'
          bat "${msbuild} /p:Configuration=Release /p:Platform=\"Any CPU\" /v:m Test_Jenkins/Test_Jenkins.sln"
        } 
      } 
    } 
  } 
} 