node {
   stage('Preparation') {
      echo 'Pulling code from repo...'
      git 'https://github.com/ibrahimahmed-aurea/IMI_WMS.git'
      echo 'Code fetched.'
   }
   
   stage('Build') {
    //   echo 'Building...'
    //   bat 'npm install'
    //   echo "Killing already running node processes"
    //   bat '''tasklist /fi "imagename eq node.exe" |find ":" > nul
    //   if errorlevel 1 taskkill /f /im "node.exe"'''
    //   bat "npm run prod"
    //  bat 'D:/JenkinsServer/scripts/Demo/Demo.bat'
    //   echo 'Build deployed!'
    bat returnStatus: true, script: 'call buil.bat'
   }
   
   stage('Deployment') {
       bat '''@echo off
                echo *************************************
                echo 	Demo Build Started
                echo 	@author: Ayushya
                echo *************************************
                echo Trying to kill all node processes.
               // taskkill /f /im node.exe
               // echo Running the application...
               // CALL start echo HELLO WORLD....
                echo Success...'''
   }
   
   stage('Automation Testing'){
        echo 'Testing...'
        echo 'Tests passed!'
   }
}