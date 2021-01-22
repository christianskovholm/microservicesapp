locals {
  rg_name = "rg-microservicesapp-${local.suffix}"
}

resource "azurerm_resource_group" "rg" {
  name     = local.rg_name
  location = var.location
}