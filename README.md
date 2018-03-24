# Unique VM Certificate Provisioning

## Introduction 

A Microsoft customer is implementing IPSec networking between all the VMs in a deployment. This project prepares the VMs in an Azure VM scale set for IPSec by installing signed digital certificates on each VM.

The scale set is defined and deployed via a Terraform template. The template defines a custom script extension that downloads and runs the necessary scripts to each VM. The script files are hosted as blobs in Azure Storage.

Certificates may be retrieved using one of two methods:

- Pre-created certificates are securely stored in Key Vault. Access to Key Vault is secured via Managed Service Identity (MSI).
- A certificate service creates certificates on demand. (Certificate signing can be delegated to an intermediate certificate authority (CA)).

## Components

- Python script that uploads the scripts to Azure
- Docker container to generate test certificates
- Docker container to test script download
- A certificate generation service to generate signed certificates on demand. (Written in .NET Core so that it runs on Windows and Linux.)
- Terraform template that provides the following:
    - Deploys the VM scale set
    - Specifies a custom script extension that downloads and execute the scripts on first boot
    - Configures MSI on the VM scale set. (Note: MSI for scale sets is not yet in the official terraform-azurerm repo. A binary version of terraform-provider-azurerm is included for use until the capability is more generally supported).
    - Creates a Key Vault access policy that gives each VM in the scale set permission to authenticate to Key Vault using MSI and retreive its certficate.

## Prerequisites

- [Docker CE](https://www.docker.com/get-docker)
- [Terraform](https://www.terraform.io/downloads.html)
- Python 2.7
- Azure subscription

## Config Setup

Config is located in ~/build/config.py. Config values must be updated with your Azure subscription information before building deploying the project.

### Azure subscription config

1. Create a new [Azure Service Principal]
    a. Go to Active Directory --> App Registrationsb. 
    b. Follow the "Create an Azure Active Directory application" on the second half of the following web page: (https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-create-service-principal-portal)
2. Create a new Azure KeyVault
    a. Add the Service Principal created in step 1 to the Key Vault's access policies
3. Open ~/scripts/config.py in an editor
4. Replace values of the config properties with the SPN APP ID and key secret you created above

### Storage account config

1. Create a new Azure storage account
2. Create a new SAS token for access to the storage account
3. Open ~/build/config.py in an editor
4. Replace values of storage account config properties with your storage account

### Copy scripts to storage account
Create an archive (.tar) file containing the scripts and upload it to Azure. Also uploads a bootstrap.sh script that is executed on first launch.

```
cd <root>
python ./build/build.py
```

### Terraform config

Update the variables.tf file with configuration info from your Azure subscription. 

## Build and Deploy Docker images to test the solution

1. Open a terminal / console

```
cd <root>
```
2. Build the docker image

```
docker-compose build
```

3. Update ~/scripts/vm-names.json with VM names you're going to provision

4. Create root\intermediate CAs & certs for all your VMs and upload them to Key Vault.

```
docker-compose up generator
```
5. Test that the secrets can be accessed via the VM scripts

```
docker-compose up worker
```

6. Initialize Terraform

```
cd terraform
terraform init
```

7. Deploy VMSS cluster with 5 VMs. They download scripts from blob storage & scripts will download certs from KV (pre-provisioned)

```
terraform apply
```

8. Debugging may require that you re-deploy the Azure infrastructure. You can do so using the following Terraform command:

```
terraform destroy
```

# Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.