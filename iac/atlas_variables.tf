###-----------------------------------------------------------------------------
### Atlas
###-----------------------------------------------------------------------------

variable "atlas_cluster_tier" {
  type    = string
  default = "M0" # M0 is the free tier
}

variable "atlas_project_name" {
  type    = string
  default = "yourEasyRent"
}


variable "atlas_org_id" {
  type        = string
  sensitive   = true
  description = "the ID of your MongoDB Atlas organization"
}

variable "atlas_pub_key" {
  type        = string
  description = "public key for MongoDB Atlas"
}

variable "atlas_priv_key" {
  type        = string
  sensitive   = true
  description = "private key for MongoDB Atlas"
}

variable "atlas_cluster_region" {
  type        = string
  description = "the Atlas region in which to create the database cluster"
  default     = "WESTERN_EUROPE"
}

###-----------------------------------------------------------------------------
### MongoDB 
###-----------------------------------------------------------------------------

variable "db_name" {
  type        = string
  description = "the name of the database to configure"
  default     = "yourEasyRent"
}

variable "db_user" {
  type        = string
  description = "the username used to connect to the mongodb cluster"
  default     = "mongo"
}