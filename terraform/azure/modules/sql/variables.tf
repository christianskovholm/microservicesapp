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

variable "sql_name" {
    description = "Name of the SQL Server."
}

variable "sql_admin_username" {
    description = "Username for the SQL Server admin user."
}

variable "sql_admin_password" {
    description = "Password for the SQL Server admin user."
}

variable "psql_admin_username" {
  description = "Username of the postgresql server admin."
}

variable "psql_admin_password" {
  description = "Password of the postgresql server admin."
}

variable "psql_version" {
  description = "Version of the postgresql server admin."
  default     = "11"
}