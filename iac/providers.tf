terraform {
  backend "gcs" {
    bucket = "beauty-scrp-bucket-tfstate"
    prefix = "terraform/state"
  }

  required_version = ">= 0.14"

  required_providers {
    google = ">= 3.3"
  }
}

provider "google" {
  project = var.project_id
  region  = var.region
}