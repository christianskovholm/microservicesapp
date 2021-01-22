provider "azurerm" {
  version = "~>2.0"
  features {}
}

terraform {
    backend "azurerm" {}
}

locals {
  suffix = "${var.environment}-${var.location}-001"
}

variable "environment" {
  default = "prod"
}

variable "location" {
  default = "northeurope"
}

variable "tags" {
  type = map(string)

  default = {
    source      = "terraform"
    environment = "prod"
  }
}

module "aks" {
  source = "./modules/aks"

  aks_sp_app_id        = var.sp_app_id
  aks_sp_object_id     = var.sp_object_id
  aks_sp_client_secret = var.sp_client_secret

  aks_linux_profile_admin_username = "vmuser1"

  suffix      = local.suffix
  location    = var.location
  rg_name     = azurerm_resource_group.rg.name

  tags = var.tags

  depends_on = [azurerm_resource_group.rg]
}

module "sql" {
  source = "./modules/sql"

  suffix   = local.suffix
  location = var.location
  rg_name  = azurerm_resource_group.rg.name

  sql_name           = var.sql_name
  sql_admin_username = var.sql_admin_username
  sql_admin_password = var.sql_admin_password 

  psql_admin_username = var.psql_admin_username
  psql_admin_password = var.psql_admin_password

  tags = var.tags
  
  depends_on = [azurerm_resource_group.rg]
}