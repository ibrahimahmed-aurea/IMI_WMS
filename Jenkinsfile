node {
   stage('Preparation') {
      echo 'Pulling code from repo...'
      git 'https://github.com/ibrahimahmed-aurea/IMI_WMS.git'
      echo 'Code fetched.'
   }
   
   stage('Build') {
    echo 'Start building all projects...'
    bat returnStatus: true, script: 'call buil.bat'
	echo 'Build finished...'
   }
   
   stage('Deployment') {
                echo Running the application...
   }
   
   stage('Automation Testing'){
        echo 'Testing...'
		bat returnStatus: true, script: 'call dotnet/test.bat'
        echo 'Tests passed!'
   }
}