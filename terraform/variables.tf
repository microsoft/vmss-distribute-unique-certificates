# Kay Vault Variables
variable "key_vault_name" {
  default = "bacKV"
}

variable "key_vault_subscription_id" {
  default = "71e74b5f-5ba1-4bfc-be64-7a1ed30ccb26"
}

variable "key_vault_client_id" {
  default = "8e20cb41-7be7-4491-bcdf-ec69b9923cd0"
}

variable "key_vault_client_secret" {
  default = "GMRZ8IZFqnX00wVUsk9hoiGRirLF/sm9U+GiYhsu4YY="
}

variable "key_vault_tenant_id" {
  description = "Active Directory 'Directory ID' property."
  default = "72f988bf-86f1-41af-91ab-2d7cd011db47"
}

variable "key_vault_resource_group_name" {
  description = "The name of the resource group in which to create the virtual network."
  default = "bac-certificates-rg"
}

variable "key_vault_location" {
  description = "The location/region where the virtual network is created. Changing this forces a new resource to be created."
  default     = "west us 2"
}

# Scale Set Variables
variable "subscription_id" {
  default = "71e74b5f-5ba1-4bfc-be64-7a1ed30ccb26"
}

variable "client_id" {
  default = "8e20cb41-7be7-4491-bcdf-ec69b9923cd0"
}

variable "client_secret" {
  default = "GMRZ8IZFqnX00wVUsk9hoiGRirLF/sm9U+GiYhsu4YY="
}

variable "tenant_id" {
  description = "Active Directory 'Directory ID' property."
  default = "72f988bf-86f1-41af-91ab-2d7cd011db47"
}

variable "resource_group_name" {
  description = "The name of the resource group in which to create the virtual network."
  default = "vmsscertificates"
}

variable "location" {
  description = "The location/region where the virtual network is created. Changing this forces a new resource to be created."
  default     = "west us 2"
}

variable "storage_account_tier" {
  description = "Defines the Tier of storage account to be created. Valid options are Standard and Premium."
  default     = "Standard"
}

variable "storage_replication_type" {
  description = "Defines the Replication Type to use for this storage account. Valid options include LRS, GRS etc."
  default     = "LRS"
}

variable "vmss_prefix" {
  default = "vmss-certs"
}

variable "vm_sku" {
  default = "Standard_A1"
}

variable "sku" {
  default = "7.3"
}

variable "image_publisher" {
  description = "The name of the publisher of the image (az vm image list)"
  default     = "OpenLogic"
}

variable "image_offer" {
  description = "The name of the offer (az vm image list)"
  default     = "CentOS"
}

variable "instance_count" {
  description = "Number of VM instances (100 or less)."
  default     = "5"
}

variable "admin_username" {
  default = "jeff"
}

variable "admin_password" {
  default = "W00fdawg!!"
}

variable "command" {
  default = "bash bootstrap.sh"
}

variable "file1" {
  default = "https://baccertificates.blob.core.windows.net/vm-scripts-files/bootstrap.sh"
}

variable "file2" {
  default = "https://baccertificates.blob.core.windows.net/vm-scripts-files/vm_scripts.tar.gz"
}