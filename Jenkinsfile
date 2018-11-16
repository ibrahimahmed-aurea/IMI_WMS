pipeline {
	agent any
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
					bat 'call dotnet/build.bat > build-result.txt'
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
	post {
        always { 
            echo 'I will always say Hello again!'
            	emailext attachmentsPattern: '**/build-result.txt',
				recipientProviders: [[$class: 'DevelopersRecipientProvider'], [$class: 'RequesterRecipientProvider']],
				body: "${currentBuild.currentResult}: Job ${env.JOB_NAME} build ${env.BUILD_NUMBER}\n More info at: ${env.BUILD_URL}",
				subject: "Jenkins Build ${currentBuild.currentResult}: Job ${env.JOB_NAME}"
        }
    }	
}