#!groovy
properties([[$class: 'GitLabConnectionProperty', gitLabConnection: 'bincentive-gitlab']])
node {
try {

    // Configuration variables:
    def globalConfig = null
    configFileProvider([configFile(fileId: 'ci-global-config', variable: 'CI_GLOBAL_CONFIG_FILE')]) {
        globalConfig = readYaml(file: CI_GLOBAL_CONFIG_FILE)
    }
    def DOCKER_IMAGE_NAME = 'bi-app-net-scanner'
    def ECR_IMAGE_NAME = "${globalConfig.aws.ecr.server}/${DOCKER_IMAGE_NAME}"
    def ECR_IMAGE_TAG = null
    def WORKSPACE = pwd()

    stage('Clean Workspace') {
        cleanWs(cleanWhenAborted: true, cleanWhenFailure: true, cleanWhenNotBuilt: true, cleanWhenSuccess: true, cleanWhenUnstable: true, deleteDirs: true)
    }

    // Checkout stage
    // The checkout stage should not be included in the gitlabCommitStatus block
    // See: https://github.com/jenkinsci/gitlab-plugin#scripted-pipeline-jobs
    stage('Checkout') {
        checkout scm
    }
    gitlabCommitStatus {
        docker.withServer(globalConfig.docker.host) {
            stage('Validate') {
                PACKAGE_VERSION = readJSON(file: 'scanner/api-scanner/appsettings.json').version
                if(!(PACKAGE_VERSION ==~ /\d+\.\d+\.\d+/)) {
                    error "Package version ${PACKAGE_VERSION} is not a valid version number. (Expect x.y.z)"
                }

                if (env.gitlabTargetBranch == 'stg') {
                    VersionValidate(globalConfig, 'bi-app-net-scanner-stg', PACKAGE_VERSION)
                }

                if (env.gitlabActionType == 'PUSH') {
                    // trigger by PUSH (Accept Merge)
                    if (env.gitlabBranch == 'master' || env.gitlabBranch == 'stg') {
                        ECR_IMAGE_TAG =  "${ECR_IMAGE_NAME}:${PACKAGE_VERSION}"
                    } else if (env.gitlabBranch == 'develop') {
                        ECR_IMAGE_TAG =  "${ECR_IMAGE_NAME}:${PACKAGE_VERSION}-sit"
                    }
                } else {
                    // trigger by Merge Request
                    if(env.gitlabTargetBranch == 'master') {
                        if (env.gitlabSourceBranch != 'stg') {
                            error "master branch only accept Merge Request from stg branch"
                        }
                    }
                }
            }
            if (env.gitlabTargetBranch == 'stg' || env.gitlabTargetBranch == 'develop') {
                stage('Test') {
                    docker.image("mcr.microsoft.com/dotnet/core/sdk:3.0-buster").inside("-u root") {
                    }
                }
                if (env.gitlabActionType == 'MERGE') {
                    stage('TestBuild') {
                        docker.image("mcr.microsoft.com/dotnet/core/sdk:3.0-buster").inside("-u root") {
                        }
                    }
                }
            }

            if(env.gitlabActionType == 'PUSH') {
                if (env.gitlabBranch == 'stg' || env.gitlabBranch == 'develop') {
                    stage('Build') {
                        docker.image("mcr.microsoft.com/dotnet/core/sdk:3.0-buster").inside("-u root") {
                            sh "dotnet publish scanner/api-scanner/api-scanner.csproj -c Release -r linux-x64 --self-contained true"
                        }
                    }
                    stage('Deploy Image') {
                        docker.withRegistry("https://${globalConfig.aws.ecr.server}", globalConfig.aws.ecr.credsId) {
                            def image = docker.build(ECR_IMAGE_TAG, '-f Dockerfile .')
                            image.push()
                        }
                    }
                }
                if (env.gitlabBranch == 'master') {
                    stage('Rollout') {
                        AWSrolloutManual(globalConfig, 'aws-tasks/task-def-prod.template.json', ECR_IMAGE_TAG, 'api-prod-cluster', 'bi-app-net-scanner-prod', PACKAGE_VERSION)
                    }
                } else if (env.gitlabBranch == 'stg') {
                    stage('Rollout') {
                        AWSrolloutAuto(globalConfig, 'aws-tasks/task-def-stg.template.json', ECR_IMAGE_TAG, 'api-stg-cluster', 'bi-app-net-scanner-stg')
                    }
                } else if (env.gitlabBranch == 'develop') {
                    stage('Rollout') {
                        AWSrolloutAuto(globalConfig, 'aws-tasks/task-def-sit.template.json', ECR_IMAGE_TAG, 'api-sit-cluster', 'bi-app-net-scanner-sit')
                    }
                }
            }
        }
    }
} catch (e) {
 currentBuild.result = "FAILED"
 notifyFailed()
 throw e
}
}

def VersionValidate(globalConfig, TaskDefValidate, PACKAGE_VERSION) {
    docker.withRegistry("https://${globalConfig.aws.ecr.server}", globalConfig.aws.ecr.credsId) {
        withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: globalConfig.aws.user.credsId]]) {
            docker.image("${globalConfig.aws.ecr.server}/docker-awscli:1.16.9").inside("-e AWS_ACCESS_KEY_ID=${AWS_ACCESS_KEY_ID} -e AWS_SECRET_ACCESS_KEY=${AWS_SECRET_ACCESS_KEY} -e AWS_DEFAULT_REGION=${globalConfig.aws.region}") {
                def taskDefOutput = sh (script: "aws ecs describe-task-definition --task-definition ${TaskDefValidate}", returnStdout: true).trim()
                taskDef = readJSON(text: taskDefOutput)
            }
        }
    }
    docker.image("marcelocorreia/semver:5.5.0").inside("-e PACKAGE_VERSION=${PACKAGE_VERSION}") {
        def versionOutput = sh (script:"echo ${taskDef['taskDefinition']['containerDefinitions'][0]['image']} | cut -d\':\' -s -f2", returnStdout: true).trim()
        def updateMessage = "You need to update application version. It should be greater than latest version."
        sh "semver \"${PACKAGE_VERSION}\" -r \">${versionOutput}\" || (echo \"--------${updateMessage}--------\" && false)"
    }
}

def AWSrolloutManual(globalConfig, ReadTaskFile, ECRimage, AWScluster, AWSservice, PACKAGE_VERSION) {
    def taskDef = readJSON(file: ReadTaskFile)
    taskDef['containerDefinitions'][0]['image'] = ECRimage.toString()

    writeJSON(file: "${ReadTaskFile}.deploy", json: taskDef, pretty: 4)

    docker.withRegistry("https://${globalConfig.aws.ecr.server}", globalConfig.aws.ecr.credsId) {
        withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: globalConfig.aws.user.credsId]]) {
            docker.image("${globalConfig.aws.ecr.server}/docker-awscli:1.16.9").inside("-e AWS_ACCESS_KEY_ID=${AWS_ACCESS_KEY_ID} -e AWS_SECRET_ACCESS_KEY=${AWS_SECRET_ACCESS_KEY} -e AWS_DEFAULT_REGION=${globalConfig.aws.region}") {
                def taskDefOutput = sh (script: "aws ecs register-task-definition --cli-input-json file://${ReadTaskFile}.deploy", returnStdout: true).trim()
                taskDef = readJSON(text: taskDefOutput)

                withCredentials([string(credentialsId: 'Discord-token-deploy-prod', variable: 'DISCORD_AUTH_DEPLOY_PROD')]) {
                    sh "curl -X POST -d \'{\"content\":\"__**${taskDef['taskDefinition']['family']}**__ is ready to deploy~~!! :wink: \\nversion=${PACKAGE_VERSION} \\nhttps://ci.bincentive.com/job/infra/job/ecs-deploy/buildWithParameters?token=ez1234&configId=ci-global-config&cluster=${AWScluster}&service=${AWSservice}&PackageVersion=${PACKAGE_VERSION}&taskDefFamily=${taskDef['taskDefinition']['family']}&taskDefRevision=${taskDef['taskDefinition']['revision']}\"}\' https://discordapp.com/api/webhooks/557124636543025174/${DISCORD_AUTH_DEPLOY_PROD}"
                }
            }
        }
    }
}

def AWSrolloutAuto(globalConfig, ReadTaskFile, ECRimage, AWScluster, AWSservice) {
    def taskDef = readJSON(file: ReadTaskFile)
    taskDef['containerDefinitions'][0]['image'] = ECRimage.toString()

    writeJSON(file: "${ReadTaskFile}.deploy", json: taskDef, pretty: 4)

    docker.withRegistry("https://${globalConfig.aws.ecr.server}", globalConfig.aws.ecr.credsId) {
      withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: globalConfig.aws.user.credsId]]) {
        docker.image("${globalConfig.aws.ecr.server}/docker-awscli:1.16.9").inside("-e AWS_ACCESS_KEY_ID=${AWS_ACCESS_KEY_ID} -e AWS_SECRET_ACCESS_KEY=${AWS_SECRET_ACCESS_KEY} -e AWS_DEFAULT_REGION=${globalConfig.aws.region}") {
            def taskDefOutput = sh (script: "aws ecs register-task-definition --cli-input-json file://${ReadTaskFile}.deploy", returnStdout: true).trim()
            taskDef = readJSON(text: taskDefOutput)
            sh "aws ecs update-service --force-new-deployment --cluster ${AWScluster} --service ${AWSservice} --task-definition ${taskDef['taskDefinition']['family']}:${taskDef['taskDefinition']['revision']}"
            }
        }
    }
}

def notifyFailed() {
    def DISCORD_CH_ID = '534295017712254976'
    withCredentials([string(credentialsId: 'Discord-token', variable: 'DISCORD_AUTH')]) {
        sh "curl -X POST -d \'{\"content\":\"Build-Status: FAILURE\\nUserName: ${env.gitlabUserName}\\nJobName: ${env.JOB_NAME}\"}\' https://discordapp.com/api/webhooks/${DISCORD_CH_ID}/${DISCORD_AUTH}"
    }
    emailext (
        subject: "FAILED: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]'",
        body: """<p>FAILED: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]':</p><p>Check console output at "<a href="${env.BUILD_URL}">${env.JOB_NAME} [${env.BUILD_NUMBER}]</a>"</p>""",
        attachLog: true,
        to: "${env.gitlabUserEmail}"
    )
}
