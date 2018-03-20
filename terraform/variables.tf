variable "subscription_id" {
  default = ""
}

variable "client_id" {
  default = ""
}

variable "client_secret" {
  default = ""
}

variable "tenant_id" {
  default = ""
}

variable "resource_group_name" {
  description = "The name of the resource group in which to create the virtual network."
}

variable "location" {
  description = "The location/region where the virtual network is created. Changing this forces a new resource to be created."
  default     = "southcentralus"
}

variable "storage_account_tier" {
  description = "Defines the Tier of storage account to be created. Valid options are Standard and Premium."
  default     = "Standard"
}

variable "storage_replication_type" {
  description = "Defines the Replication Type to use for this storage account. Valid options include LRS, GRS etc."
  default     = "LRS"
}

variable "hostname" {
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

variable "vmss_name_prefix" {}

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
