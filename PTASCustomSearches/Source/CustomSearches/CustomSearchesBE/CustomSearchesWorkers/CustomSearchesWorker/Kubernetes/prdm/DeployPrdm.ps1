$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

az account set --subscription e2e5ecc4-5bf0-4dc0-9d85-fdbcaebd8740
az acr login --name dunphyacrtest3                      
docker build -t servercore/customsearchesworker-prdm:1.15 "$($scriptPath)\..\..\Docker" 
docker tag servercore/customsearchesworker-prdm:1.15 dunphyacrtest3.azurecr.io/servercore/customsearchesworker-prdm:1.15
docker push dunphyacrtest3.azurecr.io/servercore/customsearchesworker-prdm:1.15

kubectl apply -f .\Namespace.yaml --namespace ptas-prdm
kubectl apply -f .\StorageSecret.yaml --namespace ptas-prdm
kubectl apply -f .\AzureIdentity.yaml --namespace ptas-prdm
kubectl apply -f .\AzureIdentityBinding.yaml --namespace ptas-prdm
kubectl apply -f .\CustomSearchesWorker.yaml --namespace ptas-prdm
kubectl apply -f .\MapServerService.yaml --namespace ptas-prdm
kubectl apply -f .\MapServerRestartJob.yaml --namespace ptas-prdm