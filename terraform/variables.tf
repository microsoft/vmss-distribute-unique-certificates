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
  default = ""
}

variable "client_secret" {
  description = "Secret value obtained by creating a key for the App Registration."
  default = ""
}

variable "tenant_id" {
  description = "Active Directory 'Directory ID' property."
  default = ""
}

###############################################################################
# Key Vault Variables
###############################################################################
variable "key_vault_subscription_id" {
  default = ""
}

variable "key_vault_resource_group_name" {
  description = "The name of the resource group in which to create the virtual network."
  default = ""
}

variable "key_vault_location" {
  description = "The location/region where the virtual network is created. Changing this forces a new resource to be created."
  default     = ""
}

variable "key_vault_name" {
  default = ""
}

###############################################################################
# Scale Set Variables
###############################################################################
variable "subscription_id" {
  default = ""
}

variable "resource_group_name" {
  description = "The name of the resource group in which to create the virtual network."
  default = ""
}

variable "location" {
  description = "The location/region where the virtual network is created. Changing this forces a new resource to be created."
  default     = ""
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
  default = ""
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
  default = ""
}

variable "admin_password" {
  default = ""
}

variable "command" {
  default = "bash bootstrap.sh"
}

variable "file1" {
  default = "https://<storage_name>.blob.core.windows.net/vm-scripts-files/bootstrap.sh"
}

variable "file2" {
  default = "https://<storage_name>.blob.core.windows.net/vm-scripts-files/vm_scripts.tar.gz"
}
