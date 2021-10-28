$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

az account set --subscription e2e5ecc4-5bf0-4dc0-9d85-fdbcaebd8740
az acr login --name dunphyacrtest3                      
docker build -t servercore/customsearchesworker-test:1.10 "$($scriptPath)\..\..\Docker" 
docker tag servercore/customsearchesworker-test:1.10 dunphyacrtest3.azurecr.io/servercore/customsearchesworker-test:1.10
docker push dunphyacrtest3.azurecr.io/servercore/customsearchesworker-test:1.10

kubectl apply -f .\Namespace.yaml --namespace ptas-test
kubectl apply -f .\StorageSecret.yaml --namespace ptas-test
kubectl apply -f .\AzureIdentity.yaml --namespace ptas-test
kubectl apply -f .\AzureIdentityBinding.yaml --namespace ptas-test
kubectl apply -f .\CustomSearchesWorker.yaml --namespace ptas-test
#kubectl apply -f .\MapServerService.yaml --namespace ptas-test
#kubectl apply -f .\MapServerRestartJob.yaml --namespace ptas-test
kubectl apply -f .\CustomSearchesJob.yaml --namespace ptas-test