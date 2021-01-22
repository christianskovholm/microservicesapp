locals {
    rg_aks_name = "rg-microservicesapp-aks-${var.suffix}"
}

variable "location" {
  description = "Location of the application."
}

variable "suffix" {
  description = "Suffix to append to all resource names."
}

variable "rg_name" {
  description = "Name of the default resource group."
}

variable "tags" {
  description = "Tags to append to resources."
}

variable "aks_kubernetes_version" {
  description = "The version of Kubernetes."
  default     = "1.17.9"
}

variable "aks_linux_profile_admin_username" {
  description = "Username to use for the creation of an admin profile for the nodes in the cluster."
}

variable "aks_public_ssh_key_path" {
  description = "Public key path for SSH."
  default     = "~/.ssh/id_rsa.pub"
}

variable "aks_node_count" {
  description = "The number of nodes for the cluster."
  default     = 2
}

variable "aks_node_vm_size" {
  description = "The size of the Virtual Machine."
  default     = "Standard_B2s"
}

variable "aks_node_disk_size" {
  description = "Disk size (in GB) to provision for each of the agent pool nodes. This value ranges from 0 to 1023. Specifying 0 applies the default disk size for that agentVMSize."
  default     = 30
}

variable "aks_sp_app_id" {
  description = "Application ID/Client ID  of the service principal. Used by AKS to manage AKS related resources on Azure like vms, subnets."
}

variable "aks_sp_client_secret" {
  description = "Secret of the service principal. Used by AKS to manage Azure."
}

variable "aks_sp_object_id" {
  description = "Object ID of the service principal."
}