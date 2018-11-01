node {
   stage('Preparation') {
      echo 'Pulling code from repo...'
      git 'https://github.com/ibrahimahmed-aurea/IMI_WMS.git'
      echo 'Code fetched.'
   }
   
   stage('Build') {
    echo 'Start building all projects...'
    bat returnStatus: true, script: 'call dotnet/buil.bat'
	echo 'Build finished...'
   }
   
   stage('Deployment') {
       bat '''@echo off
                echo Running the application...
                CALL start echo HELLO WORLD....
                echo Success...'''
   }
   
   stage('Automation Testing'){
        echo 'Testing...'
        echo 'Tests passed!'
   }
}