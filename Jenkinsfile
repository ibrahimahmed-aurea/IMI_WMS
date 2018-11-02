node {
   stage('Preparation') {
      echo 'Pulling code from repo...'
      git 'https://github.com/ibrahimahmed-aurea/IMI_WMS.git'
      echo 'Code fetched.'
   }
   
   stage('Build') {
    echo 'Start building all projects...'
    bat returnStatus: true, script: 'call dotnet/build.bat'
	error("Build failed....")
	echo 'Build finished...'
   }
   
   stage('Unit Tests'){
        echo 'Testing...'
		bat returnStatus: true, script: 'call dotnet/test.bat'
        echo 'Tests passed!'
   }
   
   stage('Publish Unit Tests Report'){
		echo 'Displaying the unit tests result'
		nunit testResultsPattern: 'TestResult.xml'
   }
}