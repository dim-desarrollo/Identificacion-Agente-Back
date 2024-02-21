/*
    CREDENCIALES NECESARIAS
    - SonarQube     (Token de acceso)
    - Github        (Usuario y clave)
    - DockerHub     (Usuario y clave)
    - Kubernetes    (Token del service account de Jenkins)

    HERRAMIENTAS NECESARIAS
    - Java 20
    - Docker 24.0.2 (Cualquier versión que soporte --password-stdin)
    - Maven 3.9.3
    - NodeJS 16.20.1

    PLUGINS NECESARIOS
    - Slack
    - Docker
    - Kubernetes
    - JUnit
    - EmailText

    CONSIDERACIONES
    * Se debe configurar como multibranch pipeline
    * Deben existir las ramas master y develop
    * Se debe configurar la cloud de kubernetes para el acceso al servidor
 */
 
def ARTIFACT_ID
def IDENTIFICADOR_PROYECTO
def IDENTIFICADOR_UNICO_BUILD
def RAMA_PARA_CLONAR

pipeline {

agent any 

   tools {
        // Install the Maven version configured as "M3" and add it to the path.
        maven "maven3.8.8"
        jdk 'java20'
        dockerTool 'docker-2'
        nodejs 'node-js'
    }

    environment {

        HORA_DESPLIEGUE = sh(returnStdout: true, script: "date '+%A %W %Y %X'").trim()

        GITHUB_MONOLITO_URL = "https://github.com/dim-desarrollo/Identificacion-Agente-Back.git"

        GITHUB_CREDENCIALES = "github-test-1"
        GITHUB_CREDENCIALES_DEPLOY = "dim-desarrollo"

        SONARQUBE_CREDENCIALES = 'sonarqube'
        PUERTO_EXTERNO = 2020

        IDENTIFICADOR_UNICO_BUILD = 'backend-inspectores' //agregado a mano, ver si se hace de otra manera
        IDENTIFICADOR_IMAGEN = 'backend-inspectores'

        CANAL_SLACK = "#canal-slack"            // TODO: Por reemplazar
        CORREO_A_NOTIFICAR = "dim@gmail.com"    // TODO: Por reemplazar

        CARPETA_APLICACION = './'
        //CARPETA_DESPLIEGUE = 'despliegue'


    }


   stages {


    stage('Message start deploy dev') {

        steps {

            discordSend description: "Inicio de deploy en DEV!!!", footer: "Inicado", link: env.BUILD_URL, result: currentBuild.currentResult, title: "(DEV) Deploy backEnd-inspectores", webhookURL: "https://discord.com/api/webhooks/1173648912838561922/iB8YUryvKbcj66EWQa2e6161BDuygkfaMx57VUalxPnDAMvoRHcYKxJTaxV4nfBEdoxi"

        }

    }


    stage('Iniciando variables') {

        steps {

            dir("${CARPETA_APLICACION}"){

                script {

                    IDENTIFICADOR_UNICO_BUILD = "${IDENTIFICADOR_PROYECTO}.${BUILD_NUMBER}"

                }

            }


        }

    }

       /*

    stage('SonarQube Analysis') {

    environment {

        SONAR_SCANNER_HOME = tool 'sonarScaner' //nombre en la configuracion de las tools de jenkins 
        SONAR_SERVER = 'sonarqube' 
        SONAR_HOST_IP = '172.17.0.4' // IP interna de Docker de SonarQube, debido a que SonarQube corre en un contenedor (docker inspect nombre_contenedo_SonarQube)
        SONAR_PORT = '9000' //puerto donde esta trabajando el contenedor
        SONAR_SRC = 'src/'
        SONAR_ENCODING = 'UTF-8'

    }

   
    steps {

      
            
            dir("${CARPETA_APLICACION}"){
                withSonarQubeEnv(installationName: "${SONAR_SERVER}", credentialsId: "${SONARQUBE_CREDENCIALES}") {
                    sh "${SONAR_SCANNER_HOME}/bin/sonar-scanner \
                        -Dsonar.projectName=${ARTIFACT_ID} \
                        -Dsonar.projectVersion=${PROYECTO_VERSION} \
                        -Dsonar.projectKey=${IDENTIFICADOR_PROYECTO} \
                        -Dsonar.host.url=http://${SONAR_HOST_IP}:${SONAR_PORT} \
                        -Dsonar.sources=${SONAR_SRC} \
                        -Dsonar.java.binaries=. \
                        -Dsonar.sourceEncoding=${SONAR_ENCODING}"
                }
            }

       
c
        c}

     
    }  
            */

      stage('Tools initialization') {
          steps {
              script {

                    if (env.BRANCH_NAME){

                        RAMA_PARA_CLONAR = env.BRANCH_NAME

                    }
                    else{

                        RAMA_PARA_CLONAR = 'Dev'

                    }


                  DOCKER_VERSION = sh(returnStdout: true, script: 'docker version').trim()
                  NODE_VERSION = sh(returnStdout: true, script: 'npm -v').trim()

                  echo "Docker version: ${DOCKER_VERSION}"
                  echo "Node version: ${NODE_VERSION}"

              }
          }
      }

     stage('Build and run to Docker') {

         environment{
             PUERTO_INTERNO = 8080
             NOMBRE_CONTENEDOR = "backend-inspectores"
         }

        steps {
            script {

                dir ("${CARPETA_APLICACION}"){
                    // Verifica si existe un archivo Dockerfile en la subcarpeta actual
                    if (!fileExists("Dockerfile")) {
                        error "Dockerfile not found"
                    }

                    sh "docker build -t ${IDENTIFICADOR_IMAGEN} ."

                    //sh "docker run -d -p ${PUERTO_EXTERNO}:${PUERTO_INTERNO} --name ${NOMBRE_CONTENEDOR} ${IDENTIFICADOR_IMAGEN}"

                    sh "docker run -d -p ${PUERTO_EXTERNO}:${PUERTO_INTERNO} --network estaciones_my-network-inspector --name ${NOMBRE_CONTENEDOR} ${IDENTIFICADOR_IMAGEN} -e ASPNETCORE_ENVIRONMENT=Production" 
                }

            }
        }

     }

    stage('Message finish deploy') {


        steps {

            sh "echo ${HORA_DESPLIEGUE}"

            discordSend description: "(DEV) Deploy backEnd-inspectores echo!!!", footer: "Hora de inicio de despliegue: ${HORA_DESPLIEGUE} ", link: env.BUILD_URL, result: currentBuild.currentResult, title: "(DEV) Deploy front-ubicacion-padronesEstaciones", webhookURL: "https://discord.com/api/webhooks/1173648912838561922/iB8YUryvKbcj66EWQa2e6161BDuygkfaMx57VUalxPnDAMvoRHcYKxJTaxV4nfBEdoxi"

        }

    }

   }

 }
