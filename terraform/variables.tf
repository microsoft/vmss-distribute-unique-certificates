###############################################################################
# Shared Variables
###############################################################################

# Add this application to the subscriptions' IAM with permission to create resources
# NOTE: Add the app to BOTH the scale set's and key vault's subscription
# (if they are different)
#
# Use this same value as the client_id in scripts/config.py (used by cert Generator)
variable "client_id" {
  description = "Application ID value from an Active Directory App Registration."
  default = "8e20cb41-7be7-4491-bcdf-ec69b9923cd0"
}

variable "client_secret" {
  description = "Secret value obtained by creating a key for the App Registration."
  default = "GMRZ8IZFqnX00wVUsk9hoiGRirLF/sm9U+GiYhsu4YY="
}

variable "tenant_id" {
  description = "Active Directory 'Directory ID' property."
  default = "72f988bf-86f1-41af-91ab-2d7cd011db47"
}

###############################################################################
# Key Vault Variables
###############################################################################
variable "key_vault_subscription_id" {
  default = "b9039980-7a20-4eb7-bd14-2f03195ebdc1"
}

variable "key_vault_resource_group_name" {
  description = "The name of the resource group in which to create the virtual network."
  default = "bac-certificates"
}

variable "key_vault_location" {
  description = "The location/region where the virtual network is created. Changing this forces a new resource to be created."
  default     = "west us 2"
}

variable "key_vault_name" {
  default = "backeyvault"
}

###############################################################################
# Scale Set Variables
###############################################################################
variable "subscription_id" {
  default = "71e74b5f-5ba1-4bfc-be64-7a1ed30ccb26"
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

variable "vmss_name_prefix" {
  default = ""
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
