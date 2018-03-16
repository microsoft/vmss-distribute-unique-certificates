# Unique VM Certificate Provisioning

## Introduction 

TODO: Describe project 

## Prerequisites

- [Docker CE](https://www.docker.com/get-docker)
- [Terraform](https://www.terraform.io/downloads.html)
- Python 2.7
- Azure subscription

## Config Setup

Config is located in ~/build/config.py. Config values must be updated with your Azure subscription information before building deploying the project.

### Azure subscription config

1. Create a new [Azure Service Principal](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-create-service-principal-portal)
2. Create a new Azure KeyVault
3. Open ~/build/config.py in an editor
4. Replace values of the config properties with the Azure resource values you created above

### Storage account config

1. Create a new Azure storage account
2. Create a new SAS token for access to the storage account
3. Open ~/scripts/config.py in an editor
4. Replace values of storage account config properties with your storage account

### Terraform config

TODO: 

## Build and Deploy

1. Open a terminal / console

```
cd <root>
```
2. Build the docker image

```
Docker-compose build
```

3. Update ~/scripts/vm-names.json with VM names you're going to provision
4. Create root\intermediate CAs & certs for all your VMs & uploads it to KV

```
Docker-compose up generator
```

5. Initialize Terraform

```
cd terraform
terraform init
```

7. Deploy VMSS cluster with 5 VMs. They download scripts from blob storage & scripts will download certs from KV (pre-provisioned)

```
terraform apply
```