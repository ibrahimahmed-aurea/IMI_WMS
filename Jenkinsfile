pipeline {
 stages {
	   stage('Preparation') {
		steps {
			  echo 'Pulling code from repo...'
			  git 'https://github.com/ibrahimahmed-aurea/IMI_WMS.git'
			  echo 'Code fetched.'
		  }
	   }
	   
	   stage('Build') {
			steps {
				echo 'Start building all projects...'
				bat 'call dotnet/build.bat'
				echo 'Build finished...'
			}
	   }
	   
	   stage('Unit Tests'){
			steps {
				echo 'Testing...'
				bat returnStatus: true, script: 'call dotnet/test.bat'
				echo 'Tests passed!'
			}
	   }
	   
	   stage('Publish Unit Tests Report'){
			steps {
				echo 'Displaying the unit tests result'
				nunit testResultsPattern: 'TestResult.xml'
			}
	   }
	}	
}