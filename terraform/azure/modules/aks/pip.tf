locals {
    pip_aks_name = "pip-aks-${var.suffix}"
}

resource "azurerm_public_ip" "aks" {
    name                = local.pip_aks_name
    resource_group_name = local.rg_aks_name
    location            = var.location

    allocation_method   = "Static"
    sku                 = "Standard"

    tags = var.tags

    depends_on = [azurerm_kubernetes_cluster.aks]
}